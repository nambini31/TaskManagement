using Application.Helper.Security;
namespace Application.Services
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
                throw;
                //LogHelper.LogException(e);
                //return string.Empty;
            }
        }
    }
}