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

		public bool Hire(ServantVO servant)
		{
            if (!Servants.ContainsKey(servant.Slot) && RemoveFromDictionary(Applicants, servant))
            {
                servant.Status = ServantStatus.Hired;
                Servants[servant.Slot] = servant;
                AmbitionApp.SendMessage<ServantVO>(ServantMessages.SERVANT_HIRED, servant);
                return true;
            }
            return false;
		}

        public bool Fire(string servantType)
        {
            ServantVO servant;

            if (Servants.TryGetValue(servantType, out servant) && servant.Status != ServantStatus.Permanent)
            {
                servant.Status = ServantStatus.Introduced;
                AmbitionApp.SendMessage<ServantVO>(ServantMessages.SERVANT_FIRED, servant);
                AddToDictionary(Applicants, servant);
                return true;
            }
            return false;
        }

		public bool Fire(ServantVO servant)
		{
            return (Servants.ContainsKey(servant.Slot)
                    && Servants[servant.Slot] == servant)
                    && Fire(servant.Slot);
		}

		public bool Introduce(ServantVO servant)
		{
            if (RemoveFromDictionary(Unknown, servant) && AddToDictionary(Applicants, servant))
            {
                servant.Status = ServantStatus.Introduced;
                AmbitionApp.SendMessage(ServantMessages.SERVANT_INTRODUCED, servant);
                return true;
            }
            return false;
		}

        public bool Introduce(string servantType)
        {
            List<ServantVO> servants;
            if (!Unknown.TryGetValue(servantType, out servants) || servants.Count == 0)
            {
                return false;
            }
            ServantVO servant = Util.RNG.TakeRandom(servants.ToArray());
            if (AddToDictionary(Applicants, servant))
            {
                Unknown[servantType].Remove(servant);
                servant.Status = ServantStatus.Introduced;
                AmbitionApp.SendMessage(ServantMessages.SERVANT_INTRODUCED, servant);
                return true;
            }
            return false;
        }

        private bool AddToDictionary(Dictionary<string, List<ServantVO>> dictionary, ServantVO servant)
        {
            if (!dictionary.ContainsKey(servant.Slot))
            {
                dictionary[servant.Slot] = new List<ServantVO>();
            }
            else if (dictionary[servant.Slot].Contains(servant))
            {
                return false;
            }
            dictionary[servant.Slot].Add(servant);
            return true;
        }

        private bool RemoveFromDictionary(Dictionary<string, List<ServantVO>> dictionary, ServantVO servant)
        {
            string slot = servant.Slot;
            bool result = dictionary.ContainsKey(slot) && dictionary[slot].Remove(servant);
            if (dictionary[slot].Count == 0) dictionary.Remove(slot);
            return result;
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
                            AddToDictionary(Applicants, servant);
							break;
						default:
                            AddToDictionary(Unknown, servant);
							break;
					}
				}
			}
		}
	}
}
