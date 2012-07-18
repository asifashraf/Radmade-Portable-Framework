using System;
using System.Web.Script.Serialization;
using Areas.Common.Types;
using Areas.WebAuth.Exceptions;

namespace Areas.WebAuth.Types
{
    public class Auth
    {
        public string CreateToken(string userPrimaryKey, string userNameOrEmail, string userIPAddress, DateTime expiryDate)
        {
            var token = new Token
                            {
                                Time =  DateTime.Now,
                                UserIPAddress = userIPAddress,
                                UserPrimaryKey = userPrimaryKey,
                                UserNameOrEmail = userNameOrEmail,
                                Expiry = expiryDate
                            };
            var crypto = new Crypto();

            var tokenJs = (new JavaScriptSerializer()).Serialize(token);

            return crypto.EncryptStringAES(tokenJs, WebAuthSettings.EncryptionSharedSecret);
        }

        public string RefreshToken(string tokenString, DateTime newExpiry)
        {
            var crypto = new Crypto();

            var tokenJs = crypto.DecryptStringAES(tokenString, WebAuthSettings.EncryptionSharedSecret);

            var token = (new JavaScriptSerializer()).Deserialize<Token>(tokenJs);

            if(token.Expiry < token.Time)
            {
                throw new TokenExpiredException();
            }

            token.Expiry = newExpiry;

            return CreateToken(token.UserPrimaryKey, token.UserNameOrEmail, token.UserIPAddress, token.Expiry);

        }
    }
}
