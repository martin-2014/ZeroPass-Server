using System.Threading.Tasks;
using ZeroPass.Model.Service;
using ZeroPass.Storage;

namespace ZeroPass.Service
{
    public partial class UserService : IUserService
    {
        readonly IUnitOfWorkFactory UnitOfWorkFactory;

        public UserService(IUnitOfWorkFactory factory)
        {
            UnitOfWorkFactory = factory;
        }

        public async Task<bool> ExistsUserByEmail(string email)
        {
            using var unitOfWork = await UnitOfWorkFactory.CreateRead();
            return await unitOfWork.Users.GetByEmail(email) != null;
        }
    }
}
