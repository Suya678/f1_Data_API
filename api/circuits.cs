using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

/// <summary>
/// Provides API endpoints for Formula 1 circuit data.
/// </summary>
public static class CircuitAPI
{
    /// <summary>
    /// The MongoDB collection name for circuits.
    /// </summary>
    private static readonly string circuitCollectionName = "circuits";

    /// <summary>
    /// Configures the circuit endpoint for retrieving F1 circuit data.
    /// </summary>
    /// <param name="app">The application's endpoint route builder.</param>
    public static void CircuitEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet(
                "/api/circuits/",
                async (string? id, [FromServices] MongoDbService db) =>
                {
                    try
                    {
                        var collection = db.GetCollection<Circuit>(circuitCollectionName);

                        if (id is null)
                        {
                            var circuits = await collection.Find(_ => true).ToListAsync();
                            return Results.Ok(circuits);
                        }

                        return await QueryHandlerService.HandleRequestWithIntParam(
                            id,
                            async (parsedId) =>
                                await collection.Find(c => c.circuitId == parsedId).FirstOrDefaultAsync()
                        );
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
                Get Formula 1 circuit information:
                - No parameters: Returns all circuits
                - id: Returns specific circuit by ID (number)

                Example:
                - GET /api/circuits     - Get all circuits
                - GET /api/circuits?id=1 - Get circuit with ID 1
                """
            )
            .WithSummary("Get F1 circuit information")
            .WithOpenApi();
    }
}
