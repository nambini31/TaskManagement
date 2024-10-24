using System.Reflection;
using Newtonsoft.Json;

namespace Domain.Helper.Security;

internal static class EncryptedSettingsHelper
{
    private static readonly string JsonFilePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, "config.json");

    internal static void EncryptSetting(string value)
    {
        var jsonString = JsonConvert.SerializeObject(new EncryptedSettings
        {
            SecretEncKey = EncryptionConstants.EncryptedValuePrefix + DpApiEncryptionProvider.Encrypt(value)
        });
        File.WriteAllText(JsonFilePath, jsonString);
    }

    internal static string RemovePrefix(string text) =>
        text.Replace(EncryptionConstants.EncryptedValuePrefix, string.Empty);

    internal static string GetEncryptedSettings()
    {
        var jsonString = File.ReadAllText(JsonFilePath);
        var settings = JsonConvert.DeserializeObject<EncryptedSettings>(jsonString);
        return settings.SecretEncKey;
    }
}