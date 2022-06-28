using System.Threading.Tasks;
using ZeroPass.Model.Models;

namespace ZeroPass.Model.Service
{
    public interface IActivationService
    {
        Task<CodeVerifyResultModel> CodeVerify(CodeVerifyModel model);

        Task<ActivatedAccountModel> ActivateAccount(ActivateAccountModel model);
    }
}
