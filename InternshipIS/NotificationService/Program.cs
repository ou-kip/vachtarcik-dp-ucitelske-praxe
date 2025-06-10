using NotificationService;
using NotificationService.Middleware;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddLogging();
builder.Services.AddCustomHttpLogging();

//add mediatr
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

//swagger
builder.Services.AddSwaggerGen();
builder.Services.AddVersioning();
builder.Services.AddSwaggerConfiguration();

//emailing
builder.Services.AddEmailing(builder.Configuration);

//client settings
builder.Services.AddClientSettings(builder.Configuration);

//messaging
builder.Services.AddMessaging(builder.Configuration);

//cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder.AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseCustomSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpLogging();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseCors("AllowAll");

app.Run();
