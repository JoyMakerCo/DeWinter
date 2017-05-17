using System;

namespace Ambition
{
	public class DialogButtonHandler
	{
		public string Key;

		public void OpenDialog ()
		{
			AmbitionApp.OpenDialog(Key);
		}
	}
}