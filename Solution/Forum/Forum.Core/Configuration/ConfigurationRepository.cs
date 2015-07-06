using System;
using System.Configuration;
using Forum.Core.Contracts;
using Newtonsoft.Json;

namespace Forum.Core.Configuration
{
    public class ConfigurationRepository : IConfigurationRepository
    {
        public TReference GetComplexSetting<TReference>(string settingName) where TReference : class
        {
            var settingValue = ConfigurationManager.AppSettings[settingName];

            return string.IsNullOrWhiteSpace(settingValue) 
                ? null 
                : JsonConvert.DeserializeObject<TReference>(settingValue);
        }

        public TValue GetSimpleSetting<TValue>(string settingName) where TValue : IConvertible
        {
            try
            {
                return (TValue)Convert.ChangeType(ConfigurationManager.AppSettings[settingName], typeof(TValue));
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException(string.Format("Unspecified configuration setting {0}", settingName), exception);
            }
        }

        public TValue GetSimpleSettingOrDefault<TValue>(string settingName, Func<TValue> defaultValue) where TValue : IConvertible
        {
            try
            {
                var setting = (TValue)Convert.ChangeType(ConfigurationManager.AppSettings[settingName], typeof(TValue));

                if (setting == null)
                {
                    return defaultValue();
                }

                return setting;
            }
            catch (Exception)
            {
                return defaultValue();
            }
        }
    }
}
