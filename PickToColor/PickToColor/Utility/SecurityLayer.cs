using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using PickToColor.EntityFramework;
using PickToColor.Models;

namespace PickToColor.Utility
{
    public class SecurityLayer
    {
        public string SaltedPassword(string Password)
        {
            //Create the salt value with a cryptographic PRNG:
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

            //Create the Rfc2898DeriveBytes and get the hash value:
            var pbkdf2 = new Rfc2898DeriveBytes(Password, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);

            //Combine the salt and password bytes for later use:
            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);
            //Convert bytes to string
            string savedPasswordHash = Convert.ToBase64String(hashBytes);
            return savedPasswordHash;

        }

        public bool AuthenticateUser(string UserName, string Password,string UserType, out UserModel CurrentUser)
        {
            CurrentUser = null;
            DataContext context = new DataContext();
            if (UserType == "Operator")
            {
                CurrentUser = context.Users.FirstOrDefault(a => a.UserName == UserName && a.IsActive && !a.IsManager);
                if (CurrentUser == null) return false;
                return true;
            }

            CurrentUser = context.Users.FirstOrDefault(a => a.UserName == UserName && a.IsActive);
            /* Fetch the stored value */
            if (CurrentUser == null) return false;
            string savedPasswordHash = CurrentUser.Password;
            /* Extract the bytes */
            byte[] hashBytes = Convert.FromBase64String(savedPasswordHash);
            /* Get the salt */
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);
            /* Compute the hash on the password the user entered */
            var pbkdf2 = new Rfc2898DeriveBytes(Password, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);
            /* Compare the results */
            for (int i = 0; i < 20; i++)
                if (hashBytes[i + 16] != hash[i])
                    return false;

            return true;
        }

        public UserModel ReturnCurrentUser()
        {
            DataContext dc = new DataContext();
            if (HttpContext.Current.User.Identity.IsAuthenticated)
                return dc.Users.FirstOrDefault(a => a.UserName == HttpContext.Current.User.Identity.Name && a.IsActive);
            return null;
        }

        public List<UserModel> ReturnAllUsers()
        {
            DataContext dc = new DataContext();
            var UserList = dc.Users.Where(a => a.IsActive).ToList();
            return UserList;
        }
    }
}