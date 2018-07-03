using System.Collections.Generic;

namespace Ambition
{
    public class GuestActionFactory : Core.IFactory<string, GuestActionVO>
    {
        public Dictionary<string, GuestActionVO> Actions = new Dictionary<string, GuestActionVO>();
        public GuestActionVO Create(string type)
        {
            GuestActionVO action;
            return Actions.TryGetValue(type, out action) ? new GuestActionVO(action) : null;
        }
    }
}
