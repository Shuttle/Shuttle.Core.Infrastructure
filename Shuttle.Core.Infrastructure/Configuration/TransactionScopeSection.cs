using System;
using System.Configuration;
using System.Transactions;

namespace Shuttle.Core.Infrastructure
{
    public class TransactionScopeSection : ConfigurationSection
    {
        private static bool _initialized;
        private static readonly object Padlock = new object();
        private static TransactionScopeSection _section;
        private const int DefaultTimeoutSeconds = 30;
        private const IsolationLevel DefaultIsolationLevel = IsolationLevel.ReadCommitted;

        [ConfigurationProperty("enabled", IsRequired = false, DefaultValue = true)]
        public bool Enabled
        {
            get { return (bool)this["enabled"]; }
        }

        [ConfigurationProperty("isolationLevel", IsRequired = false, DefaultValue = DefaultIsolationLevel)]
        public IsolationLevel IsolationLevel
        {
            get
            {
                var value = this["isolationLevel"];

                if (value == null || string.IsNullOrEmpty(value.ToString()))
                {
                    return DefaultIsolationLevel;
                }

                try
                {
                    return (IsolationLevel)Enum.Parse(typeof(IsolationLevel), value.ToString());
                }
                catch
                {
                    return DefaultIsolationLevel;
                }
            }
        }

        [ConfigurationProperty("timeoutSeconds", IsRequired = false, DefaultValue = "30")]
        public int TimeoutSeconds
        {
            get
            {
                var value = this["timeoutSeconds"].ToString();

                if (string.IsNullOrEmpty(value))
                {
                    return DefaultTimeoutSeconds;
                }

                int result;

                return int.TryParse(value, out result) ? result : DefaultTimeoutSeconds;
            }
        }

        public static TransactionScopeSection Get()
        {
            lock (Padlock)
            {
                if (!_initialized)
                {
                    _section = ConfigurationSectionProvider.Open<TransactionScopeSection>("shuttle", "transactionScope");

                    _initialized = true;
                }

                return _section;
            }
        }
    }
}