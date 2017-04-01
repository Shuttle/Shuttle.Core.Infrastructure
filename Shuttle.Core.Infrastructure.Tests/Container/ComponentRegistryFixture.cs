using System;
using Moq;
using NUnit.Framework;

namespace Shuttle.Core.Infrastructure.Tests
{
	[TestFixture]
	public class ComponentRegistryExtensionsFixture
	{
		[Test]
		public void Should_be_able_to_register_as_default_singleton()
		{
			var mock = new Mock<IComponentRegistry>();

			mock.Object.Register<ISomeDependency, SomeDependency>();
			mock.Object.Register<ISomeDependency, SomeDependency>();

			mock.Verify(m => m.IsRegistered(It.IsAny<Type>()), Times.Never);
			mock.Verify(m => m.Register(typeof(ISomeDependency), typeof(SomeDependency), Lifestyle.Singleton), Times.Exactly(2));
		}

		[Test]
		public void Should_be_able_to_register_as_singleton()
		{
			var mock = new Mock<IComponentRegistry>();

			mock.Object.Register<ISomeDependency, SomeDependency>(Lifestyle.Singleton);
			mock.Object.Register<ISomeDependency, SomeDependency>(Lifestyle.Singleton);

			mock.Verify(m => m.IsRegistered(It.IsAny<Type>()), Times.Never);
			mock.Verify(m => m.Register(typeof(ISomeDependency), typeof(SomeDependency), Lifestyle.Singleton), Times.Exactly(2));
		}

		[Test]
		public void Should_be_able_to_register_as_transient()
		{
			var mock = new Mock<IComponentRegistry>();

			mock.Object.Register<ISomeDependency, SomeDependency>(Lifestyle.Transient);
			mock.Object.Register<ISomeDependency, SomeDependency>(Lifestyle.Transient);

			mock.Verify(m => m.IsRegistered(It.IsAny<Type>()), Times.Never);
			mock.Verify(m => m.Register(typeof(ISomeDependency), typeof(SomeDependency), Lifestyle.Transient), Times.Exactly(2));
		}

		[Test]
		public void Should_be_able_to_register_implementation_as_default_singleton()
		{
			var mock = new Mock<IComponentRegistry>();

			mock.Object.Register<SomeDependency>();
			mock.Object.Register<SomeDependency>();

			mock.Verify(m => m.IsRegistered(It.IsAny<Type>()), Times.Never);
			mock.Verify(m => m.Register(typeof(SomeDependency), typeof(SomeDependency), Lifestyle.Singleton), Times.Exactly(2));
		}

		[Test]
		public void Should_be_able_to_register_implementation_as_singleton()
		{
			var mock = new Mock<IComponentRegistry>();

			mock.Object.Register<SomeDependency>(Lifestyle.Singleton);
			mock.Object.Register<SomeDependency>(Lifestyle.Singleton);

			mock.Verify(m => m.IsRegistered(It.IsAny<Type>()), Times.Never);
			mock.Verify(m => m.Register(typeof(SomeDependency), typeof(SomeDependency), Lifestyle.Singleton), Times.Exactly(2));
		}

		[Test]
		public void Should_be_able_to_register_implementation_as_transient()
		{
			var mock = new Mock<IComponentRegistry>();

			mock.Object.Register<SomeDependency>(Lifestyle.Transient);
			mock.Object.Register<SomeDependency>(Lifestyle.Transient);

			mock.Verify(m => m.IsRegistered(It.IsAny<Type>()), Times.Never);
			mock.Verify(m => m.Register(typeof(SomeDependency), typeof(SomeDependency), Lifestyle.Transient), Times.Exactly(2));
		}

		[Test]
		public void Should_be_able_to_register_instance()
		{
			var mock = new Mock<IComponentRegistry>();

			mock.Object.Register<ISomeDependency>(new SomeDependency());
			mock.Object.Register<ISomeDependency>(new SomeDependency());

			mock.Verify(m => m.IsRegistered(It.IsAny<Type>()), Times.Never);
			mock.Verify(m => m.Register(typeof(ISomeDependency), It.IsAny<SomeDependency>()), Times.Exactly(2));
		}

		[Test]
		public void Should_be_able_to_attempt_register_as_default_singleton()
		{
			var mock = new Mock<IComponentRegistry>();

			mock.Setup(m => m.IsRegistered(typeof(ISomeDependency))).Returns(false);

			mock.Object.AttemptRegister<ISomeDependency, SomeDependency>();

			mock.Setup(m => m.IsRegistered(typeof(ISomeDependency))).Returns(true);

			mock.Object.AttemptRegister<ISomeDependency, SomeDependency>();

			mock.Verify(m => m.IsRegistered(It.IsAny<Type>()), Times.Exactly(2));
			mock.Verify(m => m.Register(typeof(ISomeDependency), typeof(SomeDependency), Lifestyle.Singleton), Times.Once);
		}

		[Test]
		public void Should_be_able_to_attempt_register_as_singleton()
		{
			var mock = new Mock<IComponentRegistry>();

			mock.Setup(m => m.IsRegistered(typeof(ISomeDependency))).Returns(false);

			mock.Object.AttemptRegister<ISomeDependency, SomeDependency>(Lifestyle.Singleton);

			mock.Setup(m => m.IsRegistered(typeof(ISomeDependency))).Returns(true);

			mock.Object.AttemptRegister<ISomeDependency, SomeDependency>(Lifestyle.Singleton);

			mock.Verify(m => m.IsRegistered(It.IsAny<Type>()), Times.Exactly(2));
			mock.Verify(m => m.Register(typeof(ISomeDependency), typeof(SomeDependency), Lifestyle.Singleton), Times.Once);
		}

		[Test]
		public void Should_be_able_to_attempt_register_as_transient()
		{
			var mock = new Mock<IComponentRegistry>();

			mock.Setup(m => m.IsRegistered(typeof(ISomeDependency))).Returns(false);

			mock.Object.AttemptRegister<ISomeDependency, SomeDependency>(Lifestyle.Transient);

			mock.Setup(m => m.IsRegistered(typeof(ISomeDependency))).Returns(true);

			mock.Object.AttemptRegister<ISomeDependency, SomeDependency>(Lifestyle.Transient);

			mock.Verify(m => m.IsRegistered(It.IsAny<Type>()), Times.Exactly(2));
			mock.Verify(m => m.Register(typeof(ISomeDependency), typeof(SomeDependency), Lifestyle.Transient), Times.Once);
		}

		[Test]
		public void Should_be_able_to_attempt_register_implementation_as_default_singleton()
		{
			var mock = new Mock<IComponentRegistry>();

			mock.Setup(m => m.IsRegistered(typeof(SomeDependency))).Returns(false);

			mock.Object.AttemptRegister<SomeDependency>();

			mock.Setup(m => m.IsRegistered(typeof(SomeDependency))).Returns(true);

			mock.Object.AttemptRegister<SomeDependency>();

			mock.Verify(m => m.IsRegistered(It.IsAny<Type>()), Times.Exactly(2));
			mock.Verify(m => m.Register(typeof(SomeDependency), typeof(SomeDependency), Lifestyle.Singleton), Times.Once);
		}

		[Test]
		public void Should_be_able_to_attempt_register_implementation_as_singleton()
		{
			var mock = new Mock<IComponentRegistry>();

			mock.Setup(m => m.IsRegistered(typeof(SomeDependency))).Returns(false);

			mock.Object.AttemptRegister<SomeDependency>(Lifestyle.Singleton);

			mock.Setup(m => m.IsRegistered(typeof(SomeDependency))).Returns(true);

			mock.Object.AttemptRegister<SomeDependency>(Lifestyle.Singleton);

			mock.Verify(m => m.IsRegistered(It.IsAny<Type>()), Times.Exactly(2));
			mock.Verify(m => m.Register(typeof(SomeDependency), typeof(SomeDependency), Lifestyle.Singleton), Times.Once);
		}

		[Test]
		public void Should_be_able_to_attempt_register_implementation_as_transient()
		{
			var mock = new Mock<IComponentRegistry>();

			mock.Setup(m => m.IsRegistered(typeof(SomeDependency))).Returns(false);

			mock.Object.AttemptRegister<SomeDependency>(Lifestyle.Transient);

			mock.Setup(m => m.IsRegistered(typeof(SomeDependency))).Returns(true);

			mock.Object.AttemptRegister<SomeDependency>(Lifestyle.Transient);

			mock.Verify(m => m.IsRegistered(It.IsAny<Type>()), Times.Exactly(2));
			mock.Verify(m => m.Register(typeof(SomeDependency), typeof(SomeDependency), Lifestyle.Transient), Times.Once);
		}

		[Test]
		public void Should_be_able_to_attempt_register_instance()
		{
			var mock = new Mock<IComponentRegistry>();

			mock.Setup(m => m.IsRegistered(typeof(ISomeDependency))).Returns(false);

			mock.Object.AttemptRegister<ISomeDependency>(new SomeDependency());

			mock.Setup(m => m.IsRegistered(typeof(ISomeDependency))).Returns(true);

			mock.Object.AttemptRegister<ISomeDependency>(new SomeDependency());

			mock.Verify(m => m.IsRegistered(It.IsAny<Type>()), Times.Exactly(2));
			mock.Verify(m => m.Register(typeof(ISomeDependency), It.IsAny<SomeDependency>()), Times.Once);
		}
	}
}