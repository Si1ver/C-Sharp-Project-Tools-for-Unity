// Copyright (c) Anton Vasiliev. All rights reserved.
// Licensed under the MIT license.
// See the License.md file in the project root for full license information.

namespace Silvers.CsharpProjectTools
{
    using System;
    using System.Collections.Generic;
    using System.Xml;
    using System.Xml.Linq;
    using JetBrains.Annotations;

    internal sealed class ProjectFileModifier
    {
        private const string ProcessedDocumentCommentText = "Document is processed by Silver's C# Project Tools.";

        private XDocument projectFileDocument;

        private List<XElement> compiles;
        private List<XElement> references;
        private List<XElement> projectReferences;
        private List<XElement> packageReferences;
        private List<XElement> analyzers;
        private List<XElement> additionalFiles;
        private List<XElement> otherElements;

        public ProjectFileModifier([NotNull] string projectFileContent)
        {
            Verify.ArgumentNotNull(projectFileContent, nameof(projectFileContent));

            projectFileDocument = XDocument.Parse(projectFileContent);
        }

        public bool IsAlreadyProcessed()
        {
            foreach (XNode node in projectFileDocument.Nodes())
            {
                if (node.NodeType == XmlNodeType.Comment)
                {
                    string commentText = ((XComment)node).Value;

                    if (string.Equals(commentText, ProcessedDocumentCommentText, StringComparison.Ordinal))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public void Parse()
        {
            CleanupProject(projectFileDocument.Root);

            ParseProject();
        }

        public void Compose()
        {
            XElement projectRoot = projectFileDocument.Root;

            compiles.Sort(IncludeAttributeComparer);
            references.Sort(IncludeAttributeComparer);
            projectReferences.Sort(IncludeAttributeComparer);
            packageReferences.Sort(IncludeAttributeComparer);
            analyzers.Sort(IncludeAttributeComparer);
            additionalFiles.Sort(IncludeAttributeComparer);

            AddItemGroupIfNotEmpty(projectRoot, compiles);
            AddItemGroupIfNotEmpty(projectRoot, references);
            AddItemGroupIfNotEmpty(projectRoot, projectReferences);
            AddItemGroupIfNotEmpty(projectRoot, packageReferences);
            AddItemGroupIfNotEmpty(projectRoot, analyzers);
            AddItemGroupIfNotEmpty(projectRoot, additionalFiles);
            AddItemGroupIfNotEmpty(projectRoot, otherElements);

            var documentProcessedComment = new XComment(ProcessedDocumentCommentText);
            projectRoot.AddBeforeSelf(documentProcessedComment);
        }

        public string GetContent()
        {
            var writer = new Utf8StringWriter();

            projectFileDocument.Save(writer);

            return writer.ToString();
        }

        public void AddCompileProjectItem([NotNull] string path)
        {
            Verify.ArgumentNotNull(path, nameof(path));

            if (string.IsNullOrEmpty(path))
            {
                UnityEngine.Debug.Log("Can't add empty compile item.");
                return;
            }

            string normalizedPath = PathUtilities.NormalizeSlashesInPath(path);

            int positionInCollection = SearchForProjectItemInCollection(compiles, normalizedPath);

            if (positionInCollection != -1)
            {
                UnityEngine.Debug.Log($"Already have compile item '{normalizedPath}'");
                return;
            }

            XAttribute includeAttribute = new XAttribute(ProjectFileConstants.IncludeAttributeXName, normalizedPath);
            XElement compileElement = new XElement(ProjectFileConstants.CompileElementXName, includeAttribute);

            compiles.Add(compileElement);
        }

        public void AddPackageReferenceProjectItem([NotNull] string packageName, [NotNull] string version, [NotNull] string includeAssets, [NotNull] string privateAssets)
        {
            Verify.ArgumentNotNull(packageName, nameof(packageName));
            Verify.ArgumentNotNull(version, nameof(version));
            Verify.ArgumentNotNull(includeAssets, nameof(includeAssets));
            Verify.ArgumentNotNull(privateAssets, nameof(privateAssets));

            if (string.IsNullOrEmpty(packageName))
            {
                UnityEngine.Debug.Log("Can't add package without name.");
                return;
            }

            if (string.IsNullOrEmpty(version))
            {
                UnityEngine.Debug.Log("Can't add package without version.");
                return;
            }

            int positionInCollection = SearchForProjectItemInCollection(packageReferences, packageName);

            if (positionInCollection != -1)
            {
                UnityEngine.Debug.Log($"Already have package '{packageName}'");
                return;
            }

            var elements = new List<XObject>(4);

            XAttribute includeAttribute = new XAttribute(ProjectFileConstants.IncludeAttributeXName, packageName);
            elements.Add(includeAttribute);
            XElement versionElement = new XElement(ProjectFileConstants.VersionElementXName, version);
            elements.Add(versionElement);

            if (!string.IsNullOrEmpty(includeAssets))
            {
                XElement includeAssetsElement = new XElement(ProjectFileConstants.IncludeAssetsElementXName, includeAssets);
                elements.Add(includeAssetsElement);
            }

            if (!string.IsNullOrEmpty(privateAssets))
            {
                XElement privateAssetsElement = new XElement(ProjectFileConstants.PrivateAssetsElementXName, privateAssets);
                elements.Add(privateAssetsElement);
            }

            object[] packageReferenceContent = elements.ToArray();

            XElement packageReferenceElement = new XElement(ProjectFileConstants.PackageReferenceElementXName, packageReferenceContent);

            packageReferences.Add(packageReferenceElement);
        }

        public void AddRemoveAnalyzersTarget()
        {
            XAttribute removeAttribute = new XAttribute(ProjectFileConstants.RemoveAttributeXName, "@(Analyzer)");
            XElement analyzerElement = new XElement(ProjectFileConstants.AnalyzerElementXName, removeAttribute);

            XElement itemGroupElement = new XElement(ProjectFileConstants.ItemGroupElementXName, analyzerElement);

            XAttribute nameAttribute = new XAttribute(ProjectFileConstants.NameAttributeXName, "RemoveAnalyzers");
            XAttribute beforeTargetsAttribute = new XAttribute(ProjectFileConstants.BeforeTargetsAttributeXName, "CoreCompile");

            XElement targetElement = new XElement(ProjectFileConstants.TargetElementXName, nameAttribute, beforeTargetsAttribute, itemGroupElement);

            XElement projectRoot = projectFileDocument.Root;
            AddTarget(projectRoot, targetElement);
        }

        public void AddAnalyzerProjectItem([NotNull] string path)
        {
            Verify.ArgumentNotNull(path, nameof(path));

            if (string.IsNullOrEmpty(path))
            {
                UnityEngine.Debug.Log("Can't add empty analyzer item.");
                return;
            }

            string normalizedPath = PathUtilities.NormalizeSlashesInPath(path);

            int positionInCollection = SearchForProjectItemInCollection(analyzers, normalizedPath);

            if (positionInCollection != -1)
            {
                UnityEngine.Debug.Log($"Already have analyzer item '{normalizedPath}'");
                return;
            }

            XAttribute includeAttribute = new XAttribute(ProjectFileConstants.IncludeAttributeXName, normalizedPath);
            XElement analyzerElement = new XElement(ProjectFileConstants.AnalyzerElementXName, includeAttribute);

            analyzers.Add(analyzerElement);
        }

        public void AddAdditionalFileProjectItem([NotNull] string path)
        {
            Verify.ArgumentNotNull(path, nameof(path));

            if (string.IsNullOrEmpty(path))
            {
                UnityEngine.Debug.Log("Can't add empty additional file item.");
                return;
            }

            string normalizedPath = PathUtilities.NormalizeSlashesInPath(path);

            int positionInCollection = SearchForProjectItemInCollection(additionalFiles, normalizedPath);

            if (positionInCollection != -1)
            {
                UnityEngine.Debug.Log($"Already have additional file item '{normalizedPath}'");
                return;
            }

            XAttribute includeAttribute = new XAttribute(ProjectFileConstants.IncludeAttributeXName, normalizedPath);
            XElement additionalFileElement = new XElement(ProjectFileConstants.AdditionalFilesElementXName, includeAttribute);

            additionalFiles.Add(additionalFileElement);
        }

        private static int SearchForProjectItemInCollection([NotNull] List<XElement> projectItems, [NotNull] string itemPath)
        {
            Verify.ArgumentNotNull(projectItems, nameof(projectItems));
            Verify.ArgumentNotNull(itemPath, nameof(itemPath));

            int length = projectItems.Count;
            for (int i = 0; i < length; ++i)
            {
                XElement projectItem = projectItems[i];
                string path = projectItem.Attribute(ProjectFileConstants.IncludeAttributeXName)?.Value;
                if (!string.IsNullOrEmpty(path) && string.Equals(path, itemPath, StringComparison.OrdinalIgnoreCase))
                {
                    return i;
                }
            }

            return -1;
        }

        private static void AddItemGroupIfNotEmpty([NotNull] XElement documentRoot, [NotNull] List<XElement> elements)
        {
            Verify.ArgumentNotNull(documentRoot, nameof(documentRoot));
            Verify.ArgumentNotNull(elements, nameof(elements));

            if (elements.Count != 0)
            {
                XElement elementBeforeItemGroup = GetElementBeforeItemGroup(documentRoot);

                XElement itemGroup = XmlProjectFileUtilities.CreateElementWithItems(ProjectFileConstants.ItemGroupElementXName, elements);

                if (elementBeforeItemGroup != null)
                {
                    elementBeforeItemGroup.AddAfterSelf(itemGroup);
                }
                else
                {
                    documentRoot.AddFirst(itemGroup);
                }
            }
        }

        private static void AddTarget([NotNull] XElement documentRoot, [NotNull] XElement targetElement)
        {
            Verify.ArgumentNotNull(documentRoot, nameof(documentRoot));
            Verify.ArgumentNotNull(targetElement, nameof(targetElement));

            XElement elementBeforeTarget = GetElementBeforeTarget(documentRoot);

            if (elementBeforeTarget != null)
            {
                elementBeforeTarget.AddAfterSelf(targetElement);
            }
            else
            {
                documentRoot.AddFirst(targetElement);
            }
        }

        [CanBeNull]
        private static XElement GetElementBeforeItemGroup([NotNull] XElement documentRoot)
        {
            Verify.ArgumentNotNull(documentRoot, nameof(documentRoot));

            List<XElement> itemGroups = XmlProjectFileUtilities.GetElements(documentRoot, ProjectFileConstants.ItemGroupElementXName, null);

            if (itemGroups.Count != 0)
            {
                return itemGroups[itemGroups.Count - 1];
            }

            itemGroups = XmlProjectFileUtilities.GetElements(documentRoot, ProjectFileConstants.PropertyGroupElementXName, null);

            if (itemGroups.Count != 0)
            {
                return itemGroups[itemGroups.Count - 1];
            }

            return null;
        }

        [CanBeNull]
        private static XElement GetElementBeforeTarget([NotNull] XElement documentRoot)
        {
            Verify.ArgumentNotNull(documentRoot, nameof(documentRoot));

            List<XElement> targets = XmlProjectFileUtilities.GetElements(documentRoot, ProjectFileConstants.TargetElementXName, null);

            if (targets.Count != 0)
            {
                return targets[targets.Count - 1];
            }

            return GetElementBeforeItemGroup(documentRoot);
        }

        private static int IncludeAttributeComparer([NotNull] XElement left, [NotNull] XElement right)
        {
            Verify.ArgumentNotNull(left, nameof(left));
            Verify.ArgumentNotNull(right, nameof(right));

            string leftInclude = left.Attribute(ProjectFileConstants.IncludeAttributeXName)?.Value;
            string rightInclude = right.Attribute(ProjectFileConstants.IncludeAttributeXName)?.Value;

            return string.Compare(leftInclude, rightInclude, StringComparison.Ordinal);
        }

        [NotNull]
        private static List<XElement> GetElementsFromItemGroup([NotNull] XElement projectRoot, [NotNull] XName elementName)
        {
            Verify.ArgumentNotNull(projectRoot, nameof(projectRoot));
            Verify.ArgumentNotNull(elementName, nameof(elementName));

            var elements = new List<XElement>();

            List<XElement> itemGroups = XmlProjectFileUtilities.GetElements(projectRoot, ProjectFileConstants.ItemGroupElementXName, XmlProjectFileUtilities.ElementHasNoAttributes);

            foreach (XElement itemGroup in itemGroups)
            {
                List<XElement> innerElements = XmlProjectFileUtilities.GetElements(itemGroup, elementName);

                if (innerElements.Count > 0)
                {
                    elements.AddRange(innerElements);
                }
            }

            return elements;
        }

        private static void NormalizeIncludePaths([NotNull] XElement projectRoot)
        {
            Verify.ArgumentNotNull(projectRoot, nameof(projectRoot));

            List<XElement> compileElements = GetElementsFromItemGroup(projectRoot, ProjectFileConstants.CompileElementXName);

            foreach (XElement element in compileElements)
            {
                XAttribute includeAttribute = element.Attribute(ProjectFileConstants.IncludeAttributeName);

                if (includeAttribute != null)
                {
                    string includePath = includeAttribute.Value;
                    string normalizedPath = PathUtilities.NormalizeSlashesInPath(includePath);
                    includeAttribute.SetValue(normalizedPath);
                }
            }
        }

        private static void NormalizeHintPaths([NotNull] XElement projectRoot)
        {
            Verify.ArgumentNotNull(projectRoot, nameof(projectRoot));

            List<XElement> referenceElements = GetElementsFromItemGroup(projectRoot, ProjectFileConstants.ReferenceElementXName);

            foreach (XElement element in referenceElements)
            {
                List<XElement> hintPaths = XmlProjectFileUtilities.GetElements(element, ProjectFileConstants.HintPathElementXName);

                foreach (XElement hintPathElement in hintPaths)
                {
                    string hintPath = hintPathElement.Value;
                    string normalizedHintPath = PathUtilities.NormalizeSlashesInPath(hintPath);
                    hintPathElement.SetValue(normalizedHintPath);
                }
            }
        }

        private static void SortDefines([NotNull] XElement projectRoot)
        {
            Verify.ArgumentNotNull(projectRoot, nameof(projectRoot));

            List<XElement> propertyGroups = XmlProjectFileUtilities.GetElements(projectRoot, ProjectFileConstants.PropertyGroupElementXName);

            foreach (XElement propertyGroup in propertyGroups)
            {
                List<XElement> defineConstants = XmlProjectFileUtilities.GetElements(propertyGroup, ProjectFileConstants.DefineConstantsElenemtXName);

                foreach (XElement definesElement in defineConstants)
                {
                    string concatenatedDefines = definesElement.Value;
                    string[] defines = concatenatedDefines.Split(ProjectFileConstants.DefineConstantsSeparatorArray, StringSplitOptions.RemoveEmptyEntries);
                    Array.Sort(defines, StringComparer.Ordinal);
                    string sortedConcatenatedDefines = string.Join(ProjectFileConstants.DefineConstantsSeparatorString, defines);
                    definesElement.SetValue(sortedConcatenatedDefines);
                }
            }
        }

        private void CleanupProject([NotNull] XElement projectRoot)
        {
            Verify.ArgumentNotNull(projectRoot, nameof(projectRoot));

            SortDefines(projectRoot);
            NormalizeIncludePaths(projectRoot);
            NormalizeHintPaths(projectRoot);
        }

        private void ParseProject()
        {
            XElement projectRoot = projectFileDocument.Root;

            compiles = GetElementsFromItemGroup(projectRoot, ProjectFileConstants.CompileElementXName);
            XmlProjectFileUtilities.RemoveAllElements(compiles);

            references = GetElementsFromItemGroup(projectRoot, ProjectFileConstants.ReferenceElementXName);
            XmlProjectFileUtilities.RemoveAllElements(references);

            projectReferences = GetElementsFromItemGroup(projectRoot, ProjectFileConstants.ProjectReferenceElementXName);
            XmlProjectFileUtilities.RemoveAllElements(projectReferences);

            packageReferences = GetElementsFromItemGroup(projectRoot, ProjectFileConstants.PackageReferenceElementXName);
            XmlProjectFileUtilities.RemoveAllElements(packageReferences);

            analyzers = GetElementsFromItemGroup(projectRoot, ProjectFileConstants.AnalyzerElementXName);
            XmlProjectFileUtilities.RemoveAllElements(analyzers);

            additionalFiles = GetElementsFromItemGroup(projectRoot, ProjectFileConstants.AdditionalFilesElementXName);
            XmlProjectFileUtilities.RemoveAllElements(additionalFiles);

            otherElements = new List<XElement>();

            List<XElement> itemGroups = XmlProjectFileUtilities.GetElements(projectRoot, ProjectFileConstants.ItemGroupElementXName, XmlProjectFileUtilities.ElementHasNoAttributes);
            foreach (XElement itemGroup in itemGroups)
            {
                IEnumerable<XElement> otherElementsInGroup = itemGroup.Elements();
                otherElements.AddRange(otherElementsInGroup);
            }

            XmlProjectFileUtilities.RemoveAllElements(otherElements);

            itemGroups = XmlProjectFileUtilities.GetElements(projectRoot, ProjectFileConstants.ItemGroupElementXName, XmlProjectFileUtilities.ElementIsEmpty);
            XmlProjectFileUtilities.RemoveAllElements(itemGroups, XmlProjectFileUtilities.ElementIsEmpty);
        }
    }
}
