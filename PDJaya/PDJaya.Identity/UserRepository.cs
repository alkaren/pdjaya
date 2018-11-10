using PDJaya.Identity.Helpers;
using IdentityModel;
using IdentityServer4.Test;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PDJaya.Identity
{
    public class UserRepository:IUserRepository
    {
        private readonly PDJayaDB _context;

        public UserRepository(PDJayaDB db)
        {
            _context = db;
        }


        public async Task<User> FindAsync(string Username)
        {
            var data = from x in _context.UserProfiles
                       where x.UserName == Username
                       select x;
            User selUser = null;
            foreach(var item in data)
            {
                selUser = new User();
                selUser.Email = item.Email;
                selUser.Firstname = item.Firstname;
                selUser.IsActive = item.IsActive;
                selUser.Lastname = item.Lastname;
                selUser.Password = item.Password;
                selUser.PhoneNumber = item.Phone;
                selUser.Role = item.Role;
                selUser.UserId = item.Id;
                break;
            }
            return selUser;
        }
        public async Task<User> FindAsync(long Uid)
        {
            var data = from x in _context.UserProfiles
                       where x.Id == Uid
                       select x;
            User selUser = null;
            foreach (var item in data)
            {
                selUser = new User();
                selUser.Email = item.Email;
                selUser.Firstname = item.Firstname;
                selUser.IsActive = item.IsActive;
                selUser.Lastname = item.Lastname;
                selUser.Password = item.Password;
                selUser.PhoneNumber = item.Phone;
                selUser.Role = item.Role;
                selUser.UserId = item.Id;
                break;
            }
            return selUser;
        }

        public TestUser AutoProvisionUser(string provider, string userId, List<Claim> claims)
        {
            throw new NotImplementedException();
        }
        public TestUser FindByExternalProvider(string provider, string userId)
        {
            throw new NotImplementedException();
        }

        public TestUser FindBySubjectId(string subjectId)
        {
            throw new NotImplementedException();
        }

        public TestUser FindByUsername(string username)
        {
            var data = from x in _context.UserProfiles
                       where x.UserName == username
                       select x;
            TestUser selUser = null;
            foreach (var user in data)
            {
                selUser = new TestUser();
                selUser.Claims = new Claim[]
            {
            new Claim("user_id", user.Id.ToString() ?? ""),
            new Claim(JwtClaimTypes.Name, (!string.IsNullOrEmpty(user.Firstname) && !string.IsNullOrEmpty(user.Lastname)) ? (user.Firstname + " " + user.Lastname) : ""),
            new Claim(JwtClaimTypes.GivenName, user.Firstname  ?? ""),
            new Claim(JwtClaimTypes.FamilyName, user.Lastname  ?? ""),
            new Claim(JwtClaimTypes.Email, user.Email  ?? ""),
            new Claim("PhoneNumber", user.Phone ?? ""),

            //roles
            new Claim(JwtClaimTypes.Role, user.Role)
            };
                selUser.IsActive =user.IsActive;

                selUser.Password = user.Password;
                selUser.ProviderName = "MySQL";
                selUser.ProviderSubjectId = "DB";
                selUser.SubjectId = "UserDB";
                selUser.Username = user.UserName;
                break;
            }
            return selUser;
        }

        public bool ValidateCredentials(string username, string password)
        {
            var data = from x in _context.UserProfiles
                       where x.UserName == username && x.Password == password
                       select x;
            if (data != null && data.Count() > 0)
                return true;
            return false;
        }
    }
}
