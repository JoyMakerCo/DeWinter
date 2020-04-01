using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Newtonsoft.Json;

namespace Ambition
{
	public class ServantModel : DocumentModel, IConsoleEntity
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

        // used by console only to inspect any servant by name, on staff or not 
        public ServantVO[] GetAllServants()
        {
            List<ServantVO> allServants = new List<ServantVO>();

            foreach (var servant in Servants.Values)
            {
                allServants.Add( servant );
            }

            foreach (var akv in Applicants)
            {
                foreach (var applicant in akv.Value)
                {
                    allServants.Add(applicant);
                }
            }

            foreach (var ukv in Unknown)
            {
                foreach (var unknown in ukv.Value)
                {
                    allServants.Add(unknown);
                }
            }

            return allServants.ToArray(); 
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
                
                
        public string[] Dump()
        {
            var dateFormat = "MMMM d, yyyy";

            var lines = new List<string>()
            {
                "ServantModel:",

            };

            lines.Add( "Servants: " + Servants.Count.ToString() );
            foreach (var servant in Servants.Values)
            {
                lines.Add( string.Format( "  {0}", servant.ToString() ));
            }

            lines.Add( "Applicants:" );
            foreach (var akv in Applicants)
            {
                var names = string.Join(", ", akv.Value.Select( s => s.Name).ToArray());
                lines.Add( string.Format("  {0}: {1} ({2})", akv.Key, akv.Value.Count, names ) );
            }

            lines.Add( "Unknown:" );
            foreach (var ukv in Unknown)
            {
                var names = string.Join(", ", ukv.Value.Select( s => s.Name).ToArray());
                lines.Add( string.Format("  {0}: {1} ({2})", ukv.Key, ukv.Value.Count, names ) );
            } 

            return lines.ToArray();
        }

        public void Invoke( string[] args )
        {
            ConsoleModel.warn("ServantModel has no invocation.");
        }  
	}
}
