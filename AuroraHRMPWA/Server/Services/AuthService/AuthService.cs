﻿
using System.Security.Cryptography;

namespace AuroraHRMPWA.Server.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly DataContext _context;

        public AuthService(DataContext context)
        {
            _context = context;
        }
        public async Task<ServiceResponse<int>> Register(User user, string password)
        {
            if(await UserExists(user.Email)){
                return new ServiceResponse<int> 
                { 
                    Success = false,
                    Message = "User with the same email already exists."
                };
            }

            //out is used so that we can get values and set to the user.PasswordHash and user.PasswordSalt
            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new ServiceResponse<int>
            {
                Data = user.Id,
                Message = "New user has been added successfully."
            };
        }

        public async Task<bool> UserExists(string email)
        {
            if(await _context.Users
                .AnyAsync(user=>user.Email.ToLower()
                .Equals(email.ToLower())))
            {
                return true;
            }
            return false;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            //hmacsha512 algorithm for encrypting password
            //Giving our pass inside the algorithm,
            //Algorithm instance creates a key which can be used for Password Salt
            //The computeHash method will use the password and the key to create the pass hash
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac
                    .ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }
}