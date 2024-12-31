using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

/// <summary>
/// Provides API endpoints for Formula 1 qualifying result data.
/// </summary>
public static class QualifyingResultApi
{
    /// <summary>
    /// The MongoDB collection name for qualifying results.
    /// </summary>
    private static readonly string qualifyingResultCollectionName = "qualifyingResults";

    /// <summary>
    /// Configures the qualifying results endpoint for retrieving F1 qualifying data.
    /// </summary>
    /// <param name="app">The application's endpoint route builder.</param>
    public static void QualifyingResultEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet(
                "/api/qualifyingResults",
                async (string? season, string? id, [FromServices] MongoDbService db) =>
                {
                    try
                    {
                        var collection = db.GetCollection<QualifyingResult>(qualifyingResultCollectionName);

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
            .WithDescription(
                """
                Get Formula 1 qualifying results. Requires either season or qualifying ID parameter.

                Parameters:
                - season: Filter by year (e.g., "2023")
                - id: Get qualifying results by race ID (number)

                Examples:
                - GET /api/qualifyingResults?season=2023  - Get all qualifying results from 2023
                - GET /api/qualifyingResults?id=1052      - Get qualifying results for race ID 1052

                """
            )
            .WithSummary("Get F1 qualifying results by season or race ID")
            .WithOpenApi();
    }
}
