using System;
using UnityEngine;

namespace DeWinter
{
	public class DialogButtonHandler : MonoBehaviour
	{
		public void OpenDialog (string DialogID)
		{
			DeWinterApp.OpenDialog(DialogID);
		}
	}
}
