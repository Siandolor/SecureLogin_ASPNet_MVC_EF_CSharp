namespace PasswordSolution.Models;

public class ApplicationUser : IdentityUser
{
    [Required]
    public byte[] PasswordSHA512Salt { get; set; } = Array.Empty<byte>();
    
    [Required]
    public byte[] PasswordARGON2IDSalt { get; set; } = Array.Empty<byte>();
}
