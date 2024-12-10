namespace ConverterAPI.DB.Session;

public record Session()
{
    public int sessionid { get; set; }
    public int userid { get; set; }
    public DateTime datetime { get; set; }
    public int conversionsamount { get; set; }
    public bool active { get; set; }
}