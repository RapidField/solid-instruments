// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

namespace RapidField.SolidInstruments.Core.Caching
{
    /// <summary>
    /// Represents a read-write client for accessing strongly-typed, remotely cached objects using textual keys.
    /// </summary>
    public interface IDistributedCacheClient : ICacheClient
    {
    }
}