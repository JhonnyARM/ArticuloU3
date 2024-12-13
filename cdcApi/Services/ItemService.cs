using MongoDB.Driver;
using MongoDB.Bson;
using cdcApi.Models;
using Microsoft.Extensions.Logging;

namespace cdcApi.Services;

public class ItemService
{
  private readonly IMongoCollection<Item> _itemsCollection;
  private readonly IMongoCollection<Productos> _productosCollection;
  private readonly IMongoCollection<AfterData> _afterDataCollection;
  private readonly IMongoCollection<BsonDocument> _afterDatasCollection;
  public ItemService(IConfiguration configuration)
  {
    var mongoClient = new MongoClient(configuration["MongoDB:ConnectionString"]);
    var mongoDatabase = mongoClient.GetDatabase(configuration["MongoDB:DatabaseName"]);

    _itemsCollection = mongoDatabase.GetCollection<Item>("Items");
    _afterDataCollection = mongoDatabase.GetCollection<AfterData>("AfterData");
    _afterDatasCollection = mongoDatabase.GetCollection<BsonDocument>("BsonDocument");

  }

  public async Task<List<Item>> GetAsync() =>
      await _itemsCollection.Find(_ => true).ToListAsync();
  public async Task<List<AfterData>> GetAfter() =>
    await _afterDataCollection.Find(_ => true).ToListAsync();

  public async Task<List<BsonDocument>> GetBsonDocument()
  {
    // Aquí accedemos al _id correctamente como ObjectId
    var bsonDocuments = await _afterDatasCollection.Find(_ => true).ToListAsync();
    return bsonDocuments.Select(doc =>
    {
      // Correctamente acceder al _id como ObjectId
      var document = doc;
      var objectId = document["_id"].AsObjectId;  // Acceder al ObjectId
      return document;
    }).ToList();
  }



  public async Task<Item?> GetAsync(string id) =>
      await _itemsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

  public async Task CreateAsync(Item newItem) =>
      await _itemsCollection.InsertOneAsync(newItem);

  public async Task UpdateAsync(string id, Item updatedItem) =>
      await _itemsCollection.ReplaceOneAsync(x => x.Id == id, updatedItem);

  public async Task RemoveAsync(string id) =>
      await _itemsCollection.DeleteOneAsync(x => x.Id == id);

  public async Task InsertIntoMongoDB(AfterData newItems) =>
    await _afterDataCollection.InsertOneAsync(newItems);

  public async Task InsertOrUpdateIntoMongoDB(AfterData newItem)
  {
    var filter = Builders<AfterData>.Filter.Eq(item => item.Id, newItem.Id);

    var update = Builders<AfterData>.Update
        .Set(item => item.Name, newItem.Name)
        .Set(item => item.Description, newItem.Description)
        .Set(item => item.Price, newItem.Price)
        .Set(item => item.CreatedAt, newItem.CreatedAt);

    await _afterDataCollection.UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = true });
  }
  public async Task InsertRawDataAsync(string rawDataJson)
  {
    try
    {
      // Convertir el rawDataJson a BsonDocument
      var bsonDocument = BsonDocument.Parse(rawDataJson);

      // Insertar el BsonDocument en la colección
      await _afterDatasCollection.InsertOneAsync(bsonDocument);
    }
    catch (Exception ex)
    {
      // Si hay un error al insertar, lo puedes manejar aquí
      // (aunque no sea necesario si estás omitiendo el logger)
    }
  }





}
