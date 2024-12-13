namespace cdcApi.Models;
public class Productos
{
  public int id { get; set; }
  public string name { get; set; }
  public string? description { get; set; }
  public decimal? price { get; set; }
  public DateTime? created_at { get; set; }



}