using System;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
	public class DialogButtonHandler : MonoBehaviour
	{
		public string DialogID;

		void Start ()
		{
			Button btn = gameObject.GetComponent<Button>();
			if (btn) btn.onClick.AddListener(OpenDialog);
		}

		protected void OpenDialog()
		{
			AmbitionApp.OpenDialog(DialogID);
		}
	}
}