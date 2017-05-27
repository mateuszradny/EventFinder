﻿using FlatFinder.Contracts.Services;
using Microsoft.AspNet.Cryptography.KeyDerivation;
using System;
using System.Security.Cryptography;

namespace EventFinder.Application.Services
{
    public class CryptographyService : ICryptographyService
    {
        public byte[] GetSalt()
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
                rng.GetBytes(salt);

            return salt;
        }

        public string HashPassword(string password, byte[] salt)
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(password, salt, KeyDerivationPrf.HMACSHA1, 100, 10000));
        }
    }
}