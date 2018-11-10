using IdentityServer4.Test;
using PDJaya.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PDJaya.Identity
{
    public interface IUserRepository
    {
        Task<User> FindAsync(string Username);
        Task<User> FindAsync(long Uid);
        
        TestUser AutoProvisionUser(string provider, string userId, List<Claim> claims);

        TestUser FindByExternalProvider(string provider, string userId);

        TestUser FindBySubjectId(string subjectId);

        TestUser FindByUsername(string username);

        bool ValidateCredentials(string username, string password);


    }
}
