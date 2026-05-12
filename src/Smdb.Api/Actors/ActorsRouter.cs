namespace Smdb.Api.Actors;

using Shared.Http;

public class ActorsRouter : HttpRouter
{
    public ActorsRouter(ActorsController actorsController)
    {
        UseParametrizedRouteMatching();
        MapGet("/",           actorsController.ReadActors);
        MapPost("/",          HttpUtils.ReadRequestBodyAsText, actorsController.CreateActor);
        MapGet("/:id",        actorsController.ReadActor);
        MapPut("/:id",        HttpUtils.ReadRequestBodyAsText, actorsController.UpdateActor);
        MapDelete("/:id",     actorsController.DeleteActor);
        MapGet("/:id/movies", actorsController.ReadMoviesByActor);
    }
}
