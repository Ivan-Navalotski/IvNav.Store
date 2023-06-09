using IvNav.Store.Setup.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace IvNav.Store.Setup.Configurations;

/// <summary>
/// Swagger configuration
/// </summary>
public static class SwaggerConfiguration
{
    /// <summary>
    /// Swagger options
    /// </summary>
    public class RegisterSwaggerOptions
    {
        /// <summary>
        /// Security scheme for auth
        /// </summary>
        public OpenApiSecurityScheme? SecurityScheme { get; set; }

        /// <summary>
        /// Assemblies names for applying annotations
        /// </summary>
        public string[]? AssembliesForAnnotations { get; set; }

        /// <summary>
        /// Disable information about required roles in method descriptions
        /// </summary>
        public bool DisableRolesInfo { get; set; }

        /// <summary>
        /// Disable information about required policies in method descriptions
        /// </summary>
        public bool DisablePoliciesInfo { get; set; }

        internal RegisterSwaggerOptions()
        {

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

    /// <summary>
    /// Add swagger
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <param name="registerOptions"></param>
    /// <returns></returns>
    public static IServiceCollection AddSwagger(this IServiceCollection services, IConfiguration configuration,
        Action<RegisterSwaggerOptions>? registerOptions = null)
    {
        var apiDescription = configuration.GetSection("ApiInfoSettings");

        var apiVersionService = (IApiVersionDescriptionProvider?)services.BuildServiceProvider().GetService(typeof(IApiVersionDescriptionProvider));

        static string GetXmlCommentPath(string assemblyName) => Path.Combine(AppContext.BaseDirectory, assemblyName + ".xml");

        var registerOptionsData = new RegisterSwaggerOptions();
        registerOptions?.Invoke(registerOptionsData);

        services.AddEndpointsApiExplorer();
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
                var sortingNumber = sorting.TryGetValue(method, out var value) ? value : sorting["UNKNOWN"];

                var path = apiDesc.RelativePath?.Replace('/', '.');

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

            // Add auth icon
            c.OperationFilter<AuthorizationHeaderOperationHeader>(registerOptionsData);

            // Add headers
            c.OperationFilter<HeadersAttributeOperationFilter>();
        });

        return services;
    }

    /// <summary>
    /// Use registered swagger with UI
    /// </summary>
    /// <param name="app"></param>
    /// <param name="configuration">Configuration</param>
    /// <returns></returns>
    // ReSharper disable once InconsistentNaming
    public static IApplicationBuilder UseSwaggerWithUI(this IApplicationBuilder app, IConfiguration? configuration)
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

    internal class AuthorizationHeaderOperationHeader : IOperationFilter
    {
        private readonly RegisterSwaggerOptions _registerSwaggerOptions;

        public AuthorizationHeaderOperationHeader(RegisterSwaggerOptions registerSwaggerOptions)
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

    public class HeadersAttributeOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Parameters ??= new List<OpenApiParameter>();

            if (context.MethodInfo.GetCustomAttribute(typeof(RequestHeadersAttribute)) is RequestHeadersAttribute attribute)
            {
                foreach (var header in attribute.Headers)
                {
                    var existingParam = operation.Parameters.FirstOrDefault(p =>
                        p.In == ParameterLocation.Header && p.Name == header.HeaderName);

                    // Remove description from [FromHeader] argument attribute
                    if (existingParam != null) 
                    {
                        operation.Parameters.Remove(existingParam);
                    }

                    operation.Parameters.Add(new OpenApiParameter
                    {
                        Name = header.HeaderName,
                        In = ParameterLocation.Header,
                        Description = header.Description,
                        Required = false,
                        Schema = string.IsNullOrEmpty(header.DefaultValue)
                            ? null
                            : new OpenApiSchema
                            {
                                Type = "String",
                                Default = new OpenApiString(header.DefaultValue)
                            }
                    });
                }
            }
        }
    }
}
