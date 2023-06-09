using IvNav.Store.Setup.Configurations;
using IvNav.Store.Setup.Middleware;

// Builder
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDefaultApiVersioning();
builder.Services.AddControllers();
builder.Services.AddJsonOptions();

builder.Services.AddSwagger(builder.Configuration, c =>
{
    c.SecurityScheme = SwaggerConfiguration.GetBearerSecurityScheme();
    c.AssembliesForAnnotations = new[] { "IvNav.Store.Web", "IvNav.Store.Enums" };
});


// App
var app = builder.Build();

app.UseForwardedHeaders();
app.UseHttpsRedirection();
app.UseApiVersioning();

app.UseMiddleware<ActivityMiddleware>("IvNav.Store.Web.Mail");

app.MapDefaultControllerRoute();
app.UseStaticFiles();
app.UseSwaggerWithUI(app.Configuration);

app.Run();
