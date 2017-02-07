using System;
using Core;

namespace DeWinter
{
	public class CalculateAccessoryPriceCmd : ICommand<AccessoryVO>
	{
		public void Execute (AccessoryVO accessory)
		{
			Random rnd = new Random();
			accessory.Price = rnd.Next(accessory.PriceMin, accessory.PriceMax);
			accessory.Price += rnd.Next(accessory.PriceMin, accessory.PriceMax);
			accessory.Price *= 0.5;
    	}
    }
}