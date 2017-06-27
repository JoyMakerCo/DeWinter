using System;
using UnityEngine;

namespace Ambition
{
	public class ButtonMessage : MonoBehaviour
	{
		public void SendButtonMessage(string messageID)
		{
			AmbitionApp.SendMessage(messageID);
		}
	}
}