namespace Smdb.Core.Users;

using Shared.Http;

public interface IUserRepository
{
    Task<PagedResult<User>?> ReadUsers(int page, int size);
    Task<User?>              CreateUser(User newUser);
    Task<User?>              ReadUser(int id);
    Task<User?>              ReadUserByUsername(string username);
    Task<User?>              UpdateUser(int id, User newData);
    Task<User?>              DeleteUser(int id);
}
