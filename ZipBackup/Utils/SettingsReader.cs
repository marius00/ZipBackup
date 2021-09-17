using System;
using System.IO;
using log4net;
using Newtonsoft.Json;
using ZipBackup.Services;

namespace ZipBackup.Utils {
    class SettingsReader {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(SettingsReader));

        private static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Culture = System.Globalization.CultureInfo.InvariantCulture,
            ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver(),
            NullValueHandling = NullValueHandling.Ignore
        };

        private readonly SettingsService _data;
        private readonly string _persistentStorage;

        public SettingsReader(SettingsService data, string persistentStorage) {
            _persistentStorage = persistentStorage;
            _data = data;

            _data.OnMutate += (_, __) => Persist();
        }

        private void Persist() {
            string json = JsonConvert.SerializeObject(_data, JsonSerializerSettings);
            try {
                File.WriteAllText(_persistentStorage, json);
            }
            catch (Exception ex) {
                Logger.Warn("Error storing settings, once of twice of these is unfortunate but can live with it..", ex);
            }
        }

        public SettingsService Settings => _data;

        public static SettingsReader Load(string filename) {
            if (File.Exists(filename)) {
                try {
                    string json = File.ReadAllText(filename);
                    var template = JsonConvert.DeserializeObject<SettingsService>(json, JsonSerializerSettings);
                    if (template != null) {
                        return new SettingsReader(template, filename);
                    }
                }
                catch (IOException ex) {
                    Logger.Error($"Error reading settings from {filename}, discarding settings.", ex);
                }
                catch (JsonReaderException ex) {
                    Logger.Error($"Error parsing settings from {filename}, discarding settings.", ex);
                }
            }

            Logger.Info("Could not find settings JSON, defaulting to no settings.");
            return new SettingsReader(new SettingsService {
                FilenamePattern = "dddd"
            }, filename);
        }
    }
}