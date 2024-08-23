using AutoMapper;
using MessangerBackend.Requests;
using Microsoft.EntityFrameworkCore;
using MinAPIMusicProject.Data;
using MinAPIMusicProject.DTOs;
using MinAPIMusicProject.Interfaces;
using MinAPIMusicProject.Models;

namespace MinAPIMusicProject.Services;

public class UserService : IUserService
{
    private readonly MusicContext _context;
    private readonly IMapper _mapper;

    public UserService(MusicContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<UserDTO> Register(RegisterRequest request, CancellationToken cancellationToken = default)
    {
        if (await _context.Users.AnyAsync(u => u.Login == request.Login, cancellationToken))
            throw new Exception("User already exists!");

        var user = new User
        {
            Name = request.Name,
            Login = request.Login,
            Password = BCrypt.Net.BCrypt.HashPassword(request.Password)
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<UserDTO>(user);
    }

    public async Task<UserDTO> Login(LoginRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _context.Users.SingleOrDefaultAsync(u => u.Login == request.Login, cancellationToken);

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            throw new Exception("Invalid login or password!");

        return _mapper.Map<UserDTO>(user);
    }

    public async Task LikeTrack(int userId, int trackId, CancellationToken cancellationToken = default)
    {
        var user = await _context.Users.Include(u => u.LikedTracks)
            .SingleOrDefaultAsync(u => u.Id == userId, cancellationToken);
        var track = await _context.Tracks.SingleOrDefaultAsync(t => t.Id == trackId, cancellationToken);

        if (user == null || track == null)
            throw new ArgumentException("User or track not found!");

        if (!user.LikedTracks.Contains(track))
            user.LikedTracks.Add(track);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<TrackDTO>> GetLikedTracks(int userId, CancellationToken cancellationToken = default)
    {
        var user = await _context.Users.Include(u => u.LikedTracks)
            .SingleOrDefaultAsync(u => u.Id == userId, cancellationToken);

        if (user == null)
            throw new ArgumentException("User not found!");

        return user.LikedTracks.Select(t => _mapper.Map<TrackDTO>(t)).ToList();
    }
}