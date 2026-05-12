namespace Smdb.Api.ActorMovies;

using Shared.Http;

public class ActorMoviesRouter : HttpRouter
{
    public ActorMoviesRouter(ActorMoviesController actorMoviesController)
    {
        UseParametrizedRouteMatching();
        MapGet("/",    actorMoviesController.ReadActorMovies);
        MapPost("/",   HttpUtils.ReadRequestBodyAsText, actorMoviesController.CreateActorMovie);
        MapGet("/:id", actorMoviesController.ReadActorMovie);
        MapPut("/:id", HttpUtils.ReadRequestBodyAsText, actorMoviesController.UpdateActorMovie);
        MapDelete("/:id", actorMoviesController.DeleteActorMovie);
    }
}
