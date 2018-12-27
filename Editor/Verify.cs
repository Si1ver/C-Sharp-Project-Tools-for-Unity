// Copyright (c) Anton Vasiliev. All rights reserved.
// Licensed under the MIT license.
// See the License.md file in the project root for full license information.

namespace Silvers.CsharpProjectTools
{
    using System;
    using JetBrains.Annotations;

    public static class Verify
    {
        [Pure]
        public static void ArgumentNotNull<T>([NotNull, NoEnumeration] T parameter, [NotNull] string parameterName)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException(parameterName);
            }
        }
    }
}
