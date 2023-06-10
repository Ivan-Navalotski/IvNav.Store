using IvNav.Store.Setup.Configurations;
using IvNav.Store.Setup.Helpers;
using IvNav.Store.Setup.Middleware;
using IvNav.Store.WebApi.Web.Configurations;

// Builder
var builder = WebApplication.CreateBuilder(args);

builder.AddAppSettings("appsettings-api-info.json", "appsettings-logger.json");
builder.AddLogger();

builder.Services.AddCors(options =>
{
    options.AddPolicy("allow-all",
        configurePolicy => configurePolicy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});
builder.Services.AddAutoMapperProfiles(o =>
{
    //o.AddProfile<WebAutoMapperProfile>();
});

//builder.Services.AddCoreDependencies(builder.Configuration);
builder.Services.AddAuthentication(builder.Configuration, o =>
{
    o.TokenValidationParameters = new JwtHelper(builder.Configuration).GetValidationParameters();
});
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

app.UseCors("allow-all");
app.UseForwardedHeaders();
app.UseHttpsRedirection();
app.UseApiVersioning();

app.UseRouting();

app.UseAuthentication();
app.UseMiddleware<HeaderAuthenticationMiddleware>();
app.UseAuthorization();

app.UseMiddleware<ActivityMiddleware>("IvNav.Store.WebApi.Web");
app.UseMiddleware<UnhandledExceptionMiddleware>();
app.UseMiddleware<OperationCanceledMiddleware>();
app.UseMiddleware<IdentityMiddleware>();


app.MapGet("/", context =>
{
    context.Response.Redirect("/swagger/index.html", false);
    return Task.CompletedTask;
});

app.MapDefaultControllerRoute();

app.UseMiddleware<RequiredHeadersMiddleware>();

app.UseStaticFiles();
app.UseSwaggerWithUI(app.Configuration);

// Run
app.Run();
