using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

/// <summary>
/// Provides endpoint for  handling for Formula 1 race results requests.
/// </summary>
public static class RaceResultsApi
{
    /// <summary>
    /// The MongoDB collection name for race results.
    /// </summary>
    private static readonly string raceResultsCollectionName = "raceResults";

    /// <summary>
    /// Configures the race results endpoint for retrieving F1 race result data.
    /// </summary>
    /// <param name="app">The application's endpoint route builder.</param>
    public static void RaceResultsEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet(
                "/api/raceResults",
                async (string? season, string? id, [FromServices] MongoDbService db) =>
                {
                    try
                    {
                        var collection = db.GetCollection<RaceResult>(raceResultsCollectionName);

                        if (!string.IsNullOrWhiteSpace(id))
                        {
                            return await QueryHandlerService.HandleRequestWithIntParam(
                                id,
                                async (raceId) => await collection.Find(c => c.id == raceId).ToListAsync()
                            );
                        }

                        if (!string.IsNullOrWhiteSpace(season))
                        {
                            return await QueryHandlerService.HandleRequestWithIntParam(
                                season,
                                async (year) => await collection.Find(c => c.race.year == year).ToListAsync()
                            );
                        }

                        return Results.BadRequest(new { Error = "Either season or id parameter is required" });
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
            .WithDescription("""
            Get Formula 1 race results. Requires either season or race ID parameter.

            Parameters:
            - season: Filter by year (e.g., "2023")
            - id: Get specific race result by ID (number)
       
            Examples:
            - GET /api/raceResults?season=2023  - Get all races from 2023
            - GET /api/raceResults?id=1052      - Get race result with ID 1052
            """)
            .WithSummary("Get F1 race results")
            .WithOpenApi();;

    }
}
