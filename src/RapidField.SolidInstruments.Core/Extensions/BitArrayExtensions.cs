// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core.ArgumentValidation;
using System;
using System.Collections;
using System.Diagnostics;
using System.Text;

namespace RapidField.SolidInstruments.Core.Extensions
{
    /// <summary>
    /// Extends the <see cref="BitArray" /> class with general purpose features.
    /// </summary>
    public static class BitArrayExtensions
    {
        /// <summary>
        /// Performs a circular shift on the bits in the current <see cref="BitArray" />.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="BitArray" />.
        /// </param>
        /// <param name="direction">
        /// A direction to shift the bits in the current <see cref="BitArray" />. The default value is
        /// <see cref="BitShiftDirection.Right" />.
        /// </param>
        /// <param name="bitShiftCount">
        /// The number of bits to shift by.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The value of <paramref name="bitShiftCount" /> is less than zero.
        /// </exception>
        public static BitArray PerformCircularShift(this BitArray target, BitShiftDirection direction, Int32 bitShiftCount)
        {
            var arrayLength = target.Length;

            if (arrayLength < 2)
            {
                return target;
            }

            Int32 directionalBitShiftCount;

            switch (direction)
            {
                case BitShiftDirection.Left:

                    // Sign the shift count negative for left shifts.
                    directionalBitShiftCount = (bitShiftCount.RejectIf().IsLessThan(0, nameof(bitShiftCount)) * -1);
                    break;

                case BitShiftDirection.Right:

                    // Sign the shift count positive for right shifts.
                    directionalBitShiftCount = bitShiftCount.RejectIf().IsLessThan(0, nameof(bitShiftCount));
                    break;

                default:

                    goto case BitShiftDirection.Right;
            }

            var newBitArray = new BitArray(arrayLength, false);

            for (var i = 0; i < arrayLength; i++)
            {
                newBitArray.Set(DetermineNewCircularPosition(i, arrayLength, directionalBitShiftCount), target.Get(i));
            }

            return newBitArray;
        }

        /// <summary>
        /// Reverses the ordering of the bits in the current <see cref="BitArray" />.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="BitArray" />.
        /// </param>
        /// <returns>
        /// A <see cref="BitArray" /> that represents the reverse order of the target.
        /// </returns>
        public static BitArray ReverseOrder(this BitArray target)
        {
            var fieldLength = target.Length;

            if (fieldLength < 2)
            {
                return target;
            }

            var reverseOrderBitArray = new BitArray(target);
            var fieldMidPosition = (fieldLength / 2);

            for (var i = 0; i < fieldMidPosition; i++)
            {
                // Trade the current position bit for the mirror position bit.
                var currentPositionBit = reverseOrderBitArray[i];
                var mirrorPosition = (fieldLength - i - 1);
                reverseOrderBitArray[i] = reverseOrderBitArray[mirrorPosition];
                reverseOrderBitArray[mirrorPosition] = currentPositionBit;
            }

            return reverseOrderBitArray;
        }

        /// <summary>
        /// Converts the binary data underlying the current <see cref="BitArray" /> to its equivalent string representation.
        /// </summary>
        /// <param name="target">
        /// The current <see cref="BitArray" />.
        /// </param>
        /// <returns>
        /// A string representation of the binary data underlying the current <see cref="BitArray" />.
        /// </returns>
        public static String ToBinaryString(this BitArray target)
        {
            if (target.Length == 0)
            {
                return String.Empty;
            }

            var stringBuilder = new StringBuilder();

            foreach (var bit in target)
            {
                stringBuilder.Append((Boolean)bit ? '1' : '0');
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Determines the new position of a bit in a bit field when performing a circular shift given the current position, the
        /// array length and the directional shift.
        /// </summary>
        /// <param name="currentPosition">
        /// A bit's current position in an array as a zero-based index.
        /// </param>
        /// <param name="arrayLength">
        /// The length of the bit field.
        /// </param>
        /// <param name="directionalShift">
        /// A signed, directional bit count representing the shift to be performed on the bit.
        /// </param>
        /// <returns>
        /// The calculated new position as a zero-based index.
        /// </returns>
        [DebuggerHidden]
        private static Int32 DetermineNewCircularPosition(Int32 currentPosition, Int32 arrayLength, Int32 directionalShift)
        {
            // Perform an open-ended (non-circular) shift.
            var newPosition = (currentPosition + directionalShift);

            while (newPosition < 0)
            {
                // Rotate the field right to bring the new position back into frame.
                newPosition = (newPosition + arrayLength);
            }

            while (newPosition >= arrayLength)
            {
                // Rotate the field left to bring the new position back into frame.
                newPosition = (newPosition - arrayLength);
            }

            return newPosition;
        }
    }
}