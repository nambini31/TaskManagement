using System.Security.Cryptography;
using System.Text;

namespace Domain.Helper.Security
{
    /// <summary>
    /// Provides encryption services. These methods are supported only on Windows OS.
    /// </summary>
    internal static class DpApiEncryptionProvider
    {
        internal static string Decrypt(string encryptedText, byte[]? entropy = null)
        {
            if (encryptedText == null) throw new ArgumentNullException(nameof(encryptedText));

            var encryptedBytes = Convert.FromBase64String(encryptedText);
            var decryptedBytes = ProtectedData.Unprotect(encryptedBytes, entropy, DataProtectionScope.CurrentUser);

            return Encoding.UTF8.GetString(decryptedBytes);
        }

        internal static string Encrypt(string clearText, byte[]? entropy = null)
        {
            if (clearText == null) throw new ArgumentNullException(nameof(clearText));

            var clearBytes = Encoding.UTF8.GetBytes(clearText);
            var encryptedBytes = ProtectedData.Protect(clearBytes, entropy, DataProtectionScope.CurrentUser);

            return Convert.ToBase64String(encryptedBytes);
        }

        internal static bool IsEncrypted(string text) =>
            text.StartsWith(EncryptionConstants.EncryptedValuePrefix, StringComparison.OrdinalIgnoreCase);
    }
}