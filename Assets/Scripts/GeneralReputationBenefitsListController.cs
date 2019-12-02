using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Core;

namespace Ambition
{
	public class GeneralReputationBenefitsListController : MonoBehaviour
	{
		private Text _text;
	    void Start()
	    {
	    	
	        _text = this.GetComponent<Text>();
			AmbitionApp.Subscribe<ReputationVO>(HandleReputation);
	    }

	    void OnDestroy()
	    {
			AmbitionApp.Unsubscribe<ReputationVO>(HandleReputation);
	    }

	    private void HandleReputation(ReputationVO rep)
	    {
			string str = "Reputation Level Benefits\n";
			for (int i=1; i<rep.Level; i++)
			{
				str += AmbitionApp.Localize("reputation_text." + i.ToString() + "\n");
			}
			_text.text = str;
	    }
	}
}
