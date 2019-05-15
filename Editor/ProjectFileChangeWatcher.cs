// Copyright (c) Anton Vasiliev. All rights reserved.
// Licensed under the MIT license.
// See the License.md file in the project root for full license information.

namespace Silvers.CsharpProjectTools
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Timers;
    using JetBrains.Annotations;
    using UnityEngine;

    public class ProjectFileChangeWatcher
    {
        private const double ProcessFileDelay = 2500.0;

        private const string CsprojFilter = "*.csproj";

        [NotNull]
        private readonly Timer changeTimeoutTimer;

        [NotNull]
        private readonly FileSystemWatcher fileSystemWatcher;

        [NotNull]
        private readonly object lockObject;

        [NotNull]
        private HashSet<string> filesToProcess;

        public ProjectFileChangeWatcher()
        {
            lockObject = new object();

            filesToProcess = new HashSet<string>(StringComparer.Ordinal);

            changeTimeoutTimer = new Timer
            {
                AutoReset = false,
                Interval = ProcessFileDelay,
            };

            changeTimeoutTimer.Elapsed += OnTimerElapsed;

            fileSystemWatcher = new FileSystemWatcher
            {
                Path = UnityPathUtilities.UnityProjectRootDirectory,
                NotifyFilter = NotifyFilters.LastWrite,
                Filter = CsprojFilter,
            };

            fileSystemWatcher.Changed += OnFileChanged;
            fileSystemWatcher.Created += OnFileChanged;
            fileSystemWatcher.Deleted += OnFileChanged;

            fileSystemWatcher.EnableRaisingEvents = true;
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs arguments)
        {
            HashSet<string> filesToProcessCopy;

            lock (lockObject)
            {
                filesToProcessCopy = filesToProcess;

                filesToProcess = new HashSet<string>(StringComparer.Ordinal);
            }

            foreach (string file in filesToProcessCopy)
            {
                string projectDirectory = UnityPathUtilities.UnityProjectRootDirectory;

                try
                {
                    string absoluteFilePath = Path.Combine(projectDirectory, file);

                    string fileContent = File.ReadAllText(absoluteFilePath);

                    (bool modified, string modifiedContent) = ProjectFileProcessor.ProcessProjectFile(absoluteFilePath, fileContent);

                    if (!modified)
                    {
                        continue;
                    }

                    File.WriteAllText(absoluteFilePath, modifiedContent);
                }
                catch (Exception exception)
                {
                    Debug.LogError($"Failed to cleanup C# project file {file} with error '{exception.Message}'");
                }
            }
        }

        private void OnFileChanged(object sender, FileSystemEventArgs arguments)
        {
            changeTimeoutTimer.Stop();

            lock (lockObject)
            {
                if (arguments.ChangeType != WatcherChangeTypes.Deleted)
                {
                    filesToProcess.Add(arguments.Name);
                }
                else
                {
                    filesToProcess.Remove(arguments.Name);
                }
            }

            changeTimeoutTimer.Start();
        }
    }
}
