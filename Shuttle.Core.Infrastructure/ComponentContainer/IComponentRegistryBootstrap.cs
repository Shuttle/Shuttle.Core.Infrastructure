namespace Shuttle.Core.Infrastructure
{
	public interface IComponentRegistryBootstrap
	{
		void Register(IComponentRegistry registry);
	}
}