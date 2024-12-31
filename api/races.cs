using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

/// <summary>
/// Provides API endpoints for Formula 1 race data.
/// </summary>
public static class RacesApi
{
    /// <summary>
    /// The MongoDB collection name for races.
    /// </summary>
    private static readonly string racesCollectionName = "races";

    /// <summary>
    /// Configures the races endpoint for retrieving F1 race data.
    /// </summary>
    /// <param name="app">The application's endpoint route builder.</param>
    public static void RacesEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet(
                "/api/races",
                async (string? season, string? id, [FromServices] MongoDbService db) =>
                {
                    try
                    {
                        var collection = db.GetCollection<Race>(racesCollectionName);

                        if (!string.IsNullOrWhiteSpace(season))
                        {
                            return await QueryHandlerService.HandleRequestWithIntParam(
                                season,
                                async (year) => await collection.Find(c => c.year == year).ToListAsync()
                            );
                        }

                        if (!string.IsNullOrWhiteSpace(id))
                        {
                            return await QueryHandlerService.HandleRequestWithIntParam(
                                id,
                                async (raceId) => await collection.Find(c => c.id == raceId).FirstOrDefaultAsync()
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
            .WithDescription(
                """
                Get Formula 1 race information

                Parameters:
                - season: Get races by year (e.g., "2023")
                - id: Get race by ID (number)

                Examples:
                - GET /api/races?season=2023  - Get all races from 2023
                - GET /api/races?id=1052      - Get race with ID 1052
                """
            )
            .WithSummary("Get F1 races by season or race ID")
            .WithOpenApi();
    }
}
