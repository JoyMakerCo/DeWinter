using System;
using System.Collections.Generic;
using Core;

namespace DeWinter
{
	public class SelectGuestCmd : ICommand<KeyValuePair<GuestVO, RemarkVO>>
	{
		public void Execute (KeyValuePair<GuestVO, RemarkVO> payload)
		{
			
		}
	}
}