using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ambition
{
    public class RequirementsSvc : Core.IAppService
    {
        private Dictionary<CommodityType, Func<RequirementVO, bool>> _handlers = new Dictionary<CommodityType, Func<RequirementVO, bool>>();
        public void Register(CommodityType type, Func<RequirementVO, bool> Check) => _handlers[type] = Check;
        public bool Check(RequirementVO req)
        {
#if DEBUG
            try
            {
                return _handlers[req.Type] (req);
            }
            catch(Exception e)
            {
                throw new Exception("Requirement type \"" + req.Type.ToString() + "\" not found", e);
            }
#else
            return _handlers.TryGetValue(req.Type, out Func<RequirementVO, bool> handler) && handler(req);
#endif
        }
        public bool Check(RequirementVO[] reqs) 
        {
            Debug.LogFormat( "Checking {0} requirements", reqs?.Length );

            return  (reqs == null)
                    || (reqs.Length == 0)
                    || Array.TrueForAll(reqs, Check);
        }

        // Utility function for checking with an operator
        public static bool Check(RequirementVO req, int query)
        {
            var result = _Check(req,query);
            Debug.LogFormat("Check: commod {0} ID {1} value {2} operator {3} target {4} result {5}", req.Type, req.ID, req.Value, req.Operator, query, result);
            return result;
        }
        public static bool _Check(RequirementVO req, int query)
        {
            switch (req.Operator)
            {
                case RequirementOperator.Less:
                    return query < req.Value;
                case RequirementOperator.LessOrEqual:
                    return query <= req.Value;
                case RequirementOperator.Greater:
                    return query > req.Value;
                case RequirementOperator.GreaterOrEqual:
                    return query >= req.Value;
            }
            return query == req.Value;
        }

        public void Dispose() => _handlers.Clear();
    }
}
