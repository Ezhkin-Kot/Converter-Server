namespace ConverterAPI.DB;

public record UserDb
{
    public int Id { get; set; }
    public string? Login { get; set; }
    public string? Password { get; set; }
    public bool Premium { get; set; }
}

public class User
{
    private static List<UserDb> _users = new List<UserDb>();

    public static List<UserDb> GetUsers()
    {
        return _users;
    }

    public static UserDb? GetUser(int id)
    {
        return _users.SingleOrDefault(user => user.Id == id);
    }

    public static UserDb CreateUser(UserDb userDb)
    {
        _users.Add(userDb);
        return userDb;
    }

    public static UserDb UpdateUser(UserDb update)
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