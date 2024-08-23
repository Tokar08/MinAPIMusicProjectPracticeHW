using System.ComponentModel.DataAnnotations;

namespace MessangerBackend.Requests;

public class LoginRequest
{
    public string Login { get; set; }
    public string Password { get; set; }
}