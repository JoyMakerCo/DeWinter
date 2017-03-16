using System;
using UnityEngine;

namespace DeWinter
{
	public class ButtonMessage : MonoBehaviour
	{
		public void SendButtonMessage(string messageID)
		{
			DeWinterApp.SendMessage(messageID);
		}
	}
}