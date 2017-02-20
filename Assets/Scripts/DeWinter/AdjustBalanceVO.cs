using System.Collections;

namespace DeWinter
{
	public class AdjustBalanceVO
	{
		public string Type;
		public double Amount;
		public bool IsRequest=true;

		public AdjustBalanceVO(string type, double amount=0d, bool isRequest=true)
		{
			Type = type;
			Amount = amount;
			IsRequest = isRequest;
		}

		public AdjustBalanceVO(string type=null, int amount=0, bool isRequest=true)
		{
			Type = type;
			Amount = (double)amount;
			IsRequest = isRequest;
		}
	}
}