using IvNav.Store.Identity.Core.Configurations;
using IvNav.Store.Identity.Web.Helpers.Mapper;
using IvNav.Store.Setup.Configurations;
using IvNav.Store.Setup.Middleware;

// Builder
var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings-api-info.json", true);
builder.AddLogger();

builder.Services.AddAutoMapperProfiles(o =>
{
    o.AddProfile<WebAutoMapperProfile>();
});

builder.Services.AddCoreDependencies(builder.Configuration);
builder.Services.AddCustomIdentityServer(builder.Configuration).AddJwtBearerFromConfig(builder.Configuration);
builder.Services.AddRazorPages();
builder.Services.AddControllers();
builder.Services.AddJsonOptions();

builder.Services.AddSwagger(builder.Configuration, c =>
{
    c.SecurityScheme = SwaggerConfiguration.GetBearerSecurityScheme();
    c.AssembliesForAnnotations = new[] { "IvNav.Store.Identity.Web", "IvNav.Store.Enums" };
});


// App
var app = builder.Build();

app.UseExceptionHandler("/Home/Error");
app.UseCustomIdentityServer();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ActivityMiddleware>("IvNav.Store.Identity.Web");
app.UseMiddleware<UnhandledExceptionMiddleware>();
app.UseMiddleware<OperationCanceledMiddleware>();
app.UseMiddleware<IdentityMiddleware>();

app.MapRazorPages();
app.MapGet("/", context =>
{
    context.Response.Redirect("/Account/Index", false);
    return Task.CompletedTask;
});
app.MapDefaultControllerRoute();

app.UseStaticFiles();
app.UseSwaggerWithUI(app.Configuration);

// Run
app.Run();
