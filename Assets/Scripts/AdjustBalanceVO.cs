using System.Collections;

public class AdjustValueVO
{
	public string Type;
	public double Amount;
	public bool IsRequest=true;

	public AdjustValueVO(string type, double amount, bool isRequest=true)
	{
		Type = type;
		Amount = amount;
		IsRequest = isRequest;
	}

	public AdjustValueVO(string type, int amount, bool isRequest=true)
	{
		Type = type;
		Amount = (double)amount;
		IsRequest = isRequest;
	}
}