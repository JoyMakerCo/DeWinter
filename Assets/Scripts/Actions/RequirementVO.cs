using System;

namespace DeWinter
{
	public class RequirementVO
	{
		CriteriaType Type;
		string Value;
		int Quantity=0;

		public RequirementVO (string value, CriteriaType type=CriteriaType.Balance, int quantity=1)
		{
			Type = type;
			Value = value;
			Quantity = quantity;
		}
	}
}