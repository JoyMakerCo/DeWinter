using System;

namespace DeWinter
{
	public class MessageDialogVO
	{
		public const string DEFAULT_BUTTON_LABEL = "Okay";

		public string Title;
		public string Body;
		public string Button;

		public MessageDialogVO (string title="", string body="", string button=DEFAULT_BUTTON_LABEL)
		{
			Title = title;
			Body = body;
			Button = button;
		}
	}
}