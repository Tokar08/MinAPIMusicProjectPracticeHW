using MinAPIMusicProject.DTOs;

namespace MinAPIMusicProject.Interfaces;

public interface IGenreService
{
    Task<GenreDTO> AddGenre(GenreDTO genre, CancellationToken cancellationToken = default);
    Task DeleteGenre(int id, CancellationToken cancellationToken = default);
    Task<List<GenreDTO>> GetGenres(int page, int size, string? q, CancellationToken cancellationToken = default);
    Task<List<GenreDTO>> GetGenresBySearch(string name, CancellationToken cancellationToken = default);
}