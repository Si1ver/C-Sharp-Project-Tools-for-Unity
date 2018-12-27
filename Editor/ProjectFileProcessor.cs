// Copyright (c) Anton Vasiliev. All rights reserved.
// Licensed under the MIT license.
// See the License.md file in the project root for full license information.

namespace Silvers.CsharpProjectTools
{
    using System;
    using System.Diagnostics;
    using System.Text;
    using JetBrains.Annotations;
    using UnityEditor;
    using UnityEditor.Compilation;
    using UnityEngine;

    public static class ProjectFileProcessor
    {
        private const string AssetsDirectory = "Assets";

        private const string TestScriptFileName = "TestScript.cs";

        private const string StyleCopAnalyzersPackageName = "StyleCop.Analyzers";
        private const string StyleCopAnalyzersPackageVersion = "1.1.1-beta.61";
        private const string StyleCopAnalyzersUnstablePackageName = "StyleCop.Analyzers.Unstable";
        private const string StyleCopAnalyzersUnstablePackageVersion = "1.1.1.61";

        private const string StyleCopAlanyzersIncludeAssets = "runtime; build; native; contentfiles; analyzers";
        private const string StyleCopAlanyzersPrivateAssets = "all";

        private const string StylecopJsonFilePath = @"stylecop.json";

        public static string ProcessProjectFile(string path, string content)
        {
            string modifiedContent;

            CsharpProjectToolsSettings settings = CsharpProjectToolsSettings.Load;

            var executionStopwatch = new Stopwatch();
            executionStopwatch.Start();

            var report = new ProjectProcessingReport();
            report.ProjectFilePath = path;

            try
            {
                string assemblyDirectory = GetAssemblyDirectoryFromProjectFile(path);
                report.AssemblyDirectory = assemblyDirectory;

                string styleCopFilePath = UnityPathUtilities.Combine(assemblyDirectory, "stylecop.json");
                TextAsset styleCopFileAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(styleCopFilePath);

                var projectFileModifier = new ProjectFileModifier(content);

                if (settings.AddStyleCop)
                {
                    if (styleCopFileAsset != null)
                    {
                        AddStyleCopPackage(projectFileModifier, assemblyDirectory);
                    }
                    else
                    {
                        AddRemoveAnalyzersTask(projectFileModifier);
                    }
                }

                projectFileModifier.Compose();

                modifiedContent = projectFileModifier.GetContent();
            }
            catch (SystemException exception)
            {
                report.Exception = exception;

                modifiedContent = content;
            }

            executionStopwatch.Stop();
            report.ExecutionTimeMs = executionStopwatch.ElapsedMilliseconds;

            if (settings.VerboseLogging || report.Exception != null)
            {
                PrintReport(report);
            }

            return modifiedContent;
        }

        private static void PrintReport([NotNull] ProjectProcessingReport report)
        {
            Verify.ArgumentNotNull(report, nameof(report));

            var builder = new StringBuilder();

            if (report.Exception != null)
            {
                builder.AppendLine($"Failed to process project file {report.ProjectFilePath} in {report.ExecutionTimeMs} ms.");
                builder.AppendLine($"Exception: {report.Exception.Message}.");
                builder.AppendLine($"Stack trace: {report.Exception.StackTrace}.");

                UnityEngine.Debug.LogError(builder.ToString());
            }
            else
            {
                builder.AppendLine($"Processed project file {report.ProjectFilePath} in {report.ExecutionTimeMs} ms.");
                builder.AppendLine($"Assembly directory: {report.AssemblyDirectory}");

                UnityEngine.Debug.Log(builder.ToString());
            }
        }

        private static void AddStyleCopPackage([NotNull] ProjectFileModifier projectFileModifier, [NotNull] string assemblyDefinitionDirectory)
        {
            Verify.ArgumentNotNull(projectFileModifier, nameof(projectFileModifier));
            Verify.ArgumentNotNull(assemblyDefinitionDirectory, nameof(assemblyDefinitionDirectory));

            string styleCopFilePath = UnityPathUtilities.Combine(assemblyDefinitionDirectory, StylecopJsonFilePath);

            projectFileModifier.AddPackageReferenceProjectItem(StyleCopAnalyzersPackageName, StyleCopAnalyzersPackageVersion, StyleCopAlanyzersIncludeAssets, StyleCopAlanyzersPrivateAssets);
            projectFileModifier.AddPackageReferenceProjectItem(StyleCopAnalyzersUnstablePackageName, StyleCopAnalyzersUnstablePackageVersion, StyleCopAlanyzersIncludeAssets, StyleCopAlanyzersPrivateAssets);

            projectFileModifier.AddAdditionalFileProjectItem(styleCopFilePath);
        }

        private static void AddRemoveAnalyzersTask([NotNull] ProjectFileModifier projectFileModifier)
        {
            Verify.ArgumentNotNull(projectFileModifier, nameof(projectFileModifier));

            projectFileModifier.AddRemoveAnalyzersTarget();
        }

        [NotNull]
        private static string GetAssemblyDirectoryFromProjectFile([NotNull] string path)
        {
            Verify.ArgumentNotNull(path, nameof(path));

            string assemblyName = UnityPathUtilities.GetFileNameWithoutExtension(path);

            string assemblyDefinitionFile = CompilationPipeline.GetAssemblyDefinitionFilePathFromAssemblyName(assemblyName);

            if (string.IsNullOrEmpty(assemblyDefinitionFile))
            {
                return AssetsDirectory;
            }
            else
            {
                return UnityPathUtilities.GetDirectoryName(assemblyDefinitionFile);
            }
        }
    }
}
