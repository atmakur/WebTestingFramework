using MongoDB.Bson;
using System.Collections.Generic;

namespace Atmakur.Testing.Core.Utilities
{
    //
    // Summary:
    //     Interface for DataProvider
    public interface IDataProvider
    {
        //
        // Summary:
        //     Deletes a document
        //
        // Parameters:
        //   collectionName:
        //     Collection name
        //
        //   id:
        //     Object id of the document
        //
        // Type parameters:
        //   T:
        //     Data type
        //
        // Returns:
        //     Status of deletion, true/false
        bool DeleteSingleDocumentById<T>(string collectionName, ObjectId id);
        //
        // Summary:
        //     Returns all the data in the provided collection
        //
        // Parameters:
        //   collectionName:
        //     Collection name
        //
        //   jsonQuery:
        //     Query to filter
        //
        // Type parameters:
        //   T:
        //     Data type
        //
        // Returns:
        //     A list of the type provided
        List<T> GetAllData<T>(string collectionName, string jsonQuery = null);
        //
        // Summary:
        //     Returns all the data in the collection with limit
        //
        // Parameters:
        //   collectionName:
        //     Collection name
        //
        //   limit:
        //     Limit the results
        //
        //   jsonQuery:
        //     Query to filter
        //
        // Type parameters:
        //   T:
        //     Data type
        //
        // Returns:
        //     A list of the type provided
        List<T> GetAllData<T>(string collectionName, int limit, string jsonQuery = null);
        //
        // Summary:
        //     Gets a list of a specific field
        //
        // Parameters:
        //   collectionName:
        //     Collection name
        //
        //   fieldName:
        //     Field name in the document
        //
        //   jsonQuery:
        //     Query to filter
        //
        // Type parameters:
        //   T:
        //     Data type
        //
        // Returns:
        //     A list of field in string format
        List<string> GetField<T>(string collectionName, string fieldName, string jsonQuery = null);
        //
        // Summary:
        //     Gets a single document
        //
        // Parameters:
        //   collectionName:
        //     Collection name
        //
        //   jsonQuery:
        //     Query to filter
        //
        // Type parameters:
        //   T:
        //     Data type
        //
        // Returns:
        //     A single document which matches the criteria
        T GetSingleDocument<T>(string collectionName, string jsonQuery = null);
        //
        // Summary:
        //     Gets a single document by Id field
        //
        // Parameters:
        //   collectionName:
        //     Collection name
        //
        //   id:
        //     Object Id of the document
        //
        // Type parameters:
        //   T:
        //     Data type
        //
        // Returns:
        //     A single document which matches the criteria
        T GetSingleDocumentById<T>(string collectionName, ObjectId id);
        //
        // Summary:
        //     Inserts a single document
        //
        // Parameters:
        //   collectionName:
        //     Collection name
        //
        //   insertItem:
        //     The item to insert
        //
        // Type parameters:
        //   T:
        //     Data type
        void InsertSingleDocument<T>(string collectionName, T insertItem);
        //
        // Summary:
        //     Upsert a single document, updates if available else inserts a new document
        //
        // Parameters:
        //   collectionName:
        //     Collection name
        //
        //   id:
        //     Object Id of the docuemnt
        //
        //   updatedItem:
        //     The updated document
        //
        // Type parameters:
        //   T:
        //     Data type
        //
        // Returns:
        //     Status of insert/update, true/false
        bool UpsertSingleDocumentById<T>(string collectionName, ObjectId id, T updatedItem);
    }
}
