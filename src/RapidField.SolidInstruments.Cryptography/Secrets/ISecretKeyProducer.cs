// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using System;

namespace RapidField.SolidInstruments.Cryptography.Secrets
{
    /// <summary>
    /// Represents a production facility for named secret keys which are encrypted and pinned in memory at rest.
    /// </summary>
    public interface ISecretKeyProducer : IAsyncDisposable, IDisposable
    {
        /// <summary>
        /// Generates a new <see cref="CascadingSymmetricKeySecret" /> and returns its assigned name.
        /// </summary>
        /// <returns>
        /// The textual name assigned to the new secret.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public String NewCascadingSymmetricKey();

        /// <summary>
        /// Generates a new <see cref="CascadingSymmetricKeySecret" /> with the specified name.
        /// </summary>
        /// <param name="name">
        /// The textual name to assign to the new secret.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="name" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// The <see cref="ISecretKeyProducer" /> already contains a secret with the specified name.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public void NewCascadingSymmetricKey(String name);

        /// <summary>
        /// Generates a new <see cref="SymmetricKeySecret" /> and returns its assigned name.
        /// </summary>
        /// <returns>
        /// The textual name assigned to the new secret.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public String NewSymmetricKey();

        /// <summary>
        /// Generates a new <see cref="SymmetricKeySecret" /> with the specified name.
        /// </summary>
        /// <param name="name">
        /// The textual name to assign to the new secret.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="name" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// The <see cref="ISecretKeyProducer" /> already contains a secret with the specified name.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        public void NewSymmetricKey(String name);
    }
}