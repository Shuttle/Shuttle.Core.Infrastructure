using System;
using NUnit.Framework;

namespace Shuttle.Core.Infrastructure.Tests
{
	[TestFixture]
    public class ArgumentsTest 
    {
        [Test]
        public void Should_be_able_to_parse_simple_arguments()
        {
            var arguments = new Arguments("-arg1:arg1value", "/arg2", "arg2value", "--enabled", "/threads=5");

            Assert.AreEqual("arg1value", arguments["arg1"]);
            Assert.AreEqual("arg2value", arguments["arg2"]);

            Assert.IsTrue(arguments.Get("enabled", false));
            Assert.IsTrue(arguments.Get("disabled", true));

            Assert.AreEqual(5, arguments.Get("threads", 5));
            Assert.AreEqual("5", arguments["threads"]);

            Assert.IsNull(arguments["bogus"]);
            Assert.Throws<InvalidOperationException>(() => arguments.Get<int>("bogus"));
        }
    }
}