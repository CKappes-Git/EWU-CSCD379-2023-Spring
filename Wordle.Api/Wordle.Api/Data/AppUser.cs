using Microsoft.AspNetCore.Identity;

namespace Wordle.Api.Data;

public class AppUser : IdentityUser
{
    public Guid UserId { get; set; }
    public required string Name { get; set; }
    public int NumPosts { get; set; }
    
}