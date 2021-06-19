using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Json;

namespace ScrapMetal.Configuration
{
    public class PersistentConfiguration
    {
        internal IEnumerable<PropertyInfo> _properties;

        public PersistentConfiguration()
        {
            _properties = typeof(PersistentConfiguration).GetProperties();
        }

        public PersistentConfiguration Write()
        {
            using var fs = new FileStream("config", FileMode.Open, FileAccess.Write, FileShare.Read);
            using (var sw = new StreamWriter(fs))
            {
                sw.Write(JsonSerializer.Serialize(this));
            }

            return this;
        }

        public static TPersistentConfiguration Load<TPersistentConfiguration>() where TPersistentConfiguration : PersistentConfiguration, new()
        {
            string cfg = null;
            using var fs = new FileStream("config", FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read);
            using (var sr = new StreamReader(fs))
            {
                cfg = sr.ReadToEnd();
            }

            return JsonSerializer.Deserialize<TPersistentConfiguration>(cfg) ?? (TPersistentConfiguration)new TPersistentConfiguration().Write();
        }
    }
}