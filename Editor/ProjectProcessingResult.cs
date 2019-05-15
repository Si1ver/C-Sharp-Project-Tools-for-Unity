// Copyright (c) Anton Vasiliev. All rights reserved.
// Licensed under the MIT license.
// See the License.md file in the project root for full license information.

namespace Silvers.CsharpProjectTools
{
    using System;

    public enum ProjectProcessingResult
    {
        SuccessfullyProcessed = 0,
        AlreadyWasProcessed = 1,
        ProcessingFailed = 2,
    }
}
