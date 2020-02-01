// =================================================================================================================================
// Copyright (c) RapidField LLC. Licensed under the MIT License. See LICENSE.txt in the project root for license information.
// =================================================================================================================================

using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidField.SolidInstruments.Core;
using RapidField.SolidInstruments.Core.Concurrency;
using System;

namespace RapidField.SolidInstruments.ObjectComposition.UnitTests
{
    [TestClass]
    public class ObjectContainerTests
    {
        [TestMethod]
        public void Get_ShouldRaiseArgumentException_ForUnsupportedType_UsingBuilderConfiguration()
        {
            using (var builder = new ObjectContainerBuilder())
            {
                // Arrange.
                var productionFunction = new Func<SimulatedInstrument>(() => new SimulatedInstrument(ConcurrencyControlMode.SingleThreadLock));
                builder
                    .Configure(productionFunction)
                    .Configure<Instrument, SimulatedInstrument>(productionFunction);

                using (var target = builder.ToResult())
                {
                    // Act.
                    var action = new Action(() =>
                    {
                        target.Get<ReferenceManager>();
                    });

                    // Assert.
                    action.Should().Throw<ArgumentException>();
                }
            }
        }

        [TestMethod]
        public void Get_ShouldRaiseArgumentException_ForUnsupportedType_UsingFactoryConfiguration()
        {
            // Arrange.
            var objectFactory = new SimulatedInstrumentFactory();
            var definitionConfigurator = new Action<ObjectContainerConfigurationDefinitions>((definitions) =>
            {
                definitions
                    .Add<SimulatedInstrument>()
                    .Add<Instrument, SimulatedInstrument>();
            });

            using (var target = new ObjectContainer(objectFactory, definitionConfigurator))
            {
                // Act.
                var action = new Action(() =>
                {
                    target.Get<ReferenceManager>();
                });

                // Assert.
                action.Should().Throw<ArgumentException>();
            }
        }

        [TestMethod]
        public void Get_ShouldRaiseArgumentException_ForUnsupportedType_UsingManualConfiguration()
        {
            // Arrange.
            var factoryConfigurator = new Action<ObjectFactoryConfigurationProductionFunctions>((functions) =>
            {
                functions.Add(() => new SimulatedInstrument(ConcurrencyControlMode.SingleThreadLock));
            });
            var definitionConfigurator = new Action<ObjectContainerConfigurationDefinitions>((definitions) =>
            {
                definitions
                    .Add<SimulatedInstrument>()
                    .Add<Instrument, SimulatedInstrument>();
            });

            using (var target = new ObjectContainer(factoryConfigurator, definitionConfigurator))
            {
                // Act.
                var action = new Action(() =>
                {
                    target.Get<ReferenceManager>();
                });

                // Assert.
                action.Should().Throw<ArgumentException>();
            }
        }

        [TestMethod]
        public void Get_ShouldReturnNewObjectOfSpecifiedType_ForSupportedType_UsingBuilderConfiguration()
        {
            using (var builder = new ObjectContainerBuilder())
            {
                // Arrange.
                var productionFunction = new Func<SimulatedInstrument>(() => new SimulatedInstrument(ConcurrencyControlMode.SingleThreadLock));
                builder
                    .Configure(productionFunction)
                    .Configure<Instrument, SimulatedInstrument>(productionFunction);

                using (var target = builder.ToResult())
                {
                    // Act.
                    var result = target.Get<Instrument>();

                    // Assert.
                    result.Should().NotBeNull();
                    result.Should().BeOfType<SimulatedInstrument>();
                }
            }
        }

        [TestMethod]
        public void Get_ShouldReturnNewObjectOfSpecifiedType_ForSupportedType_UsingFactoryConfiguration()
        {
            // Arrange.
            var objectFactory = new SimulatedInstrumentFactory();
            var definitionConfigurator = new Action<ObjectContainerConfigurationDefinitions>((definitions) =>
            {
                definitions
                    .Add<SimulatedInstrument>()
                    .Add<Instrument, SimulatedInstrument>();
            });

            using (var target = new ObjectContainer(objectFactory, definitionConfigurator))
            {
                // Act.
                var result = target.Get<Instrument>();

                // Assert.
                result.Should().NotBeNull();
                result.Should().BeOfType<SimulatedInstrument>();
            }
        }

        [TestMethod]
        public void Get_ShouldReturnNewObjectOfSpecifiedType_ForSupportedType_UsingManualConfiguration()
        {
            // Arrange.
            var factoryConfigurator = new Action<ObjectFactoryConfigurationProductionFunctions>((functions) =>
            {
                functions.Add(() => new SimulatedInstrument(ConcurrencyControlMode.SingleThreadLock));
            });
            var definitionConfigurator = new Action<ObjectContainerConfigurationDefinitions>((definitions) =>
            {
                definitions
                    .Add<SimulatedInstrument>()
                    .Add<Instrument, SimulatedInstrument>();
            });

            using (var target = new ObjectContainer(factoryConfigurator, definitionConfigurator))
            {
                // Act.
                var result = target.Get<Instrument>();

                // Assert.
                result.Should().NotBeNull();
                result.Should().BeOfType<SimulatedInstrument>();
            }
        }

        [TestMethod]
        public void Get_ShouldReturnSameObjectInstance_ForRepeatedCallsForEquivalentRequestType_UsingBuilderConfiguration()
        {
            using (var builder = new ObjectContainerBuilder())
            {
                // Arrange.
                var productionFunction = new Func<SimulatedInstrument>(() => new SimulatedInstrument(ConcurrencyControlMode.SingleThreadLock));
                builder
                    .Configure(productionFunction)
                    .Configure<Instrument, SimulatedInstrument>(productionFunction);

                using (var target = builder.ToResult())
                {
                    // Act.
                    var resultOne = target.Get<Instrument>();
                    var resultTwo = target.Get<SimulatedInstrument>();

                    // Assert.
                    resultOne.Should().NotBeNull();
                    resultTwo.Should().NotBeNull();
                    resultOne.Should().BeSameAs(resultTwo);
                }
            }
        }

        [TestMethod]
        public void Get_ShouldReturnSameObjectInstance_ForRepeatedCallsForEquivalentRequestType_UsingFactoryConfiguration()
        {
            // Arrange.
            var objectFactory = new SimulatedInstrumentFactory();
            var definitionConfigurator = new Action<ObjectContainerConfigurationDefinitions>((definitions) =>
            {
                definitions
                    .Add<SimulatedInstrument>()
                    .Add<Instrument, SimulatedInstrument>();
            });

            using (var target = new ObjectContainer(objectFactory, definitionConfigurator))
            {
                // Act.
                var resultOne = target.Get<Instrument>();
                var resultTwo = target.Get<SimulatedInstrument>();

                // Assert.
                resultOne.Should().NotBeNull();
                resultTwo.Should().NotBeNull();
                resultOne.Should().BeSameAs(resultTwo);
            }
        }

        [TestMethod]
        public void Get_ShouldReturnSameObjectInstance_ForRepeatedCallsForEquivalentRequestType_UsingManualConfiguration()
        {
            // Arrange.
            var factoryConfigurator = new Action<ObjectFactoryConfigurationProductionFunctions>((functions) =>
            {
                functions.Add(() => new SimulatedInstrument(ConcurrencyControlMode.SingleThreadLock));
            });
            var definitionConfigurator = new Action<ObjectContainerConfigurationDefinitions>((definitions) =>
            {
                definitions
                    .Add<SimulatedInstrument>()
                    .Add<Instrument, SimulatedInstrument>();
            });

            using (var target = new ObjectContainer(factoryConfigurator, definitionConfigurator))
            {
                // Act.
                var resultOne = target.Get<Instrument>();
                var resultTwo = target.Get<SimulatedInstrument>();

                // Assert.
                resultOne.Should().NotBeNull();
                resultTwo.Should().NotBeNull();
                resultOne.Should().BeSameAs(resultTwo);
            }
        }

        [TestMethod]
        public void Get_ShouldReturnSameObjectInstance_ForRepeatedCallsForSameRequestType_UsingBuilderConfiguration()
        {
            using (var builder = new ObjectContainerBuilder())
            {
                // Arrange.
                var productionFunction = new Func<SimulatedInstrument>(() => new SimulatedInstrument(ConcurrencyControlMode.SingleThreadLock));
                builder
                    .Configure(productionFunction)
                    .Configure<Instrument, SimulatedInstrument>(productionFunction);

                using (var target = builder.ToResult())
                {
                    // Act.
                    var resultOne = target.Get<Instrument>();
                    var resultTwo = target.Get<Instrument>();

                    // Assert.
                    resultOne.Should().NotBeNull();
                    resultTwo.Should().NotBeNull();
                    resultOne.Should().BeSameAs(resultTwo);
                }
            }
        }

        [TestMethod]
        public void Get_ShouldReturnSameObjectInstance_ForRepeatedCallsForSameRequestType_UsingFactoryConfiguration()
        {
            // Arrange.
            var objectFactory = new SimulatedInstrumentFactory();
            var definitionConfigurator = new Action<ObjectContainerConfigurationDefinitions>((definitions) =>
            {
                definitions
                    .Add<SimulatedInstrument>()
                    .Add<Instrument, SimulatedInstrument>();
            });

            using (var target = new ObjectContainer(objectFactory, definitionConfigurator))
            {
                // Act.
                var resultOne = target.Get<Instrument>();
                var resultTwo = target.Get<Instrument>();

                // Assert.
                resultOne.Should().NotBeNull();
                resultTwo.Should().NotBeNull();
                resultOne.Should().BeSameAs(resultTwo);
            }
        }

        [TestMethod]
        public void Get_ShouldReturnSameObjectInstance_ForRepeatedCallsForSameRequestType_UsingManualConfiguration()
        {
            // Arrange.
            var factoryConfigurator = new Action<ObjectFactoryConfigurationProductionFunctions>((functions) =>
            {
                functions.Add(() => new SimulatedInstrument(ConcurrencyControlMode.SingleThreadLock));
            });
            var definitionConfigurator = new Action<ObjectContainerConfigurationDefinitions>((definitions) =>
            {
                definitions
                    .Add<SimulatedInstrument>()
                    .Add<Instrument, SimulatedInstrument>();
            });

            using (var target = new ObjectContainer(factoryConfigurator, definitionConfigurator))
            {
                // Act.
                var resultOne = target.Get<Instrument>();
                var resultTwo = target.Get<Instrument>();

                // Assert.
                resultOne.Should().NotBeNull();
                resultTwo.Should().NotBeNull();
                resultOne.Should().BeSameAs(resultTwo);
            }
        }

        [TestMethod]
        public void GetNew_ShouldRaiseArgumentException_ForUnsupportedType_UsingBuilderConfiguration()
        {
            using (var builder = new ObjectContainerBuilder())
            {
                // Arrange.
                var productionFunction = new Func<SimulatedInstrument>(() => new SimulatedInstrument(ConcurrencyControlMode.SingleThreadLock));
                builder
                    .Configure(productionFunction)
                    .Configure<Instrument, SimulatedInstrument>(productionFunction);

                using (var target = builder.ToResult())
                {
                    // Act.
                    var action = new Action(() =>
                    {
                        target.GetNew<ReferenceManager>();
                    });

                    // Assert.
                    action.Should().Throw<ArgumentException>();
                }
            }
        }

        [TestMethod]
        public void GetNew_ShouldRaiseArgumentException_ForUnsupportedType_UsingFactoryConfiguration()
        {
            // Arrange.
            var objectFactory = new SimulatedInstrumentFactory();
            var definitionConfigurator = new Action<ObjectContainerConfigurationDefinitions>((definitions) =>
            {
                definitions
                    .Add<SimulatedInstrument>()
                    .Add<Instrument, SimulatedInstrument>();
            });

            using (var target = new ObjectContainer(objectFactory, definitionConfigurator))
            {
                // Act.
                var action = new Action(() =>
                {
                    target.GetNew<ReferenceManager>();
                });

                // Assert.
                action.Should().Throw<ArgumentException>();
            }
        }

        [TestMethod]
        public void GetNew_ShouldRaiseArgumentException_ForUnsupportedType_UsingManualConfiguration()
        {
            // Arrange.
            var factoryConfigurator = new Action<ObjectFactoryConfigurationProductionFunctions>((functions) =>
            {
                functions.Add(() => new SimulatedInstrument(ConcurrencyControlMode.SingleThreadLock));
            });
            var definitionConfigurator = new Action<ObjectContainerConfigurationDefinitions>((definitions) =>
            {
                definitions
                    .Add<SimulatedInstrument>()
                    .Add<Instrument, SimulatedInstrument>();
            });

            using (var target = new ObjectContainer(factoryConfigurator, definitionConfigurator))
            {
                // Act.
                var action = new Action(() =>
                {
                    target.GetNew<ReferenceManager>();
                });

                // Assert.
                action.Should().Throw<ArgumentException>();
            }
        }

        [TestMethod]
        public void GetNew_ShouldReturnDifferentObjectInstances_ForRepeatedCallsForSameRequestType_UsingBuilderConfiguration()
        {
            using (var builder = new ObjectContainerBuilder())
            {
                // Arrange.
                var productionFunction = new Func<SimulatedInstrument>(() => new SimulatedInstrument(ConcurrencyControlMode.SingleThreadLock));
                builder
                    .Configure(productionFunction)
                    .Configure<Instrument, SimulatedInstrument>(productionFunction);

                using (var target = builder.ToResult())
                {
                    // Act.
                    var resultOne = target.GetNew<Instrument>();
                    var resultTwo = target.GetNew<Instrument>();

                    // Assert.
                    resultOne.Should().NotBeNull();
                    resultTwo.Should().NotBeNull();
                    resultOne.Should().NotBeSameAs(resultTwo);
                }
            }
        }

        [TestMethod]
        public void GetNew_ShouldReturnDifferentObjectInstances_ForRepeatedCallsForSameRequestType_UsingFactoryConfiguration()
        {
            // Arrange.
            var objectFactory = new SimulatedInstrumentFactory();
            var definitionConfigurator = new Action<ObjectContainerConfigurationDefinitions>((definitions) =>
            {
                definitions
                    .Add<SimulatedInstrument>()
                    .Add<Instrument, SimulatedInstrument>();
            });

            using (var target = new ObjectContainer(objectFactory, definitionConfigurator))
            {
                // Act.
                var resultOne = target.GetNew<Instrument>();
                var resultTwo = target.GetNew<Instrument>();

                // Assert.
                resultOne.Should().NotBeNull();
                resultTwo.Should().NotBeNull();
                resultOne.Should().NotBeSameAs(resultTwo);
            }
        }

        [TestMethod]
        public void GetNew_ShouldReturnDifferentObjectInstances_ForRepeatedCallsForSameRequestType_UsingManualConfiguration()
        {
            // Arrange.
            var factoryConfigurator = new Action<ObjectFactoryConfigurationProductionFunctions>((functions) =>
            {
                functions.Add(() => new SimulatedInstrument(ConcurrencyControlMode.SingleThreadLock));
            });
            var definitionConfigurator = new Action<ObjectContainerConfigurationDefinitions>((definitions) =>
            {
                definitions
                    .Add<SimulatedInstrument>()
                    .Add<Instrument, SimulatedInstrument>();
            });

            using (var target = new ObjectContainer(factoryConfigurator, definitionConfigurator))
            {
                // Act.
                var resultOne = target.GetNew<Instrument>();
                var resultTwo = target.GetNew<Instrument>();

                // Assert.
                resultOne.Should().NotBeNull();
                resultTwo.Should().NotBeNull();
                resultOne.Should().NotBeSameAs(resultTwo);
            }
        }

        [TestMethod]
        public void GetNew_ShouldReturnNewObjectOfSpecifiedType_ForSupportedType_UsingBuilderConfiguration()
        {
            using (var builder = new ObjectContainerBuilder())
            {
                // Arrange.
                var productionFunction = new Func<SimulatedInstrument>(() => new SimulatedInstrument(ConcurrencyControlMode.SingleThreadLock));
                builder
                    .Configure(productionFunction)
                    .Configure<Instrument, SimulatedInstrument>(productionFunction);

                using (var target = builder.ToResult())
                {
                    // Act.
                    var result = target.GetNew<Instrument>();

                    // Assert.
                    result.Should().NotBeNull();
                    result.Should().BeOfType<SimulatedInstrument>();
                }
            }
        }

        [TestMethod]
        public void GetNew_ShouldReturnNewObjectOfSpecifiedType_ForSupportedType_UsingFactoryConfiguration()
        {
            // Arrange.
            var objectFactory = new SimulatedInstrumentFactory();
            var definitionConfigurator = new Action<ObjectContainerConfigurationDefinitions>((definitions) =>
            {
                definitions
                    .Add<SimulatedInstrument>()
                    .Add<Instrument, SimulatedInstrument>();
            });

            using (var target = new ObjectContainer(objectFactory, definitionConfigurator))
            {
                // Act.
                var result = target.GetNew<Instrument>();

                // Assert.
                result.Should().NotBeNull();
                result.Should().BeOfType<SimulatedInstrument>();
            }
        }

        [TestMethod]
        public void GetNew_ShouldReturnNewObjectOfSpecifiedType_ForSupportedType_UsingManualConfiguration()
        {
            // Arrange.
            var factoryConfigurator = new Action<ObjectFactoryConfigurationProductionFunctions>((functions) =>
            {
                functions.Add(() => new SimulatedInstrument(ConcurrencyControlMode.SingleThreadLock));
            });
            var definitionConfigurator = new Action<ObjectContainerConfigurationDefinitions>((definitions) =>
            {
                definitions
                    .Add<SimulatedInstrument>()
                    .Add<Instrument, SimulatedInstrument>();
            });

            using (var target = new ObjectContainer(factoryConfigurator, definitionConfigurator))
            {
                // Act.
                var result = target.GetNew<Instrument>();

                // Assert.
                result.Should().NotBeNull();
                result.Should().BeOfType<SimulatedInstrument>();
            }
        }

        [TestMethod]
        public void InstanceTypes_ShouldReturnConfiguredTypes_UsingBuilderConfiguration()
        {
            using (var builder = new ObjectContainerBuilder())
            {
                // Arrange.
                var productionFunction = new Func<SimulatedInstrument>(() => new SimulatedInstrument(ConcurrencyControlMode.SingleThreadLock));
                builder
                    .Configure(productionFunction)
                    .Configure<Instrument, SimulatedInstrument>(productionFunction);

                using (var target = builder.ToResult())
                {
                    // Act.
                    var instanceTypes = target.InstanceTypes;

                    // Assert.
                    instanceTypes.Should().Contain(typeof(Instrument));
                    instanceTypes.Should().Contain(typeof(SimulatedInstrument));
                }
            }
        }

        [TestMethod]
        public void InstanceTypes_ShouldReturnConfiguredTypes_UsingFactoryConfiguration()
        {
            // Arrange.
            var objectFactory = new SimulatedInstrumentFactory();
            var definitionConfigurator = new Action<ObjectContainerConfigurationDefinitions>((definitions) =>
            {
                definitions
                    .Add<SimulatedInstrument>()
                    .Add<Instrument, SimulatedInstrument>();
            });

            using (var target = new ObjectContainer(objectFactory, definitionConfigurator))
            {
                // Act.
                var instanceTypes = target.InstanceTypes;

                // Assert.
                instanceTypes.Should().Contain(typeof(Instrument));
                instanceTypes.Should().Contain(typeof(SimulatedInstrument));
            }
        }

        [TestMethod]
        public void InstanceTypes_ShouldReturnConfiguredTypes_UsingManualConfiguration()
        {
            // Arrange.
            var factoryConfigurator = new Action<ObjectFactoryConfigurationProductionFunctions>((functions) =>
            {
                functions.Add(() => new SimulatedInstrument(ConcurrencyControlMode.SingleThreadLock));
            });
            var definitionConfigurator = new Action<ObjectContainerConfigurationDefinitions>((definitions) =>
            {
                definitions
                    .Add<SimulatedInstrument>()
                    .Add<Instrument, SimulatedInstrument>();
            });

            using (var target = new ObjectContainer(factoryConfigurator, definitionConfigurator))
            {
                // Act.
                var instanceTypes = target.InstanceTypes;

                // Assert.
                instanceTypes.Should().Contain(typeof(Instrument));
                instanceTypes.Should().Contain(typeof(SimulatedInstrument));
            }
        }
    }
}