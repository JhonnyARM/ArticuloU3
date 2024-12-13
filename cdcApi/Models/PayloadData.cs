using System.Text.Json.Serialization;

public class PayloadData
{
  public AfterData Before { get; set; }
  public AfterData After { get; set; }
  public SourceData Source { get; set; }
  public string Op { get; set; }
}

public class AfterData
{
  [JsonPropertyName("id")]
  public int Id { get; set; }

  [JsonPropertyName("name")]
  public string Name { get; set; }

  [JsonPropertyName("description")]
  public string Description { get; set; }

  [JsonPropertyName("price")]
  public int Price { get; set; }

  [JsonPropertyName("created_at")]
  public DateTime CreatedAt { get; set; }
}



public class SourceData
{
  public string Version { get; set; }
  public string Connector { get; set; }
  public string Db { get; set; }
  public string Table { get; set; }
  public long Pos { get; set; }
}

public class KafkaMessage
{
  public PayloadData Payload { get; set; }
}
