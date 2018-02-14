using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Atmakur.Testing.Core.Utilities
{
    public class MongoDataProvider : IDataProvider, IDisposable
    {

        private readonly MongoClient _dbConn = null;
        private readonly IMongoDatabase _db = null;

        /// <summary>
        /// Constructor for Mongo Data Provider, Creates an instance of the class
        /// </summary>
        /// <param name="connectionString">Connection string. Ex: mongodb://127.0.0.1:27017</param>
        /// <param name="dbName">Database name</param>
        public MongoDataProvider(string connectionString, string dbName)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentNullException("Connection String cannot be NULL/Empty!");
            if (string.IsNullOrWhiteSpace(dbName))
                throw new ArgumentNullException("Database Name cannot be NULL/Empty!");

            _dbConn = new MongoClient(connectionString);
            _db = _dbConn.GetDatabase(dbName);
        }

        /// <summary>
        /// Constructor for Mongo Data Provider, Creates an instance of the class
        /// </summary>
        /// <param name="connectionString">Connection string, including databasename Ex: mongodb://127.0.0.1:27017/MyDatabase</param>
        public MongoDataProvider(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentNullException("Connection String cannot be NULL/Empty!");
            var databaseName = MongoUrl.Create(connectionString).DatabaseName;
            _dbConn = new MongoClient(connectionString);
            _db = _dbConn.GetDatabase(databaseName);
        }

        private BsonDocument GetQuery(string jsonQuery)
        {
            if (string.IsNullOrWhiteSpace(jsonQuery))
                return new BsonDocument();
            else
                return BsonSerializer.Deserialize<BsonDocument>(jsonQuery);
        }

        /// <summary>
        /// Returns all the data in the provided collection
        /// </summary>
        /// <typeparam name="T">Data type</typeparam>
        /// <param name="collectionName">Collection name</param>
        /// <param name="jsonQuery">Query to filter</param>
        /// <returns>A list of the type provided</returns>
        public List<T> GetAllData<T>(string collectionName, string jsonQuery = null)
        {
            var coll = _db.GetCollection<T>(collectionName);
            return coll.Find(GetQuery(jsonQuery)).ToList();
        }

        /// <summary>
        /// Returns all the data in the collection with limit
        /// </summary>
        /// <typeparam name="T">Data type</typeparam>
        /// <param name="collectionName">Collection name</param>
        /// <param name="limit">Limit the results</param>
        /// <param name="jsonQuery">Query to filter</param>
        /// <returns>A list of the type provided</returns>
        public List<T> GetAllData<T>(string collectionName, int limit, string jsonQuery = null)
        {
            var coll = _db.GetCollection<T>(collectionName);
            return coll.Find(GetQuery(jsonQuery)).Limit(limit).ToList();
        }

        /// <summary>
        /// Gets a list of a specific field
        /// </summary>
        /// <typeparam name="T">Data type</typeparam>
        /// <param name="collectionName">Collection name</param>
        /// <param name="fieldName">Field name in the document</param>
        /// <param name="jsonQuery">Query to filter</param>
        /// <returns>A list of field in string format</returns>
        public List<string> GetField<T>(string collectionName, string fieldName, string jsonQuery = null)
        {
            List<string> resultList = new List<string>();

            var coll = _db.GetCollection<T>(collectionName);
            var result = coll.Find(GetQuery(jsonQuery)).Project(new BsonDocument(fieldName, 1).Add("_id", 0)).ToList();
            result.ForEach(x => resultList.Add(x[fieldName].ToString()));

            return resultList;
        }

        /// <summary>
        /// Gets a single document
        /// </summary>
        /// <typeparam name="T">Data type</typeparam>
        /// <param name="collectionName">Collection name</param>
        /// <param name="jsonQuery">Query to filter</param>
        /// <returns>A single document which matches the criteria</returns>
        public T GetSingleDocument<T>(string collectionName, string jsonQuery = null)
        {
            var coll = _db.GetCollection<T>(collectionName);
            return coll.Find(GetQuery(jsonQuery)).FirstOrDefault();
        }

        /// <summary>
        /// Gets a single document by Id field
        /// </summary>
        /// <typeparam name="T">Data type</typeparam>
        /// <param name="collectionName">Collection name</param>
        /// <param name="id">Object Id of the document</param>
        /// <returns>A single document which matches the criteria</returns>
        public T GetSingleDocumentById<T>(string collectionName, ObjectId id)
        {
            var coll = _db.GetCollection<T>(collectionName);
            return coll.Find(new BsonDocument("_id", id)).FirstOrDefault();
        }

        /// <summary>
        /// Deletes a document
        /// </summary>
        /// <typeparam name="T">Data type</typeparam>
        /// <param name="collectionName">Collection name</param>
        /// <param name="id">Object id of the document</param>
        /// <returns>Status of deletion, true/false</returns>
        public bool DeleteSingleDocumentById<T>(string collectionName, ObjectId id)
        {
            var coll = _db.GetCollection<T>(collectionName);
            var result = coll.DeleteOne(new BsonDocument("_id", id));
            return (result.IsAcknowledged && result.DeletedCount == 1) ? true : false;
        }

        /// <summary>
        /// Upsert a single document, updates if available else inserts a new document
        /// </summary>
        /// <typeparam name="T">Data type</typeparam>
        /// <param name="collectionName">Collection name</param>
        /// <param name="id">Object Id of the docuemnt</param>
        /// <param name="updatedItem">The updated document</param>
        /// <returns>Status of insert/update, true/false</returns>
        public bool UpsertSingleDocumentById<T>(string collectionName, ObjectId id, T updatedItem)
        {
            var coll = _db.GetCollection<T>(collectionName);
            var result = coll.ReplaceOne(new BsonDocument("_id", id), updatedItem, new UpdateOptions { IsUpsert = true });
            return (result.IsAcknowledged && result.ModifiedCount == 1) || (result.UpsertedId != null && result.UpsertedId != ObjectId.Empty) ? true : false;
        }

        /// <summary>
        /// Inserts a single document
        /// </summary>
        /// <typeparam name="T">Data type</typeparam>
        /// <param name="collectionName">Collection name</param>
        /// <param name="insertItem">The item to insert</param>
        public void InsertSingleDocument<T>(string collectionName, T insertItem)
        {
            var coll = _db.GetCollection<T>(collectionName);
            coll.InsertOne(insertItem);
        }

        /// <summary>
        /// Disposes the instance.
        /// </summary>
        public void Dispose()
        {
            this.Dispose();
        }
    }
}
