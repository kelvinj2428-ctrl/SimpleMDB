namespace Smdb.Core.Users;

using Shared.Http;

public interface IUserService
{
    Task<Result<PagedResult<User>>> ReadUsers(int page, int size);
    Task<Result<User>>              CreateUser(User user);
    Task<Result<User>>              ReadUser(int id);
    Task<Result<User>>              UpdateUser(int id, User newData);
    Task<Result<User>>              DeleteUser(int id);
    Task<Result<User>>              Login(string username, string password);
}
