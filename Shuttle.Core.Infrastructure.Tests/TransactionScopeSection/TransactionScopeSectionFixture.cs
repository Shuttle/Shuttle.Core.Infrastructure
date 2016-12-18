using System;
using System.IO;
using System.Transactions;
using NUnit.Framework;

namespace Shuttle.Core.Infrastructure.Tests.Section
{
    public class TransactionScopeSectionFixture
    {
        private TransactionScopeSection Open(string file)
        {
            return ConfigurationSectionProvider.OpenFile<TransactionScopeSection>("shuttle", "transactionScope",
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format(@".\TransactionScopeSection\files\{0}", file)));
        }

        [Test]
        [TestCase("TransactionScope.config")]
        [TestCase("TransactionScope-Grouped.config")]
        public void Should_be_able_to_load_a_valid_configuration(string file)
        {
            var section = Open(file);

            Assert.IsNotNull(section);
            Assert.IsFalse(section.Enabled);
            Assert.AreEqual(IsolationLevel.RepeatableRead, section.IsolationLevel);
            Assert.AreEqual(300, section.TimeoutSeconds);
        }

        [Test]
        public void Should_be_able_to_load_an_empty_configuration()
        {
            var section = Open("Empty.config");

            Assert.IsTrue(section.Enabled);
            Assert.AreEqual(IsolationLevel.ReadCommitted, section.IsolationLevel);
            Assert.AreEqual(30, section.TimeoutSeconds);
        }
    }
}