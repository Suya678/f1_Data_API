using Scalar.AspNetCore;
using dotenv.net;
DotEnv.Load();


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options => 
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin()
             .AllowAnyMethod()
             .AllowAnyHeader()));
// Add Scalar and OpenAPI services
builder.Services.AddOpenApi();
// Configure Mongo Db Database service
try
{
    var envVars = DotEnv.Read();
    builder.Services.AddSingleton<MongoDbService>(new MongoDbService(envVars["MONGO_DB_CONNECTION_STRING"], envVars["MONGO_DB_DATABASE_NAME"]));
}
catch (Exception ex)
{
    Console.WriteLine($"Error initializing MongoDB: {ex.Message}");
    Environment.Exit(1);
}

var app = builder.Build();
app.UseCors();
app.MapOpenApi()
    .WithDescription(
        """
        Formula 1 Historical Data API

        Access historical Formula 1 racing data including:
        - Circuits
        - Constructors (Teams)
        - Drivers
        - Races
        - Race Results
        - Qualifying Results
        - Constructor Results

        All endpoints return JSON data and support various query parameters
        for filtering results.

        Database: MongoDB
        Base URL: /api
        """
    );

// Enable Scalar API documentaion
app.MapScalarApiReference();

/// All endpoints
app.CircuitEndpoint();
app.ConstructorsEndpoint();
app.DriversEndpoint();
app.RacesEndpoint();
app.RaceResultsEndpoint();
app.QualifyingResultEndpoint();
app.ConstructorResultsEndpoint();


app.Run();
