using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core
{	
	public delegate void ConsoleCommandHandler( string[] args );  

	public class ConsoleCommand
	{
		string _name;
		ConsoleCommandHandler _handler;
		string _helpText;

		public ConsoleCommand( string name, ConsoleCommandHandler cch, string help, Action setup = null )
		{
			_name = name.ToLower();
			_handler = cch;
			_helpText = help;
			if (setup != null)
			{
				setup();
			}
		}

		public string name {
			get { return _name; }
		}

		public ConsoleCommandHandler handler {
			get { return _handler; }
		}

		public string help {
			get { return _helpText; }
		}

	}
}