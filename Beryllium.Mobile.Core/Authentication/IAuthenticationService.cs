namespace Beryllium.Mobile.Core.Authentication
{
    using System.Threading.Tasks;

    public interface IAuthenticationService
    {
        Task<User> Authenticate();
    }
}
