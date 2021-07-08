using Core.ErrorHandling;
using Newtonsoft.Json;
using System;
using System.Security.Cryptography;

namespace Core.Util
{
    public static class Authentication
    {
        public const int TokenDurationInMinutes = 30;

        private static readonly AesEncryption aes = new AesEncryption();

        public static string GenerateToken(string email, string password)
        {
            AuthenticationData data = new AuthenticationData()
            {
                Email = email,
                Password = password,
                ExpirationDate = DateTime.UtcNow.AddMinutes(TokenDurationInMinutes)
            };

            return aes.AesEncrypt(JsonConvert.SerializeObject(data));
        }

        public static AuthenticationData ParseAuthenticationToken(string token)
        {
            try
            {
                byte[] tokenBytes = Convert.FromBase64String(token);
                string json = aes.AesDecrypt(tokenBytes);
                return JsonConvert.DeserializeObject<AuthenticationData>(json);
            }
            catch (JsonReaderException)
            {
                throw new UnauthorizedException();
            }
            catch (FormatException)
            {
                throw new UnauthorizedException();
            }
            catch (CryptographicException)
            {
                throw new ForbiddenAccessException();
            }
        }
    }

    public class AuthenticationData
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
