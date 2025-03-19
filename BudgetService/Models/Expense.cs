using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BudgetService.Models;

public class Expense {
    [BsonId]
    [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
    public required string Id {get; set; }
    [BsonElement("_username"), BsonRepresentation(BsonType.String)]
    public required string Username {get; set;}
    [BsonElement("_userid"), BsonRepresentation(BsonType.Int64)]
    public required long UserId {get; set;}
    [BsonElement("_description"), BsonRepresentation(BsonType.String)]
    public required string Description {get; set;}
    [BsonElement("_amount"), BsonRepresentation(BsonType.Int64)]
    public required long Amount {get; set;}
    [BsonElement("_budget")]
    public required Budget Budget { get; set; }
}