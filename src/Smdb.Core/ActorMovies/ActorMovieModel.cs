namespace Smdb.Core.ActorMovies;

public class ActorMovie
{
    public int    Id      { get; set; }
    public int    ActorId { get; set; }
    public int    MovieId { get; set; }
    public string Role    { get; set; }

    public ActorMovie(int id, int actorId, int movieId, string role)
    {
        Id      = id;
        ActorId = actorId;
        MovieId = movieId;
        Role    = role;
    }

    public override string ToString()
        => $"ActorMovie[Id={Id}, ActorId={ActorId}, MovieId={MovieId}, Role={Role}]";
}
