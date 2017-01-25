using System.Collections;

namespace DeWinter
{
	public class AdjustBalanceVO
	{
		public string Type;
		public double Amount;

		public AdjustBalanceVO(string type, double amount)
		{
			Type = type;
			Amount = amount;
		}

		public AdjustBalanceVO(string type=null, int amount=0)
		{
			Type = type;
			Amount = (double)amount;
		}
	}
}