namespace Smdb.Api.Auth;

using System.Collections;
using System.Net;
using System.Text.Json;
using Shared.Http;
using Smdb.Core.Users;

public class AuthController
{
    private IUserService userService;

    // Simple in-memory session store (token -> userId).
    // In production, use JWT or a proper session mechanism.
    private static Dictionary<string, int> sessions = new();

    public AuthController(IUserService userService)
    {
        this.userService = userService;
    }

    // POST /api/v1/auth/register
    public async Task Register(
        HttpListenerRequest req, HttpListenerResponse res, Hashtable props, Func<Task> next)
    {
        var text = (string)props["req.text"]!;
        var user = JsonSerializer.Deserialize<User>(text, JsonSerializerOptions.Web);
        if (user != null && string.IsNullOrWhiteSpace(user.Role))
            user.Role = "user";

        var result = await userService.CreateUser(user!);
        await JsonUtils.SendResultResponse(req, res, props, result);
        await next();
    }

    // POST /api/v1/auth/login
    public async Task Login(
        HttpListenerRequest req, HttpListenerResponse res, Hashtable props, Func<Task> next)
    {
        var text = (string)props["req.text"]!;
        var payload = JsonSerializer.Deserialize<LoginPayload>(text, JsonSerializerOptions.Web);

        if (payload == null)
        {
            await HttpUtils.SendResponse(req, res, props,
                (int)HttpStatusCode.BadRequest, "Invalid login payload.");
            await next();
            return;
        }

        var result = await userService.Login(payload.Username ?? "", payload.Password ?? "");
        if (result.IsError)
        {
            await JsonUtils.SendResultResponse(req, res, props, result);
            await next();
            return;
        }

        // Generate a simple token
        string token = Guid.NewGuid().ToString("N");
        sessions[token] = result.Payload!.Id;

        var response = new { token, user = result.Payload };
        await HttpUtils.SendResponse(req, res, props,
            (int)HttpStatusCode.OK,
            JsonSerializer.Serialize(response, JsonSerializerOptions.Web),
            "application/json");
        await next();
    }

    // POST /api/v1/auth/logout
    public async Task Logout(
        HttpListenerRequest req, HttpListenerResponse res, Hashtable props, Func<Task> next)
    {
        string? token = req.Headers["Authorization"]?.Replace("Bearer ", "");
        if (token != null) sessions.Remove(token);

        await HttpUtils.SendResponse(req, res, props,
            (int)HttpStatusCode.OK,
            JsonSerializer.Serialize(new { message = "Logged out successfully." }, JsonSerializerOptions.Web),
            "application/json");
        await next();
    }

    // Middleware: check if user is authenticated
    public async Task RequireAuth(
        HttpListenerRequest req, HttpListenerResponse res, Hashtable props, Func<Task> next)
    {
        string? token = req.Headers["Authorization"]?.Replace("Bearer ", "");
        if (token == null || !sessions.TryGetValue(token, out int userId))
        {
            await HttpUtils.SendResponse(req, res, props,
                (int)HttpStatusCode.Unauthorized,
                JsonSerializer.Serialize(new { error = "Authentication required." }, JsonSerializerOptions.Web),
                "application/json");
            return;
        }
        props["auth.userId"] = userId;
        await next();
    }

    // Middleware: check if user is admin
    public async Task RequireAdmin(
        HttpListenerRequest req, HttpListenerResponse res, Hashtable props, Func<Task> next)
    {
        string? token = req.Headers["Authorization"]?.Replace("Bearer ", "");
        if (token == null || !sessions.TryGetValue(token, out int userId))
        {
            await HttpUtils.SendResponse(req, res, props,
                (int)HttpStatusCode.Unauthorized,
                JsonSerializer.Serialize(new { error = "Authentication required." }, JsonSerializerOptions.Web),
                "application/json");
            return;
        }

        var result = await userService.ReadUser(userId);
        if (result.IsError || result.Payload?.Role != "admin")
        {
            await HttpUtils.SendResponse(req, res, props,
                (int)HttpStatusCode.Forbidden,
                JsonSerializer.Serialize(new { error = "Admin access required." }, JsonSerializerOptions.Web),
                "application/json");
            return;
        }

        props["auth.userId"] = userId;
        await next();
    }

    private class LoginPayload
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
    }
}
