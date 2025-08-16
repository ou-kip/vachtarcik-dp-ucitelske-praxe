using AuthorizationService;
using AuthorizationService.Middleware;
using AuthorizationService.Swagger;
using Core.Messaging;
using Identity;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

var sslPath = Environment.GetEnvironmentVariable("SSL_PATH")!;
var sslPassword = Environment.GetEnvironmentVariable("SSL_PASSWORD")!;

var builder = WebApplication.CreateBuilder(args);

var cert = new X509Certificate2(sslPath, sslPassword);
builder.WebHost.ConfigureKestrel(options =>
{    
    options.ListenAnyIP(8081, listenOptions =>
    {
        listenOptions.UseHttps(cert);
    });
});

// Add services to the container.
builder.Services.AddControllers(options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddLogging();

//swagger
builder.Services.AddSwagger();
builder.Services.AddVersioning();
builder.Services.AddSwaggerConfiguration();

//add mediatr
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

//repositories
builder.Services.AddAuthorizationRepositories();

//db contexts
builder.Services.AddAuthorizationWithDbContext(builder.Configuration);

//messaging
builder.Services.AddMessaging(builder.Configuration);

//cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder.WithOrigins("https://praxeosu.cz")
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .WithExposedHeaders("set-cookie")
                            .AllowCredentials());
});

//auth
builder.Services.AddCoreAuthentication(builder.Configuration);

//memory cache
builder.Services.AddCustomMemoryCache();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseCustomSwaggerUI();
}

app.UseCors("AllowAll");

app.UseMiddleware<ExceptionMiddleware>();

//app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Services.ApplyAuthorizationDbMigrations();

app.Run();
