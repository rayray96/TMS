using DAL.EF;

namespace DAL.Repositories
{
    public class RefreshTokenRepository : Repository<RefreshToken>
    {
        public RefreshTokenRepository(ApplicationDbContext context)
            : base(context)
        {
        }
    }
}
