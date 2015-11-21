using System.Linq;
using NUnit.Framework;

namespace Shuttle.Core.Infrastructure.Tests
{
    [TestFixture]
    public class TypeExtensionsTest
    {
        [Test]
        public void Should_be_able_to_determine_if_a_type_is_assignable_to_a_generic()
        {
            var generic = typeof (IHaveGenericParameter<>);

            Assert.True(typeof (HasGenericParameterString).IsAssignableTo(generic));
            Assert.True(typeof (SubClassGenericParameterString).IsAssignableTo(generic));
        }

        [Test]
        public void Should_be_able_to_get_the_first_interface_assignable_to_given_interface()
        {
            var interfaces = typeof (FirstInterface).FirstInterface(typeof (IFirstInterfaceTop));

            Assert.AreEqual(1, interfaces.Count());
            Assert.AreSame(typeof (IFirstInterfaceBottom), interfaces.First());
        }
    }
}