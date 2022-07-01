using AutoMapper;
using ZeroPass.Model.Models;
using ZeroPass.Model.Models.UserProfiles;
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
            
            // User Profile
            CreateMap<UserProfileView, UserProfileModel>();
            CreateMap<DomainUserDetailView, DomainOfUserModel>();
        }
    }
}
