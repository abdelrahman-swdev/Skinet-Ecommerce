using StackExchange.Redis;
using Core.Entities.identity;

var builder = WebApplication.CreateBuilder(args);

// add services to the container
builder.Services.AddControllers();

builder.Services.AddDbContext<StoreContext>(options => {
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddDbContext<AppIdentityDbContext>(op => {
    op.UseNpgsql(builder.Configuration.GetConnectionString("IdentityConnection"));
});

// add application Services
builder.Services.AddApplicationServices();

// add identity Services
builder.Services.AddIdentityServices(builder.Configuration);

// add swagger
builder.Services.AddSwaggerDocumentation();

// add automapper
builder.Services.AddAutoMapper(typeof(MappingProfiles));

// add Redis
builder.Services.AddSingleton<IConnectionMultiplexer>(c =>
{
    var configuration = ConfigurationOptions.Parse(builder.Configuration.GetConnectionString("Redis"), true);
    return ConnectionMultiplexer.Connect(configuration);
});

// add cors
builder.Services.AddCors(op => {
    op.AddPolicy("CorsPolicy", policy => {
        policy.AllowAnyHeader()
        .AllowAnyMethod()
        .WithOrigins("https://localhost:4200");
    });
});
//*************************************************************************//

// configure http request pipeline

var app = builder.Build();
// exception middleware
app.UseMiddleware<ExceptionMiddleware>();


// add middleware for redirect to errors path when requesting endpoint that doesnot exist
app.UseStatusCodePagesWithReExecute("/errors/{0}");

app.UseHttpsRedirection();

// use static files
app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "Content")
    ),
    RequestPath = "/content"
});

// use cors
app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

// user swagger middleware
app.UseSwaggerDocumentation();

app.MapControllers();
app.MapFallbackToController("Index", "Fallback");


using var scope = app.Services.CreateScope();

var services = scope.ServiceProvider;
var loggerFactory = services.GetRequiredService<ILoggerFactory>();
try
{
    var context = services.GetRequiredService<StoreContext>();
    await context.Database.MigrateAsync();
    await StoreContextSeed.SeedAsync(context, loggerFactory);

    var userManager = services.GetRequiredService<UserManager<AppUser>>();
    var identityContext = services.GetRequiredService<AppIdentityDbContext>();
    await identityContext.Database.MigrateAsync();
    await AppIdentityDbContextSeed.SeedUsersAsync(userManager);
}
catch(Exception e)
{
    var logger = loggerFactory.CreateLogger<Program>();
    logger.LogError(e, "An error occured during migration");
}

await app.RunAsync();

