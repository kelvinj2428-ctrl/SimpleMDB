namespace Smdb.Core.ActorMovies;

using Shared.Http;
using Smdb.Core.Movies;
using Smdb.Core.Actors;
using System.Net;

public class DefaultActorMovieService : IActorMovieService
{
    private IActorMovieRepository actorMovieRepository;

    public DefaultActorMovieService(IActorMovieRepository actorMovieRepository)
    {
        this.actorMovieRepository = actorMovieRepository;
    }

    public async Task<Result<PagedResult<ActorMovie>>> ReadActorMovies(int page, int size)
    {
        if (page < 1)
            return new Result<PagedResult<ActorMovie>>(
                new Exception("Page must be >= 1."), (int)HttpStatusCode.BadRequest);
        if (size < 1)
            return new Result<PagedResult<ActorMovie>>(
                new Exception("Page size must be >= 1."), (int)HttpStatusCode.BadRequest);

        var result = await actorMovieRepository.ReadActorMovies(page, size);
        return result == null
            ? new Result<PagedResult<ActorMovie>>(
                new Exception("Could not read actor-movies."), (int)HttpStatusCode.NotFound)
            : new Result<PagedResult<ActorMovie>>(result, (int)HttpStatusCode.OK);
    }

    public async Task<Result<ActorMovie>> CreateActorMovie(ActorMovie newActorMovie)
    {
        var validation = ValidateActorMovie(newActorMovie);
        if (validation != null) return validation;

        var actorMovie = await actorMovieRepository.CreateActorMovie(newActorMovie);
        return actorMovie == null
            ? new Result<ActorMovie>(
                new Exception("Could not create actor-movie."),
                (int)HttpStatusCode.InternalServerError)
            : new Result<ActorMovie>(actorMovie, (int)HttpStatusCode.Created);
    }

    public async Task<Result<ActorMovie>> ReadActorMovie(int id)
    {
        var actorMovie = await actorMovieRepository.ReadActorMovie(id);
        return actorMovie == null
            ? new Result<ActorMovie>(
                new Exception($"Could not read actor-movie with id {id}."),
                (int)HttpStatusCode.NotFound)
            : new Result<ActorMovie>(actorMovie, (int)HttpStatusCode.OK);
    }

    public async Task<Result<ActorMovie>> UpdateActorMovie(int id, ActorMovie newData)
    {
        var validation = ValidateActorMovie(newData);
        if (validation != null) return validation;

        var actorMovie = await actorMovieRepository.UpdateActorMovie(id, newData);
        return actorMovie == null
            ? new Result<ActorMovie>(
                new Exception($"Could not update actor-movie with id {id}."),
                (int)HttpStatusCode.NotFound)
            : new Result<ActorMovie>(actorMovie, (int)HttpStatusCode.OK);
    }

    public async Task<Result<ActorMovie>> DeleteActorMovie(int id)
    {
        var actorMovie = await actorMovieRepository.DeleteActorMovie(id);
        return actorMovie == null
            ? new Result<ActorMovie>(
                new Exception($"Could not delete actor-movie with id {id}."),
                (int)HttpStatusCode.NotFound)
            : new Result<ActorMovie>(actorMovie, (int)HttpStatusCode.OK);
    }

    public async Task<Result<PagedResult<Movie>>> ReadMoviesByActor(int actorId, int page, int size)
    {
        if (page < 1)
            return new Result<PagedResult<Movie>>(
                new Exception("Page must be >= 1."), (int)HttpStatusCode.BadRequest);
        if (size < 1)
            return new Result<PagedResult<Movie>>(
                new Exception("Page size must be >= 1."), (int)HttpStatusCode.BadRequest);

        var result = await actorMovieRepository.ReadMoviesByActor(actorId, page, size);
        return result == null
            ? new Result<PagedResult<Movie>>(
                new Exception($"Could not read movies for actor {actorId}."),
                (int)HttpStatusCode.NotFound)
            : new Result<PagedResult<Movie>>(result, (int)HttpStatusCode.OK);
    }

    public async Task<Result<PagedResult<Actor>>> ReadActorsByMovie(int movieId, int page, int size)
    {
        if (page < 1)
            return new Result<PagedResult<Actor>>(
                new Exception("Page must be >= 1."), (int)HttpStatusCode.BadRequest);
        if (size < 1)
            return new Result<PagedResult<Actor>>(
                new Exception("Page size must be >= 1."), (int)HttpStatusCode.BadRequest);

        var result = await actorMovieRepository.ReadActorsByMovie(movieId, page, size);
        return result == null
            ? new Result<PagedResult<Actor>>(
                new Exception($"Could not read actors for movie {movieId}."),
                (int)HttpStatusCode.NotFound)
            : new Result<PagedResult<Actor>>(result, (int)HttpStatusCode.OK);
    }

    private static Result<ActorMovie>? ValidateActorMovie(ActorMovie? data)
    {
        if (data is null)
            return new Result<ActorMovie>(
                new Exception("ActorMovie payload is required."), (int)HttpStatusCode.BadRequest);

        if (data.ActorId <= 0)
            return new Result<ActorMovie>(
                new Exception("ActorId must be a positive integer."), (int)HttpStatusCode.BadRequest);

        if (data.MovieId <= 0)
            return new Result<ActorMovie>(
                new Exception("MovieId must be a positive integer."), (int)HttpStatusCode.BadRequest);

        if (string.IsNullOrWhiteSpace(data.Role))
            return new Result<ActorMovie>(
                new Exception("Role is required."), (int)HttpStatusCode.BadRequest);

        return null;
    }
}
