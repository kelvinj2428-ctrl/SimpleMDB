namespace Smdb.Core.Actors;

using Shared.Http;
using System.Net;

public class DefaultActorService : IActorService
{
    private IActorRepository actorRepository;

    public DefaultActorService(IActorRepository actorRepository)
    {
        this.actorRepository = actorRepository;
    }

    public async Task<Result<PagedResult<Actor>>> ReadActors(int page, int size)
    {
        if (page < 1)
            return new Result<PagedResult<Actor>>(
                new Exception("Page must be >= 1."), (int)HttpStatusCode.BadRequest);

        if (size < 1)
            return new Result<PagedResult<Actor>>(
                new Exception("Page size must be >= 1."), (int)HttpStatusCode.BadRequest);

        var pagedResult = await actorRepository.ReadActors(page, size);
        return pagedResult == null
            ? new Result<PagedResult<Actor>>(
                new Exception($"Could not read actors from page {page} and size {size}."),
                (int)HttpStatusCode.NotFound)
            : new Result<PagedResult<Actor>>(pagedResult, (int)HttpStatusCode.OK);
    }

    public async Task<Result<Actor>> CreateActor(Actor newActor)
    {
        var validation = ValidateActor(newActor);
        if (validation != null) return validation;

        var actor = await actorRepository.CreateActor(newActor);
        return actor == null
            ? new Result<Actor>(
                new Exception($"Could not create actor {newActor}."),
                (int)HttpStatusCode.InternalServerError)
            : new Result<Actor>(actor, (int)HttpStatusCode.Created);
    }

    public async Task<Result<Actor>> ReadActor(int id)
    {
        var actor = await actorRepository.ReadActor(id);
        return actor == null
            ? new Result<Actor>(
                new Exception($"Could not read actor with id {id}."),
                (int)HttpStatusCode.NotFound)
            : new Result<Actor>(actor, (int)HttpStatusCode.OK);
    }

    public async Task<Result<Actor>> UpdateActor(int id, Actor newData)
    {
        var validation = ValidateActor(newData);
        if (validation != null) return validation;

        var actor = await actorRepository.UpdateActor(id, newData);
        return actor == null
            ? new Result<Actor>(
                new Exception($"Could not update actor with id {id}."),
                (int)HttpStatusCode.NotFound)
            : new Result<Actor>(actor, (int)HttpStatusCode.OK);
    }

    public async Task<Result<Actor>> DeleteActor(int id)
    {
        var actor = await actorRepository.DeleteActor(id);
        return actor == null
            ? new Result<Actor>(
                new Exception($"Could not delete actor with id {id}."),
                (int)HttpStatusCode.NotFound)
            : new Result<Actor>(actor, (int)HttpStatusCode.OK);
    }

    private static Result<Actor>? ValidateActor(Actor? actorData)
    {
        if (actorData is null)
            return new Result<Actor>(
                new Exception("Actor payload is required."), (int)HttpStatusCode.BadRequest);

        if (string.IsNullOrWhiteSpace(actorData.FirstName))
            return new Result<Actor>(
                new Exception("First name is required."), (int)HttpStatusCode.BadRequest);

        if (string.IsNullOrWhiteSpace(actorData.LastName))
            return new Result<Actor>(
                new Exception("Last name is required."), (int)HttpStatusCode.BadRequest);

        if (actorData.Rating < 0 || actorData.Rating > 10)
            return new Result<Actor>(
                new Exception("Rating must be between 0 and 10."), (int)HttpStatusCode.BadRequest);

        return null;
    }
}
