using GameInfo.Core.Models;
using GameInfo.Core.Models.Requests;
using System.Threading.Tasks;

namespace GameInfo.Core.Interfaces
{
    public interface IAuthenticateService
    {
        Task<ServiceResult<AuthToken>> Authenticate(SignInRequest request);
    }
}
