// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace RapidField.SolidInstruments.Core.Extensions
{
    /// <summary>
    /// Extends the <see cref="Type" /> class with general purpose features.
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Creates a default instance of the specified type.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="Type" />.
        /// </param>
        /// <returns>
        /// A default instance of the specified type.
        /// </returns>
        public static Object GetDefaultValue(this Type target) => target.IsValueType ? Activator.CreateInstance(target) : null;

        /// <summary>
        /// Returns an identifier for the current <see cref="Type" /> that is unique for generic variants.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="Type" />.
        /// </param>
        /// <returns>
        /// An identifier for the current <see cref="Type" /> that is unique for generic variants.
        /// </returns>
        public static Guid GetUniqueIdentifier(this Type target) => target.ToByteArray().GenerateChecksumIdentity();

        /// <summary>
        /// Determines whether or not the current <see cref="Type" /> implements <see cref="IDisposable" /> or is
        /// <see cref="IDisposable" />.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="Type" />.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the current <see cref="Type" /> implements <see cref="IDisposable" /> or is
        /// <see cref="IDisposable" />, otherwise <see langword="false" />.
        /// </returns>
        public static Boolean IsDisposable(this Type target) => DisposableType.IsAssignableFrom(target);

        /// <summary>
        /// Converts the current <see cref="Type" /> to an array of bytes.
        /// </summary>
        /// <param name="target">
        /// The current instance of the <see cref="Type" />.
        /// </param>
        /// <returns>
        /// An array of bytes representing the current <see cref="Type" />.
        /// </returns>
        public static Byte[] ToByteArray(this Type target)
        {
            var bytes = new List<Byte>();
            bytes.AddRange(target.FullName.ToByteArray(Encoding.UTF8));

            if (target.ContainsGenericParameters)
            {
                foreach (var genericTypeArgument in target.GenericTypeArguments)
                {
                    bytes.AddRange(genericTypeArgument.ToByteArray());
                }
            }

            return bytes.ToArray();
        }

        /// <summary>
        /// Represents the <see cref="IDisposable" /> type.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Type DisposableType = typeof(IDisposable);
    }
}