// Copyright (c) Anton Vasiliev. All rights reserved.
// Licensed under the MIT license.
// See the License.md file in the project root for full license information.

namespace Silvers.CsharpProjectTools
{
    using System.IO;
    using JetBrains.Annotations;

    public static class PathUtilities
    {
        [NotNull]
        public static string NormalizeSlashesInPath([NotNull] string path)
        {
            Verify.ArgumentNotNull(path, nameof(path));

            return path.Replace(DirectorySeparators.InvalidCharacter, DirectorySeparators.ValidCharacter);
        }

        public static class DirectorySeparators
        {
            public static readonly char ValidCharacter = Path.DirectorySeparatorChar;
            public static readonly char InvalidCharacter = ValidCharacter == '/' ? '\\' : '/';
        }
    }
}
