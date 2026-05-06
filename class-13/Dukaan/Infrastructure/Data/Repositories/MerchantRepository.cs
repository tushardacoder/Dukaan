using Dukaan.Infrastructure.Data.DbContext;
using Dukaan.Infrastructure.Data.Model;
using Microsoft.AspNetCore.Identity;

namespace Dukaan.Infrastructure.Data.Repositories
{
    public class MerchantRepository
    {

        private readonly UserManager<Merchant> _userManager;

        public MerchantRepository(UserManager<Merchant> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Merchant?> GetByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<bool> GetByPasswordAsync(Merchant user,string password)
        {
            return await _userManager.CheckPasswordAsync(user,password);
        }
    }
}
