// Copyright (c) Anton Vasiliev. All rights reserved.
// Licensed under the MIT license.
// See the License.md file in the project root for full license information.

namespace Silvers.CsharpProjectTools
{
    using UnityEditor;

    [InitializeOnLoad]
    public static class VisualStudioBridgeHook
    {
        static VisualStudioBridgeHook()
        {
#if ENABLE_VSTU
            // When Visual Studio is installed without "Game development with Unity" payload,
            // ENABLE_VSTU macros is present but SyntaxTree.VisualStudio.Unity.Bridge assembly is not loaded.
            // If you have compilation error here, please check payloads installed with your Visual Studio.
            SyntaxTree.VisualStudio.Unity.Bridge.ProjectFilesGenerator.ProjectFileGeneration += ProcessProjectFile;
#else
            if (UnityEngine.Application.platform == UnityEngine.RuntimePlatform.WindowsEditor)
            {
                UnityEngine.Debug.LogWarning("C# Project Tools can't hook up to Visual Studio bridge because Visual Studio Tools for Unity is not available. Project file processing is disabled.");
            }
#endif
        }

        private static string ProcessProjectFile(string path, string content)
        {
            return ProjectFileProcessor.ProcessProjectFile(path, content);
        }
    }
}
