using System.Collections;

public class AdjustBalanceVO
{
	public string Type;
	public double Amount;
	public bool IsRequest=true;

	public AdjustBalanceVO(string type, double amount, bool isRequest=true)
	{
		Type = type;
		Amount = amount;
		IsRequest = isRequest;
	}

	public AdjustBalanceVO(string type, int amount, bool isRequest=true)
	{
		Type = type;
		Amount = (double)amount;
		IsRequest = isRequest;
	}
}