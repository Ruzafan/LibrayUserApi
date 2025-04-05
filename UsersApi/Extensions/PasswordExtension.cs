using System.Security.Cryptography;
using System.Text;

namespace UsersApi.Extensions;

public static class PasswordExtension
{
    public static byte[] CalculateSha256(this string str)
        => SHA256.HashData(new UTF8Encoding().GetBytes(str));
}