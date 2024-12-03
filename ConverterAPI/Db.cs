namespace ConverterAPI;

public record User
{
    public int Id { get; set; }
    public string? Login { get; set; }
    public string? Password { get; set; }
    public bool Premium { get; set; }
}

public class UserDb
{
    private static List<User> _users = new List<User>();

    public static List<User> GetUsers()
    {
        return _users;
    }

    public static User? GetUser(int id)
    {
        return _users.SingleOrDefault(user => user.Id == id);
    }

    public static User CreateUser(User user)
    {
        _users.Add(user);
        return user;
    }

    public static User UpdateUser(User update)
    {
        _users = _users.Select(user =>
        {
            if (user.Id == update.Id)
            {
                user.Login = update.Login;
                user.Password = update.Password;
                user.Premium = update.Premium;
            }
            return user;
        }).ToList();
        return update;
    }

    public static void RemoveUser(int id)
    {
        _users = _users.FindAll(user => user.Id != id).ToList();
    }
}