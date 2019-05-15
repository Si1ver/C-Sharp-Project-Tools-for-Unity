// Copyright (c) Anton Vasiliev. All rights reserved.
// Licensed under the MIT license.
// See the License.md file in the project root for full license information.

#if !ENABLE_VSTU || DISABLE_VSTU_HOOK
namespace Silvers.CsharpProjectTools
{
    using UnityEditor;

    [InitializeOnLoad]
    public static class FileWatcherHook
    {
        private static readonly ProjectFileChangeWatcher projectFileChangeWatcher;

        static FileWatcherHook()
        {
            if (projectFileChangeWatcher == null)
            {
                projectFileChangeWatcher = new ProjectFileChangeWatcher();
            }
        }
    }
}
#endif
