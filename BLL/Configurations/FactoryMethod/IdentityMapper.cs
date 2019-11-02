using AutoMapper;
using BLL.DTO;
using DAL.Entities;

namespace BLL.Configurations.FactoryMethod
{
    public class IdentityMapper : IWrappedMapper
    {
        public IMapper CreateMapping()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Person, PersonDTO>();
                cfg.CreateMap<ApplicationUser, UserDTO>();
                cfg.CreateMap<RefreshToken, RefreshTokenDTO>();

                cfg.CreateMap<PersonDTO, Person>();
                cfg.CreateMap<UserDTO, ApplicationUser>();
                cfg.CreateMap<RefreshTokenDTO, RefreshToken>();
            });

            return config.CreateMapper();
        }
    }
}
