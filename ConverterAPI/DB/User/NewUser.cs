namespace ConverterAPI.DB.User;

public record NewUser
{
    public string? Login { get; set; }
    public string? Password { get; set; }
}