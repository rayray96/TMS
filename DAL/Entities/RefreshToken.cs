using DAL.Entities;
using System;
/// <summary>
/// This entity is for Jwt authentication.
/// </summary>
public class RefreshToken
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public string Token { get; set; }
    public DateTime Issue { get; set; }
    public DateTime Expires { get; set; }

    public virtual ApplicationUser ApplicationUser { get; set; }
}