using System.Collections;

namespace DeWinter
{
	public class AdjustValueVO
	{
		public string Type;
		public double Amount;
		public bool IsRequest=true;

		public AdjustValueVO(string type, double amount=0d, bool isRequest=true)
		{
			Type = type;
			Amount = amount;
			IsRequest = isRequest;
		}

		public AdjustValueVO(string type=null, int amount=0, bool isRequest=true)
		{
			Type = type;
			Amount = (double)amount;
			IsRequest = isRequest;
		}
	}
}