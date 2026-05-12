namespace Smdb.Core.Actors;

using Shared.Http;
using Smdb.Core.Db;

public class MemoryActorRepository : IActorRepository
{
    private MemoryDatabase db;

    public MemoryActorRepository(MemoryDatabase db)
    {
        this.db = db;
    }

    public async Task<PagedResult<Actor>?> ReadActors(int page, int size)
    {
        int totalCount = db.Actors.Count;
        int start  = Math.Clamp((page - 1) * size, 0, totalCount);
        int length = Math.Clamp(size, 0, totalCount - start);
        var values = db.Actors.Slice(start, length);
        return await Task.FromResult(new PagedResult<Actor>(totalCount, values));
    }

    public async Task<Actor?> CreateActor(Actor newActor)
    {
        newActor.Id = db.NextActorId();
        db.Actors.Add(newActor);
        return await Task.FromResult(newActor);
    }

    public async Task<Actor?> ReadActor(int id)
    {
        Actor? result = db.Actors.FirstOrDefault(a => a.Id == id);
        return await Task.FromResult(result);
    }

    public async Task<Actor?> UpdateActor(int id, Actor newData)
    {
        Actor? result = db.Actors.FirstOrDefault(a => a.Id == id);
        if (result != null)
        {
            result.FirstName = newData.FirstName;
            result.LastName  = newData.LastName;
            result.Rating    = newData.Rating;
            result.Bio       = newData.Bio;
        }
        return await Task.FromResult(result);
    }

    public async Task<Actor?> DeleteActor(int id)
    {
        Actor? result = db.Actors.FirstOrDefault(a => a.Id == id);
        if (result != null) { db.Actors.Remove(result); }
        return await Task.FromResult(result);
    }
}
