using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace DeWinter
{
	public class ReputationTracker : MonoBehaviour {
	    public Text numberText;
	    public Text toolTipText;
	    
	    // Use this for initialization
	    void Start()
	    {
	        DeWinterApp.Subscribe<PlayerReputationVO>(HandlePlayerReputation);
	    }

		private void HandlePlayerReputation(PlayerReputationVO vo)
	    {
	    	if (vo.Reputation > -20)
	    	{
		        numberText.text = vo.Level.ToString() + " (" + vo.Reputation.ToString("#,##0") + ")";
		    }
		    else
		    {
	            LevelManager man = GameObject.Find("LevelManager").GetComponent<LevelManager>();
	            man.LoadLevel("Game_EndScreen");
		    }
	    }
	}
}