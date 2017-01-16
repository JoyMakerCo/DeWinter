using System.Collections;

public class AdjustBalanceVO
{
	public string Type;
	public double Amount;

	public AdjustBalanceVO(string type, double amount)
	{
		Type = type;
		Amount = amount;
	}

	public AdjustBalanceVO(string type, int amount)
	{
		Type = type;
		Amount = (double)amount;
	}
}