namespace Smdb.Api.ActorMovies;

using System.Collections;
using System.Collections.Specialized;
using System.Net;
using System.Text.Json;
using Shared.Http;
using Smdb.Core.ActorMovies;
using Smdb.Core.Movies;

public class ActorMoviesController
{
    private IActorMovieService actorMovieService;

    public ActorMoviesController(IActorMovieService actorMovieService)
    {
        this.actorMovieService = actorMovieService;
    }

    // GET /api/v1/actors-movies?page=1&size=10
    public async Task ReadActorMovies(
        HttpListenerRequest req, HttpListenerResponse res, Hashtable props, Func<Task> next)
    {
        int page = int.TryParse(req.QueryString["page"], out int p) ? p : 1;
        int size = int.TryParse(req.QueryString["size"], out int s) ? s : 9;
        var result = await actorMovieService.ReadActorMovies(page, size);
        await JsonUtils.SendPagedResultResponse(req, res, props, result, page, size);
        await next();
    }

    // POST /api/v1/actors-movies
    public async Task CreateActorMovie(
        HttpListenerRequest req, HttpListenerResponse res, Hashtable props, Func<Task> next)
    {
        var text       = (string)props["req.text"]!;
        var actorMovie = JsonSerializer.Deserialize<ActorMovie>(text, JsonSerializerOptions.Web);
        var result     = await actorMovieService.CreateActorMovie(actorMovie!);
        await JsonUtils.SendResultResponse(req, res, props, result);
        await next();
    }

    // GET /api/v1/actors-movies/:id
    public async Task ReadActorMovie(
        HttpListenerRequest req, HttpListenerResponse res, Hashtable props, Func<Task> next)
    {
        var uParams = (NameValueCollection)props["req.params"]!;
        int id = int.TryParse(uParams["id"], out int i) ? i : -1;
        var result = await actorMovieService.ReadActorMovie(id);
        await JsonUtils.SendResultResponse(req, res, props, result);
        await next();
    }

    // PUT /api/v1/actors-movies/:id
    public async Task UpdateActorMovie(
        HttpListenerRequest req, HttpListenerResponse res, Hashtable props, Func<Task> next)
    {
        var uParams    = (NameValueCollection)props["req.params"]!;
        int id         = int.TryParse(uParams["id"], out int i) ? i : -1;
        var text       = (string)props["req.text"]!;
        var actorMovie = JsonSerializer.Deserialize<ActorMovie>(text, JsonSerializerOptions.Web);
        var result     = await actorMovieService.UpdateActorMovie(id, actorMovie!);
        await JsonUtils.SendResultResponse(req, res, props, result);
        await next();
    }

    // DELETE /api/v1/actors-movies/:id
    public async Task DeleteActorMovie(
        HttpListenerRequest req, HttpListenerResponse res, Hashtable props, Func<Task> next)
    {
        var uParams = (NameValueCollection)props["req.params"]!;
        int id = int.TryParse(uParams["id"], out int i) ? i : -1;
        var result = await actorMovieService.DeleteActorMovie(id);
        await JsonUtils.SendResultResponse(req, res, props, result);
        await next();
    }

    // GET /api/v1/movies/:id/actors?page=1&size=10
    public async Task ReadActorsByMovie(
        HttpListenerRequest req, HttpListenerResponse res, Hashtable props, Func<Task> next)
    {
        var uParams = (NameValueCollection)props["req.params"]!;
        int movieId = int.TryParse(uParams["id"], out int i) ? i : -1;
        int page    = int.TryParse(req.QueryString["page"], out int p) ? p : 1;
        int size    = int.TryParse(req.QueryString["size"], out int s) ? s : 9;
        var result  = await actorMovieService.ReadActorsByMovie(movieId, page, size);
        await JsonUtils.SendPagedResultResponse(req, res, props, result, page, size);
        await next();
    }
}
