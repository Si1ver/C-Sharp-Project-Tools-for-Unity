// Copyright (c) Anton Vasiliev. All rights reserved.
// Licensed under the MIT license.
// See the License.md file in the project root for full license information.

namespace Silvers.CsharpProjectTools.Test
{
    using System;
    using JetBrains.Annotations;
    using NUnit.Framework;

    public static class PathUtilitiesTest
    {
        [Test]
        [TestCaseSource(typeof(TestValues), nameof(TestValues.NormalizeSlashesInPath))]
        public static string NormalizeSlashesInPath(string path)
        {
            return PathUtilities.NormalizeSlashesInPath(path);
        }

        [Test]
        public static void NormalizeSlashesInPathNullPath()
        {
            Assert.Throws<ArgumentNullException>(
                () =>
                {
                    PathUtilities.NormalizeSlashesInPath(null);
                },
                "path");
        }

        [UsedImplicitly]
        public static class TestValues
        {
            public static readonly char Valid = PathUtilities.DirectorySeparators.ValidCharacter;

            [UsedImplicitly]
            public static readonly object[] NormalizeSlashesInPath = new TestCaseData[]
            {
                new TestCaseData(string.Empty).Returns(string.Empty),
                new TestCaseData(@"/").Returns(new string(Valid, 1)),
                new TestCaseData(@"\").Returns(new string(Valid, 1)),
                new TestCaseData(@"/foo/").Returns(Valid + "foo" + Valid),
                new TestCaseData(@"/foo\").Returns(Valid + "foo" + Valid),
                new TestCaseData(@"\foo/").Returns(Valid + "foo" + Valid),
                new TestCaseData(@"\foo\").Returns(Valid + "foo" + Valid),
                new TestCaseData(@"foo.bar").Returns("foo.bar"),
                new TestCaseData(@"foo/bar.baz").Returns("foo" + Valid + "bar.baz"),
                new TestCaseData(@"foo\bar.baz").Returns("foo" + Valid + "bar.baz"),
            };
        }
    }
}
