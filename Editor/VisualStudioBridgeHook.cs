// Copyright (c) Anton Vasiliev. All rights reserved.
// Licensed under the MIT license.
// See the License.md file in the project root for full license information.

#if ENABLE_VSTU && !DISABLE_VSTU_HOOK
namespace Silvers.CsharpProjectTools
{
    using UnityEditor;

    [InitializeOnLoad]
    public static class VisualStudioBridgeHook
    {
        static VisualStudioBridgeHook()
        {
            // When Visual Studio is installed without "Game development with Unity" payload,
            // ENABLE_VSTU macros is present but SyntaxTree.VisualStudio.Unity.Bridge assembly is not loaded.
            // If you have compilation error here, please check payloads installed with your Visual Studio.
            SyntaxTree.VisualStudio.Unity.Bridge.ProjectFilesGenerator.ProjectFileGeneration += ProcessProjectFile;
        }

        private static string ProcessProjectFile(string path, string content)
        {
            (bool modified, string modifiedContent) = ProjectFileProcessor.ProcessProjectFile(path, content);

            return modified ? modifiedContent : content;
        }
    }
}
#endif
