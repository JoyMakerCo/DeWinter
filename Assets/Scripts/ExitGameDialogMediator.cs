using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Dialog;

namespace Ambition
{
	public class ExitGameDialogMediator : DialogView
	{	
		public Button QuitBtn;
		public Button CancelBtn;

		public override void OnOpen ()
		{
			base.OnOpen ();
			QuitBtn.onClick.AddListener(UnityEngine.Application.Quit);
			CancelBtn.onClick.AddListener(Close);
		}
	}
}
