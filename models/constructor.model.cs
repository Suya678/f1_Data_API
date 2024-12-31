using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.EntityFrameworkCore;

[Collection("constructors")]
public class Constructor
{
    [BsonId]
    public ObjectId _id;

    public int constructorId { get; set; }

    public string? constructorRef { get; set; }

    public string? name { get; set; }

    public string? nationality { get; set; }

    public string? url { get; set; }
}



