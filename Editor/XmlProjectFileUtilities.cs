// Copyright (c) Anton Vasiliev. All rights reserved.
// Licensed under the MIT license.
// See the License.md file in the project root for full license information.

namespace Silvers.CsharpProjectTools
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Linq;
    using JetBrains.Annotations;

    internal static class XmlProjectFileUtilities
    {
        public static void RemoveAllElements([NotNull] List<XElement> elements, [CanBeNull] Func<XElement, bool> includeFilter = null)
        {
            Verify.ArgumentNotNull(elements, nameof(elements));

            foreach (XElement element in elements)
            {
                if (includeFilter == null || includeFilter(element))
                {
                    if (element.Parent != null)
                    {
                        element.Remove();
                    }
                }
            }
        }

        [NotNull]
        public static XElement CreateElementWithItems([NotNull] XName name, [NotNull] List<XElement> innerElements)
        {
            Verify.ArgumentNotNull(name, nameof(name));
            Verify.ArgumentNotNull(innerElements, nameof(innerElements));

            var element = new XElement(name);

            element.Add(innerElements);

            return element;
        }

        [NotNull]
        public static List<XElement> GetElements([NotNull] XElement root, [NotNull] XName elementName, [CanBeNull] Func<XElement, bool> includeFilter = null)
        {
            Verify.ArgumentNotNull(root, nameof(root));
            Verify.ArgumentNotNull(elementName, nameof(elementName));

            var elements = new List<XElement>();

            foreach (XElement element in root.Elements(elementName))
            {
                if (includeFilter == null || includeFilter(element))
                {
                    elements.Add(element);
                }
            }

            return elements;
        }

        [CanBeNull]
        public static XElement GetLastElementWithName([NotNull] XElement root, [NotNull] XName elementName)
        {
            Verify.ArgumentNotNull(root, nameof(root));
            Verify.ArgumentNotNull(elementName, nameof(elementName));

            XElement lastElement = null;

            foreach (XElement element in root.Elements(elementName))
            {
                lastElement = element;
            }

            return lastElement;
        }

        public static bool ElementHasNoAttributes([NotNull] XElement element)
        {
            Verify.ArgumentNotNull(element, nameof(element));

            return !element.HasAttributes;
        }

        public static bool ElementIsEmpty([NotNull] XElement element)
        {
            Verify.ArgumentNotNull(element, nameof(element));

            return !element.HasElements && !element.HasAttributes;
        }
    }
}
