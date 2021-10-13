using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Core;

namespace Ambition
{
	public class ServantVO
	{
		public string ID;
		public ServantType Type;
		public int Wage; //How much they cost per week
		public Dictionary<string,float> Modifiers = new Dictionary<string, float>();
		public ServantStatus Status = ServantStatus.Introduced;
        public bool IsHired => Status == ServantStatus.Hired || Status == ServantStatus.Permanent;

		public override string ToString()
		{
			return string.Format( "{0} type: {1} wage £{2} status {3}", ID, Type, Wage, Status );
		}
	}
}
