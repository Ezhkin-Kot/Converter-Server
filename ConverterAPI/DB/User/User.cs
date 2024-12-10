namespace ConverterAPI.DB.User;

public record User
{
    public int Id { get; set; }
    public string? Login { get; set; }
    public string? Password { get; set; }
    public bool Premium { get; set; }
}