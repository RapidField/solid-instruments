// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace RapidField.SolidInstruments.DataAccess.UnitTests
{
    [TestClass]
    public class DataAccessTransactionTests
    {
        [TestMethod]
        public void Begin_ShouldRaiseInvalidOperationException_ForCommittedTransaction()
        {
            using (var target = new SimulatedFooTransaction())
            {
                // Arrange.
                target.Begin();
                target.Commit();

                // Act.
                var action = new Action(() =>
                {
                    target.Begin();
                });

                // Assert.
                action.Should().Throw<InvalidOperationException>($"because {nameof(target)} is committed");
            }
        }

        [TestMethod]
        public void Begin_ShouldRaiseInvalidOperationException_ForInProgressTransaction()
        {
            using (var target = new SimulatedFooTransaction())
            {
                // Arrange.
                target.Begin();

                // Act.
                var action = new Action(() =>
                {
                    target.Begin();
                });

                // Assert.
                action.Should().Throw<InvalidOperationException>($"because {nameof(target)} has already begun");
            }
        }

        [TestMethod]
        public void Begin_ShouldRaiseInvalidOperationException_ForRejectedTransaction()
        {
            using (var target = new SimulatedFooTransaction())
            {
                // Arrange.
                target.Begin();
                target.Reject();

                // Act.
                var action = new Action(() =>
                {
                    target.Begin();
                });

                // Assert.
                action.Should().Throw<InvalidOperationException>($"because {nameof(target)} is rejected");
            }
        }

        [TestMethod]
        public void BeginAsync_ShouldRaiseInvalidOperationException_ForCommittedTransaction()
        {
            using (var target = new SimulatedFooTransaction())
            {
                // Arrange.
                target.BeginAsync().Wait();
                target.CommitAsync().Wait();

                // Act.
                var action = new Action(() =>
                {
                    target.BeginAsync().Wait();
                });

                // Assert.
                action.Should().Throw<InvalidOperationException>($"because {nameof(target)} is committed");
            }
        }

        [TestMethod]
        public void BeginAsync_ShouldRaiseInvalidOperationException_ForInProgressTransaction()
        {
            using (var target = new SimulatedFooTransaction())
            {
                // Arrange.
                target.BeginAsync().Wait();

                // Act.
                var action = new Action(() =>
                {
                    target.BeginAsync().Wait();
                });

                // Assert.
                action.Should().Throw<InvalidOperationException>($"because {nameof(target)} has already begun");
            }
        }

        [TestMethod]
        public void BeginAsync_ShouldRaiseInvalidOperationException_ForRejectedTransaction()
        {
            using (var target = new SimulatedFooTransaction())
            {
                // Arrange.
                target.BeginAsync().Wait();
                target.RejectAsync().Wait();

                // Act.
                var action = new Action(() =>
                {
                    target.BeginAsync().Wait();
                });

                // Assert.
                action.Should().Throw<InvalidOperationException>($"because {nameof(target)} is rejected");
            }
        }

        [TestMethod]
        public void Commit_ShouldRaiseInvalidOperationException_ForCommittedTransaction()
        {
            using (var target = new SimulatedFooTransaction())
            {
                // Arrange.
                target.Begin();
                target.Commit();

                // Act.
                var action = new Action(() =>
                {
                    target.Commit();
                });

                // Assert.
                action.Should().Throw<InvalidOperationException>($"because {nameof(target)} is committed");
            }
        }

        [TestMethod]
        public void Commit_ShouldRaiseInvalidOperationException_ForReadyStateTransaction()
        {
            using (var target = new SimulatedFooTransaction())
            {
                // Act.
                var action = new Action(() =>
                {
                    target.Commit();
                });

                // Assert.
                action.Should().Throw<InvalidOperationException>($"because {nameof(target)} has not yet begun");
            }
        }

        [TestMethod]
        public void Commit_ShouldRaiseInvalidOperationException_ForRejectedTransaction()
        {
            using (var target = new SimulatedFooTransaction())
            {
                // Arrange.
                target.Begin();
                target.Reject();

                // Act.
                var action = new Action(() =>
                {
                    target.Commit();
                });

                // Assert.
                action.Should().Throw<InvalidOperationException>($"because {nameof(target)} is rejected");
            }
        }

        [TestMethod]
        public void CommitAsync_ShouldRaiseInvalidOperationException_ForCommittedTransaction()
        {
            using (var target = new SimulatedFooTransaction())
            {
                // Arrange.
                target.BeginAsync().Wait();
                target.CommitAsync().Wait();

                // Act.
                var action = new Action(() =>
                {
                    target.CommitAsync().Wait();
                });

                // Assert.
                action.Should().Throw<InvalidOperationException>($"because {nameof(target)} is committed");
            }
        }

        [TestMethod]
        public void CommitAsync_ShouldRaiseInvalidOperationException_ForReadyStateTransaction()
        {
            using (var target = new SimulatedFooTransaction())
            {
                // Act.
                var action = new Action(() =>
                {
                    target.CommitAsync().Wait();
                });

                // Assert.
                action.Should().Throw<InvalidOperationException>($"because {nameof(target)} has not yet begun");
            }
        }

        [TestMethod]
        public void CommitAsync_ShouldRaiseInvalidOperationException_ForRejectedTransaction()
        {
            using (var target = new SimulatedFooTransaction())
            {
                // Arrange.
                target.BeginAsync().Wait();
                target.RejectAsync().Wait();

                // Act.
                var action = new Action(() =>
                {
                    target.CommitAsync().Wait();
                });

                // Assert.
                action.Should().Throw<InvalidOperationException>($"because {nameof(target)} is rejected");
            }
        }

        [TestMethod]
        public void FunctionalLifeSpanTest_ShouldProduceDesiredResults_ForAsynchronousInvocation()
        {
            // Arrange.
            var target = (DataAccessTransaction)null;

            using (target = new SimulatedFooTransaction())
            {
                // Assert.
                target.State.Should().Be(DataAccessTransactionState.Ready);

                // Act.
                target.BeginAsync().Wait();

                // Assert.
                target.State.Should().Be(DataAccessTransactionState.InProgress);

                // Act.
                target.CommitAsync().Wait();

                // Assert.
                target.State.Should().Be(DataAccessTransactionState.Committed);
            }

            // Assert.
            target.State.Should().Be(DataAccessTransactionState.Unspecified);

            // Act.
            var action = new Action(() =>
            {
                target.BeginAsync().Wait();
            });

            // Assert.
            action.Should().Throw<InvalidOperationException>($"because {nameof(target)} is disposed");

            using (target = new SimulatedFooTransaction())
            {
                // Assert.
                target.State.Should().Be(DataAccessTransactionState.Ready);

                // Act.
                target.BeginAsync().Wait();

                // Assert.
                target.State.Should().Be(DataAccessTransactionState.InProgress);

                // Act.
                target.RejectAsync().Wait();

                // Assert.
                target.State.Should().Be(DataAccessTransactionState.Rejected);
            }
        }

        [TestMethod]
        public void FunctionalLifeSpanTest_ShouldProduceDesiredResults_ForSynchronousInvocation()
        {
            // Arrange.
            var target = (DataAccessTransaction)null;

            using (target = new SimulatedFooTransaction())
            {
                // Assert.
                target.State.Should().Be(DataAccessTransactionState.Ready);

                // Act.
                target.Begin();

                // Assert.
                target.State.Should().Be(DataAccessTransactionState.InProgress);

                // Act.
                target.Commit();

                // Assert.
                target.State.Should().Be(DataAccessTransactionState.Committed);
            }

            // Assert.
            target.State.Should().Be(DataAccessTransactionState.Unspecified);

            // Act.
            var action = new Action(() =>
            {
                target.Begin();
            });

            // Assert.
            action.Should().Throw<InvalidOperationException>($"because {nameof(target)} is disposed");

            using (target = new SimulatedFooTransaction())
            {
                // Assert.
                target.State.Should().Be(DataAccessTransactionState.Ready);

                // Act.
                target.Begin();

                // Assert.
                target.State.Should().Be(DataAccessTransactionState.InProgress);

                // Act.
                target.Reject();

                // Assert.
                target.State.Should().Be(DataAccessTransactionState.Rejected);
            }
        }

        [TestMethod]
        public void Reject_ShouldRaiseInvalidOperationException_ForCommittedTransaction()
        {
            using (var target = new SimulatedFooTransaction())
            {
                // Arrange.
                target.Begin();
                target.Commit();

                // Act.
                var action = new Action(() =>
                {
                    target.Reject();
                });

                // Assert.
                action.Should().Throw<InvalidOperationException>($"because {nameof(target)} is committed");
            }
        }

        [TestMethod]
        public void Reject_ShouldRaiseInvalidOperationException_ForReadyStateTransaction()
        {
            using (var target = new SimulatedFooTransaction())
            {
                // Act.
                var action = new Action(() =>
                {
                    target.Reject();
                });

                // Assert.
                action.Should().Throw<InvalidOperationException>($"because {nameof(target)} has not yet begun");
            }
        }

        [TestMethod]
        public void Reject_ShouldRaiseInvalidOperationException_ForRejectedTransaction()
        {
            using (var target = new SimulatedFooTransaction())
            {
                // Arrange.
                target.Begin();
                target.Reject();

                // Act.
                var action = new Action(() =>
                {
                    target.Reject();
                });

                // Assert.
                action.Should().Throw<InvalidOperationException>($"because {nameof(target)} is rejected");
            }
        }

        [TestMethod]
        public void RejectAsync_ShouldRaiseInvalidOperationException_ForCommittedTransaction()
        {
            using (var target = new SimulatedFooTransaction())
            {
                // Arrange.
                target.BeginAsync().Wait();
                target.CommitAsync().Wait();

                // Act.
                var action = new Action(() =>
                {
                    target.RejectAsync().Wait();
                });

                // Assert.
                action.Should().Throw<InvalidOperationException>($"because {nameof(target)} is committed");
            }
        }

        [TestMethod]
        public void RejectAsync_ShouldRaiseInvalidOperationException_ForReadyStateTransaction()
        {
            using (var target = new SimulatedFooTransaction())
            {
                // Act.
                var action = new Action(() =>
                {
                    target.RejectAsync().Wait();
                });

                // Assert.
                action.Should().Throw<InvalidOperationException>($"because {nameof(target)} has not yet begun");
            }
        }

        [TestMethod]
        public void RejectAsync_ShouldRaiseInvalidOperationException_ForRejectedTransaction()
        {
            using (var target = new SimulatedFooTransaction())
            {
                // Arrange.
                target.BeginAsync().Wait();
                target.RejectAsync().Wait();

                // Act.
                var action = new Action(() =>
                {
                    target.RejectAsync().Wait();
                });

                // Assert.
                action.Should().Throw<InvalidOperationException>($"because {nameof(target)} is rejected");
            }
        }
    }
}