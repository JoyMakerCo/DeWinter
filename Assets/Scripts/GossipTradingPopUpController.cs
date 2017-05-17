using UnityEngine;
using System.Collections;

namespace Ambition
{
	public class GossipTradingPopUpController : MonoBehaviour
	{
	    public GossipTrading gossipTrading;
	    public string tradeFor;
		
		public void TradeGossip()
	    {
	        switch (tradeFor)
	        {
	            case "Livres":
	                gossipTrading.SellForLivres();
	                break;
	            case "Allegiance":
	                gossipTrading.LeakForAllegiance();
	                break;
	            case "Power":
	                gossipTrading.PropagandaForPower();
	                break;
	        }
	    }
	}
}