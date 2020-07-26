// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;

namespace RapidField.SolidInstruments.Cryptography.Asymmetric
{
    /// <summary>
    /// Provides facilities for performing asymmetric key operations upon byte arrays.
    /// </summary>
    public interface IAsymmetricProcessor : IAsymmetricProcessor<Byte[]>
    {
    }

    /// <summary>
    /// Provides facilities for performing asymmetric key operations upon typed objects.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the object upon which operations are performed.
    /// </typeparam>
    public interface IAsymmetricProcessor<T> : ICryptographicProcessor<T>
        where T : class
    {
    }
}