using System;

namespace Ambition
{
	public class RequirementVO
	{
		public CriteriaType Type;
		public string Value;
		public int Quantity=0;

		public RequirementVO (string value, CriteriaType type=CriteriaType.Balance, int quantity=1)
		{
			Type = type;
			Value = value;
			Quantity = quantity;
		}
	}
}