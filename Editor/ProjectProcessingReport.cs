// Copyright (c) Anton Vasiliev. All rights reserved.
// Licensed under the MIT license.
// See the License.md file in the project root for full license information.

#if ENABLE_VSTU
namespace Silvers.CsharpProjectTools
{
    using System;

    public class ProjectProcessingReport
    {
        public string ProjectFilePath { get; set; }

        public string AssemblyDirectory { get; set; }

        public long ExecutionTimeMs { get; set; }

        public Exception Exception { get; set; }
    }
}
#endif
