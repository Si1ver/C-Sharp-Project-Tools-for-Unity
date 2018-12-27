// Copyright (c) Anton Vasiliev. All rights reserved.
// Licensed under the MIT license.
// See the License.md file in the project root for full license information.

namespace Silvers.CsharpProjectTools.Test
{
    using System;
    using JetBrains.Annotations;
    using NUnit.Framework;

    public static class UnityPathUtilitiesTest
    {
        [Test]
        [TestCaseSource(typeof(TestValues), nameof(TestValues.NormalizeSlashesInPath))]
        public static string NormalizeSlashesInPath(string path)
        {
            return UnityPathUtilities.NormalizeSlashesInPath(path);
        }

        [Test]
        public static void NormalizeSlashesInPathNullPath()
        {
            Assert.Throws<ArgumentNullException>(
                () =>
                {
                    UnityPathUtilities.NormalizeSlashesInPath(null);
                },
                "path");
        }

        [Test]
        [TestCaseSource(typeof(TestValues), nameof(TestValues.GetDirectoryName))]
        public static string GetDirectoryName(string path)
        {
            return UnityPathUtilities.GetDirectoryName(path);
        }

        [Test]
        public static void GetDirectoryNameNullPath()
        {
            Assert.Throws<ArgumentNullException>(
                () =>
                {
                    UnityPathUtilities.GetDirectoryName(null);
                },
                "path");
        }

        [Test]
        [TestCaseSource(typeof(TestValues), nameof(TestValues.Combine))]
        public static string Combine(string path1, string path2)
        {
            return UnityPathUtilities.Combine(path1, path2);
        }

        [Test]
        public static void CombineNullPath()
        {
            Assert.Throws<ArgumentNullException>(
                () =>
                {
                    UnityPathUtilities.Combine(null, "foo");
                },
                "path1");

            Assert.Throws<ArgumentNullException>(
                () =>
                {
                    UnityPathUtilities.Combine("foo", null);
                },
                "path2");
        }

        [Test]
        [TestCaseSource(typeof(TestValues), nameof(TestValues.GetFileName))]
        public static string GetFileName(string path)
        {
            return UnityPathUtilities.GetFileName(path);
        }

        [Test]
        public static void GetFileNameNullPath()
        {
            Assert.Throws<ArgumentNullException>(
                () =>
                {
                    UnityPathUtilities.GetFileName(null);
                },
                "path");
        }

        [Test]
        [TestCaseSource(typeof(TestValues), nameof(TestValues.GetFileNameWithoutExtension))]
        public static string GetFileNameWithoutExtension(string path)
        {
            return UnityPathUtilities.GetFileNameWithoutExtension(path);
        }

        [Test]
        public static void GetFileNameWithoutExtensionNullPath()
        {
            Assert.Throws<ArgumentNullException>(
                () =>
                {
                    UnityPathUtilities.GetFileNameWithoutExtension(null);
                },
                "path");
        }

        [UsedImplicitly]
        public static class TestValues
        {
            public static readonly char Valid = UnityPathUtilities.DirectorySeparators.ValidCharacter;

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

            [UsedImplicitly]
            public static readonly object[] GetDirectoryName = new TestCaseData[]
            {
                new TestCaseData(string.Empty).Returns(string.Empty),
                new TestCaseData(@"/").Returns(string.Empty),
                new TestCaseData(@"\").Returns(string.Empty),
                new TestCaseData(@"/foo/").Returns(Valid + "foo"),
                new TestCaseData(@"/foo\").Returns(Valid + "foo"),
                new TestCaseData(@"\foo/").Returns(Valid + "foo"),
                new TestCaseData(@"\foo\").Returns(Valid + "foo"),
                new TestCaseData(@"foo.bar").Returns(string.Empty),
                new TestCaseData(@"/foo.bar").Returns(string.Empty),
                new TestCaseData(@"\foo.bar").Returns(string.Empty),
                new TestCaseData(@"foo/bar.baz").Returns("foo"),
                new TestCaseData(@"foo\bar.baz").Returns("foo"),
                new TestCaseData(@"foo\bar\baz").Returns("foo" + Valid + "bar"),
                new TestCaseData(@"foo\bar/baz").Returns("foo" + Valid + "bar"),
                new TestCaseData(@"foo/bar\baz").Returns("foo" + Valid + "bar"),
                new TestCaseData(@"foo/bar/baz").Returns("foo" + Valid + "bar"),
            };

            [UsedImplicitly]
            public static readonly object[] Combine = new TestCaseData[]
            {
                new TestCaseData(string.Empty, string.Empty).Returns(string.Empty),
                new TestCaseData(@"/", string.Empty).Returns(string.Empty),
                new TestCaseData(@"\", string.Empty).Returns(string.Empty),
                new TestCaseData(string.Empty, @"/").Returns(string.Empty),
                new TestCaseData(string.Empty, @"\").Returns(string.Empty),
                new TestCaseData(@"/foo/", string.Empty).Returns("foo"),
                new TestCaseData(string.Empty, @"/foo/").Returns("foo"),
                new TestCaseData(@"foo", string.Empty).Returns("foo"),
                new TestCaseData(string.Empty, @"foo").Returns("foo"),
                new TestCaseData(@"foo", @"/").Returns("foo"),
                new TestCaseData(@"/", @"foo").Returns("foo"),
                new TestCaseData(@"foo", @"bar").Returns("foo" + Valid + "bar"),
                new TestCaseData(@"foo", @"bar/baz").Returns("foo" + Valid + "bar" + Valid + "baz"),
                new TestCaseData(@"foo/bar", @"baz").Returns("foo" + Valid + "bar" + Valid + "baz"),
                new TestCaseData(@"foo/bar", @"baz/qux").Returns("foo" + Valid + "bar" + Valid + "baz" + Valid + "qux"),
            };

            [UsedImplicitly]
            public static readonly object[] GetFileName = new TestCaseData[]
            {
                new TestCaseData(string.Empty).Returns(string.Empty),
                new TestCaseData(@"/").Returns(string.Empty),
                new TestCaseData(@"\").Returns(string.Empty),
                new TestCaseData(@"/foo/").Returns(string.Empty),
                new TestCaseData(@"/foo\").Returns(string.Empty),
                new TestCaseData(@"\foo/").Returns(string.Empty),
                new TestCaseData(@"\foo\").Returns(string.Empty),
                new TestCaseData(@"foo").Returns("foo"),
                new TestCaseData(@"\foo").Returns("foo"),
                new TestCaseData(@"/foo").Returns("foo"),
                new TestCaseData(@"foo.bar").Returns("foo.bar"),
                new TestCaseData(@"/foo.bar").Returns("foo.bar"),
                new TestCaseData(@"foo/bar.baz").Returns("bar.baz"),
                new TestCaseData(@"foo/bar/baz").Returns("baz"),
            };

            [UsedImplicitly]
            public static readonly object[] GetFileNameWithoutExtension = new TestCaseData[]
            {
                new TestCaseData(string.Empty).Returns(string.Empty),
                new TestCaseData(@"/").Returns(string.Empty),
                new TestCaseData(@"\").Returns(string.Empty),
                new TestCaseData(@"/foo/").Returns(string.Empty),
                new TestCaseData(@"/foo\").Returns(string.Empty),
                new TestCaseData(@"\foo/").Returns(string.Empty),
                new TestCaseData(@"\foo\").Returns(string.Empty),
                new TestCaseData(@"foo").Returns("foo"),
                new TestCaseData(@"/foo").Returns("foo"),
                new TestCaseData(@"foo.bar").Returns("foo"),
                new TestCaseData(@"/foo.bar").Returns("foo"),
                new TestCaseData(@"foo.bar.baz").Returns("foo.bar"),
                new TestCaseData(@"foo/bar.baz").Returns("bar"),
                new TestCaseData(@"foo/bar/baz").Returns("baz"),
                new TestCaseData(@".").Returns(string.Empty),
                new TestCaseData(@"/.").Returns(string.Empty),
                new TestCaseData(@".foo").Returns(string.Empty),
                new TestCaseData(@"/.foo").Returns(string.Empty),
                new TestCaseData(@"foo.").Returns("foo"),
                new TestCaseData(@"/foo.").Returns("foo"),
                new TestCaseData(@"./foo.").Returns("foo"),
            };
        }
    }
}
