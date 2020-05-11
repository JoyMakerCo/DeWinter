using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core
{	
	public delegate string[] DumpableDelegate();  

	public class ConsoleEntity : IConsoleEntity
	{
		string _id;
		Action<string[]> _invoke;
		DumpableDelegate _dump;

		public ConsoleEntity( string id, Action<string[]> invoke = null, DumpableDelegate dump = null )
		{
			_id = id;
			_invoke = invoke;
			_dump = dump;
		}

		public string[] Dump()
		{
			if (_dump != null)
			{
				return _dump();
			}

			return new string[0];
		}

		public void Invoke( string[] args )
		{
			if (_invoke != null)
			{
				_invoke(args);
			}
		}
	}
}