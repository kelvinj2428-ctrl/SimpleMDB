namespace Smdb.Core.Actors;

using Shared.Http;

public interface IActorService
{
    Task<Result<PagedResult<Actor>>> ReadActors(int page, int size);
    Task<Result<Actor>>              CreateActor(Actor actor);
    Task<Result<Actor>>              ReadActor(int id);
    Task<Result<Actor>>              UpdateActor(int id, Actor newData);
    Task<Result<Actor>>              DeleteActor(int id);
}
