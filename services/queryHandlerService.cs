/// <summary>
/// Provides generic query handling services for HTTP endpoints.
/// </summary>
public static class QueryHandlerService
{
    /// <summary>
    /// Handles requests that require integer parameter validation and document retrieval.
    /// </summary>
    /// <typeparam name="T">The type of document to retrieve.</typeparam>
    /// <param name="intParam">The string parameter to parse as an integer.</param>
    /// <param name="findDoc">A function that retrieves a document using the parsed integer parameter.</param>
    /// <returns>
    /// - 200 OK with the document if found
    /// - 400 Bad Request if the parameter cannot be parsed as an integer
    /// - 404 Not Found if no document matches the parameter
    /// </returns>
    public static async Task<IResult> HandleRequestWithIntParam<T>(string intParam, Func<int, Task<T?>> findDoc)
    {
        if (!int.TryParse(intParam, out int parsedId))
        {
            return Results.BadRequest(new { Error = "Invalid query format" });
        }

        var document = await findDoc(parsedId);
        if (document is null || (document is IEnumerable<object> enumerable && !enumerable.Any()))
        {
            return Results.NotFound(new { Error = $"Document with {intParam} field not found" });
        }

        return Results.Ok(document);
    }

    /// <summary>
    /// Handles requests that require string parameter validation and document retrieval.
    /// </summary>
    /// <typeparam name="T">The type of document to retrieve.</typeparam>
    /// <param name="stringParam">The string parameter to search for.</param>
    /// <param name="findDoc">A function that retrieves a document using the string parameter.</param>
    /// <returns>
    /// - 200 OK with the document if found
    /// - 404 Not Found if no document matches the parameter
    /// </returns>
    public static async Task<IResult> HandleRequestWithStringParam<T>(
        string stringParam,
        Func<string, Task<T?>> findDoc
    )
    {
        var document = await findDoc(stringParam);
        if (document is null || (document is IEnumerable<object> enumerable && !enumerable.Any()))
        {
            return Results.NotFound(new { Error = $"Document with {stringParam} field not found" });
        }

        return Results.Ok(document);
    }
}
