// Copyright (c) Anton Vasiliev. All rights reserved.
// Licensed under the MIT license.
// See the License.md file in the project root for full license information.

namespace Silvers.CsharpProjectTools
{
    using UnityEditor;
    using UnityEngine;

    public class CsharpProjectToolsSettings : ScriptableObject
    {
        private const bool DefaultCleanupProjectValue = true;
        private const bool DefaultAddStyleCopValue = true;
        private const bool DefaultVerboseLoggingValue = false;

        private const string SettingsAssetPath = @"Assets/Silvers/CsharpProjectTools/Editor/CsharpProjectToolsSettings.asset";

        [SerializeField]
        private bool cleanupProject = DefaultCleanupProjectValue;

        [SerializeField]
        private bool addStyleCop = DefaultAddStyleCopValue;

        [SerializeField]
        private bool verboseLogging = DefaultVerboseLoggingValue;

        public static CsharpProjectToolsSettings Load
        {
            get
            {
                return LoadOrCreateInstance();
            }
        }

        public bool CleanupProject
        {
            get { return cleanupProject; }
            set { cleanupProject = value; }
        }

        public bool AddStyleCop
        {
            get { return addStyleCop; }
            set { addStyleCop = value; }
        }

        public bool VerboseLogging
        {
            get { return verboseLogging; }
            set { verboseLogging = value; }
        }

        public static void EnsureDirectoryExists(string directory)
        {
            if (!AssetDatabase.IsValidFolder(directory))
            {
                string parentDirectory = UnityPathUtilities.GetDirectoryName(directory);
                string directoryName = UnityPathUtilities.GetFileName(directory);

                EnsureDirectoryExists(parentDirectory);
                AssetDatabase.CreateFolder(parentDirectory, directoryName);
            }
        }

        public static CsharpProjectToolsSettings LoadOrCreateInstance()
        {
            CsharpProjectToolsSettings settings;

            string assetSettingsDirectory = UnityPathUtilities.GetDirectoryName(SettingsAssetPath);
            EnsureDirectoryExists(assetSettingsDirectory);

            settings = AssetDatabase.LoadAssetAtPath<CsharpProjectToolsSettings>(SettingsAssetPath);

            if (settings != null)
            {
                return settings;
            }

            settings = CreateInstance<CsharpProjectToolsSettings>();

            AssetDatabase.CreateAsset(settings, SettingsAssetPath);

            return settings;
        }
    }
}
