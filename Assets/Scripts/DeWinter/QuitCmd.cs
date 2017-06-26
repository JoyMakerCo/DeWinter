using System;
using UnityEngine;
using Core;

namespace Ambition
{
	public class QuitCmd : ICommand
	{
		public void Execute()
		{
			Application.Quit();
		}
	}
}