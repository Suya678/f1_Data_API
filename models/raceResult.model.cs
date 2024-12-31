

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.EntityFrameworkCore;

[Collection("raceResults")]
public class RaceResult
{

    [BsonId]
    public ObjectId _id;

    public int id { get; set; }

    public int position { get; set; }

    public int grid { get; set; }

    public int laps { get; set; }
    public double points { get; set; }

    public Object? time { get; set; }  // Could be an empty string or a double
    public Object? fastestLap { get; set; } // Could be an empty string or an int

    [BsonElement("race")]
    public NestedRace? race { get; set; }

    [BsonElement("driver")]
    public NestedDriver? driver { get; set; }

    [BsonElement("constructor")]
    public NestedConstructor? constructor { get; set; }

}