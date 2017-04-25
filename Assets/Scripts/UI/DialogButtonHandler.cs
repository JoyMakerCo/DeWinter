using System;

namespace DeWinter
{
	public class DialogButtonHandler
	{
		public string Key;

		public void OpenDialog ()
		{
			DeWinterApp.OpenDialog(Key);
		}
	}
}