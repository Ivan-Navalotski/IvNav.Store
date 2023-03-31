using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace IvNav.Store.Setup.Configurations;

/// <summary>
/// RegisterSwaggerConfiguration
/// </summary>
public static class SwaggerConfiguration
{
    /// <summary>
    /// RegisterSwaggerOptions
    /// </summary>
    public class RegisterSwaggerOptions
    {
        /// <summary>
        /// SecurityScheme
        /// </summary>
        public OpenApiSecurityScheme? SecurityScheme { get; set; }

        /// <summary>
        /// AssembliesForAnnotations
        /// </summary>
        public string[]? AssembliesForAnnotations { get; set; }

        /// <summary>
        /// DisableRolesInfo
        /// </summary>
        public bool DisableRolesInfo { get; set; }

        /// <summary>
        /// DisablePoliciesInfo
        /// </summary>
        public bool DisablePoliciesInfo { get; set; }
    }

    /// <summary>
    /// Use redirect to swagger
    /// </summary>
    /// <param name="endpointRouteBuilder"></param>
    /// <returns></returns>
    public static IEndpointRouteBuilder UseRedirectToSwagger(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapGet("", async context =>
        {
            context.Response.Redirect("/swagger/index.html", false);
            await context.Response.WriteAsync("");
        });

        return endpointRouteBuilder;
    }

    /// <summary>
    /// RegisterSwagger
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <param name="registerOptions"></param>
    /// <returns></returns>
    public static IServiceCollection RegisterSwagger(this IServiceCollection services, IConfiguration configuration,
        Action<RegisterSwaggerOptions>? registerOptions = null)
    {
        var apiDescription = configuration.GetSection("ApiInfoSettings");

        var apiVersionService = (IApiVersionDescriptionProvider?)services.BuildServiceProvider().GetService(typeof(IApiVersionDescriptionProvider));

        static string GetXmlCommentPath(string assemblyName) => Path.Combine(AppContext.BaseDirectory, assemblyName + ".xml");

        var registerOptionsData = new RegisterSwaggerOptions();
        registerOptions?.Invoke(registerOptionsData);

        services.AddSwaggerGen(c =>
        {
            c.EnableAnnotations();

            if (registerOptionsData.AssembliesForAnnotations?.Any() ?? false)
            {
                foreach (var annotationNamespace in registerOptionsData.AssembliesForAnnotations)
                {
                    var xmlPath = GetXmlCommentPath(annotationNamespace);
                    if (File.Exists(xmlPath))
                    {
                        c.IncludeXmlComments(xmlPath, true);
                    }
                }
            }

            // Priority for sorting
            var sorting = new[]
                {
                    "GET",
                    "HEAD",
                    "POST",
                    "PUT",
                    "PATCH",
                    "DELETE",
                    "OPTIONS",
                    "CONNECT",
                    "TRACE",
                    "UNKNOWN",
                }
                .Select((value, index) => new { value, index })
                .ToDictionary(i => i.value, i => i.index.ToString().PadLeft(2, '0'));

            // Controllers sorting
            c.TagActionsBy(apiDesc =>
            {
                var controllerName = apiDesc.ActionDescriptor.RouteValues["controller"];
                return new[] { controllerName };
            });

            // Methods sorting
            c.OrderActionsBy(apiDesc =>
            {
                var method = apiDesc.HttpMethod?.ToUpper() ?? "UNKNOWN";
                var sortingNumber = sorting.ContainsKey(method) ? sorting[method] : sorting["UNKNOWN"];

                var path = apiDesc.RelativePath?.Replace('/', '_');

                return $"{path}_{sortingNumber}_{method}";
            });

            if (apiVersionService != null)
            {
                // Documentation for versions
                foreach (var description in apiVersionService.ApiVersionDescriptions)
                {
                    c.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description, apiDescription));
                }
            }
            else
            {
                c.SwaggerDoc("V1", CreateInfoForApiVersion(null, apiDescription));
            }

            // Auth
            if (registerOptionsData.SecurityScheme != null)
            {
                c.AddSecurityDefinition("Bearer", registerOptionsData.SecurityScheme);
            }

            // Auth icon
            c.OperationFilter<AddAuthorizationHeaderOperationHeader>(registerOptionsData);
        });

        return services;
    }

    /// <summary>
    /// UseRegisteredSwagger
    /// </summary>
    /// <param name="app"></param>
    /// <param name="configuration">Configuration</param>
    /// <returns></returns>
    public static IApplicationBuilder UseRegisteredSwagger(this IApplicationBuilder app, IConfiguration? configuration)
    {
        var apiVersionService = (IApiVersionDescriptionProvider?)app.ApplicationServices.GetService(typeof(IApiVersionDescriptionProvider));
        var apiDescription = configuration?.GetSection("ApiInfoSettings");

        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.DocumentTitle = apiDescription?.GetSection("Title").Value ?? "Swagger";
            if (!string.IsNullOrEmpty(apiDescription?.GetSection("StylesPath").Value))
            {
                options.InjectStylesheet(apiDescription.GetSection("StylesPath").Value);
            }

            if (apiVersionService != null)
            {
                foreach (var description in apiVersionService.ApiVersionDescriptions)
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
            }
            else
            {
                options.SwaggerEndpoint("/swagger/V1/swagger.json", "V1");
            }

        });

        return app;
    }


    /// <summary>
    /// CreateInfoForApiVersion
    /// </summary>
    /// <param name="description"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription? description, IConfiguration? configuration)
    {
        var baseInfo = configuration?.GetSection("Description").Value;

        var info = new OpenApiInfo
        {
            Title = configuration?.GetSection("Title").Value,
            Version = description?.ApiVersion.ToString() ?? "1.0",
            Description = baseInfo + "<br/><br/>" + configuration?.GetSection($"{description?.ApiVersion}:Description").Value
        };

        if (description?.IsDeprecated ?? false)
        {
            info.Description += "<br/><br/><b>This API version has been deprecated.</b>";
        }

        return info;
    }

    internal class AddAuthorizationHeaderOperationHeader : IOperationFilter
    {
        private readonly RegisterSwaggerOptions _registerSwaggerOptions;

        public AddAuthorizationHeaderOperationHeader(RegisterSwaggerOptions registerSwaggerOptions)
        {
            _registerSwaggerOptions = registerSwaggerOptions;
        }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var actionMetadata = context.ApiDescription.ActionDescriptor.EndpointMetadata;
            var authorizedAttributes = actionMetadata.OfType<AuthorizeAttribute>().ToArray();
            var allowAnonymous = actionMetadata.Any(i => i is AllowAnonymousAttribute);

            if (!authorizedAttributes.Any() || allowAnonymous)
            {
                return;
            }
            operation.Parameters ??= new List<OpenApiParameter>();

            // Roles to summary
            if (!_registerSwaggerOptions.DisableRolesInfo)
            {
                var rolesStr = string.Join(", ",
                    authorizedAttributes
                        .Where(i => !string.IsNullOrEmpty(i.Roles))
                        .Select(i => i.Roles)
                        .Distinct()
                        .OrderBy(i => i));

                if (!string.IsNullOrEmpty(rolesStr))
                {
                    operation.Summary += $" {{Roles: {rolesStr}}}";
                    operation.Summary = operation.Summary.Trim();
                }
            }

            // Polities to summary
            if (!_registerSwaggerOptions.DisablePoliciesInfo)
            {
                var policiesStr = string.Join(", ",
                    authorizedAttributes
                        .Where(i => !string.IsNullOrEmpty(i.Policy))
                        .Select(i => i.Policy)
                        .Distinct()
                        .OrderBy(i => i));

                if (!string.IsNullOrEmpty(policiesStr))
                {
                    operation.Summary += $" {{Policies: {policiesStr}}}";
                    operation.Summary = operation.Summary.Trim();
                }
            }

            operation.Security = new List<OpenApiSecurityRequirement>
            {
                //Add JWT bearer type
                new()
                {
                    {
                        GetBearerSecurityScheme(),
                        Array.Empty<string>()
                    }
                }
            };
        }
    }

    /// <summary>
    /// Get Bearer SecurityScheme
    /// </summary>
    /// <returns></returns>
    public static OpenApiSecurityScheme GetBearerSecurityScheme()
    {
        var result = new OpenApiSecurityScheme
        {
            Description = "Please insert JWT with Bearer into field. Example: \"Bearer MyAccessToken12345\"",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,

            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            }
        };

        return result;
    }
}
