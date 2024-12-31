using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

/// <summary>
/// Provides API endpoints for Formula 1 constructor race results.
/// </summary>
public static class ConstructorResultsApi
{
    /// <summary>
    /// The MongoDB collection name for race results.
    /// </summary>
    private static readonly string raceResultsCollection = "raceResults";

    /// <summary>
    /// Configures the constructor results endpoint for retrieving F1 constructor race data.
    /// </summary>
    /// <param name="app">The application's endpoint route builder.</param>
    public static void ConstructorResultsEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet(
                "/api/constructorResults",
                async (string? constructorRef, int? season, [FromServices] MongoDbService db) =>
                {
                    if (season is null || string.IsNullOrWhiteSpace(constructorRef))
                    {
                        return Results.BadRequest(
                            new { Error = "Both season and constructorRef parameters are required" }
                        );
                    }

                    constructorRef = constructorRef.ToLower();

                    try
                    {
                        var collection = db.GetCollection<RaceResult>(raceResultsCollection);
                        var constructorResult = await collection
                            .Find(c => c.race.year == season && c.constructor.constructorRef == constructorRef)
                            .Project(r => new
                            {
                                year = r.race.year,
                                date = r.race.date,
                                name = r.race.name,
                                round = r.race.round,
                                resultId = r.id,
                                positionOrder = r.position,
                                grid = r.grid,
                                driverRef = r.driver.driverRef,
                                forename = r.driver.forename,
                                surname = r.driver.surname,
                            })
                            .ToListAsync();

                        if (constructorResult.Count is 0)
                        {
                            return Results.NotFound(
                                new { Error = "Constructor race results for the specified season not found" }
                            );
                        }

                        return Results.Ok(constructorResult);
                    }
                    catch (Exception e)
                    {
                        return Results.Problem(
                            detail: $"An error occurred: {e.Message}",
                            statusCode: StatusCodes.Status500InternalServerError
                        );
                    }
                }
            )
            .WithDescription(
                """
                Get Formula 1 constructor race results by team reference and season. Both parameters are required.

                Parameters:
                - constructorRef: Team reference code (case-insensitive, e.g., "mercedes", "red_bull")
                - season: Year of the F1 season (e.g., 2023)

                Returns filtered race data including:
                - Race information (year, date, name, round)
                - Result details (position, grid position)
                - Driver information (reference, full name)

                Example:
                GET /api/constructorResults?constructorRef=mercedes&season=2023
                """
            )
            .WithSummary("Get F1 constructor race results by team and season")
            .WithOpenApi();
    }
}
