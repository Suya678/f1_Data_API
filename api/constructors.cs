using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

/// <summary>
/// Provides API endpoints for Formula 1 constructor data.
/// </summary>
public static class ConstructorAPI
{
    /// <summary>
    /// The MongoDB collection name for constructors.
    /// </summary>
    private static readonly string constructorCollectionName = "constructors";

    /// <summary>
    /// Configures the constructors endpoint for retrieving F1 constructor data.
    /// </summary>
    /// <param name="app">The application's endpoint route builder.</param>
    public static void ConstructorsEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet(
                "/api/constructors",
                async ([FromQuery] string? constructorRef, [FromQuery] string? id, [FromServices] MongoDbService db) =>
                {
                    try
                    {
                        var collection = db.GetCollection<Constructor>(constructorCollectionName);

                        if (id is not null)
                        {
                            return await QueryHandlerService.HandleRequestWithIntParam(
                                id,
                                async (consId) =>
                                    await collection.Find(c => c.constructorId == consId).FirstOrDefaultAsync()
                            );
                        }

                        if (!string.IsNullOrEmpty(constructorRef))
                        {
                            constructorRef = constructorRef.ToLower();
                            return await QueryHandlerService.HandleRequestWithStringParam(
                                constructorRef,
                                async (stringParam) =>
                                    await collection.Find(c => c.constructorRef == stringParam).FirstOrDefaultAsync()
                            );
                        }

                        var constructors = await collection.Find(_ => true).ToListAsync();
                        return Results.Ok(constructors);
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
            Usage:
            - No parameters: Get all constructors
            - id: Get constructor by ID (number)
            - constructorRef: Get constructor by team code (e.g., 'mercedes')
            """)
            .WithSummary("Get F1 constructor information");
    }
}
