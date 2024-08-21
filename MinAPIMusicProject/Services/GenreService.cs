using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MinAPIMusicProject.Data;
using MinAPIMusicProject.DTOs;
using MinAPIMusicProject.Interfaces;
using MinAPIMusicProject.Models;

namespace MinAPIMusicProject.Services;

public class GenreService : IGenreService
{
    private readonly MusicContext _context;
    private readonly IMapper _mapper;

    public GenreService(MusicContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<GenreDTO> AddGenre(GenreDTO genre, CancellationToken cancellationToken = default)
    {
        var genreEntity = _context.Genres.Add(_mapper.Map<Genre>(genre));
        await _context.SaveChangesAsync(cancellationToken);
        return _mapper.Map<GenreDTO>(genreEntity.Entity);
    }
    
    public async Task DeleteGenre(int id, CancellationToken cancellationToken = default)
    {
        var genre = await _context.Genres.FindAsync(new object[]{id}, cancellationToken: cancellationToken);
        if (genre == null)
        {
            throw new ArgumentNullException(nameof(genre), "Жанр не найден.");
        }
        _context.Genres.Remove(genre);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<GenreDTO>> GetGenres(int page, int size, string? q, CancellationToken cancellationToken = default)
    {
        var query = string.IsNullOrEmpty(q)
            ? _context.Genres
            : _context.Genres.Where(g => g.Name.Contains(q));

        var genres = await query.Skip(page * size)
            .Take(size)
            .ToListAsync(cancellationToken);

        return genres.Select(g => _mapper.Map<GenreDTO>(g)).ToList();
    }


    public async Task<List<GenreDTO>> GetGenresBySearch(string name, CancellationToken cancellationToken = default)
    {
        var lowerName = string.IsNullOrWhiteSpace(name) ? string.Empty : name.ToLower();

        var genres = await _context.Genres
            .Where(g => g.Name.ToLower().Contains(lowerName))
            .ToListAsync(cancellationToken);

        return genres.Select(g => _mapper.Map<GenreDTO>(g)).ToList();
    }
    
    
}