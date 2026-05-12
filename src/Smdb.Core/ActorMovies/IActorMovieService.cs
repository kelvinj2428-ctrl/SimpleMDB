namespace Smdb.Core.ActorMovies;

using Shared.Http;
using Smdb.Core.Movies;
using Smdb.Core.Actors;

public interface IActorMovieService
{
    Task<Result<PagedResult<ActorMovie>>> ReadActorMovies(int page, int size);
    Task<Result<ActorMovie>>              CreateActorMovie(ActorMovie actorMovie);
    Task<Result<ActorMovie>>              ReadActorMovie(int id);
    Task<Result<ActorMovie>>              UpdateActorMovie(int id, ActorMovie newData);
    Task<Result<ActorMovie>>              DeleteActorMovie(int id);
    Task<Result<PagedResult<Movie>>>      ReadMoviesByActor(int actorId, int page, int size);
    Task<Result<PagedResult<Actor>>>      ReadActorsByMovie(int movieId, int page, int size);
}
