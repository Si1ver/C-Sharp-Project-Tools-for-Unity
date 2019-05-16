// Copyright (c) Anton Vasiliev. All rights reserved.
// Licensed under the MIT license.
// See the License.md file in the project root for full license information.

namespace Silvers.CsharpProjectTools
{
    using System.IO;
    using JetBrains.Annotations;
    using UnityEngine;

    public static class UnityPathUtilities
    {
        [NotNull]
        public static readonly string UnityProjectRootDirectory;

        [NotNull]
        public static readonly string UnityLibraryDirectory;

        [NotNull]
        public static readonly string UnityAssetsDirectory;

        [NotNull]
        public static readonly string UnityPackagesDirectory;

        [NotNull]
        public static readonly string UnityProjectSettingsDirectory;

        [NotNull]
        public static readonly string UnityTempDirectory;

        static UnityPathUtilities()
        {
            UnityProjectRootDirectory = GetProjectRootDirectory();

            UnityAssetsDirectory = Path.Combine(UnityProjectRootDirectory, "Assets");

            UnityLibraryDirectory = Path.Combine(UnityProjectRootDirectory, "Library");

            UnityPackagesDirectory = Path.Combine(UnityProjectRootDirectory, "Packages");

            UnityProjectSettingsDirectory = Path.Combine(UnityProjectRootDirectory, "ProjectSettings");

            UnityTempDirectory = Path.Combine(UnityProjectRootDirectory, "Temp");
        }

        [NotNull]
        public static string NormalizeSlashesInPath([NotNull] string path)
        {
            Verify.ArgumentNotNull(path, nameof(path));

            if (path.IndexOf(DirectorySeparators.InvalidCharacter) != -1)
            {
                return path.Replace(DirectorySeparators.InvalidCharacter, DirectorySeparators.ValidCharacter);
            }
            else
            {
                return path;
            }
        }

        [NotNull]
        public static string GetDirectoryName([NotNull] string path)
        {
            Verify.ArgumentNotNull(path, nameof(path));

            string normalizedPath = NormalizeSlashesInPath(path);

            int lastSlashPosition = normalizedPath.LastIndexOf(DirectorySeparators.ValidCharacter);

            if (lastSlashPosition < 1)
            {
                return string.Empty;
            }
            else
            {
                return normalizedPath.Substring(0, lastSlashPosition);
            }
        }

        [NotNull]
        public static string Combine([NotNull] string path1, [NotNull] string path2)
        {
            Verify.ArgumentNotNull(path1, nameof(path1));
            Verify.ArgumentNotNull(path2, nameof(path2));

            string normalizedPath1 = NormalizeSlashesInPath(path1).Trim(DirectorySeparators.ValidCharacterArray);
            string normalizedPath2 = NormalizeSlashesInPath(path2).Trim(DirectorySeparators.ValidCharacterArray);

            if (normalizedPath1.Length == 0)
            {
                return normalizedPath2;
            }

            if (normalizedPath2.Length == 0)
            {
                return normalizedPath1;
            }

            return $"{normalizedPath1}{DirectorySeparators.ValidCharacter}{normalizedPath2}";
        }

        [NotNull]
        public static string GetFileName([NotNull] string path)
        {
            Verify.ArgumentNotNull(path, nameof(path));

            string normalizedPath = NormalizeSlashesInPath(path);

            int nameLength = GetFileNameLength(normalizedPath);

            if (nameLength == 0)
            {
                return string.Empty;
            }
            else
            {
                return normalizedPath.Substring(normalizedPath.Length - nameLength);
            }
        }

        [NotNull]
        public static string GetFileNameWithoutExtension([NotNull] string path)
        {
            Verify.ArgumentNotNull(path, nameof(path));

            string normalizedPath = NormalizeSlashesInPath(path);

            int nameLength = GetFileNameLength(normalizedPath);

            if (nameLength == 0)
            {
                return string.Empty;
            }

            int lastCharacterPosition = normalizedPath.Length - 1;
            int lastDotPosition = normalizedPath.LastIndexOf('.', lastCharacterPosition, nameLength);

            if (lastDotPosition == -1)
            {
                return normalizedPath.Substring(normalizedPath.Length - nameLength);
            }
            else
            {
                int extensionLength = lastCharacterPosition - lastDotPosition;
                int nameWithoutExtensionLength = nameLength - extensionLength - 1;

                if (nameWithoutExtensionLength == 0)
                {
                    return string.Empty;
                }
                else
                {
                    return normalizedPath.Substring(normalizedPath.Length - nameLength, nameWithoutExtensionLength);
                }
            }
        }

        [NotNull]
        private static string GetProjectRootDirectory()
        {
            string normalizedAssetsDirectory = PathUtilities.NormalizeSlashesInPath(Application.dataPath);

            string assetsDirectory = PathUtilities.RemoveTrailingSlashIfPresent(normalizedAssetsDirectory);

            return Path.GetDirectoryName(assetsDirectory) ?? string.Empty;
        }

        private static int GetFileNameLength([NotNull] string normalizedPath)
        {
            Verify.ArgumentNotNull(normalizedPath, nameof(normalizedPath));

            int lastSlashPosition = normalizedPath.LastIndexOf(DirectorySeparators.ValidCharacter);

            if (lastSlashPosition == -1)
            {
                return normalizedPath.Length;
            }
            else
            {
                int lastCharacterPosition = normalizedPath.Length - 1;
                return lastCharacterPosition - lastSlashPosition;
            }
        }

        public static class DirectorySeparators
        {
            public const char ValidCharacter = '/';
            public const char InvalidCharacter = '\\';

            public static readonly char[] ValidCharacterArray = new char[] { '/' };
        }
    }
}
