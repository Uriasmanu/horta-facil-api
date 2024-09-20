﻿using horta_facil_api.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace horta_facil_api.Data
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext()
        {
            // String de conexão com MongoDB Atlas
            var client = new MongoClient("mongodb+srv://maanoelaurias:eiOBjzDkGXKfMp1x@cluster0.vergn.mongodb.net/?retryWrites=true&w=majority&appName=Cluster0");

            // Selecionar o banco de dados
            _database = client.GetDatabase("horta_facil_db");
        }

        // Método para obter a coleção de 'Login'
        public IMongoCollection<Login> Logins
        {
            get
            {
                return _database.GetCollection<Login>("Logins");
            }
        }

    }

}