namespace Smdb.Core.Users;

using Shared.Http;
using System.Net;

public class DefaultUserService : IUserService
{
    private IUserRepository userRepository;

    public DefaultUserService(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public async Task<Result<PagedResult<User>>> ReadUsers(int page, int size)
    {
        if (page < 1)
            return new Result<PagedResult<User>>(
                new Exception("Page must be >= 1."), (int)HttpStatusCode.BadRequest);
        if (size < 1)
            return new Result<PagedResult<User>>(
                new Exception("Page size must be >= 1."), (int)HttpStatusCode.BadRequest);

        var result = await userRepository.ReadUsers(page, size);
        return result == null
            ? new Result<PagedResult<User>>(
                new Exception("Could not read users."), (int)HttpStatusCode.NotFound)
            : new Result<PagedResult<User>>(result, (int)HttpStatusCode.OK);
    }

    public async Task<Result<User>> CreateUser(User newUser)
    {
        var validation = ValidateUser(newUser);
        if (validation != null) return validation;

        // Check for duplicate username
        var existing = await userRepository.ReadUserByUsername(newUser.Username);
        if (existing != null)
            return new Result<User>(
                new Exception($"Username '{newUser.Username}' is already taken."),
                (int)HttpStatusCode.Conflict);

        var user = await userRepository.CreateUser(newUser);
        return user == null
            ? new Result<User>(
                new Exception("Could not create user."),
                (int)HttpStatusCode.InternalServerError)
            : new Result<User>(user, (int)HttpStatusCode.Created);
    }

    public async Task<Result<User>> ReadUser(int id)
    {
        var user = await userRepository.ReadUser(id);
        return user == null
            ? new Result<User>(
                new Exception($"Could not read user with id {id}."),
                (int)HttpStatusCode.NotFound)
            : new Result<User>(user, (int)HttpStatusCode.OK);
    }

    public async Task<Result<User>> UpdateUser(int id, User newData)
    {
        var validation = ValidateUser(newData, requirePassword: false);
        if (validation != null) return validation;

        var user = await userRepository.UpdateUser(id, newData);
        return user == null
            ? new Result<User>(
                new Exception($"Could not update user with id {id}."),
                (int)HttpStatusCode.NotFound)
            : new Result<User>(user, (int)HttpStatusCode.OK);
    }

    public async Task<Result<User>> DeleteUser(int id)
    {
        var user = await userRepository.DeleteUser(id);
        return user == null
            ? new Result<User>(
                new Exception($"Could not delete user with id {id}."),
                (int)HttpStatusCode.NotFound)
            : new Result<User>(user, (int)HttpStatusCode.OK);
    }

    public async Task<Result<User>> Login(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            return new Result<User>(
                new Exception("Username and password are required."),
                (int)HttpStatusCode.BadRequest);

        var user = await userRepository.ReadUserByUsername(username);
        if (user == null || user.Password != password)
            return new Result<User>(
                new Exception("Invalid username or password."),
                (int)HttpStatusCode.Unauthorized);

        return new Result<User>(user, (int)HttpStatusCode.OK);
    }

    private static Result<User>? ValidateUser(User? userData, bool requirePassword = true)
    {
        if (userData is null)
            return new Result<User>(
                new Exception("User payload is required."), (int)HttpStatusCode.BadRequest);

        if (string.IsNullOrWhiteSpace(userData.Username))
            return new Result<User>(
                new Exception("Username is required."), (int)HttpStatusCode.BadRequest);

        if (requirePassword && string.IsNullOrWhiteSpace(userData.Password))
            return new Result<User>(
                new Exception("Password is required."), (int)HttpStatusCode.BadRequest);

        if (userData.Role != "admin" && userData.Role != "user")
            return new Result<User>(
                new Exception("Role must be 'admin' or 'user'."), (int)HttpStatusCode.BadRequest);

        return null;
    }
}
