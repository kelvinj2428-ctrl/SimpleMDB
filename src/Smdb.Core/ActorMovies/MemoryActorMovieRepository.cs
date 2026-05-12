namespace Smdb.Core.ActorMovies;

using Shared.Http;
using Smdb.Core.Db;
using Smdb.Core.Movies;
using Smdb.Core.Actors;

public class MemoryActorMovieRepository : IActorMovieRepository
{
    private MemoryDatabase db;

    public MemoryActorMovieRepository(MemoryDatabase db)
    {
        this.db = db;
    }

    public async Task<PagedResult<ActorMovie>?> ReadActorMovies(int page, int size)
    {
        int totalCount = db.ActorMovies.Count;
        int start  = Math.Clamp((page - 1) * size, 0, totalCount);
        int length = Math.Clamp(size, 0, totalCount - start);
        var values = db.ActorMovies.Slice(start, length);
        return await Task.FromResult(new PagedResult<ActorMovie>(totalCount, values));
    }

    public async Task<ActorMovie?> CreateActorMovie(ActorMovie newActorMovie)
    {
        newActorMovie.Id = db.NextActorMovieId();
        db.ActorMovies.Add(newActorMovie);
        return await Task.FromResult(newActorMovie);
    }

    public async Task<ActorMovie?> ReadActorMovie(int id)
    {
        ActorMovie? result = db.ActorMovies.FirstOrDefault(am => am.Id == id);
        return await Task.FromResult(result);
    }

    public async Task<ActorMovie?> UpdateActorMovie(int id, ActorMovie newData)
    {
        ActorMovie? result = db.ActorMovies.FirstOrDefault(am => am.Id == id);
        if (result != null)
        {
            result.ActorId = newData.ActorId;
            result.MovieId = newData.MovieId;
            result.Role    = newData.Role;
        }
        return await Task.FromResult(result);
    }

    public async Task<ActorMovie?> DeleteActorMovie(int id)
    {
        ActorMovie? result = db.ActorMovies.FirstOrDefault(am => am.Id == id);
        if (result != null) { db.ActorMovies.Remove(result); }
        return await Task.FromResult(result);
    }

    public async Task<PagedResult<Movie>?> ReadMoviesByActor(int actorId, int page, int size)
    {
        var movieIds = db.ActorMovies
            .Where(am => am.ActorId == actorId)
            .Select(am => am.MovieId)
            .ToHashSet();

        var allMovies = db.Movies.Where(m => movieIds.Contains(m.Id)).ToList();
        int totalCount = allMovies.Count;
        int start  = Math.Clamp((page - 1) * size, 0, totalCount);
        int length = Math.Clamp(size, 0, totalCount - start);
        var values = allMovies.Slice(start, length);
        return await Task.FromResult(new PagedResult<Movie>(totalCount, values));
    }

    public async Task<PagedResult<Actor>?> ReadActorsByMovie(int movieId, int page, int size)
    {
        var actorIds = db.ActorMovies
            .Where(am => am.MovieId == movieId)
            .Select(am => am.ActorId)
            .ToHashSet();

        var allActors = db.Actors.Where(a => actorIds.Contains(a.Id)).ToList();
        int totalCount = allActors.Count;
        int start  = Math.Clamp((page - 1) * size, 0, totalCount);
        int length = Math.Clamp(size, 0, totalCount - start);
        var values = allActors.Slice(start, length);
        return await Task.FromResult(new PagedResult<Actor>(totalCount, values));
    }
}
