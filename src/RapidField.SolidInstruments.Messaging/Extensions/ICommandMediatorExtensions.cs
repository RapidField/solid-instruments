// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using RapidField.SolidInstruments.Command;
using RapidField.SolidInstruments.Core.ArgumentValidation;
using RapidField.SolidInstruments.Messaging.CommandMessages;
using RapidField.SolidInstruments.Messaging.EventMessages;
using System;
using System.Threading.Tasks;

namespace RapidField.SolidInstruments.Messaging.Extensions
{
    /// <summary>
    /// Extends the <see cref="ICommandMediator" /> interface with message handling features.
    /// </summary>
    public static class ICommandMediatorExtensions
    {
        /// <summary>
        /// Transmits the specified <see cref="ICommandMessage" />.
        /// </summary>
        /// <typeparam name="TCommandMessage">
        /// The type of the command message to transmit.
        /// </typeparam>
        /// <param name="target">
        /// The current <see cref="ICommandMediator" />.
        /// </param>
        /// <param name="createMessageFunction">
        /// A function that returns the command message to transmit.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="createMessageFunction" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="CommandHandlingException">
        /// An exception was raised while processing the message.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// <paramref name="target" /> is disposed.
        /// </exception>
        public static void TransmitCommandMessage<TCommandMessage>(this ICommandMediator target, Func<ICommandMessageRegister, TCommandMessage> createMessageFunction)
            where TCommandMessage : class, ICommandMessage
        {
            try
            {
                _ = target.Process(createMessageFunction.RejectIf().IsNull(nameof(createMessageFunction)).TargetArgument(CommandMessageRegister.Instance));
            }
            catch (CommandHandlingException)
            {
                throw;
            }
            catch (ObjectDisposedException)
            {
                throw;
            }
            catch (Exception exception)
            {
                throw new CommandHandlingException(typeof(TCommandMessage), exception);
            }
        }

        /// <summary>
        /// Asynchronously transmits the specified <see cref="ICommandMessage" />.
        /// </summary>
        /// <typeparam name="TCommandMessage">
        /// The type of the command message to transmit.
        /// </typeparam>
        /// <param name="target">
        /// The current <see cref="ICommandMediator" />.
        /// </param>
        /// <param name="createMessageFunction">
        /// A function that returns the command message to transmit.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="createMessageFunction" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="CommandHandlingException">
        /// An exception was raised while processing the message.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// <paramref name="target" /> is disposed.
        /// </exception>
        public static Task TransmitCommandMessageAsync<TCommandMessage>(this ICommandMediator target, Func<ICommandMessageRegister, TCommandMessage> createMessageFunction)
            where TCommandMessage : class, ICommandMessage
        {
            try
            {
                return target.ProcessAsync(createMessageFunction.RejectIf().IsNull(nameof(createMessageFunction)).TargetArgument(CommandMessageRegister.Instance));
            }
            catch (CommandHandlingException)
            {
                throw;
            }
            catch (ObjectDisposedException)
            {
                throw;
            }
            catch (Exception exception)
            {
                throw new CommandHandlingException(typeof(TCommandMessage), exception);
            }
        }

        /// <summary>
        /// Transmits the specified <see cref="IEventMessage{TEvent}" />.
        /// </summary>
        /// <typeparam name="TEventMessage">
        /// The type of the event message to transmit.
        /// </typeparam>
        /// <param name="target">
        /// The current <see cref="ICommandMediator" />.
        /// </param>
        /// <param name="createMessageFunction">
        /// A function that returns the event message to transmit.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="createMessageFunction" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="CommandHandlingException">
        /// An exception was raised while processing the message.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// <paramref name="target" /> is disposed.
        /// </exception>
        public static void TransmitEventMessage<TEventMessage>(this ICommandMediator target, Func<IEventMessageRegister, TEventMessage> createMessageFunction)
            where TEventMessage : class, IMessage, IEventMessageBase
        {
            try
            {
                _ = target.Process(createMessageFunction.RejectIf().IsNull(nameof(createMessageFunction)).TargetArgument(EventMessageRegister.Instance));
            }
            catch (CommandHandlingException)
            {
                throw;
            }
            catch (ObjectDisposedException)
            {
                throw;
            }
            catch (Exception exception)
            {
                throw new CommandHandlingException(typeof(TEventMessage), exception);
            }
        }

        /// <summary>
        /// Asynchronously transmits the specified <see cref="IEventMessage{TEvent}" />.
        /// </summary>
        /// <typeparam name="TEventMessage">
        /// The type of the event message to transmit.
        /// </typeparam>
        /// <param name="target">
        /// The current <see cref="ICommandMediator" />.
        /// </param>
        /// <param name="createMessageFunction">
        /// A function that returns the event message to transmit.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="createMessageFunction" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="CommandHandlingException">
        /// An exception was raised while processing the message.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// <paramref name="target" /> is disposed.
        /// </exception>
        public static Task TransmitEventMessageAsync<TEventMessage>(this ICommandMediator target, Func<IEventMessageRegister, TEventMessage> createMessageFunction)
            where TEventMessage : class, IMessage, IEventMessageBase
        {
            try
            {
                return target.ProcessAsync(createMessageFunction.RejectIf().IsNull(nameof(createMessageFunction)).TargetArgument(EventMessageRegister.Instance));
            }
            catch (CommandHandlingException)
            {
                throw;
            }
            catch (ObjectDisposedException)
            {
                throw;
            }
            catch (Exception exception)
            {
                throw new CommandHandlingException(typeof(TEventMessage), exception);
            }
        }

        /// <summary>
        /// Transmits the specified <see cref="IMessage{TResult}" />.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message to transmit.
        /// </typeparam>
        /// <typeparam name="TResult">
        /// The type of the result that is produced by processing the message.
        /// </typeparam>
        /// <param name="target">
        /// The current <see cref="ICommandMediator" />.
        /// </param>
        /// <param name="createMessageFunction">
        /// A function that returns the message to transmit.
        /// </param>
        /// <returns>
        /// The result that is produced by processing the message.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="createMessageFunction" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="CommandHandlingException">
        /// An exception was raised while processing the message.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// <paramref name="target" /> is disposed.
        /// </exception>
        public static TResult TransmitMessage<TMessage, TResult>(this ICommandMediator target, Func<IMessageRegister, TMessage> createMessageFunction)
            where TMessage : class, IMessage<TResult>
        {
            try
            {
                return target.Process(createMessageFunction.RejectIf().IsNull(nameof(createMessageFunction)).TargetArgument(MessageRegister.Instance));
            }
            catch (CommandHandlingException)
            {
                throw;
            }
            catch (ObjectDisposedException)
            {
                throw;
            }
            catch (Exception exception)
            {
                throw new CommandHandlingException(typeof(TMessage), exception);
            }
        }

        /// <summary>
        /// Asynchronously transmits the specified <see cref="IMessage{TResult}" />.
        /// </summary>
        /// <typeparam name="TMessage">
        /// The type of the message to transmit.
        /// </typeparam>
        /// <typeparam name="TResult">
        /// The type of the result that is produced by processing the message.
        /// </typeparam>
        /// <param name="target">
        /// The current <see cref="ICommandMediator" />.
        /// </param>
        /// <param name="createMessageFunction">
        /// A function that returns the message to transmit.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation and containing the result that is produced by processing the message.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="createMessageFunction" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="CommandHandlingException">
        /// An exception was raised while processing the message.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// <paramref name="target" /> is disposed.
        /// </exception>
        public static Task<TResult> TransmitMessageAsync<TMessage, TResult>(this ICommandMediator target, Func<IMessageRegister, TMessage> createMessageFunction)
            where TMessage : class, IMessage<TResult>
        {
            try
            {
                return target.ProcessAsync(createMessageFunction.RejectIf().IsNull(nameof(createMessageFunction)).TargetArgument(MessageRegister.Instance));
            }
            catch (CommandHandlingException)
            {
                throw;
            }
            catch (ObjectDisposedException)
            {
                throw;
            }
            catch (Exception exception)
            {
                throw new CommandHandlingException(typeof(TMessage), exception);
            }
        }

        /// <summary>
        /// Transmits the specified <see cref="IRequestMessage{TResponseMessage}" />.
        /// </summary>
        /// <typeparam name="TResponseMessage">
        /// The type of the response message that is produced by processing the request message.
        /// </typeparam>
        /// <param name="target">
        /// The current <see cref="ICommandMediator" />.
        /// </param>
        /// <param name="createMessageFunction">
        /// A function that returns the request message to transmit.
        /// </param>
        /// <returns>
        /// The response message that is produced by processing the request message.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="createMessageFunction" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="CommandHandlingException">
        /// An exception was raised while processing the message.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// <paramref name="target" /> is disposed.
        /// </exception>
        public static TResponseMessage TransmitRequestMessage<TResponseMessage>(this ICommandMediator target, Func<IRequestMessageRegister, IRequestMessage<TResponseMessage>> createMessageFunction)
            where TResponseMessage : class, IResponseMessage
        {
            try
            {
                return target.Process(createMessageFunction.RejectIf().IsNull(nameof(createMessageFunction)).TargetArgument(RequestMessageRegister.Instance));
            }
            catch (CommandHandlingException)
            {
                throw;
            }
            catch (ObjectDisposedException)
            {
                throw;
            }
            catch (Exception exception)
            {
                throw new CommandHandlingException(exception);
            }
        }

        /// <summary>
        /// Transmits the specified <see cref="IRequestMessage{TResponseMessage}" />.
        /// </summary>
        /// <typeparam name="TRequestMessage">
        /// The type of the request message to transmit.
        /// </typeparam>
        /// <typeparam name="TResponseMessage">
        /// The type of the response message that is produced by processing the request message.
        /// </typeparam>
        /// <param name="target">
        /// The current <see cref="ICommandMediator" />.
        /// </param>
        /// <param name="createMessageFunction">
        /// A function that returns the request message to transmit.
        /// </param>
        /// <returns>
        /// The response message that is produced by processing the request message.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="createMessageFunction" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="CommandHandlingException">
        /// An exception was raised while processing the message.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// <paramref name="target" /> is disposed.
        /// </exception>
        public static TResponseMessage TransmitRequestMessage<TRequestMessage, TResponseMessage>(this ICommandMediator target, Func<IRequestMessageRegister, TRequestMessage> createMessageFunction)
            where TRequestMessage : class, IRequestMessage<TResponseMessage>
            where TResponseMessage : class, IResponseMessage
        {
            try
            {
                return target.Process(createMessageFunction.RejectIf().IsNull(nameof(createMessageFunction)).TargetArgument(RequestMessageRegister.Instance));
            }
            catch (CommandHandlingException)
            {
                throw;
            }
            catch (ObjectDisposedException)
            {
                throw;
            }
            catch (Exception exception)
            {
                throw new CommandHandlingException(typeof(TRequestMessage), exception);
            }
        }

        /// <summary>
        /// Asynchronously transmits the specified <see cref="IRequestMessage{TResponseMessage}" />.
        /// </summary>
        /// <typeparam name="TResponseMessage">
        /// The type of the response message that is produced by processing the request message.
        /// </typeparam>
        /// <param name="target">
        /// The current <see cref="ICommandMediator" />.
        /// </param>
        /// <param name="createMessageFunction">
        /// A function that returns the request message to transmit.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation and containing the response message that is produced by processing the
        /// request message.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="createMessageFunction" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="CommandHandlingException">
        /// An exception was raised while processing the message.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// <paramref name="target" /> is disposed.
        /// </exception>
        public static Task<TResponseMessage> TransmitRequestMessageAsync<TResponseMessage>(this ICommandMediator target, Func<IRequestMessageRegister, IRequestMessage<TResponseMessage>> createMessageFunction)
            where TResponseMessage : class, IResponseMessage
        {
            try
            {
                return target.ProcessAsync(createMessageFunction.RejectIf().IsNull(nameof(createMessageFunction)).TargetArgument(RequestMessageRegister.Instance));
            }
            catch (CommandHandlingException)
            {
                throw;
            }
            catch (ObjectDisposedException)
            {
                throw;
            }
            catch (Exception exception)
            {
                throw new CommandHandlingException(exception);
            }
        }

        /// <summary>
        /// Asynchronously transmits the specified <see cref="IRequestMessage{TResponseMessage}" />.
        /// </summary>
        /// <typeparam name="TRequestMessage">
        /// The type of the request message to transmit.
        /// </typeparam>
        /// <typeparam name="TResponseMessage">
        /// The type of the response message that is produced by processing the request message.
        /// </typeparam>
        /// <param name="target">
        /// The current <see cref="ICommandMediator" />.
        /// </param>
        /// <param name="createMessageFunction">
        /// A function that returns the request message to transmit.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation and containing the response message that is produced by processing the
        /// request message.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="createMessageFunction" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="CommandHandlingException">
        /// An exception was raised while processing the message.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// <paramref name="target" /> is disposed.
        /// </exception>
        public static Task<TResponseMessage> TransmitRequestMessageAsync<TRequestMessage, TResponseMessage>(this ICommandMediator target, Func<IRequestMessageRegister, TRequestMessage> createMessageFunction)
            where TRequestMessage : class, IRequestMessage<TResponseMessage>
            where TResponseMessage : class, IResponseMessage
        {
            try
            {
                return target.ProcessAsync(createMessageFunction.RejectIf().IsNull(nameof(createMessageFunction)).TargetArgument(RequestMessageRegister.Instance));
            }
            catch (CommandHandlingException)
            {
                throw;
            }
            catch (ObjectDisposedException)
            {
                throw;
            }
            catch (Exception exception)
            {
                throw new CommandHandlingException(typeof(TRequestMessage), exception);
            }
        }
    }
}