// -------------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using EnsureThat;
using MediatR;
using Microsoft.Health.Fhir.Core.Features.Conformance;
using Microsoft.Health.Fhir.Core.Features.Persistence;
using Microsoft.Health.Fhir.Core.Features.Resources.Patch;
using Microsoft.Health.Fhir.Core.Messages.Bundle;
using Microsoft.Health.Fhir.Core.Messages.Upsert;
using Microsoft.Health.Fhir.Core.Models;

namespace Microsoft.Health.Fhir.Core.Messages.Patch
{
    public sealed class PatchResourceRequest : IRequest<UpsertResourceResponse>, IRequireCapability, IBundleInnerRequest
    {
        public PatchResourceRequest(ResourceKey resourceKey, PatchPayload payload, BundleResourceContext bundleResourceContext, WeakETag weakETag = null)
        {
            EnsureArg.IsNotNull(resourceKey, nameof(resourceKey));
            EnsureArg.IsNotNull(payload, nameof(payload));

            ResourceKey = resourceKey;
            Payload = payload;
            BundleResourceContext = bundleResourceContext;
            WeakETag = weakETag;
        }

        public PatchPayload Payload { get; }

        public ResourceKey ResourceKey { get; }

        public BundleResourceContext BundleResourceContext { get; }

        public WeakETag WeakETag { get; }

        public IEnumerable<CapabilityQuery> RequiredCapabilities()
        {
            yield return new CapabilityQuery($"CapabilityStatement.rest.resource.where(type = '{ResourceKey.ResourceType}').interaction.where(code = 'patch').exists()");
        }
    }
}
