using ZeroPass.Model.Service;

namespace ZeroPass.Api
{
    public partial class UsersController
    {
        public UsersController(IUserService userService) => UserService = userService;
    }
}