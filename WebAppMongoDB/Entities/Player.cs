using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebAppMongoDB.Entities
{
    public class Player
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string FullName { get; set; }
        public string Team { get; set; }
        public bool IsActive { get; set; }
        public int ShoeSize { get; set; }



    }
}
