// Copyright (c) Anton Vasiliev. All rights reserved.
// Licensed under the MIT license.
// See the License.md file in the project root for full license information.

namespace Silvers.CsharpProjectTools
{
    using System.Xml.Linq;

    internal static class ProjectFileConstants
    {
        public const string AdditionalFilesElementName = "AdditionalFiles";
        public const string AnalyzerElementName = "Analyzer";
        public const string CompileElementName = "Compile";
        public const string DefineConstantsElenemtName = "DefineConstants";
        public const string HintPathElementName = "HintPath";
        public const string IncludeAssetsElementName = "IncludeAssets";
        public const string ItemGroupElementName = "ItemGroup";
        public const string PackageReferenceElementName = "PackageReference";
        public const string PrivateAssetsElementName = "PrivateAssets";
        public const string ProjectReferenceElementName = "ProjectReference";
        public const string PropertyGroupElementName = "PropertyGroup";
        public const string ReferenceElementName = "Reference";
        public const string TargetElementName = "Target";
        public const string VersionElementName = "Version";

        public const string BeforeTargetsAttributeName = "BeforeTargets";
        public const string IncludeAttributeName = "Include";
        public const string NameAttributeName = "Name";
        public const string RemoveAttributeName = "Remove";

        public const char DefineConstantsSeparator = ';';
        public const string DefineConstantsSeparatorString = ";";

        public static readonly XNamespace MsBuildNamespace = "http://schemas.microsoft.com/developer/msbuild/2003";

        public static readonly XName AdditionalFilesElementXName = MsBuildNamespace + AdditionalFilesElementName;
        public static readonly XName AnalyzerElementXName = MsBuildNamespace + AnalyzerElementName;
        public static readonly XName CompileElementXName = MsBuildNamespace + CompileElementName;
        public static readonly XName DefineConstantsElenemtXName = MsBuildNamespace + DefineConstantsElenemtName;
        public static readonly XName HintPathElementXName = MsBuildNamespace + HintPathElementName;
        public static readonly XName IncludeAssetsElementXName = MsBuildNamespace + IncludeAssetsElementName;
        public static readonly XName ItemGroupElementXName = MsBuildNamespace + ItemGroupElementName;
        public static readonly XName PackageReferenceElementXName = MsBuildNamespace + PackageReferenceElementName;
        public static readonly XName PrivateAssetsElementXName = MsBuildNamespace + PrivateAssetsElementName;
        public static readonly XName ProjectReferenceElementXName = MsBuildNamespace + ProjectReferenceElementName;
        public static readonly XName PropertyGroupElementXName = MsBuildNamespace + PropertyGroupElementName;
        public static readonly XName ReferenceElementXName = MsBuildNamespace + ReferenceElementName;
        public static readonly XName TargetElementXName = MsBuildNamespace + TargetElementName;
        public static readonly XName VersionElementXName = MsBuildNamespace + VersionElementName;

        public static readonly XName BeforeTargetsAttributeXName = BeforeTargetsAttributeName;
        public static readonly XName IncludeAttributeXName = IncludeAttributeName;
        public static readonly XName NameAttributeXName = NameAttributeName;
        public static readonly XName RemoveAttributeXName = RemoveAttributeName;

        public static readonly char[] DefineConstantsSeparatorArray = new char[] { DefineConstantsSeparator };
    }
}
