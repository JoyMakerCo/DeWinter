using System;
using UnityEngine;

namespace DeWinter
{
	public class ButtonMessageHandler : MonoBehaviour
	{
		public void SendButtonMessage(string messageID)
		{
			DeWinterApp.SendMessage(messageID);
		}
	}
}