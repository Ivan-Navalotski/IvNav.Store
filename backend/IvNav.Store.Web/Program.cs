using IvNav.Store.Core.Configurations;
using IvNav.Store.Setup.Configurations;
using IvNav.Store.Setup.Middleware;
using IvNav.Store.Web.Helpers;

// Builder
var builder = WebApplication.CreateBuilder(args);

builder.UseAppSettings();
builder.UseLogger();

builder.Services.AddCors(options =>
{
    options.AddPolicy("allow-all", configurePolicy => configurePolicy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

builder.Services.RegisterCore(builder.Configuration);
builder.Services.RegisterJsonOptions();

builder.Services.RegisterAutoMapperProfiles(o =>
{
    o.AddProfile<WebAutoMapperProfile>();
});

builder.Services.RegisterSwagger(builder.Configuration, c =>
{
    c.SecurityScheme = SwaggerConfiguration.GetBearerSecurityScheme();
    c.AssembliesForAnnotations = new[] { "IvNav.Store.Web", "IvNav.Store.Enums" };
});

var app = builder.Build();

// App
app.UseCors("allow-all");
app.UseForwardedHeaders();
app.UseHttpsRedirection();
app.UseApiVersioning();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ActivityMiddleware>("IvNav.Store.Web");
app.UseMiddleware<UnhandledExceptionMiddleware>();
app.UseMiddleware<OperationCanceledMiddleware>();
app.UseMiddleware<IdentityMiddleware>();

app.UseRouting();
app.UseEndpoints(endpointRouteBuilder =>
{
    endpointRouteBuilder.UseRedirectToSwagger();
    endpointRouteBuilder.MapDefaultControllerRoute();
});

app.UseStaticFiles();
app.UseRegisteredSwagger(app.Configuration);

// Run
app.Run();
