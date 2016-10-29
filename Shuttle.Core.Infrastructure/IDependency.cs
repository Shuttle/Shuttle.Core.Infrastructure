namespace Shuttle.Core.Infrastructure
{
	public interface IDependency<T>
	{
		void Assign(T dependency);
	}

    public static class IDependencyExtensions
    {
        public static void AttemptDependencyInjection<T>(this object o, T dependency)
        {
            var target = o as IDependency<T>;

            if (target == null)
            {
                return;
            }

            target.Assign(dependency);
        }
    }
}