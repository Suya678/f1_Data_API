using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;

public class NestedCircuit
{

    [BsonElement("id")]
    [JsonPropertyName("id")]
    public int raceID { get; set; }

    [BsonElement("ref")]
    [JsonPropertyName("ref")]
    public string? circuitRef { get; set; }

    [BsonElement("name")]
    public string? name { get; set; }

    [BsonElement("location")]
    public string? location { get; set; }

    [BsonElement("country")]
    public string? country { get; set; }

    [BsonElement("lat")]
    public double? lat { get; set; }

    [BsonElement("lng")]
    public double? lng { get; set; }

    [BsonElement("url")]
    public string? url { get; set; }
}

public class NestedDriver
{

    [BsonElement("id")]
    [JsonPropertyName("id")]
    public int driverId { get; set; }

    [BsonElement("ref")]
    [JsonPropertyName("ref")]
    public string? driverRef { get; set; }

    [BsonElement("forename")]
    public string? forename { get; set; }

    [BsonElement("surname")]
    public string? surname { get; set; }

    [BsonElement("nationality")]
    public string? nationality { get; set; }
}


public class NestedRace
{

    [BsonElement("id")]
    [JsonPropertyName("id")]
    public int raceID { get; set; }

    [BsonElement("year")]
    public int year { get; set; }

    [BsonElement("round")]
    public int round { get; set; }

    [BsonElement("name")]
    public string? name { get; set; }

    [BsonElement("date")]
    public string? date { get; set; }
}



public class NestedConstructor
{

    [BsonElement("id")]
    [JsonPropertyName("id")]
    public int constructorId { get; set; }

    [BsonElement("ref")]
    [JsonPropertyName("ref")]
    public string? constructorRef { get; set; }

    [BsonElement("name")]
    public string? name { get; set; }

    [BsonElement("nationality")]
    public string? nationality { get; set; }
}
