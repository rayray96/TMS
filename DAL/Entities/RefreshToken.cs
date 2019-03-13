using DAL.Entities;
/// <summary>
/// This entity is for Jwt authentication.
/// </summary>
public class RefreshToken
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public string Refreshtoken { get; set; }
    public bool Revoked { get; set; }

    public virtual ApplicationUser ApplicationUser { get; set; }
}