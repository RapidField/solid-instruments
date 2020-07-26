// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using System;
using System.Collections.Generic;

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
    public interface IChannelCollection<TChannelA, TChannelB, TChannelC, TChannelD, TChannelE, TChannelF, TChannelG, TChannelH, TChannelI, TChannelJ, TChannelK, TChannelL, TChannelM, TChannelN, TChannelO, TChannelP> : IChannelCollection
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
    public interface IChannelCollection<TChannelA, TChannelB, TChannelC, TChannelD, TChannelE, TChannelF, TChannelG, TChannelH, TChannelI, TChannelJ, TChannelK, TChannelL, TChannelM, TChannelN, TChannelO> : IChannelCollection
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
    public interface IChannelCollection<TChannelA, TChannelB, TChannelC, TChannelD, TChannelE, TChannelF, TChannelG, TChannelH, TChannelI, TChannelJ, TChannelK, TChannelL, TChannelM, TChannelN> : IChannelCollection
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
    public interface IChannelCollection<TChannelA, TChannelB, TChannelC, TChannelD, TChannelE, TChannelF, TChannelG, TChannelH, TChannelI, TChannelJ, TChannelK, TChannelL, TChannelM> : IChannelCollection
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
    public interface IChannelCollection<TChannelA, TChannelB, TChannelC, TChannelD, TChannelE, TChannelF, TChannelG, TChannelH, TChannelI, TChannelJ, TChannelK, TChannelL> : IChannelCollection
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
    public interface IChannelCollection<TChannelA, TChannelB, TChannelC, TChannelD, TChannelE, TChannelF, TChannelG, TChannelH, TChannelI, TChannelJ, TChannelK> : IChannelCollection
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
    public interface IChannelCollection<TChannelA, TChannelB, TChannelC, TChannelD, TChannelE, TChannelF, TChannelG, TChannelH, TChannelI, TChannelJ> : IChannelCollection
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
    public interface IChannelCollection<TChannelA, TChannelB, TChannelC, TChannelD, TChannelE, TChannelF, TChannelG, TChannelH, TChannelI> : IChannelCollection
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
    public interface IChannelCollection<TChannelA, TChannelB, TChannelC, TChannelD, TChannelE, TChannelF, TChannelG, TChannelH> : IChannelCollection
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
    public interface IChannelCollection<TChannelA, TChannelB, TChannelC, TChannelD, TChannelE, TChannelF, TChannelG> : IChannelCollection
        where TChannelA : class, IChannel
        where TChannelB : class, IChannel
        where TChannelC : class, IChannel
        where TChannelD : class, IChannel
        where TChannelE : class, IChannel
        where TChannelF : class, IChannel
        where TChannelG : class, IChannel
    {
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
    public interface IChannelCollection<TChannelA, TChannelB, TChannelC, TChannelD, TChannelE, TChannelF> : IChannelCollection
        where TChannelA : class, IChannel
        where TChannelB : class, IChannel
        where TChannelC : class, IChannel
        where TChannelD : class, IChannel
        where TChannelE : class, IChannel
        where TChannelF : class, IChannel
    {
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
    public interface IChannelCollection<TChannelA, TChannelB, TChannelC, TChannelD, TChannelE> : IChannelCollection
        where TChannelA : class, IChannel
        where TChannelB : class, IChannel
        where TChannelC : class, IChannel
        where TChannelD : class, IChannel
        where TChannelE : class, IChannel
    {
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
    public interface IChannelCollection<TChannelA, TChannelB, TChannelC, TChannelD> : IChannelCollection
        where TChannelA : class, IChannel
        where TChannelB : class, IChannel
        where TChannelC : class, IChannel
        where TChannelD : class, IChannel
    {
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
    public interface IChannelCollection<TChannelA, TChannelB, TChannelC> : IChannelCollection
        where TChannelA : class, IChannel
        where TChannelB : class, IChannel
        where TChannelC : class, IChannel
    {
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
    public interface IChannelCollection<TChannelA, TChannelB> : IChannelCollection
        where TChannelA : class, IChannel
        where TChannelB : class, IChannel
    {
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
    public interface IChannelCollection<TChannelA> : IChannelCollection
        where TChannelA : class, IChannel
    {
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
    public interface IChannelCollection : IReadOnlyCollection<IChannel>
    {
        /// <summary>
        /// Gets a unique identifier for the current <see cref="IChannelCollection" />.
        /// </summary>
        public Guid Identifier
        {
            get;
        }
    }
}