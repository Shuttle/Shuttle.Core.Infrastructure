namespace Shuttle.Core.Infrastructure.Tests
{
	public class FirstInterface : IFirstInterfaceBottom
	{
	}

	public interface IFirstInterfaceBottom : IFirstInterfaceMiddle
	{
	}

	public interface IFirstInterfaceMiddle : IFirstInterfaceTop
	{
	}

	public interface IFirstInterfaceTop
	{
	}
}