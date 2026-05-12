namespace Smdb.Core.Users;

public class User
{
    public int    Id       { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Role     { get; set; }  // "admin" | "user"

    public User(int id, string username, string password, string role)
    {
        Id       = id;
        Username = username;
        Password = password;
        Role     = role;
    }

    public override string ToString()
        => $"User[Id={Id}, Username={Username}, Role={Role}]";
}
