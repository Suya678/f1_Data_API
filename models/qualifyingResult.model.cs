using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.EntityFrameworkCore;

[Collection("qualifyingResults")]
public class QualifyingResult
{

    [BsonId]
    public ObjectId _id;

    public int id { get; set; }

    public int position { get; set; }

    public int number { get; set; }

    public string? q1 { get; set; }
    public string? q2 { get; set; }
    public string? q3 { get; set; }

    [BsonElement("race")]
    public NestedRace? race { get; set; }

    [BsonElement("driver")]
    public NestedDriver? driver { get; set; }

    [BsonElement("constructor")]
    public NestedConstructor? constructor { get; set; }

}