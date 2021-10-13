using System;
using System.Collections.Generic;
using Core;
using UnityEngine;
using Newtonsoft.Json;

namespace Ambition
{
    [Saveable]
    public class ServantModel : ObservableModel<ServantModel>, IResettable
    {
        private const string SERVANT_PATH = "Servants/";

        [JsonIgnore]
        public List<ServantVO> Servants = new List<ServantVO>();

        [JsonProperty("servants")]
        private string[] _servants
        {
            get
            {
                List<string> result = new List<string>();
                List<string> hired = new List<string>();
                List<string> permanent = new List<string>();
                foreach(ServantVO servant in Servants)
                {
                    switch(servant.Status)
                    {
                        case ServantStatus.Introduced:
                            result.Add(servant.ID);
                            break;
                        case ServantStatus.Hired:
                            hired.Add(servant.ID);
                            break;
                        case ServantStatus.Permanent:
                            permanent.Add(servant.ID);
                            break;
                    }
                }
                result.Add(null);
                result.AddRange(hired);
                result.Add(null);
                result.AddRange(permanent);
                return result.ToArray();
            }
            set
            {
                ServantVO servant;
                int status = 0;
                Servants.ForEach(s => s.Status = ServantStatus.Unknown);
                foreach (string id in value)
                {
                    if (string.IsNullOrEmpty(id))
                        ++status;
                    else
                    {
                        servant = LoadServant(id);
                        if (servant != null)
                            servant.Status = (ServantStatus)status;
                    }
                }
            }
        }

        public ServantVO GetServant(ServantType type) => Servants.Find(s => s.Type == type && s.IsHired);
        public ServantVO GetServant(string servantID) => Servants.Find(s => s.ID == servantID);
        public ServantVO LoadServant(string servantID)
        {
            ServantVO servant = GetServant(servantID);
            if (servant != null) return servant;
            servant = Resources.Load<ServantConfig>(SERVANT_PATH + servantID)?.GetServant();
            if (servant != null)
            {
                Servants.Add(servant);
                Broadcast();
            }
            return servant;
        }

        // used by console only to inspect any servant by name, on staff or not 
        public ServantVO[] GetAllServants() => Servants.FindAll(s => s.Status == ServantStatus.Hired).ToArray();
        public void Reset() => Servants.Clear();

        public override string ToString()
        {
            string result = "ServantModel:\n";
            foreach (ServantVO servant in Servants)
            {
                result += "\n " + servant.ToString();
            }
            return result;
        }
    }
}
