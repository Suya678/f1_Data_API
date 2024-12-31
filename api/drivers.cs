using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

/// <summary>
/// Provides API endpoints for Formula 1 driver data.
/// </summary>
public static class DriversApi
{
    /// <summary>
    /// The MongoDB collection name for drivers.
    /// </summary>
    private static readonly string driverCollectionName = "drivers";

    /// <summary>
    /// Configures the drivers endpoint for retrieving F1 driver data.
    /// </summary>
    /// <param name="app">The application's endpoint route builder.</param>
    public static void DriversEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet(
                "/api/drivers",
                async (string? id, string? driverRef, [FromServices] MongoDbService db) =>
                {
                    try
                    {
                        var collection = db.GetCollection<Driver>(driverCollectionName);

                        if (!string.IsNullOrEmpty(id))
                        {
                            return await QueryHandlerService.HandleRequestWithIntParam(
                                id,
                                async (driverId) =>
                                    await collection.Find(c => c.driverId == driverId).FirstOrDefaultAsync()
                            );
                        }

                        if (!string.IsNullOrEmpty(driverRef))
                        {
                            driverRef = driverRef.ToLower();
                            return await QueryHandlerService.HandleRequestWithStringParam(
                                driverRef,
                                async (driverReference) =>
                                    await collection.Find(c => c.driverRef == driverReference).FirstOrDefaultAsync()
                            );
                        }

                        var drivers = await collection.Find(_ => true).ToListAsync();
                        return Results.Ok(drivers);
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
                Get Formula 1 driver information.

                Parameters:
                - id: Get driver by ID (number)
                - driverRef: Get driver by reference code (case-insensitive string)
                - No parameters: Returns all drivers

                Examples:
                - GET /api/drivers           - Get all drivers
                - GET /api/drivers?id=1      - Get driver with ID 1
                - GET /api/drivers?driverRef=max_verstappen - Get driver by reference
                """
            )
            .WithSummary("Get F1 driver information")
            .WithOpenApi();
    }
}