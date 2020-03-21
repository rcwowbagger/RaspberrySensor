using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.FileProviders;
using System;
using System.ComponentModel;
using System.Linq;

namespace RaspberrySensor.Configuration
{
    public class ConfigurationHandler
    {
        private static IConfigurationProvider _configurationProvider;
        public static void Initialise()
        {
            var jsonConfig = new JsonConfigurationSource 
            {
                ReloadOnChange = true,
                Optional = false,
                FileProvider = new PhysicalFileProvider(Environment.CurrentDirectory),
                Path = "appsettings.json"
            };

            _configurationProvider = new JsonConfigurationProvider(jsonConfig);
            _configurationProvider.Load();
        }

        public static bool TryGet(string key, out string value)
        {
            return _configurationProvider.TryGet(key, out value);
        }

        

        public static T Get <T>(string key)
        {
            try
            {
                if (!_configurationProvider.TryGet(key, out string value))
                {
                    throw new ConfigurationException($"Key {key} not found in configuration file");
                }

                return TConverter.ChangeType<T>(value);

            }
            catch (ConfigurationException e)
            {
                throw e;
            }
            catch (InvalidCastException ex)
            {
                throw new Exception($"Can't cast value for key {key} to Type {typeof(T)}", ex);
            }
        }
    }

    //https://stackoverflow.com/a/1833128/3978872
    public static class TConverter
    {
        public static T ChangeType<T>(object value)
        {
            return (T)ChangeType(typeof(T), value);
        }

        public static object ChangeType(Type t, object value)
        {
            TypeConverter tc = TypeDescriptor.GetConverter(t);
            return tc.ConvertFrom(value);
        }

        public static void RegisterTypeConverter<T, TC>() where TC : TypeConverter
        {

            TypeDescriptor.AddAttributes(typeof(T), new TypeConverterAttribute(typeof(TC)));
        }
    }

    public class ConfigurationException : Exception
    {
        public ConfigurationException(string message) :
            base(message)
        {

        }
    }
}
