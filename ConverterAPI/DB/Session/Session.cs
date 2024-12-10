namespace ConverterAPI.DB.Session;

public record Session()
{
    public int UserId { get; set; }
    public int Sessionid { get; set; }
    public DateTime Date { get; set; }
    public int ConversionsAmount { get; set; }
    public bool Active { get; set; }
}