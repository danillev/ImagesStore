using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ImagesStore.API.Services
{
    public class AuthOptions
    {
        public string ISSUER => "ImageStoreAPI"; // издатель токена
        public string AUDIENCE => "ImageStoreWeb"; // потребитель токена
        private const string KEY = "mysupersecret!_secretkey!123";   // ключ для шифрации

        public SymmetricSecurityKey GetSymmetricSecurityKey() =>
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
    }
}
