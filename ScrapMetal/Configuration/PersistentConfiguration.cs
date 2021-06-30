using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.Json;

namespace ScrapMetal.Configuration
{
    public class PersistentConfiguration
    {
        public PersistentConfiguration Write()
        {
            using var fs = new FileStream("config", FileMode.Open, FileAccess.Write, FileShare.Read);
            using (var sw = new StreamWriter(fs))
            {
                sw.Write(JsonSerializer.Serialize(this, GetType(), new JsonSerializerOptions
                {
                    WriteIndented = true,
                }));
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

            if (string.IsNullOrWhiteSpace(cfg))
            {
                return (TPersistentConfiguration)new TPersistentConfiguration().Write();
            }

            return JsonSerializer.Deserialize<TPersistentConfiguration>(cfg) ?? (TPersistentConfiguration)new TPersistentConfiguration().Write();
        }
    }
}