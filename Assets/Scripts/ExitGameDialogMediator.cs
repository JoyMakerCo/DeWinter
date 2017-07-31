﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Dialog;

namespace Ambition
{
	public class ExitGameDialogMediator : DialogView
	{	
		public Button QuitBtn;
		public Button CancelBtn;

		void Start()
		{
			QuitBtn.onClick.AddListener(Application.Quit);
			CancelBtn.onClick.AddListener(Close);
		}
	}
}