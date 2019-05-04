using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VRTeleportator.Services.Interfaces
{
    public interface ISecretKey
    {
        SigningCredentials GetSecretKey();
    }
}
