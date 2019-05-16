// Copyright (c) Anton Vasiliev. All rights reserved.
// Licensed under the MIT license.
// See the License.md file in the project root for full license information.

namespace Silvers.CsharpProjectTools
{
    using System;
    using System.IO;
    using UnityEngine;

    [Serializable]
    public class CsharpProjectToolsSettings
    {
        public bool DisableCleanupProject;

        public bool DisableAddStyleCopAnalyzer;

        public bool EnableVerboseLogging;

        private const string SettingsFileName = @"CsharpProjectToolsSettings.json";

        public static CsharpProjectToolsSettings Load()
        {
            return LoadOrCreateInstance();
        }

        public void Save()
        {
            string settingsFileFullPath = Path.Combine(UnityPathUtilities.UnityProjectSettingsDirectory, SettingsFileName);

            string serializedSettings = JsonUtility.ToJson(this, true);

            File.WriteAllText(settingsFileFullPath, serializedSettings);
        }

        private static CsharpProjectToolsSettings LoadOrCreateInstance()
        {
            string settingsFileFullPath = Path.Combine(UnityPathUtilities.UnityProjectSettingsDirectory, SettingsFileName);

            if (File.Exists(settingsFileFullPath))
            {
                string settingsFileContent = File.ReadAllText(settingsFileFullPath);

                return JsonUtility.FromJson<CsharpProjectToolsSettings>(settingsFileContent);
            }
            else
            {
                var settings = new CsharpProjectToolsSettings();

                settings.Save();

                return settings;
            }
        }
    }
}
