using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinAPIMusicProject.Data;
using MinAPIMusicProject.DTOs;
using MinAPIMusicProject.Models;

namespace MinAPIMusicProject.Endpoints;

public static class ArtistEndpoints
{
    public static void AddArtistEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoint = app.MapGroup("/api/artists");

        endpoint.MapPost("/", async (MusicContext context, ArtistDTO artist) =>
        {
            // validation
            var artistFromDb = context.Add(new Artist()
            {
                Name = artist.Name,
            });
            await context.SaveChangesAsync();

            return Results.Created($"/api/artists/{artistFromDb.Entity.Id}", artistFromDb.Entity.Id);
        });

        endpoint.MapGet("/", async (MusicContext context,
            [FromQuery]int page = 0, 
            [FromQuery]int size = 10, 
            [FromQuery]string? q = null) =>
        {
            var artists = q == null ? context.Artists : context.Artists.Where(x => x.Name.Contains(q));
            var result = await artists.Skip(page * size)
                .Take(size)
                .Select(x => new ArtistDTO() { Name = x.Name })
                .ToListAsync();
                
            return Results.Ok(result);
        });
    }
}