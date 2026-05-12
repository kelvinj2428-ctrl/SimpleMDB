namespace Smdb.Api;

using Shared.Http;
using Smdb.Api.Movies;
using Smdb.Api.Actors;
using Smdb.Api.ActorMovies;
using Smdb.Api.Users;
using Smdb.Api.Auth;
using Smdb.Core.Db;
using Smdb.Core.Movies;
using Smdb.Core.Actors;
using Smdb.Core.ActorMovies;
using Smdb.Core.Users;

public class App : HttpServer
{
    public override void Init()
    {
        // ── Data Layer ─────────────────────────────────────────────────────────
        var db = new MemoryDatabase();

        // ── Repositories ───────────────────────────────────────────────────────
        var movieRepo      = new MemoryMovieRepository(db);
        var actorRepo      = new MemoryActorRepository(db);
        var actorMovieRepo = new MemoryActorMovieRepository(db);
        var userRepo       = new MemoryUserRepository(db);

        // ── Services ───────────────────────────────────────────────────────────
        var movieServ      = new DefaultMovieService(movieRepo);
        var actorServ      = new DefaultActorService(actorRepo);
        var actorMovieServ = new DefaultActorMovieService(actorMovieRepo);
        var userServ       = new DefaultUserService(userRepo);

        // ── Controllers ────────────────────────────────────────────────────────
        var movieCtrl      = new MoviesController(movieServ);
        var actorCtrl      = new ActorsController(actorServ, actorMovieServ);
        var actorMovieCtrl = new ActorMoviesController(actorMovieServ);
        var usersCtrl      = new UsersController(userServ);
        var authCtrl       = new AuthController(userServ);

        // ── Routers ────────────────────────────────────────────────────────────
        var movieRouter      = new MoviesRouter(movieCtrl);
        var actorRouter      = new ActorsRouter(actorCtrl);
        var actorMovieRouter = new ActorMoviesRouter(actorMovieCtrl);
        var usersRouter      = new UsersRouter(usersCtrl);
        var authRouter       = new AuthRouter(authCtrl);

        // Sub-router for /api/v1/movies/:id/actors
        var movieActorsRouter = new HttpRouter();
        movieActorsRouter.UseParametrizedRouteMatching();
        movieActorsRouter.MapGet("/:id/actors", actorMovieCtrl.ReadActorsByMovie);

        // ── API v1 Router ──────────────────────────────────────────────────────
        var apiRouter = new HttpRouter();
        apiRouter.UseRouter("/auth",         authRouter);
        apiRouter.UseRouter("/movies",       movieRouter);
        apiRouter.UseRouter("/movies",       movieActorsRouter);
        apiRouter.UseRouter("/actors",       actorRouter);
        apiRouter.UseRouter("/actors-movies",actorMovieRouter);
        apiRouter.UseRouter("/users",        usersRouter);

        // ── Global Middleware Pipeline ─────────────────────────────────────────
        router.Use(HttpUtils.StructuredLogging);
        router.Use(HttpUtils.CentralizedErrorHandling);
        router.Use(HttpUtils.AddResponseCorsHeaders);
        router.Use(HttpUtils.DefaultResponse);
        router.Use(HttpUtils.ParseRequestUrl);
        router.Use(HttpUtils.ParseRequestQueryString);
        router.UseParametrizedRouteMatching();

        // Root welcome page
        router.MapGet("/", async (req, res, props, next) =>
        {
            string html = """
                <!DOCTYPE html>
                <html lang="en">
                <head>
                    <meta charset="UTF-8">
                    <meta name="viewport" content="width=device-width, initial-scale=1.0">
                    <title>SimpleMDB API</title>
                    <style>
                        body { font-family: system-ui, sans-serif; max-width: 700px; margin: 60px auto; padding: 0 20px; background: #1a1a2e; color: #eee; }
                        h1 { color: #00d4ff; }
                        a { color: #00d4ff; text-decoration: none; }
                        a:hover { text-decoration: underline; }
                        ul { line-height: 2; }
                        code { background: #16213e; padding: 2px 8px; border-radius: 4px; }
                    </style>
                </head>
                <body>
                    <h1>🎬 SimpleMDB API</h1>
                    <p>Welcome! The API is running. Available endpoints:</p>
                    <ul>
                        <li><a href="/api/v1/movies">GET /api/v1/movies</a> — List movies</li>
                        <li><a href="/api/v1/movies/1">GET /api/v1/movies/:id</a> — Get movie by ID</li>
                        <li><a href="/api/v1/actors">GET /api/v1/actors</a> — List actors</li>
                        <li><a href="/api/v1/actors/1">GET /api/v1/actors/:id</a> — Get actor by ID</li>
                        <li><code>POST /api/v1/movies</code> — Create movie</li>
                        <li><code>PUT /api/v1/movies/:id</code> — Update movie</li>
                        <li><code>DELETE /api/v1/movies/:id</code> — Delete movie</li>
                        <li><code>POST /api/v1/auth</code> — Login</li>
                    </ul>
                    <p>Try: <a href="/api/v1/movies?page=1&size=5">/api/v1/movies?page=1&size=5</a></p>
                </body>
                </html>
                """;
            await HttpUtils.SendResponse(req, res, props, 200, html, "text/html; charset=utf-8");
        });

        // Static files (wwwroot)
        router.Use(HttpUtils.ServeStaticFiles);

        // Mount API
        router.UseRouter("/api/v1", apiRouter);
    }
}
