using MessangerBackend.Requests;
using MinAPIMusicProject.DTOs;

namespace MinAPIMusicProject.Interfaces;

public interface IUserService
{
    Task<UserDTO> Register(RegisterRequest request, CancellationToken cancellationToken = default);
    Task<UserDTO> Login(LoginRequest request, CancellationToken cancellationToken = default);
    Task LikeTrack(int userId, int trackId, CancellationToken cancellationToken = default);
    Task<List<TrackDTO>> GetLikedTracks(int userId, CancellationToken cancellationToken = default);
}