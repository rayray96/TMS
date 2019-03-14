using System;

public class RefreshTokenDTO
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public string Token { get; set; }
    public DateTime Issue { get; set; }
    public DateTime Expires { get; set; }
}