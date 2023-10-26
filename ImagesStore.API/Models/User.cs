using Microsoft.AspNetCore.Identity;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace ImagesStore.API.Models
{
    public class User : IdentityUser 
    {
        
    }
    public class Test
    {
        public Test() {

            User user = new User();
                    
        }
        
    }
}
