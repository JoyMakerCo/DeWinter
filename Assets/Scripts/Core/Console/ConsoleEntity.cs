using System;

namespace Core
{	
	public class ConsoleEntity
	{
        public object Entity { get; private set; }
        Action<string[]> _invoke;

        public ConsoleEntity( object entity, Action<string[]> invoke=null)
		{
            Entity = entity;
            _invoke = invoke;
        }

        public void Invoke(string[] args) => _invoke?.Invoke(args);
        public override string ToString() => Entity?.ToString();
    }
}
