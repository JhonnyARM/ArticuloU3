using System;
using System.Text.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using cdcApi.Models;
using cdcApi.Services;

public class KafkaConsumerService : BackgroundService
{
  private readonly ILogger<KafkaConsumerService> _logger;
  private readonly ConsumerConfig _config;

  private readonly ItemService _itemService;



  public KafkaConsumerService(ILogger<KafkaConsumerService> logger, ItemService itemService)
  {
    _logger = logger;
    _itemService = itemService;
    _config = new ConsumerConfig
    {
      BootstrapServers = "localhost:9093",
      GroupId = "inventory-.inventory.products",
      AutoOffsetReset = AutoOffsetReset.Earliest
    };
  }

  protected override async Task ExecuteAsync(CancellationToken stoppingToken)
  {
    // Ejecutamos la tarea asincrónica de manera separada para evitar bloquear el hilo principal
    await Task.Run(async () =>
    {
      using var consumer = new ConsumerBuilder<Ignore, string>(_config).Build();
      consumer.Subscribe("inventory-.inventory.products");

      _logger.LogInformation("Kafka Consumer iniciado y escuchando el tema 'asdasdasd'.");

      try
      {
        while (!stoppingToken.IsCancellationRequested)
        {
          var result = consumer.Consume(stoppingToken);
          var message = result.Message.Value;

          _logger.LogInformation($"Mensaje recibido: {message}");

          // Procesar el mensaje
          ProcessMessage(message);
        }
      }
      catch (OperationCanceledException)
      {
        _logger.LogInformation("Consumo cancelado.");
      }
      catch (ConsumeException ex)
      {
        _logger.LogError($"Error al consumir: {ex.Error.Reason}");
      }
      finally
      {
        consumer.Close();
      }
    }, stoppingToken);
  }

  public void ProcessMessage(string message)
  {
    try
    {
      var jsonDocument = JsonDocument.Parse(message);
      var payload = jsonDocument.RootElement.GetProperty("payload");

      if (payload.TryGetProperty("after", out var afterDataElement) && afterDataElement.ValueKind != JsonValueKind.Null)
      {
        var afterDataJson = afterDataElement.GetRawText();
        var afterData = JsonSerializer.Deserialize<AfterData>(afterDataJson);

        if (afterData != null)
        {
          _logger.LogInformation($"ID: {afterData.Id}");
          _logger.LogInformation($"Nombre: {afterData.Name}");
          _logger.LogInformation($"Descripción: {afterData.Description}");
          _logger.LogInformation($"Precio Base64: {afterData.Price}");
          _logger.LogInformation($"Fecha de Creación: {afterData.CreatedAt}");



          // Llamar al método para insertar o actualizar en MongoDB
          _itemService.InsertOrUpdateIntoMongoDB(afterData);
        }
        else
        {
          _logger.LogWarning("El objeto 'afterData' deserializado es nulo.");
        }
      }
      else
      {
        _logger.LogWarning("El campo 'after' está vacío o no existe.");
      }
    }
    catch (Exception ex)
    {
      _logger.LogError($"Error al procesar el mensaje: {ex.Message}");
    }
  }
















}
