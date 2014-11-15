using System;


namespace Shuttle.Core.Infrastructure

{
	public class Singleton<TClass>
		where TClass : class, new()
	{
		private static readonly object _padlock = new object();
		private static TClass _instance;

		public static TClass Instance
		{
			get
			{
				lock (_padlock)
				{
					if (_instance == null)
					{
						_instance = new TClass();

						if (Environment.UserInteractive)
							Console.CancelKeyPress += (sender, args) => Dispose();
						else
							AppDomain.CurrentDomain.ProcessExit += (sender, args) => Dispose();

					}
				}
				return _instance;
			}
		}
		
		private static void Dispose()
		{
			if (_instance != null && _instance as IDisposable != null)
				(_instance as IDisposable).Dispose();
		}
	}
}