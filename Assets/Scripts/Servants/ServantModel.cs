using System;
using System.Collections.Generic;
using Core;
using Newtonsoft.Json;

namespace Ambition
{
	public class ServantModel : DocumentModel
	{
		public ServantModel () : base ("ServantData") {}

		public Dictionary<string, ServantVO> Servants = new Dictionary<string, ServantVO>();
		public Dictionary<string, List<ServantVO>> Applicants = new Dictionary<string, List<ServantVO>>();
		public Dictionary<string, List<ServantVO>> Unknown = new Dictionary<string, List<ServantVO>>();

		public void Hire(ServantVO servant)
		{
			List<ServantVO> servants;
			UnknownUtil(servant);
			if (Applicants.TryGetValue(servant.Slot, out servants))
				servants.Remove(servant);
			if (Servants.ContainsKey(servant.Slot) && Servants[servant.Slot] != servant)
				Fire(Servants[servant.Slot]);
			Servants[servant.Slot] = servant;
			servant.Status = ServantStatus.Hired;
			AmbitionApp.SendMessage<ServantVO>(ServantMessages.SERVANT_HIRED, servant);
		}

		public void Fire(ServantVO servant)
		{
			if (servant.Status == ServantStatus.Permanent) return;
			DictionaryUtil(Applicants, servant);
			UnknownUtil(servant);
			if (Servants.ContainsKey(servant.Slot) && Servants[servant.Slot] == servant)
				Servants.Remove(servant.Slot);
			servant.Status = ServantStatus.Introduced; // Fired?
			AmbitionApp.SendMessage<ServantVO>(ServantMessages.SERVANT_FIRED, servant);
		}

		public void Introduce(ServantVO servant)
		{
			DictionaryUtil(Applicants, servant);
			UnknownUtil(servant);
			if (Servants.ContainsKey(servant.Slot) && Servants[servant.Slot] == servant)
				Servants.Remove(servant.Slot);
			servant.Status = ServantStatus.Introduced;
			AmbitionApp.SendMessage<ServantVO>(ServantMessages.SERVANT_INTRODUCED, servant);
		}

		private void UnknownUtil(ServantVO servant)
		{
			List <ServantVO> servants;
			if (Unknown.TryGetValue(servant.Slot, out servants)
				&& servants.Remove(servant)
				&& servants.Count==0)
			{
				Unknown.Remove(servant.Slot);
			}
		}

		private void DictionaryUtil(Dictionary<string, List<ServantVO>> table, ServantVO servant)
		{
			List<ServantVO> servants;
			if (!table.TryGetValue(servant.Slot, out servants))
				table[servant.Slot] = new List<ServantVO>(){servant};
			else servants.Add(servant);
		}

		[JsonProperty("servants")]
		private ServantVO[] _servants
		{
			set
			{
				foreach(ServantVO servant in value)
				{
					switch(servant.Status)
					{
						case ServantStatus.Permanent:
							Servants[servant.Slot] = servant;
							break;
						case ServantStatus.Hired:
							Servants[servant.Slot] = servant;
							break;
						case ServantStatus.Introduced:
							DictionaryUtil(Applicants, servant);
							break;
						default:
							DictionaryUtil(Unknown, servant);
							break;
					}
				}
			}
		}
	}
}
