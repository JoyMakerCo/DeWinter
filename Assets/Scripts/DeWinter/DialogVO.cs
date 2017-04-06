using System;

namespace DeWinter
{
	public class DialogVO
	{
		public string DialogID;

		public DialogVO (string dialogID)
		{
			this.DialogID = dialogID;
		}
	}

	public class DialogVO<T> : DialogVO
	{
		public T Data;

		public DialogVO (string dialogID, T data) : base(dialogID)
		{
			this.Data = data;
		}
	}
}