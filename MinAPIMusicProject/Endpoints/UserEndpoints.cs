using MessangerBackend.Requests;
using Microsoft.AspNetCore.Mvc;
using MinAPIMusicProject.Interfaces;

namespace MinAPIMusicProject.Endpoints;

public static class UserEndpoints
{
    public static void AddUserEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoint = app.MapGroup("/api/users");
        
        endpoint.MapPost("/register", async (
            IUserService userService,
            RegisterRequest registerRequest,
            CancellationToken cancellationToken = default) =>
        {
            try
            {
                var user = await userService.Register(registerRequest, cancellationToken);
                return Results.Created($"/api/users/{user.Id}", user);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
        });
        
        
        endpoint.MapPost("/login", async (
            IUserService userService,
            LoginRequest loginRequest,
            CancellationToken cancellationToken = default) =>
        {
            try
            {
                var user = await userService.Login(loginRequest, cancellationToken);
                return Results.Ok(user);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
        });
        
        
        endpoint.MapPost("/{userId}/like/{trackId}", async (
            IUserService userService,
            [FromRoute] int userId,
            [FromRoute] int trackId,
            CancellationToken cancellationToken = default) =>
        {
            try
            {
                await userService.LikeTrack(userId, trackId, cancellationToken);
                return Results.NoContent();
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
        });
        
        endpoint.MapGet("/{userId}/likes", async (
            IUserService userService,
            [FromRoute] int userId,
            CancellationToken cancellationToken = default) =>
        {
            try
            {
                var likedTracks = await userService.GetLikedTracks(userId, cancellationToken);
                return Results.Ok(likedTracks);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
        });
    }
}