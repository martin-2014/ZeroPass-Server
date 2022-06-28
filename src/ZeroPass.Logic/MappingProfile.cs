using AutoMapper;
using ZeroPass.Model.Models;
using ZeroPass.Storage.Entities;

namespace ZeroPass.Service
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Activation
            CreateMap<RegistrationEntity, CodeVerifyResultModel>();

            //EncryptionKey
            CreateMap<UserKeyCreateModel, UserKeyEntity>();
        }
    }
}
