// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace RapidField.SolidInstruments.DataAccess.UnitTests
{
    [TestClass]
    public class DataAccessRepositoryTests
    {
        [TestMethod]
        public void Add_ShouldProduceDesiredResults()
        {
            // Arrange.
            var dataStore = SimulatedBarDataStore.NewDefaultInstance();
            var identifier = Guid.NewGuid();
            var entity = new SimulatedBarEntity(identifier, identifier.GetHashCode());

            using (var target = new SimulatedBarRepository(dataStore))
            {
                // Act.
                target.Add(entity);
            }

            // Assert.
            dataStore.Should().ContainKey(identifier);
        }

        [TestMethod]
        public void AddRange_ShouldProduceDesiredResults()
        {
            // Arrange.
            var dataStore = SimulatedBarDataStore.NewDefaultInstance();
            var identifiers = new Guid[] { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };
            var entities = identifiers.Select(identifier => new SimulatedBarEntity(identifier, identifier.GetHashCode()));

            using (var target = new SimulatedBarRepository(dataStore))
            {
                // Act.
                target.AddRange(entities);
            }

            // Assert.
            dataStore.Should().ContainKeys(identifiers);
        }

        [TestMethod]
        public void All_ShouldReturnAllEntities()
        {
            // Arrange.
            var dataStore = SimulatedBarDataStore.NewDefaultInstance();

            using (var target = new SimulatedBarRepository(dataStore))
            {
                // Act.
                var result = target.All();

                // Assert.
                result.Should().NotBeNullOrEmpty();

                foreach (var entity in result)
                {
                    // Assert.
                    dataStore.GetEntityByIdentifier(entity.Identifier).Value.Should().Be(entity.Value);
                }
            }
        }

        [TestMethod]
        public void Any_ShouldProduceDesiredResults_ForPositiveMatch()
        {
            // Arrange.
            var dataStore = SimulatedBarDataStore.NewDefaultInstance();

            using (var target = new SimulatedBarRepository(dataStore))
            {
                // Assert.
                target.Any().Should().BeTrue();
            }
        }

        [TestMethod]
        public void Contains_ShouldReturnFalse_ForNegativeMatch()
        {
            // Arrange.
            var dataStore = SimulatedBarDataStore.NewDefaultInstance();
            var identifier = Guid.NewGuid();
            var entity = new SimulatedBarEntity(identifier, identifier.GetHashCode());

            using (var target = new SimulatedBarRepository(dataStore))
            {
                // Assert.
                target.Contains(entity).Should().BeFalse();
            }
        }

        [TestMethod]
        public void Contains_ShouldReturnTrue_ForPositiveMatch()
        {
            // Arrange.
            var dataStore = SimulatedBarDataStore.NewDefaultInstance();
            var identifier = Guid.NewGuid();
            var entity = new SimulatedBarEntity(identifier, identifier.GetHashCode());

            using (var target = new SimulatedBarRepository(dataStore))
            {
                // Arrange.
                target.Add(entity);

                // Act.
                target.Contains(entity).Should().BeTrue();
            }
        }

        [TestMethod]
        public void ContainsWhere_ShouldReturnFalse_ForNegativeMatch()
        {
            // Arrange.
            var dataStore = SimulatedBarDataStore.NewDefaultInstance();
            var identifier = Guid.NewGuid();

            using (var target = new SimulatedBarRepository(dataStore))
            {
                // Assert.
                target.ContainsWhere(targetEntity => targetEntity.Identifier == identifier).Should().BeFalse();
            }
        }

        [TestMethod]
        public void ContainsWhere_ShouldReturnTrue_ForPositiveMatch()
        {
            // Arrange.
            var dataStore = SimulatedBarDataStore.NewDefaultInstance();
            var identifier = Guid.NewGuid();
            var entity = new SimulatedBarEntity(identifier, identifier.GetHashCode());

            using (var target = new SimulatedBarRepository(dataStore))
            {
                // Arrange.
                target.Add(entity);

                // Assert.
                target.ContainsWhere(targetEntity => targetEntity.Identifier == identifier).Should().BeTrue();
            }
        }

        [TestMethod]
        public void Count_ShouldProduceDesiredResults()
        {
            // Arrange.
            var dataStore = SimulatedBarDataStore.NewDefaultInstance();

            using (var target = new SimulatedBarRepository(dataStore))
            {
                // Assert.
                target.Count().Should().Be(dataStore.Count);
            }
        }

        [TestMethod]
        public void CountWhere_ShouldProduceDesiredResults()
        {
            // Arrange.
            var dataStore = SimulatedBarDataStore.NewDefaultInstance();
            var identifier = Guid.NewGuid();
            var entity = new SimulatedBarEntity(identifier, identifier.GetHashCode());

            using (var target = new SimulatedBarRepository(dataStore))
            {
                // Arrange.
                target.Add(entity);

                // Assert.
                target.CountWhere(targetEntity => targetEntity.Identifier == identifier).Should().Be(1);
            }
        }

        [TestMethod]
        public void EntityType_ShouldReflectRepositoryEntityType()
        {
            // Arrange.
            var dataStore = SimulatedBarDataStore.NewDefaultInstance();

            using (var target = new SimulatedBarRepository(dataStore))
            {
                // Assert.
                target.EntityType.Should().Be(typeof(SimulatedBarEntity));
            }
        }

        [TestMethod]
        public void FindWhere_ShouldProduceDesiredResults()
        {
            // Arrange.
            var dataStore = SimulatedBarDataStore.NewDefaultInstance();
            var identifier = Guid.NewGuid();
            var entity = new SimulatedBarEntity(identifier, identifier.GetHashCode());

            using (var target = new SimulatedBarRepository(dataStore))
            {
                // Arrange.
                target.Add(entity);

                // Act.
                var result = target.FindWhere(targetEntity => targetEntity.Identifier == identifier);

                // Assert.
                result.Should().NotBeNullOrEmpty();
                result.Count().Should().Be(1);
                result.First().Identifier.Should().Be(identifier);
            }
        }

        [TestMethod]
        public void Remove_ShouldProduceDesiredResults()
        {
            // Arrange.
            var dataStore = SimulatedBarDataStore.NewDefaultInstance();
            var identifier = Guid.NewGuid();
            var entity = new SimulatedBarEntity(identifier, identifier.GetHashCode());

            using (var target = new SimulatedBarRepository(dataStore))
            {
                // Arrange.
                target.Add(entity);

                // Act.
                target.Remove(entity);
            }

            // Assert.
            dataStore.Should().NotContainKey(identifier);
        }

        [TestMethod]
        public void RemoveRange_ShouldProduceDesiredResults()
        {
            // Arrange.
            var dataStore = SimulatedBarDataStore.NewDefaultInstance();
            var identifiers = new Guid[] { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };
            var entities = identifiers.Select(identifier => new SimulatedBarEntity(identifier, identifier.GetHashCode()));

            using (var target = new SimulatedBarRepository(dataStore))
            {
                // Arrange.
                target.AddRange(entities);

                // Act.
                target.RemoveRange(entities);
            }

            // Assert.
            dataStore.Should().NotContainKeys(identifiers);
        }
    }
}