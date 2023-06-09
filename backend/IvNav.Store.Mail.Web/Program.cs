using IvNav.Store.Mail.Core.Configurations;
using IvNav.Store.Setup.Configurations;
using IvNav.Store.Setup.Middleware;

// Builder
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCoreDependencies(builder.Configuration);

builder.Services.AddDefaultApiVersioning();
builder.Services.AddControllers();
builder.Services.AddJsonOptions();

builder.Services.AddSwagger(builder.Configuration, c =>
{
    c.SecurityScheme = SwaggerConfiguration.GetBearerSecurityScheme();
    c.AssembliesForAnnotations = new[] { "IvNav.Store.Mail.Web", "IvNav.Store.Enums" };
});


// App
var app = builder.Build();

app.UseForwardedHeaders();
app.UseHttpsRedirection();
app.UseApiVersioning();

app.UseMiddleware<ActivityMiddleware>("IvNav.Store.Mail.Web");
app.UseMiddleware<UnhandledExceptionMiddleware>();
app.UseMiddleware<OperationCanceledMiddleware>();
app.UseMiddleware<IdentityMiddleware>();

app.MapDefaultControllerRoute();

app.UseMiddleware<RequiredHeadersMiddleware>();

app.UseStaticFiles();
app.UseSwaggerWithUI(app.Configuration);

app.Run();
