namespace Smdb.Api.Auth;

using Shared.Http;

public class AuthRouter : HttpRouter
{
    public AuthRouter(AuthController authController)
    {
        UseParametrizedRouteMatching();
        MapPost("/register", HttpUtils.ReadRequestBodyAsText, authController.Register);
        MapPost("/login",    HttpUtils.ReadRequestBodyAsText, authController.Login);
        MapPost("/logout",   authController.Logout);
    }
}
