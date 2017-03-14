using System;
using UnityEngine;

namespace DeWinter
{
	public class ButtonMessage : Component
	{
		public void SendMessage(string messageID)
		{
			DeWinterApp.SendMessage(messageID);
		}
	}
}