using System;
using UnityEngine;

namespace Ambition
{
	public class ButtonMessageHandler : MonoBehaviour
	{
		public void SendButtonMessage(string messageID)
		{
			AmbitionApp.SendMessage(messageID);
		}
	}
}