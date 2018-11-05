using System;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
	public class ButtonMessage : MonoBehaviour
	{
		public string Message;

		public void OnClick()
		{
			AmbitionApp.SendMessage(Message);
		}
	}
}
