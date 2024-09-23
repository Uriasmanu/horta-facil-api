using MongoDB.Bson.Serialization.Attributes;

namespace horta_facil_api.Models
{
    public class Login
    {
        [BsonId]
        public Guid Id { get; set; }

        [BsonElement("nome")]
        public string Username { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }

        [BsonElement("senha")]
        public string Senha { get; set; }
    }
}
