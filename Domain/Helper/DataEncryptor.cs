using System.Security.Cryptography;
using System.Text;
using Domain.Interface;

namespace Domain.Helper;

public class DataEncryptor
{
    private readonly IDataEncryptorKeyProvider _keyProvider;

    public DataEncryptor(IDataEncryptorKeyProvider keyProvider)
    {
        _keyProvider = keyProvider;
    }

    public string Encrypt(string? texte)
    {
        var iv = new byte[16];
        byte[] array;
        texte = Reverse(texte ?? string.Empty);

        using (var aesEncryption = Aes.Create())
        {
            aesEncryption.Key = Encoding.UTF8.GetBytes(_keyProvider.GetEncryptionKey());
            aesEncryption.IV = iv;

            var encryptor = aesEncryption.CreateEncryptor(aesEncryption.Key, aesEncryption.IV);

            using var memoryStream = new MemoryStream();
            using var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            using (var streamWriter = new StreamWriter(cryptoStream))
            {
                streamWriter.Write(texte);
            }

            array = memoryStream.ToArray();
        }

        return Convert.ToBase64String(array);
    }

    public string Decrypt(string? texte)
    {
        try
        {
            if (string.IsNullOrEmpty(texte)) return "";
            var iv = new byte[16];
            var buffer = Convert.FromBase64String(texte);

            using var aesEncryption = Aes.Create();
            aesEncryption.Key = Encoding.UTF8.GetBytes(_keyProvider.GetEncryptionKey());
            aesEncryption.IV = iv;
            var decrypt = aesEncryption.CreateDecryptor(aesEncryption.Key, aesEncryption.IV);

            using var memoryStream = new MemoryStream(buffer);
            using var cryptoStream = new CryptoStream(memoryStream, decrypt, CryptoStreamMode.Read);
            using var streamReader = new StreamReader(cryptoStream);
            return Reverse(streamReader.ReadToEnd());
        }
        catch (FormatException)
        {
            return texte ?? string.Empty;
        }
        catch (CryptographicException)
        {
            return texte ?? string.Empty;
        }
        catch (ArgumentNullException)
        {
            return "";
        }
    }

    private static string Reverse(string texte)
    {
        if (string.IsNullOrEmpty(texte)) return "";
        var texteArray = texte.ToCharArray().Reverse();
        return texteArray.Aggregate("", (current, t) => current + t);
    }
}