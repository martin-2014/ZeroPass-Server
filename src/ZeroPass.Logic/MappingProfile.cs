using AutoMapper;
using ZeroPass.Model.Models;
using ZeroPass.Storage.Entities;
using ZeroPass.Storage.Entities.ClientVersions;

namespace ZeroPass.Service
{
    public sealed partial class MappingProfile : Profile
    {
        public MappingProfile() => Initialize();

        void InitializeMappings()
        {
            // Activation
            CreateMap<RegistrationEntity, CodeVerifyResultModel>();

            //EncryptionKey
            CreateMap<UserKeyCreateModel, UserKeyEntity>();

            // User Profile
            CreateMap<UserProfileView, UserProfileModel>();
            CreateMap<DomainUserDetailView, DomainOfUserModel>();
            
            //Client Version
            CreateMap<ClientMinVersionEntity, ClientMinVersionModel>();
            CreateMap<ClientVersionModel, ClientVersionView>();
        }
    }
}
