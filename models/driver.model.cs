using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.EntityFrameworkCore;

[Collection("drivers")]
public class Driver
{
    [BsonId]
    public ObjectId _id;

    public int driverId { get; set; }

    public string? driverRef { get; set; }
    public Object? number { get; set; } // some of the numbers are empty strings

    public string? code { get; set; }
    public string? forename { get; set; }

    public string? surname { get; set; }


    public string? dob { get; set; }

    public string? nationality { get; set; }

    public string? url { get; set; }
}
