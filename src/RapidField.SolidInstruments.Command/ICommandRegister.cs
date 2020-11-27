// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Command.Extensions;
using System;

namespace RapidField.SolidInstruments.Command
{
    /// <summary>
    /// Represents an extensible catalog of available commands.
    /// </summary>
    /// <remarks>
    /// Consuming libraries may use <see cref="ICommandRegister" /> as an extension method target to expose creational methods for
    /// custom commands. See <see cref="ICommandRegisterExtensions" /> for examples. An instance of an
    /// <see cref="ICommandRegister" /> is made available by
    /// <see cref="ICommandMediatorExtensions.Process{TCommand, TResult}(ICommandMediator, Func{ICommandRegister, TCommand})" /> and
    /// <see cref="ICommandMediatorExtensions.ProcessAsync{TCommand, TResult}(ICommandMediator, Func{ICommandRegister, TCommand})" />.
    /// </remarks>
    public interface ICommandRegister
    {
    }
}