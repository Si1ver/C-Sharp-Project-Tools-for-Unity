// Copyright (c) Anton Vasiliev. All rights reserved.
// Licensed under the MIT license.
// See the License.md file in the project root for full license information.

namespace Silvers.CsharpProjectTools
{
    using System;
    using JetBrains.Annotations;

    public static class Verify
    {
        public static void ArgumentNotNull<T>([CanBeNull][NoEnumeration] T parameter, [InvokerParameterName][NotNull] string parameterName)
            where T : class
        {
            if (parameter == null)
            {
                throw new ArgumentNullException(parameterName);
            }
        }
    }
}
