using System;
using UnityEngine;

namespace Ambition
{
	public class ButtonMessage : MonoBehaviour
	{
		public void SendButtonMessage(string messageID)
		{
			DeWinterApp.SendMessage(messageID);
		}
	}
}