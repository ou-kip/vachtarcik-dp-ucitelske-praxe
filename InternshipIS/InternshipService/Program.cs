using InternshipService;
using InternshipService.Middleware;
using InternshipService.Swagger;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

var builder = WebApplication.CreateBuilder(args);

var cert = new X509Certificate2("/app/Ssl/praxeosucz.pfx", "testing");

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(8071, listenOptions =>
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

//db contexts
builder.Services.AddRepositoriesWithDbContext(builder.Configuration);

//client settings
builder.Services.AddClientSettings(builder.Configuration);

//add messaging
builder.Services.AddMessaging(builder.Configuration);

//add services
builder.Services.AddServices();

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

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseCustomSwaggerUI();
}

app.UseCors("AllowAll");

//app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Services.ApplyDbMigrations();

app.Run();
