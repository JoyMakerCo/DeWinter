using System;
using Core;

namespace DeWinter
{
	public class IntroServantCmd : ICommand<string>
	{
		public void Execute (string servantType)
		{
			ServantModel model = DeWinterApp.GetModel<ServantModel>();
			ServantVO [] servants = model.GetServants(servantType);
			servants = Array.FindAll(servants, s => !s.Hired && !s.Introduced);
			if (servants.Length > 0)
			{
				servants[new Random().Next(servants.Length)].Introduced = true;
			}
		}
	}
}