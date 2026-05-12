namespace Smdb.Core.Movies;

using Shared.Http;

public interface IMovieService
{
    Task<Result<PagedResult<Movie>>> ReadMovies(int page, int size);
    Task<Result<Movie>>              CreateMovie(Movie movie);
    Task<Result<Movie>>              ReadMovie(int id);
    Task<Result<Movie>>              UpdateMovie(int id, Movie newData);
    Task<Result<Movie>>              DeleteMovie(int id);
}
