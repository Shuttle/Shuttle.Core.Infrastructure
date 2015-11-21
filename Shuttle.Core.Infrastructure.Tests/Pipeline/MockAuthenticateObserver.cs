using System;

namespace Shuttle.Core.Infrastructure.Tests
{
    public class MockAuthenticateObserver :
        IPipelineObserver<MockPipelineEvent1>,
        IPipelineObserver<MockPipelineEvent2>,
        IPipelineObserver<MockPipelineEvent3>
    {
        private string _callSequence = string.Empty;

        public string CallSequence
        {
            get { return _callSequence; }
        }

        public void Execute(MockPipelineEvent1 pipelineEvent)
        {
            Console.WriteLine("MockAuthenticateObserver.Execute() called for event '{0}'.", pipelineEvent.Name);

            _callSequence += "1";
        }

        public void Execute(MockPipelineEvent2 pipelineEvent)
        {
            Console.WriteLine("MockAuthenticateObserver.Execute() called for event '{0}'.", pipelineEvent.Name);

            _callSequence += "2";
        }

        public void Execute(MockPipelineEvent3 pipelineEvent)
        {
            Console.WriteLine("MockAuthenticateObserver.Execute() called for event '{0}'.", pipelineEvent.Name);

            _callSequence += "3";
        }
    }
}