// Copyright (c) Anton Vasiliev. All rights reserved.
// Licensed under the MIT license.
// See the License.md file in the project root for full license information.

namespace Silvers.CsharpProjectTools.Test
{
    using System;
    using NUnit.Framework;

    public static class VerifyTest
    {
        [Test]
        public static void ArgumentNotNull()
        {
            string someArgument = null;

            Assert.Throws<ArgumentNullException>(
                () =>
                {
                    Verify.ArgumentNotNull(someArgument, nameof(someArgument));
                },
                nameof(someArgument));

            someArgument = "foo";

            Verify.ArgumentNotNull(someArgument, nameof(someArgument));
        }
    }
}
