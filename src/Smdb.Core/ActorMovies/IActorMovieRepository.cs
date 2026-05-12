namespace Smdb.Core.ActorMovies;

using Shared.Http;
using Smdb.Core.Movies;
using Smdb.Core.Actors;

public interface IActorMovieRepository
{
    Task<PagedResult<ActorMovie>?> ReadActorMovies(int page, int size);
    Task<ActorMovie?>              CreateActorMovie(ActorMovie newActorMovie);
    Task<ActorMovie?>              ReadActorMovie(int id);
    Task<ActorMovie?>              UpdateActorMovie(int id, ActorMovie newData);
    Task<ActorMovie?>              DeleteActorMovie(int id);
    Task<PagedResult<Movie>?>      ReadMoviesByActor(int actorId, int page, int size);
    Task<PagedResult<Actor>?>      ReadActorsByMovie(int movieId, int page, int size);
}
