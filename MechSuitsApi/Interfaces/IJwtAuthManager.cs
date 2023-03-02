using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreInfrastructure.Auth.User;
namespace MechSuitsApi.Interfaces
{
   public interface IJwtAuthManager
    {
     string   Authenticate(string username, string password,string year);
    }
}
