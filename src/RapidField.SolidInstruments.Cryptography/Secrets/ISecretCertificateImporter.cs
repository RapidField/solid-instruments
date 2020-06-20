// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Security;
using System.Security.Cryptography.X509Certificates;

namespace RapidField.SolidInstruments.Cryptography.Secrets
{
    /// <summary>
    /// Represents an import facility for named certificates which are encrypted and pinned in memory at rest.
    /// </summary>
    public interface ISecretCertificateImporter : IAsyncDisposable, IDisposable
    {
        /// <summary>
        /// Imports all valid certificates from the current user's personal certificate store.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="SecurityException">
        /// An exception was raised while trying to access or read the specified store or one of the certificates contained therein.
        /// </exception>
        public void ImportStoreCertificates();

        /// <summary>
        /// Imports all valid certificates from the specified local certificate store.
        /// </summary>
        /// <param name="storeName">
        /// The name of the store from which the certificates are imported. The default value is <see cref="StoreName.My" />.
        /// </param>
        /// <exception cref="ArgumentException">
        /// <paramref name="storeName" /> is not a valid store name.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="SecurityException">
        /// An exception was raised while trying to access or read the specified store or one of the certificates contained therein.
        /// </exception>
        public void ImportStoreCertificates(StoreName storeName);

        /// <summary>
        /// Imports all valid certificates from the specified local certificate store.
        /// </summary>
        /// <param name="storeName">
        /// The name of the store from which the certificates are imported. The default value is <see cref="StoreName.My" />.
        /// </param>
        /// <param name="storeLocation">
        /// The location of the store from which the certificates are imported. The default value is
        /// <see cref="StoreLocation.CurrentUser" />.
        /// </param>
        /// <exception cref="ArgumentException">
        /// <paramref name="storeName" /> is not a valid store name -or- <paramref name="storeLocation" /> is not a valid store
        /// location.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The object is disposed.
        /// </exception>
        /// <exception cref="SecurityException">
        /// An exception was raised while trying to access or read the specified store or one of the certificates contained therein.
        /// </exception>
        public void ImportStoreCertificates(StoreName storeName, StoreLocation storeLocation);
    }
}