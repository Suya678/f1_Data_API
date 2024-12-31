using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.EntityFrameworkCore;


[Collection("races")]
public class Race
{
    [BsonId]
    public ObjectId _id;

    public int id { get; set; }

    public int year { get; set; }

    public int round { get; set; }

    public string? name { get; set; }

    public string? date { get; set; }

    public string? time { get; set; }
    public string? url { get; set; }

    [BsonElement("circuit")]
    public NestedCircuit? circuit { get; set; } // Nested class was not working


}



