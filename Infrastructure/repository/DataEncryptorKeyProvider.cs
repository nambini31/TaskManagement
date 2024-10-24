using Domain.
using Domain.Interface;
namespace Infrastructure.repository
{
    public class DataEncryptorKeyProvider : IDataEncryptorKeyProvider
    {
        public string GetEncryptionKey()
        {
            try
            {
                var encKey = EncryptedSettingsHelper.GetEncryptedSettings();
                if (!DpApiEncryptionProvider.IsEncrypted(encKey!))
                {
                    EncryptedSettingsHelper.EncryptSetting(encKey!);
                    encKey = EncryptedSettingsHelper.GetEncryptedSettings();
                }

                encKey = EncryptedSettingsHelper.RemovePrefix(encKey!);
                return DpApiEncryptionProvider.Decrypt(encKey);
            }
            catch (Exception e)
            {
                LogHelper.LogException(e);
                return string.Empty;
            }
        }
    }
}