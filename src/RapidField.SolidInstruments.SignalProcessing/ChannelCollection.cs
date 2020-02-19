// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Core.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace RapidField.SolidInstruments.SignalProcessing
{
    /// <summary>
    /// Represents a collection of streaming data signals.
    /// </summary>
    /// <typeparam name="TChannelA">
    /// The type of the first channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelB">
    /// The type of the second channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelC">
    /// The type of the third channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelD">
    /// The type of the fourth channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelE">
    /// The type of the fifth channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelF">
    /// The type of the sixth channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelG">
    /// The type of the seventh channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelH">
    /// The type of the eighth channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelI">
    /// The type of the ninth channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelJ">
    /// The type of the tenth channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelK">
    /// The type of the eleventh channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelL">
    /// The type of the twelfth channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelM">
    /// The type of the thirteenth channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelN">
    /// The type of the fourteenth channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelO">
    /// The type of the fifteenth channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelP">
    /// The type of the sixteenth channel in the collection.
    /// </typeparam>
    public class ChannelCollection<TChannelA, TChannelB, TChannelC, TChannelD, TChannelE, TChannelF, TChannelG, TChannelH, TChannelI, TChannelJ, TChannelK, TChannelL, TChannelM, TChannelN, TChannelO, TChannelP> : ChannelCollection, IChannelCollection<TChannelA, TChannelB, TChannelC, TChannelD, TChannelE, TChannelF, TChannelG, TChannelH, TChannelI, TChannelJ, TChannelK, TChannelL, TChannelM, TChannelN, TChannelO, TChannelP>
        where TChannelA : class, IChannel
        where TChannelB : class, IChannel
        where TChannelC : class, IChannel
        where TChannelD : class, IChannel
        where TChannelE : class, IChannel
        where TChannelF : class, IChannel
        where TChannelG : class, IChannel
        where TChannelH : class, IChannel
        where TChannelI : class, IChannel
        where TChannelJ : class, IChannel
        where TChannelK : class, IChannel
        where TChannelL : class, IChannel
        where TChannelM : class, IChannel
        where TChannelN : class, IChannel
        where TChannelO : class, IChannel
        where TChannelP : class, IChannel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelCollection{TChannelA}" /> class.
        /// </summary>
        /// <param name="channelA">
        /// The first channel in the collection.
        /// </param>
        /// <param name="channelB">
        /// The second channel in the collection.
        /// </param>
        /// <param name="channelC">
        /// The third channel in the collection.
        /// </param>
        /// <param name="channelD">
        /// The fourth channel in the collection.
        /// </param>
        /// <param name="channelE">
        /// The fifth channel in the collection.
        /// </param>
        /// <param name="channelF">
        /// The sixth channel in the collection.
        /// </param>
        /// <param name="channelG">
        /// The seventh channel in the collection.
        /// </param>
        /// <param name="channelH">
        /// The eighth channel in the collection.
        /// </param>
        /// <param name="channelI">
        /// The ninth channel in the collection.
        /// </param>
        /// <param name="channelJ">
        /// The tenth channel in the collection.
        /// </param>
        /// <param name="channelK">
        /// The eleventh channel in the collection.
        /// </param>
        /// <param name="channelL">
        /// The twelfth channel in the collection.
        /// </param>
        /// <param name="channelM">
        /// The thirteenth channel in the collection.
        /// </param>
        /// <param name="channelN">
        /// The fourteenth channel in the collection.
        /// </param>
        /// <param name="channelO">
        /// The fifteenth channel in the collection.
        /// </param>
        /// <param name="channelP">
        /// The sixteenth channel in the collection.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// One or more of the specified channels is <see langword="null" />.
        /// </exception>
        public ChannelCollection(TChannelA channelA, TChannelB channelB, TChannelC channelC, TChannelD channelD, TChannelE channelE, TChannelF channelF, TChannelG channelG, TChannelH channelH, TChannelI channelI, TChannelJ channelJ, TChannelK channelK, TChannelL channelL, TChannelM channelM, TChannelN channelN, TChannelO channelO, TChannelP channelP)
            : base(channelA, channelB, channelC, channelD, channelE, channelF, channelG, channelH, channelI, channelJ, channelK, channelL, channelM, channelN, channelO, channelP)
        {
            ChannelA = channelA;
            ChannelB = channelB;
            ChannelC = channelC;
            ChannelD = channelD;
            ChannelE = channelE;
            ChannelF = channelF;
            ChannelG = channelG;
            ChannelH = channelH;
            ChannelI = channelI;
            ChannelJ = channelJ;
            ChannelK = channelK;
            ChannelL = channelL;
            ChannelM = channelM;
            ChannelN = channelN;
            ChannelO = channelO;
            ChannelP = channelP;
        }

        /// <summary>
        /// Gets the first channel in the collection.
        /// </summary>
        public TChannelA ChannelA
        {
            get;
        }

        /// <summary>
        /// Gets the second channel in the collection.
        /// </summary>
        public TChannelB ChannelB
        {
            get;
        }

        /// <summary>
        /// Gets the third channel in the collection.
        /// </summary>
        public TChannelC ChannelC
        {
            get;
        }

        /// <summary>
        /// Gets the fourth channel in the collection.
        /// </summary>
        public TChannelD ChannelD
        {
            get;
        }

        /// <summary>
        /// Gets the fifth channel in the collection.
        /// </summary>
        public TChannelE ChannelE
        {
            get;
        }

        /// <summary>
        /// Gets the sixth channel in the collection.
        /// </summary>
        public TChannelF ChannelF
        {
            get;
        }

        /// <summary>
        /// Gets the seventh channel in the collection.
        /// </summary>
        public TChannelG ChannelG
        {
            get;
        }

        /// <summary>
        /// Gets the eighth channel in the collection.
        /// </summary>
        public TChannelH ChannelH
        {
            get;
        }

        /// <summary>
        /// Gets the ninth channel in the collection.
        /// </summary>
        public TChannelI ChannelI
        {
            get;
        }

        /// <summary>
        /// Gets the tenth channel in the collection.
        /// </summary>
        public TChannelJ ChannelJ
        {
            get;
        }

        /// <summary>
        /// Gets the eleventh channel in the collection.
        /// </summary>
        public TChannelK ChannelK
        {
            get;
        }

        /// <summary>
        /// Gets the twelfth channel in the collection.
        /// </summary>
        public TChannelL ChannelL
        {
            get;
        }

        /// <summary>
        /// Gets the thirteenth channel in the collection.
        /// </summary>
        public TChannelM ChannelM
        {
            get;
        }

        /// <summary>
        /// Gets the fourteenth channel in the collection.
        /// </summary>
        public TChannelN ChannelN
        {
            get;
        }

        /// <summary>
        /// Gets the fifteenth channel in the collection.
        /// </summary>
        public TChannelO ChannelO
        {
            get;
        }

        /// <summary>
        /// Gets the sixteenth channel in the collection.
        /// </summary>
        public TChannelP ChannelP
        {
            get;
        }
    }

    /// <summary>
    /// Represents a collection of streaming data signals.
    /// </summary>
    /// <typeparam name="TChannelA">
    /// The type of the first channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelB">
    /// The type of the second channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelC">
    /// The type of the third channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelD">
    /// The type of the fourth channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelE">
    /// The type of the fifth channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelF">
    /// The type of the sixth channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelG">
    /// The type of the seventh channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelH">
    /// The type of the eighth channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelI">
    /// The type of the ninth channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelJ">
    /// The type of the tenth channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelK">
    /// The type of the eleventh channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelL">
    /// The type of the twelfth channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelM">
    /// The type of the thirteenth channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelN">
    /// The type of the fourteenth channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelO">
    /// The type of the fifteenth channel in the collection.
    /// </typeparam>
    public class ChannelCollection<TChannelA, TChannelB, TChannelC, TChannelD, TChannelE, TChannelF, TChannelG, TChannelH, TChannelI, TChannelJ, TChannelK, TChannelL, TChannelM, TChannelN, TChannelO> : ChannelCollection, IChannelCollection<TChannelA, TChannelB, TChannelC, TChannelD, TChannelE, TChannelF, TChannelG, TChannelH, TChannelI, TChannelJ, TChannelK, TChannelL, TChannelM, TChannelN, TChannelO>
        where TChannelA : class, IChannel
        where TChannelB : class, IChannel
        where TChannelC : class, IChannel
        where TChannelD : class, IChannel
        where TChannelE : class, IChannel
        where TChannelF : class, IChannel
        where TChannelG : class, IChannel
        where TChannelH : class, IChannel
        where TChannelI : class, IChannel
        where TChannelJ : class, IChannel
        where TChannelK : class, IChannel
        where TChannelL : class, IChannel
        where TChannelM : class, IChannel
        where TChannelN : class, IChannel
        where TChannelO : class, IChannel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelCollection{TChannelA}" /> class.
        /// </summary>
        /// <param name="channelA">
        /// The first channel in the collection.
        /// </param>
        /// <param name="channelB">
        /// The second channel in the collection.
        /// </param>
        /// <param name="channelC">
        /// The third channel in the collection.
        /// </param>
        /// <param name="channelD">
        /// The fourth channel in the collection.
        /// </param>
        /// <param name="channelE">
        /// The fifth channel in the collection.
        /// </param>
        /// <param name="channelF">
        /// The sixth channel in the collection.
        /// </param>
        /// <param name="channelG">
        /// The seventh channel in the collection.
        /// </param>
        /// <param name="channelH">
        /// The eighth channel in the collection.
        /// </param>
        /// <param name="channelI">
        /// The ninth channel in the collection.
        /// </param>
        /// <param name="channelJ">
        /// The tenth channel in the collection.
        /// </param>
        /// <param name="channelK">
        /// The eleventh channel in the collection.
        /// </param>
        /// <param name="channelL">
        /// The twelfth channel in the collection.
        /// </param>
        /// <param name="channelM">
        /// The thirteenth channel in the collection.
        /// </param>
        /// <param name="channelN">
        /// The fourteenth channel in the collection.
        /// </param>
        /// <param name="channelO">
        /// The fifteenth channel in the collection.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// One or more of the specified channels is <see langword="null" />.
        /// </exception>
        public ChannelCollection(TChannelA channelA, TChannelB channelB, TChannelC channelC, TChannelD channelD, TChannelE channelE, TChannelF channelF, TChannelG channelG, TChannelH channelH, TChannelI channelI, TChannelJ channelJ, TChannelK channelK, TChannelL channelL, TChannelM channelM, TChannelN channelN, TChannelO channelO)
            : base(channelA, channelB, channelC, channelD, channelE, channelF, channelG, channelH, channelI, channelJ, channelK, channelL, channelM, channelN, channelO)
        {
            ChannelA = channelA;
            ChannelB = channelB;
            ChannelC = channelC;
            ChannelD = channelD;
            ChannelE = channelE;
            ChannelF = channelF;
            ChannelG = channelG;
            ChannelH = channelH;
            ChannelI = channelI;
            ChannelJ = channelJ;
            ChannelK = channelK;
            ChannelL = channelL;
            ChannelM = channelM;
            ChannelN = channelN;
            ChannelO = channelO;
        }

        /// <summary>
        /// Gets the first channel in the collection.
        /// </summary>
        public TChannelA ChannelA
        {
            get;
        }

        /// <summary>
        /// Gets the second channel in the collection.
        /// </summary>
        public TChannelB ChannelB
        {
            get;
        }

        /// <summary>
        /// Gets the third channel in the collection.
        /// </summary>
        public TChannelC ChannelC
        {
            get;
        }

        /// <summary>
        /// Gets the fourth channel in the collection.
        /// </summary>
        public TChannelD ChannelD
        {
            get;
        }

        /// <summary>
        /// Gets the fifth channel in the collection.
        /// </summary>
        public TChannelE ChannelE
        {
            get;
        }

        /// <summary>
        /// Gets the sixth channel in the collection.
        /// </summary>
        public TChannelF ChannelF
        {
            get;
        }

        /// <summary>
        /// Gets the seventh channel in the collection.
        /// </summary>
        public TChannelG ChannelG
        {
            get;
        }

        /// <summary>
        /// Gets the eighth channel in the collection.
        /// </summary>
        public TChannelH ChannelH
        {
            get;
        }

        /// <summary>
        /// Gets the ninth channel in the collection.
        /// </summary>
        public TChannelI ChannelI
        {
            get;
        }

        /// <summary>
        /// Gets the tenth channel in the collection.
        /// </summary>
        public TChannelJ ChannelJ
        {
            get;
        }

        /// <summary>
        /// Gets the eleventh channel in the collection.
        /// </summary>
        public TChannelK ChannelK
        {
            get;
        }

        /// <summary>
        /// Gets the twelfth channel in the collection.
        /// </summary>
        public TChannelL ChannelL
        {
            get;
        }

        /// <summary>
        /// Gets the thirteenth channel in the collection.
        /// </summary>
        public TChannelM ChannelM
        {
            get;
        }

        /// <summary>
        /// Gets the fourteenth channel in the collection.
        /// </summary>
        public TChannelN ChannelN
        {
            get;
        }

        /// <summary>
        /// Gets the fifteenth channel in the collection.
        /// </summary>
        public TChannelO ChannelO
        {
            get;
        }
    }

    /// <summary>
    /// Represents a collection of streaming data signals.
    /// </summary>
    /// <typeparam name="TChannelA">
    /// The type of the first channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelB">
    /// The type of the second channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelC">
    /// The type of the third channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelD">
    /// The type of the fourth channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelE">
    /// The type of the fifth channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelF">
    /// The type of the sixth channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelG">
    /// The type of the seventh channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelH">
    /// The type of the eighth channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelI">
    /// The type of the ninth channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelJ">
    /// The type of the tenth channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelK">
    /// The type of the eleventh channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelL">
    /// The type of the twelfth channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelM">
    /// The type of the thirteenth channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelN">
    /// The type of the fourteenth channel in the collection.
    /// </typeparam>
    public class ChannelCollection<TChannelA, TChannelB, TChannelC, TChannelD, TChannelE, TChannelF, TChannelG, TChannelH, TChannelI, TChannelJ, TChannelK, TChannelL, TChannelM, TChannelN> : ChannelCollection, IChannelCollection<TChannelA, TChannelB, TChannelC, TChannelD, TChannelE, TChannelF, TChannelG, TChannelH, TChannelI, TChannelJ, TChannelK, TChannelL, TChannelM, TChannelN>
        where TChannelA : class, IChannel
        where TChannelB : class, IChannel
        where TChannelC : class, IChannel
        where TChannelD : class, IChannel
        where TChannelE : class, IChannel
        where TChannelF : class, IChannel
        where TChannelG : class, IChannel
        where TChannelH : class, IChannel
        where TChannelI : class, IChannel
        where TChannelJ : class, IChannel
        where TChannelK : class, IChannel
        where TChannelL : class, IChannel
        where TChannelM : class, IChannel
        where TChannelN : class, IChannel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelCollection{TChannelA}" /> class.
        /// </summary>
        /// <param name="channelA">
        /// The first channel in the collection.
        /// </param>
        /// <param name="channelB">
        /// The second channel in the collection.
        /// </param>
        /// <param name="channelC">
        /// The third channel in the collection.
        /// </param>
        /// <param name="channelD">
        /// The fourth channel in the collection.
        /// </param>
        /// <param name="channelE">
        /// The fifth channel in the collection.
        /// </param>
        /// <param name="channelF">
        /// The sixth channel in the collection.
        /// </param>
        /// <param name="channelG">
        /// The seventh channel in the collection.
        /// </param>
        /// <param name="channelH">
        /// The eighth channel in the collection.
        /// </param>
        /// <param name="channelI">
        /// The ninth channel in the collection.
        /// </param>
        /// <param name="channelJ">
        /// The tenth channel in the collection.
        /// </param>
        /// <param name="channelK">
        /// The eleventh channel in the collection.
        /// </param>
        /// <param name="channelL">
        /// The twelfth channel in the collection.
        /// </param>
        /// <param name="channelM">
        /// The thirteenth channel in the collection.
        /// </param>
        /// <param name="channelN">
        /// The fourteenth channel in the collection.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// One or more of the specified channels is <see langword="null" />.
        /// </exception>
        public ChannelCollection(TChannelA channelA, TChannelB channelB, TChannelC channelC, TChannelD channelD, TChannelE channelE, TChannelF channelF, TChannelG channelG, TChannelH channelH, TChannelI channelI, TChannelJ channelJ, TChannelK channelK, TChannelL channelL, TChannelM channelM, TChannelN channelN)
            : base(channelA, channelB, channelC, channelD, channelE, channelF, channelG, channelH, channelI, channelJ, channelK, channelL, channelM, channelN)
        {
            ChannelA = channelA;
            ChannelB = channelB;
            ChannelC = channelC;
            ChannelD = channelD;
            ChannelE = channelE;
            ChannelF = channelF;
            ChannelG = channelG;
            ChannelH = channelH;
            ChannelI = channelI;
            ChannelJ = channelJ;
            ChannelK = channelK;
            ChannelL = channelL;
            ChannelM = channelM;
            ChannelN = channelN;
        }

        /// <summary>
        /// Gets the first channel in the collection.
        /// </summary>
        public TChannelA ChannelA
        {
            get;
        }

        /// <summary>
        /// Gets the second channel in the collection.
        /// </summary>
        public TChannelB ChannelB
        {
            get;
        }

        /// <summary>
        /// Gets the third channel in the collection.
        /// </summary>
        public TChannelC ChannelC
        {
            get;
        }

        /// <summary>
        /// Gets the fourth channel in the collection.
        /// </summary>
        public TChannelD ChannelD
        {
            get;
        }

        /// <summary>
        /// Gets the fifth channel in the collection.
        /// </summary>
        public TChannelE ChannelE
        {
            get;
        }

        /// <summary>
        /// Gets the sixth channel in the collection.
        /// </summary>
        public TChannelF ChannelF
        {
            get;
        }

        /// <summary>
        /// Gets the seventh channel in the collection.
        /// </summary>
        public TChannelG ChannelG
        {
            get;
        }

        /// <summary>
        /// Gets the eighth channel in the collection.
        /// </summary>
        public TChannelH ChannelH
        {
            get;
        }

        /// <summary>
        /// Gets the ninth channel in the collection.
        /// </summary>
        public TChannelI ChannelI
        {
            get;
        }

        /// <summary>
        /// Gets the tenth channel in the collection.
        /// </summary>
        public TChannelJ ChannelJ
        {
            get;
        }

        /// <summary>
        /// Gets the eleventh channel in the collection.
        /// </summary>
        public TChannelK ChannelK
        {
            get;
        }

        /// <summary>
        /// Gets the twelfth channel in the collection.
        /// </summary>
        public TChannelL ChannelL
        {
            get;
        }

        /// <summary>
        /// Gets the thirteenth channel in the collection.
        /// </summary>
        public TChannelM ChannelM
        {
            get;
        }

        /// <summary>
        /// Gets the fourteenth channel in the collection.
        /// </summary>
        public TChannelN ChannelN
        {
            get;
        }
    }

    /// <summary>
    /// Represents a collection of streaming data signals.
    /// </summary>
    /// <typeparam name="TChannelA">
    /// The type of the first channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelB">
    /// The type of the second channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelC">
    /// The type of the third channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelD">
    /// The type of the fourth channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelE">
    /// The type of the fifth channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelF">
    /// The type of the sixth channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelG">
    /// The type of the seventh channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelH">
    /// The type of the eighth channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelI">
    /// The type of the ninth channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelJ">
    /// The type of the tenth channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelK">
    /// The type of the eleventh channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelL">
    /// The type of the twelfth channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelM">
    /// The type of the thirteenth channel in the collection.
    /// </typeparam>
    public class ChannelCollection<TChannelA, TChannelB, TChannelC, TChannelD, TChannelE, TChannelF, TChannelG, TChannelH, TChannelI, TChannelJ, TChannelK, TChannelL, TChannelM> : ChannelCollection, IChannelCollection<TChannelA, TChannelB, TChannelC, TChannelD, TChannelE, TChannelF, TChannelG, TChannelH, TChannelI, TChannelJ, TChannelK, TChannelL, TChannelM>
        where TChannelA : class, IChannel
        where TChannelB : class, IChannel
        where TChannelC : class, IChannel
        where TChannelD : class, IChannel
        where TChannelE : class, IChannel
        where TChannelF : class, IChannel
        where TChannelG : class, IChannel
        where TChannelH : class, IChannel
        where TChannelI : class, IChannel
        where TChannelJ : class, IChannel
        where TChannelK : class, IChannel
        where TChannelL : class, IChannel
        where TChannelM : class, IChannel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelCollection{TChannelA}" /> class.
        /// </summary>
        /// <param name="channelA">
        /// The first channel in the collection.
        /// </param>
        /// <param name="channelB">
        /// The second channel in the collection.
        /// </param>
        /// <param name="channelC">
        /// The third channel in the collection.
        /// </param>
        /// <param name="channelD">
        /// The fourth channel in the collection.
        /// </param>
        /// <param name="channelE">
        /// The fifth channel in the collection.
        /// </param>
        /// <param name="channelF">
        /// The sixth channel in the collection.
        /// </param>
        /// <param name="channelG">
        /// The seventh channel in the collection.
        /// </param>
        /// <param name="channelH">
        /// The eighth channel in the collection.
        /// </param>
        /// <param name="channelI">
        /// The ninth channel in the collection.
        /// </param>
        /// <param name="channelJ">
        /// The tenth channel in the collection.
        /// </param>
        /// <param name="channelK">
        /// The eleventh channel in the collection.
        /// </param>
        /// <param name="channelL">
        /// The twelfth channel in the collection.
        /// </param>
        /// <param name="channelM">
        /// The thirteenth channel in the collection.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// One or more of the specified channels is <see langword="null" />.
        /// </exception>
        public ChannelCollection(TChannelA channelA, TChannelB channelB, TChannelC channelC, TChannelD channelD, TChannelE channelE, TChannelF channelF, TChannelG channelG, TChannelH channelH, TChannelI channelI, TChannelJ channelJ, TChannelK channelK, TChannelL channelL, TChannelM channelM)
            : base(channelA, channelB, channelC, channelD, channelE, channelF, channelG, channelH, channelI, channelJ, channelK, channelL, channelM)
        {
            ChannelA = channelA;
            ChannelB = channelB;
            ChannelC = channelC;
            ChannelD = channelD;
            ChannelE = channelE;
            ChannelF = channelF;
            ChannelG = channelG;
            ChannelH = channelH;
            ChannelI = channelI;
            ChannelJ = channelJ;
            ChannelK = channelK;
            ChannelL = channelL;
            ChannelM = channelM;
        }

        /// <summary>
        /// Gets the first channel in the collection.
        /// </summary>
        public TChannelA ChannelA
        {
            get;
        }

        /// <summary>
        /// Gets the second channel in the collection.
        /// </summary>
        public TChannelB ChannelB
        {
            get;
        }

        /// <summary>
        /// Gets the third channel in the collection.
        /// </summary>
        public TChannelC ChannelC
        {
            get;
        }

        /// <summary>
        /// Gets the fourth channel in the collection.
        /// </summary>
        public TChannelD ChannelD
        {
            get;
        }

        /// <summary>
        /// Gets the fifth channel in the collection.
        /// </summary>
        public TChannelE ChannelE
        {
            get;
        }

        /// <summary>
        /// Gets the sixth channel in the collection.
        /// </summary>
        public TChannelF ChannelF
        {
            get;
        }

        /// <summary>
        /// Gets the seventh channel in the collection.
        /// </summary>
        public TChannelG ChannelG
        {
            get;
        }

        /// <summary>
        /// Gets the eighth channel in the collection.
        /// </summary>
        public TChannelH ChannelH
        {
            get;
        }

        /// <summary>
        /// Gets the ninth channel in the collection.
        /// </summary>
        public TChannelI ChannelI
        {
            get;
        }

        /// <summary>
        /// Gets the tenth channel in the collection.
        /// </summary>
        public TChannelJ ChannelJ
        {
            get;
        }

        /// <summary>
        /// Gets the eleventh channel in the collection.
        /// </summary>
        public TChannelK ChannelK
        {
            get;
        }

        /// <summary>
        /// Gets the twelfth channel in the collection.
        /// </summary>
        public TChannelL ChannelL
        {
            get;
        }

        /// <summary>
        /// Gets the thirteenth channel in the collection.
        /// </summary>
        public TChannelM ChannelM
        {
            get;
        }
    }

    /// <summary>
    /// Represents a collection of streaming data signals.
    /// </summary>
    /// <typeparam name="TChannelA">
    /// The type of the first channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelB">
    /// The type of the second channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelC">
    /// The type of the third channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelD">
    /// The type of the fourth channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelE">
    /// The type of the fifth channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelF">
    /// The type of the sixth channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelG">
    /// The type of the seventh channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelH">
    /// The type of the eighth channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelI">
    /// The type of the ninth channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelJ">
    /// The type of the tenth channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelK">
    /// The type of the eleventh channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelL">
    /// The type of the twelfth channel in the collection.
    /// </typeparam>
    public class ChannelCollection<TChannelA, TChannelB, TChannelC, TChannelD, TChannelE, TChannelF, TChannelG, TChannelH, TChannelI, TChannelJ, TChannelK, TChannelL> : ChannelCollection, IChannelCollection<TChannelA, TChannelB, TChannelC, TChannelD, TChannelE, TChannelF, TChannelG, TChannelH, TChannelI, TChannelJ, TChannelK, TChannelL>
        where TChannelA : class, IChannel
        where TChannelB : class, IChannel
        where TChannelC : class, IChannel
        where TChannelD : class, IChannel
        where TChannelE : class, IChannel
        where TChannelF : class, IChannel
        where TChannelG : class, IChannel
        where TChannelH : class, IChannel
        where TChannelI : class, IChannel
        where TChannelJ : class, IChannel
        where TChannelK : class, IChannel
        where TChannelL : class, IChannel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelCollection{TChannelA}" /> class.
        /// </summary>
        /// <param name="channelA">
        /// The first channel in the collection.
        /// </param>
        /// <param name="channelB">
        /// The second channel in the collection.
        /// </param>
        /// <param name="channelC">
        /// The third channel in the collection.
        /// </param>
        /// <param name="channelD">
        /// The fourth channel in the collection.
        /// </param>
        /// <param name="channelE">
        /// The fifth channel in the collection.
        /// </param>
        /// <param name="channelF">
        /// The sixth channel in the collection.
        /// </param>
        /// <param name="channelG">
        /// The seventh channel in the collection.
        /// </param>
        /// <param name="channelH">
        /// The eighth channel in the collection.
        /// </param>
        /// <param name="channelI">
        /// The ninth channel in the collection.
        /// </param>
        /// <param name="channelJ">
        /// The tenth channel in the collection.
        /// </param>
        /// <param name="channelK">
        /// The eleventh channel in the collection.
        /// </param>
        /// <param name="channelL">
        /// The twelfth channel in the collection.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// One or more of the specified channels is <see langword="null" />.
        /// </exception>
        public ChannelCollection(TChannelA channelA, TChannelB channelB, TChannelC channelC, TChannelD channelD, TChannelE channelE, TChannelF channelF, TChannelG channelG, TChannelH channelH, TChannelI channelI, TChannelJ channelJ, TChannelK channelK, TChannelL channelL)
            : base(channelA, channelB, channelC, channelD, channelE, channelF, channelG, channelH, channelI, channelJ, channelK, channelL)
        {
            ChannelA = channelA;
            ChannelB = channelB;
            ChannelC = channelC;
            ChannelD = channelD;
            ChannelE = channelE;
            ChannelF = channelF;
            ChannelG = channelG;
            ChannelH = channelH;
            ChannelI = channelI;
            ChannelJ = channelJ;
            ChannelK = channelK;
            ChannelL = channelL;
        }

        /// <summary>
        /// Gets the first channel in the collection.
        /// </summary>
        public TChannelA ChannelA
        {
            get;
        }

        /// <summary>
        /// Gets the second channel in the collection.
        /// </summary>
        public TChannelB ChannelB
        {
            get;
        }

        /// <summary>
        /// Gets the third channel in the collection.
        /// </summary>
        public TChannelC ChannelC
        {
            get;
        }

        /// <summary>
        /// Gets the fourth channel in the collection.
        /// </summary>
        public TChannelD ChannelD
        {
            get;
        }

        /// <summary>
        /// Gets the fifth channel in the collection.
        /// </summary>
        public TChannelE ChannelE
        {
            get;
        }

        /// <summary>
        /// Gets the sixth channel in the collection.
        /// </summary>
        public TChannelF ChannelF
        {
            get;
        }

        /// <summary>
        /// Gets the seventh channel in the collection.
        /// </summary>
        public TChannelG ChannelG
        {
            get;
        }

        /// <summary>
        /// Gets the eighth channel in the collection.
        /// </summary>
        public TChannelH ChannelH
        {
            get;
        }

        /// <summary>
        /// Gets the ninth channel in the collection.
        /// </summary>
        public TChannelI ChannelI
        {
            get;
        }

        /// <summary>
        /// Gets the tenth channel in the collection.
        /// </summary>
        public TChannelJ ChannelJ
        {
            get;
        }

        /// <summary>
        /// Gets the eleventh channel in the collection.
        /// </summary>
        public TChannelK ChannelK
        {
            get;
        }

        /// <summary>
        /// Gets the twelfth channel in the collection.
        /// </summary>
        public TChannelL ChannelL
        {
            get;
        }
    }

    /// <summary>
    /// Represents a collection of streaming data signals.
    /// </summary>
    /// <typeparam name="TChannelA">
    /// The type of the first channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelB">
    /// The type of the second channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelC">
    /// The type of the third channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelD">
    /// The type of the fourth channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelE">
    /// The type of the fifth channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelF">
    /// The type of the sixth channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelG">
    /// The type of the seventh channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelH">
    /// The type of the eighth channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelI">
    /// The type of the ninth channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelJ">
    /// The type of the tenth channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelK">
    /// The type of the eleventh channel in the collection.
    /// </typeparam>
    public class ChannelCollection<TChannelA, TChannelB, TChannelC, TChannelD, TChannelE, TChannelF, TChannelG, TChannelH, TChannelI, TChannelJ, TChannelK> : ChannelCollection, IChannelCollection<TChannelA, TChannelB, TChannelC, TChannelD, TChannelE, TChannelF, TChannelG, TChannelH, TChannelI, TChannelJ, TChannelK>
        where TChannelA : class, IChannel
        where TChannelB : class, IChannel
        where TChannelC : class, IChannel
        where TChannelD : class, IChannel
        where TChannelE : class, IChannel
        where TChannelF : class, IChannel
        where TChannelG : class, IChannel
        where TChannelH : class, IChannel
        where TChannelI : class, IChannel
        where TChannelJ : class, IChannel
        where TChannelK : class, IChannel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelCollection{TChannelA}" /> class.
        /// </summary>
        /// <param name="channelA">
        /// The first channel in the collection.
        /// </param>
        /// <param name="channelB">
        /// The second channel in the collection.
        /// </param>
        /// <param name="channelC">
        /// The third channel in the collection.
        /// </param>
        /// <param name="channelD">
        /// The fourth channel in the collection.
        /// </param>
        /// <param name="channelE">
        /// The fifth channel in the collection.
        /// </param>
        /// <param name="channelF">
        /// The sixth channel in the collection.
        /// </param>
        /// <param name="channelG">
        /// The seventh channel in the collection.
        /// </param>
        /// <param name="channelH">
        /// The eighth channel in the collection.
        /// </param>
        /// <param name="channelI">
        /// The ninth channel in the collection.
        /// </param>
        /// <param name="channelJ">
        /// The tenth channel in the collection.
        /// </param>
        /// <param name="channelK">
        /// The eleventh channel in the collection.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// One or more of the specified channels is <see langword="null" />.
        /// </exception>
        public ChannelCollection(TChannelA channelA, TChannelB channelB, TChannelC channelC, TChannelD channelD, TChannelE channelE, TChannelF channelF, TChannelG channelG, TChannelH channelH, TChannelI channelI, TChannelJ channelJ, TChannelK channelK)
            : base(channelA, channelB, channelC, channelD, channelE, channelF, channelG, channelH, channelI, channelJ, channelK)
        {
            ChannelA = channelA;
            ChannelB = channelB;
            ChannelC = channelC;
            ChannelD = channelD;
            ChannelE = channelE;
            ChannelF = channelF;
            ChannelG = channelG;
            ChannelH = channelH;
            ChannelI = channelI;
            ChannelJ = channelJ;
            ChannelK = channelK;
        }

        /// <summary>
        /// Gets the first channel in the collection.
        /// </summary>
        public TChannelA ChannelA
        {
            get;
        }

        /// <summary>
        /// Gets the second channel in the collection.
        /// </summary>
        public TChannelB ChannelB
        {
            get;
        }

        /// <summary>
        /// Gets the third channel in the collection.
        /// </summary>
        public TChannelC ChannelC
        {
            get;
        }

        /// <summary>
        /// Gets the fourth channel in the collection.
        /// </summary>
        public TChannelD ChannelD
        {
            get;
        }

        /// <summary>
        /// Gets the fifth channel in the collection.
        /// </summary>
        public TChannelE ChannelE
        {
            get;
        }

        /// <summary>
        /// Gets the sixth channel in the collection.
        /// </summary>
        public TChannelF ChannelF
        {
            get;
        }

        /// <summary>
        /// Gets the seventh channel in the collection.
        /// </summary>
        public TChannelG ChannelG
        {
            get;
        }

        /// <summary>
        /// Gets the eighth channel in the collection.
        /// </summary>
        public TChannelH ChannelH
        {
            get;
        }

        /// <summary>
        /// Gets the ninth channel in the collection.
        /// </summary>
        public TChannelI ChannelI
        {
            get;
        }

        /// <summary>
        /// Gets the tenth channel in the collection.
        /// </summary>
        public TChannelJ ChannelJ
        {
            get;
        }

        /// <summary>
        /// Gets the eleventh channel in the collection.
        /// </summary>
        public TChannelK ChannelK
        {
            get;
        }
    }

    /// <summary>
    /// Represents a collection of streaming data signals.
    /// </summary>
    /// <typeparam name="TChannelA">
    /// The type of the first channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelB">
    /// The type of the second channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelC">
    /// The type of the third channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelD">
    /// The type of the fourth channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelE">
    /// The type of the fifth channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelF">
    /// The type of the sixth channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelG">
    /// The type of the seventh channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelH">
    /// The type of the eighth channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelI">
    /// The type of the ninth channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelJ">
    /// The type of the tenth channel in the collection.
    /// </typeparam>
    public class ChannelCollection<TChannelA, TChannelB, TChannelC, TChannelD, TChannelE, TChannelF, TChannelG, TChannelH, TChannelI, TChannelJ> : ChannelCollection, IChannelCollection<TChannelA, TChannelB, TChannelC, TChannelD, TChannelE, TChannelF, TChannelG, TChannelH, TChannelI, TChannelJ>
        where TChannelA : class, IChannel
        where TChannelB : class, IChannel
        where TChannelC : class, IChannel
        where TChannelD : class, IChannel
        where TChannelE : class, IChannel
        where TChannelF : class, IChannel
        where TChannelG : class, IChannel
        where TChannelH : class, IChannel
        where TChannelI : class, IChannel
        where TChannelJ : class, IChannel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelCollection{TChannelA}" /> class.
        /// </summary>
        /// <param name="channelA">
        /// The first channel in the collection.
        /// </param>
        /// <param name="channelB">
        /// The second channel in the collection.
        /// </param>
        /// <param name="channelC">
        /// The third channel in the collection.
        /// </param>
        /// <param name="channelD">
        /// The fourth channel in the collection.
        /// </param>
        /// <param name="channelE">
        /// The fifth channel in the collection.
        /// </param>
        /// <param name="channelF">
        /// The sixth channel in the collection.
        /// </param>
        /// <param name="channelG">
        /// The seventh channel in the collection.
        /// </param>
        /// <param name="channelH">
        /// The eighth channel in the collection.
        /// </param>
        /// <param name="channelI">
        /// The ninth channel in the collection.
        /// </param>
        /// <param name="channelJ">
        /// The tenth channel in the collection.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// One or more of the specified channels is <see langword="null" />.
        /// </exception>
        public ChannelCollection(TChannelA channelA, TChannelB channelB, TChannelC channelC, TChannelD channelD, TChannelE channelE, TChannelF channelF, TChannelG channelG, TChannelH channelH, TChannelI channelI, TChannelJ channelJ)
            : base(channelA, channelB, channelC, channelD, channelE, channelF, channelG, channelH, channelI, channelJ)
        {
            ChannelA = channelA;
            ChannelB = channelB;
            ChannelC = channelC;
            ChannelD = channelD;
            ChannelE = channelE;
            ChannelF = channelF;
            ChannelG = channelG;
            ChannelH = channelH;
            ChannelI = channelI;
            ChannelJ = channelJ;
        }

        /// <summary>
        /// Gets the first channel in the collection.
        /// </summary>
        public TChannelA ChannelA
        {
            get;
        }

        /// <summary>
        /// Gets the second channel in the collection.
        /// </summary>
        public TChannelB ChannelB
        {
            get;
        }

        /// <summary>
        /// Gets the third channel in the collection.
        /// </summary>
        public TChannelC ChannelC
        {
            get;
        }

        /// <summary>
        /// Gets the fourth channel in the collection.
        /// </summary>
        public TChannelD ChannelD
        {
            get;
        }

        /// <summary>
        /// Gets the fifth channel in the collection.
        /// </summary>
        public TChannelE ChannelE
        {
            get;
        }

        /// <summary>
        /// Gets the sixth channel in the collection.
        /// </summary>
        public TChannelF ChannelF
        {
            get;
        }

        /// <summary>
        /// Gets the seventh channel in the collection.
        /// </summary>
        public TChannelG ChannelG
        {
            get;
        }

        /// <summary>
        /// Gets the eighth channel in the collection.
        /// </summary>
        public TChannelH ChannelH
        {
            get;
        }

        /// <summary>
        /// Gets the ninth channel in the collection.
        /// </summary>
        public TChannelI ChannelI
        {
            get;
        }

        /// <summary>
        /// Gets the tenth channel in the collection.
        /// </summary>
        public TChannelJ ChannelJ
        {
            get;
        }
    }

    /// <summary>
    /// Represents a collection of streaming data signals.
    /// </summary>
    /// <typeparam name="TChannelA">
    /// The type of the first channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelB">
    /// The type of the second channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelC">
    /// The type of the third channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelD">
    /// The type of the fourth channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelE">
    /// The type of the fifth channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelF">
    /// The type of the sixth channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelG">
    /// The type of the seventh channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelH">
    /// The type of the eighth channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelI">
    /// The type of the ninth channel in the collection.
    /// </typeparam>
    public class ChannelCollection<TChannelA, TChannelB, TChannelC, TChannelD, TChannelE, TChannelF, TChannelG, TChannelH, TChannelI> : ChannelCollection, IChannelCollection<TChannelA, TChannelB, TChannelC, TChannelD, TChannelE, TChannelF, TChannelG, TChannelH, TChannelI>
        where TChannelA : class, IChannel
        where TChannelB : class, IChannel
        where TChannelC : class, IChannel
        where TChannelD : class, IChannel
        where TChannelE : class, IChannel
        where TChannelF : class, IChannel
        where TChannelG : class, IChannel
        where TChannelH : class, IChannel
        where TChannelI : class, IChannel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelCollection{TChannelA}" /> class.
        /// </summary>
        /// <param name="channelA">
        /// The first channel in the collection.
        /// </param>
        /// <param name="channelB">
        /// The second channel in the collection.
        /// </param>
        /// <param name="channelC">
        /// The third channel in the collection.
        /// </param>
        /// <param name="channelD">
        /// The fourth channel in the collection.
        /// </param>
        /// <param name="channelE">
        /// The fifth channel in the collection.
        /// </param>
        /// <param name="channelF">
        /// The sixth channel in the collection.
        /// </param>
        /// <param name="channelG">
        /// The seventh channel in the collection.
        /// </param>
        /// <param name="channelH">
        /// The eighth channel in the collection.
        /// </param>
        /// <param name="channelI">
        /// The ninth channel in the collection.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// One or more of the specified channels is <see langword="null" />.
        /// </exception>
        public ChannelCollection(TChannelA channelA, TChannelB channelB, TChannelC channelC, TChannelD channelD, TChannelE channelE, TChannelF channelF, TChannelG channelG, TChannelH channelH, TChannelI channelI)
            : base(channelA, channelB, channelC, channelD, channelE, channelF, channelG, channelH, channelI)
        {
            ChannelA = channelA;
            ChannelB = channelB;
            ChannelC = channelC;
            ChannelD = channelD;
            ChannelE = channelE;
            ChannelF = channelF;
            ChannelG = channelG;
            ChannelH = channelH;
            ChannelI = channelI;
        }

        /// <summary>
        /// Gets the first channel in the collection.
        /// </summary>
        public TChannelA ChannelA
        {
            get;
        }

        /// <summary>
        /// Gets the second channel in the collection.
        /// </summary>
        public TChannelB ChannelB
        {
            get;
        }

        /// <summary>
        /// Gets the third channel in the collection.
        /// </summary>
        public TChannelC ChannelC
        {
            get;
        }

        /// <summary>
        /// Gets the fourth channel in the collection.
        /// </summary>
        public TChannelD ChannelD
        {
            get;
        }

        /// <summary>
        /// Gets the fifth channel in the collection.
        /// </summary>
        public TChannelE ChannelE
        {
            get;
        }

        /// <summary>
        /// Gets the sixth channel in the collection.
        /// </summary>
        public TChannelF ChannelF
        {
            get;
        }

        /// <summary>
        /// Gets the seventh channel in the collection.
        /// </summary>
        public TChannelG ChannelG
        {
            get;
        }

        /// <summary>
        /// Gets the eighth channel in the collection.
        /// </summary>
        public TChannelH ChannelH
        {
            get;
        }

        /// <summary>
        /// Gets the ninth channel in the collection.
        /// </summary>
        public TChannelI ChannelI
        {
            get;
        }
    }

    /// <summary>
    /// Represents a collection of streaming data signals.
    /// </summary>
    /// <typeparam name="TChannelA">
    /// The type of the first channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelB">
    /// The type of the second channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelC">
    /// The type of the third channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelD">
    /// The type of the fourth channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelE">
    /// The type of the fifth channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelF">
    /// The type of the sixth channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelG">
    /// The type of the seventh channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelH">
    /// The type of the eighth channel in the collection.
    /// </typeparam>
    public class ChannelCollection<TChannelA, TChannelB, TChannelC, TChannelD, TChannelE, TChannelF, TChannelG, TChannelH> : ChannelCollection, IChannelCollection<TChannelA, TChannelB, TChannelC, TChannelD, TChannelE, TChannelF, TChannelG, TChannelH>
        where TChannelA : class, IChannel
        where TChannelB : class, IChannel
        where TChannelC : class, IChannel
        where TChannelD : class, IChannel
        where TChannelE : class, IChannel
        where TChannelF : class, IChannel
        where TChannelG : class, IChannel
        where TChannelH : class, IChannel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelCollection{TChannelA}" /> class.
        /// </summary>
        /// <param name="channelA">
        /// The first channel in the collection.
        /// </param>
        /// <param name="channelB">
        /// The second channel in the collection.
        /// </param>
        /// <param name="channelC">
        /// The third channel in the collection.
        /// </param>
        /// <param name="channelD">
        /// The fourth channel in the collection.
        /// </param>
        /// <param name="channelE">
        /// The fifth channel in the collection.
        /// </param>
        /// <param name="channelF">
        /// The sixth channel in the collection.
        /// </param>
        /// <param name="channelG">
        /// The seventh channel in the collection.
        /// </param>
        /// <param name="channelH">
        /// The eighth channel in the collection.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// One or more of the specified channels is <see langword="null" />.
        /// </exception>
        public ChannelCollection(TChannelA channelA, TChannelB channelB, TChannelC channelC, TChannelD channelD, TChannelE channelE, TChannelF channelF, TChannelG channelG, TChannelH channelH)
            : base(channelA, channelB, channelC, channelD, channelE, channelF, channelG, channelH)
        {
            ChannelA = channelA;
            ChannelB = channelB;
            ChannelC = channelC;
            ChannelD = channelD;
            ChannelE = channelE;
            ChannelF = channelF;
            ChannelG = channelG;
            ChannelH = channelH;
        }

        /// <summary>
        /// Gets the first channel in the collection.
        /// </summary>
        public TChannelA ChannelA
        {
            get;
        }

        /// <summary>
        /// Gets the second channel in the collection.
        /// </summary>
        public TChannelB ChannelB
        {
            get;
        }

        /// <summary>
        /// Gets the third channel in the collection.
        /// </summary>
        public TChannelC ChannelC
        {
            get;
        }

        /// <summary>
        /// Gets the fourth channel in the collection.
        /// </summary>
        public TChannelD ChannelD
        {
            get;
        }

        /// <summary>
        /// Gets the fifth channel in the collection.
        /// </summary>
        public TChannelE ChannelE
        {
            get;
        }

        /// <summary>
        /// Gets the sixth channel in the collection.
        /// </summary>
        public TChannelF ChannelF
        {
            get;
        }

        /// <summary>
        /// Gets the seventh channel in the collection.
        /// </summary>
        public TChannelG ChannelG
        {
            get;
        }

        /// <summary>
        /// Gets the eighth channel in the collection.
        /// </summary>
        public TChannelH ChannelH
        {
            get;
        }
    }

    /// <summary>
    /// Represents a collection of streaming data signals.
    /// </summary>
    /// <typeparam name="TChannelA">
    /// The type of the first channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelB">
    /// The type of the second channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelC">
    /// The type of the third channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelD">
    /// The type of the fourth channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelE">
    /// The type of the fifth channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelF">
    /// The type of the sixth channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelG">
    /// The type of the seventh channel in the collection.
    /// </typeparam>
    public class ChannelCollection<TChannelA, TChannelB, TChannelC, TChannelD, TChannelE, TChannelF, TChannelG> : ChannelCollection, IChannelCollection<TChannelA, TChannelB, TChannelC, TChannelD, TChannelE, TChannelF, TChannelG>
        where TChannelA : class, IChannel
        where TChannelB : class, IChannel
        where TChannelC : class, IChannel
        where TChannelD : class, IChannel
        where TChannelE : class, IChannel
        where TChannelF : class, IChannel
        where TChannelG : class, IChannel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelCollection{TChannelA}" /> class.
        /// </summary>
        /// <param name="channelA">
        /// The first channel in the collection.
        /// </param>
        /// <param name="channelB">
        /// The second channel in the collection.
        /// </param>
        /// <param name="channelC">
        /// The third channel in the collection.
        /// </param>
        /// <param name="channelD">
        /// The fourth channel in the collection.
        /// </param>
        /// <param name="channelE">
        /// The fifth channel in the collection.
        /// </param>
        /// <param name="channelF">
        /// The sixth channel in the collection.
        /// </param>
        /// <param name="channelG">
        /// The seventh channel in the collection.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// One or more of the specified channels is <see langword="null" />.
        /// </exception>
        public ChannelCollection(TChannelA channelA, TChannelB channelB, TChannelC channelC, TChannelD channelD, TChannelE channelE, TChannelF channelF, TChannelG channelG)
            : base(channelA, channelB, channelC, channelD, channelE, channelF, channelG)
        {
            ChannelA = channelA;
            ChannelB = channelB;
            ChannelC = channelC;
            ChannelD = channelD;
            ChannelE = channelE;
            ChannelF = channelF;
            ChannelG = channelG;
        }

        /// <summary>
        /// Gets the first channel in the collection.
        /// </summary>
        public TChannelA ChannelA
        {
            get;
        }

        /// <summary>
        /// Gets the second channel in the collection.
        /// </summary>
        public TChannelB ChannelB
        {
            get;
        }

        /// <summary>
        /// Gets the third channel in the collection.
        /// </summary>
        public TChannelC ChannelC
        {
            get;
        }

        /// <summary>
        /// Gets the fourth channel in the collection.
        /// </summary>
        public TChannelD ChannelD
        {
            get;
        }

        /// <summary>
        /// Gets the fifth channel in the collection.
        /// </summary>
        public TChannelE ChannelE
        {
            get;
        }

        /// <summary>
        /// Gets the sixth channel in the collection.
        /// </summary>
        public TChannelF ChannelF
        {
            get;
        }

        /// <summary>
        /// Gets the seventh channel in the collection.
        /// </summary>
        public TChannelG ChannelG
        {
            get;
        }
    }

    /// <summary>
    /// Represents a collection of streaming data signals.
    /// </summary>
    /// <typeparam name="TChannelA">
    /// The type of the first channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelB">
    /// The type of the second channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelC">
    /// The type of the third channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelD">
    /// The type of the fourth channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelE">
    /// The type of the fifth channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelF">
    /// The type of the sixth channel in the collection.
    /// </typeparam>
    public class ChannelCollection<TChannelA, TChannelB, TChannelC, TChannelD, TChannelE, TChannelF> : ChannelCollection, IChannelCollection<TChannelA, TChannelB, TChannelC, TChannelD, TChannelE, TChannelF>
        where TChannelA : class, IChannel
        where TChannelB : class, IChannel
        where TChannelC : class, IChannel
        where TChannelD : class, IChannel
        where TChannelE : class, IChannel
        where TChannelF : class, IChannel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelCollection{TChannelA}" /> class.
        /// </summary>
        /// <param name="channelA">
        /// The first channel in the collection.
        /// </param>
        /// <param name="channelB">
        /// The second channel in the collection.
        /// </param>
        /// <param name="channelC">
        /// The third channel in the collection.
        /// </param>
        /// <param name="channelD">
        /// The fourth channel in the collection.
        /// </param>
        /// <param name="channelE">
        /// The fifth channel in the collection.
        /// </param>
        /// <param name="channelF">
        /// The sixth channel in the collection.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// One or more of the specified channels is <see langword="null" />.
        /// </exception>
        public ChannelCollection(TChannelA channelA, TChannelB channelB, TChannelC channelC, TChannelD channelD, TChannelE channelE, TChannelF channelF)
            : base(channelA, channelB, channelC, channelD, channelE, channelF)
        {
            ChannelA = channelA;
            ChannelB = channelB;
            ChannelC = channelC;
            ChannelD = channelD;
            ChannelE = channelE;
            ChannelF = channelF;
        }

        /// <summary>
        /// Gets the first channel in the collection.
        /// </summary>
        public TChannelA ChannelA
        {
            get;
        }

        /// <summary>
        /// Gets the second channel in the collection.
        /// </summary>
        public TChannelB ChannelB
        {
            get;
        }

        /// <summary>
        /// Gets the third channel in the collection.
        /// </summary>
        public TChannelC ChannelC
        {
            get;
        }

        /// <summary>
        /// Gets the fourth channel in the collection.
        /// </summary>
        public TChannelD ChannelD
        {
            get;
        }

        /// <summary>
        /// Gets the fifth channel in the collection.
        /// </summary>
        public TChannelE ChannelE
        {
            get;
        }

        /// <summary>
        /// Gets the sixth channel in the collection.
        /// </summary>
        public TChannelF ChannelF
        {
            get;
        }
    }

    /// <summary>
    /// Represents a collection of streaming data signals.
    /// </summary>
    /// <typeparam name="TChannelA">
    /// The type of the first channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelB">
    /// The type of the second channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelC">
    /// The type of the third channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelD">
    /// The type of the fourth channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelE">
    /// The type of the fifth channel in the collection.
    /// </typeparam>
    public class ChannelCollection<TChannelA, TChannelB, TChannelC, TChannelD, TChannelE> : ChannelCollection, IChannelCollection<TChannelA, TChannelB, TChannelC, TChannelD, TChannelE>
        where TChannelA : class, IChannel
        where TChannelB : class, IChannel
        where TChannelC : class, IChannel
        where TChannelD : class, IChannel
        where TChannelE : class, IChannel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelCollection{TChannelA}" /> class.
        /// </summary>
        /// <param name="channelA">
        /// The first channel in the collection.
        /// </param>
        /// <param name="channelB">
        /// The second channel in the collection.
        /// </param>
        /// <param name="channelC">
        /// The third channel in the collection.
        /// </param>
        /// <param name="channelD">
        /// The fourth channel in the collection.
        /// </param>
        /// <param name="channelE">
        /// The fifth channel in the collection.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// One or more of the specified channels is <see langword="null" />.
        /// </exception>
        public ChannelCollection(TChannelA channelA, TChannelB channelB, TChannelC channelC, TChannelD channelD, TChannelE channelE)
            : base(channelA, channelB, channelC, channelD, channelE)
        {
            ChannelA = channelA;
            ChannelB = channelB;
            ChannelC = channelC;
            ChannelD = channelD;
            ChannelE = channelE;
        }

        /// <summary>
        /// Gets the first channel in the collection.
        /// </summary>
        public TChannelA ChannelA
        {
            get;
        }

        /// <summary>
        /// Gets the second channel in the collection.
        /// </summary>
        public TChannelB ChannelB
        {
            get;
        }

        /// <summary>
        /// Gets the third channel in the collection.
        /// </summary>
        public TChannelC ChannelC
        {
            get;
        }

        /// <summary>
        /// Gets the fourth channel in the collection.
        /// </summary>
        public TChannelD ChannelD
        {
            get;
        }

        /// <summary>
        /// Gets the fifth channel in the collection.
        /// </summary>
        public TChannelE ChannelE
        {
            get;
        }
    }

    /// <summary>
    /// Represents a collection of streaming data signals.
    /// </summary>
    /// <typeparam name="TChannelA">
    /// The type of the first channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelB">
    /// The type of the second channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelC">
    /// The type of the third channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelD">
    /// The type of the fourth channel in the collection.
    /// </typeparam>
    public class ChannelCollection<TChannelA, TChannelB, TChannelC, TChannelD> : ChannelCollection, IChannelCollection<TChannelA, TChannelB, TChannelC, TChannelD>
        where TChannelA : class, IChannel
        where TChannelB : class, IChannel
        where TChannelC : class, IChannel
        where TChannelD : class, IChannel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelCollection{TChannelA}" /> class.
        /// </summary>
        /// <param name="channelA">
        /// The first channel in the collection.
        /// </param>
        /// <param name="channelB">
        /// The second channel in the collection.
        /// </param>
        /// <param name="channelC">
        /// The third channel in the collection.
        /// </param>
        /// <param name="channelD">
        /// The fourth channel in the collection.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// One or more of the specified channels is <see langword="null" />.
        /// </exception>
        public ChannelCollection(TChannelA channelA, TChannelB channelB, TChannelC channelC, TChannelD channelD)
            : base(channelA, channelB, channelC, channelD)
        {
            ChannelA = channelA;
            ChannelB = channelB;
            ChannelC = channelC;
            ChannelD = channelD;
        }

        /// <summary>
        /// Gets the first channel in the collection.
        /// </summary>
        public TChannelA ChannelA
        {
            get;
        }

        /// <summary>
        /// Gets the second channel in the collection.
        /// </summary>
        public TChannelB ChannelB
        {
            get;
        }

        /// <summary>
        /// Gets the third channel in the collection.
        /// </summary>
        public TChannelC ChannelC
        {
            get;
        }

        /// <summary>
        /// Gets the fourth channel in the collection.
        /// </summary>
        public TChannelD ChannelD
        {
            get;
        }
    }

    /// <summary>
    /// Represents a collection of streaming data signals.
    /// </summary>
    /// <typeparam name="TChannelA">
    /// The type of the first channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelB">
    /// The type of the second channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelC">
    /// The type of the third channel in the collection.
    /// </typeparam>
    public class ChannelCollection<TChannelA, TChannelB, TChannelC> : ChannelCollection, IChannelCollection<TChannelA, TChannelB, TChannelC>
        where TChannelA : class, IChannel
        where TChannelB : class, IChannel
        where TChannelC : class, IChannel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelCollection{TChannelA}" /> class.
        /// </summary>
        /// <param name="channelA">
        /// The first channel in the collection.
        /// </param>
        /// <param name="channelB">
        /// The second channel in the collection.
        /// </param>
        /// <param name="channelC">
        /// The third channel in the collection.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// One or more of the specified channels is <see langword="null" />.
        /// </exception>
        public ChannelCollection(TChannelA channelA, TChannelB channelB, TChannelC channelC)
            : base(channelA, channelB, channelC)
        {
            ChannelA = channelA;
            ChannelB = channelB;
            ChannelC = channelC;
        }

        /// <summary>
        /// Gets the first channel in the collection.
        /// </summary>
        public TChannelA ChannelA
        {
            get;
        }

        /// <summary>
        /// Gets the second channel in the collection.
        /// </summary>
        public TChannelB ChannelB
        {
            get;
        }

        /// <summary>
        /// Gets the third channel in the collection.
        /// </summary>
        public TChannelC ChannelC
        {
            get;
        }
    }

    /// <summary>
    /// Represents a collection of streaming data signals.
    /// </summary>
    /// <typeparam name="TChannelA">
    /// The type of the first channel in the collection.
    /// </typeparam>
    /// <typeparam name="TChannelB">
    /// The type of the second channel in the collection.
    /// </typeparam>
    public class ChannelCollection<TChannelA, TChannelB> : ChannelCollection, IChannelCollection<TChannelA, TChannelB>
        where TChannelA : class, IChannel
        where TChannelB : class, IChannel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelCollection{TChannelA}" /> class.
        /// </summary>
        /// <param name="channelA">
        /// The first channel in the collection.
        /// </param>
        /// <param name="channelB">
        /// The second channel in the collection.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// One or more of the specified channels is <see langword="null" />.
        /// </exception>
        public ChannelCollection(TChannelA channelA, TChannelB channelB)
            : base(channelA, channelB)
        {
            ChannelA = channelA;
            ChannelB = channelB;
        }

        /// <summary>
        /// Gets the first channel in the collection.
        /// </summary>
        public TChannelA ChannelA
        {
            get;
        }

        /// <summary>
        /// Gets the second channel in the collection.
        /// </summary>
        public TChannelB ChannelB
        {
            get;
        }
    }

    /// <summary>
    /// Represents a collection of streaming data signals.
    /// </summary>
    /// <typeparam name="TChannelA">
    /// The type of the first channel in the collection.
    /// </typeparam>
    public class ChannelCollection<TChannelA> : ChannelCollection, IChannelCollection<TChannelA>
        where TChannelA : class, IChannel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelCollection{TChannelA}" /> class.
        /// </summary>
        /// <param name="channelA">
        /// The first channel in the collection.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="channelA" /> is <see langword="null" />.
        /// </exception>
        public ChannelCollection(TChannelA channelA)
            : base(channelA)
        {
            ChannelA = channelA;
        }

        /// <summary>
        /// Gets the first channel in the collection.
        /// </summary>
        public TChannelA ChannelA
        {
            get;
        }
    }

    /// <summary>
    /// Represents a collection of streaming data signals.
    /// </summary>
    /// <remarks>
    /// <see cref="ChannelCollection" /> is the default implementation of <see cref="IChannelCollection" />.
    /// </remarks>
    public abstract class ChannelCollection : IChannelCollection
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelCollection" /> class.
        /// </summary>
        /// <param name="channels">
        /// A collection of channels that is wrapped by the new collection.
        /// </param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="channels" /> is empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="channels" /> is <see langword="null" /> -or- <paramref name="channels" /> contains one or more null
        /// elements.
        /// </exception>
        protected ChannelCollection(params IChannel[] channels)
        {
            try
            {
                Channels = channels.RejectIf().IsNullOrEmpty(nameof(channels)).OrIf(argument => argument.Any(element => element is null), nameof(channels), "The specified collection contains one or more null channels.");
            }
            catch (ArgumentEmptyException)
            {
                throw;
            }
            catch (ArgumentNullException)
            {
                throw;
            }
            catch (ArgumentException exception)
            {
                throw new ArgumentNullException(nameof(channels), exception.Message);
            }

            Count = Channels.Length;
            Identifier = Guid.NewGuid();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the elements of the current <see cref="ChannelCollection" />.
        /// </summary>
        /// <returns>
        /// An enumerator that iterates through the elements of the current <see cref="ChannelCollection" />.
        /// </returns>
        public IEnumerator<IChannel> GetEnumerator()
        {
            for (var i = 0; i < Count; i++)
            {
                yield return Channels[i];
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the elements of the current <see cref="ChannelCollection" />.
        /// </summary>
        /// <returns>
        /// An enumerator that iterates through the elements of the current <see cref="ChannelCollection" />.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Converts the value of the current <see cref="ChannelCollection" /> to its equivalent string representation.
        /// </summary>
        /// <returns>
        /// A string representation of the current <see cref="ChannelCollection" />.
        /// </returns>
        public override String ToString() => $"{{ {nameof(Identifier)}: {Identifier.ToSerializedString()}, {nameof(Count)}: {Count} }}";

        /// <summary>
        /// Gets the number of channels in the collection.
        /// </summary>
        public Int32 Count
        {
            get;
        }

        /// <summary>
        /// Gets a unique identifier for the current <see cref="ChannelCollection" />.
        /// </summary>
        public Guid Identifier
        {
            get;
        }

        /// <summary>
        /// Represents a collection of channels that is wrapped by the current <see cref="ChannelCollection" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly IChannel[] Channels;
    }
}