using System;
using Util;

namespace Core
{
// TODO: Test callbacks within commands
// Possible approach: Make an interface or abstract class for commands with callbacks
// Find out if that's necessary

	public interface ICommand<T>
	{
		void Execute(T data);
	}

	public interface ICommand
	{
		void Execute();
	}

	public class CommandSvc : IAppService
	{
		// Basic send with typed value object
		public void Execute<T, U>(U data) where T:ICommand<U>, new()
		{
			(new T()).Execute(data);
		}

		// Basic send method without parameter
		public void Execute<T>() where T:ICommand, new()
		{
			(new T()).Execute();
		}
	}
}