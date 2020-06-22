// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Cryptography.Asymmetric.DigitalSignature
{
    /// <summary>
    /// Provides facilities for digitally signing byte arrays.
    /// </summary>
    public interface IDigitalSignatureProcessor : IDigitalSignatureProcessor<Byte[]>
    {
    }

    /// <summary>
    /// Provides facilities for digitally signing typed objects.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the object that can be digitally signed.
    /// </typeparam>
    public interface IDigitalSignatureProcessor<T> : IAsymmetricProcessor
        where T : class
    {
    }
}