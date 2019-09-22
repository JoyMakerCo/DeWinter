using System;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
	public class DialogButtonHandler : MonoBehaviour
	{
		public string DialogID;

		void Awake ()
		{
			gameObject.GetComponent<Button>()?.onClick.AddListener(OpenDialog);
		}

		protected void OpenDialog()
		{
			AmbitionApp.OpenDialog(DialogID);
		}

        private void OnDestroy()
        {
            gameObject.GetComponent<Button>()?.onClick.RemoveListener(OpenDialog);
        }
    }
}