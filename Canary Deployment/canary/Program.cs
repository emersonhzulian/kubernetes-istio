var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

const string varName = "DOTNET_RUNNING_IN_CONTAINER";
var runningInContainer = bool.TryParse(Environment.GetEnvironmentVariable(varName),
  out var isRunningInContainer)
  && isRunningInContainer;

var configFolder = builder.Configuration.GetValue<string>("ConfigurationFolder");

if (runningInContainer &&
  !string.IsNullOrWhiteSpace(configFolder) &&
  Directory.Exists(configFolder))
{
    builder.Configuration.AddKeyPerFile(configFolder, false, true);
}
else
{
    Console.WriteLine($"ConfigurationFolder set to: '{configFolder}'.");
    Console.WriteLine($"ConfigurationFolder exists: {Directory.Exists(configFolder)}");
    Console.WriteLine($"Running in Container: '{runningInContainer}'.");
    Console.WriteLine("Relying on default configuration");
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/", (IConfiguration config) =>
{
    return config.GetValue<string>("VERSION");
})
.WithOpenApi();

app.Run();

