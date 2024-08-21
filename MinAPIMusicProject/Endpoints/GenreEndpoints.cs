using Microsoft.AspNetCore.Mvc;
using MinAPIMusicProject.DTOs;
using MinAPIMusicProject.Interfaces;

namespace MinAPIMusicProject.Endpoints;

public static class GenreEndpoints
{
      public static void AddGenreEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoint = app.MapGroup("/api/genres");
        
        endpoint.MapGet("/", async (
            IGenreService service,
            [FromQuery] int page = 0,
            [FromQuery] int size = 10,
            [FromQuery] string? q = null,
            CancellationToken cancellationToken = default) =>
        {
            var genres = await service.GetGenres(page, size, q, cancellationToken);
            return Results.Ok(genres);
        });
        
        endpoint.MapGet("/search", async (
            IGenreService service,
            [FromQuery] string name,
            CancellationToken cancellationToken = default) =>
        {
            try
            {
                var genres = await service.GetGenresBySearch(name, cancellationToken);
                return Results.Ok(genres);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
        });
        
        endpoint.MapPost("/", async (
            IGenreService service,
            GenreDTO genre,
            CancellationToken cancellationToken = default) =>
        {
            try
            {
                var genreFromDb = await service.AddGenre(genre, cancellationToken);
                return Results.Created($"/api/genres/{genreFromDb.Id}", genreFromDb);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
        });
        
        endpoint.MapDelete("{id}", async (
            IGenreService service,
            [FromRoute] int id,
            CancellationToken cancellationToken = default) =>
        {
            try
            {
                await service.DeleteGenre(id, cancellationToken);
                return Results.NoContent();
            }
            catch (ArgumentNullException)
            {
                return Results.NotFound();
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
        });
    }
}