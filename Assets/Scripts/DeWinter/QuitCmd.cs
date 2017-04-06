using System;
using UnityEngine;
using Core;

namespace DeWinter
{
	public class QuitCmd : ICommand
	{
		public void Execute()
		{
			Application.Quit();
		}
	}
}