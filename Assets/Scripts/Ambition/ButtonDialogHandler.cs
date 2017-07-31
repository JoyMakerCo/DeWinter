using System;
using UnityEngine;

namespace Ambition
{
	public class ButtonDialogHandler : MonoBehaviour
	{
		public void OpenDialog(string dialogID)
		{
			AmbitionApp.OpenDialog(dialogID);
		}
	}
}