using System;
using Core;

namespace DeWinter
{
	public class RestockMerchantCmd : ICommand<CalendarDayVO>
	{
		public void Execute(CalendarDayVO day)
		{
//			OutfitInventory.RestockMerchantInventory();
		} 
	}
}