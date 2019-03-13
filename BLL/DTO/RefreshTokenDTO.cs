public class RefreshTokenDTO
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public string Refreshtoken { get; set; }
    public bool Revoked { get; set; }
}