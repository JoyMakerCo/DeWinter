using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Ambition
{
	public class ReputationTracker : MonoBehaviour
	{
	    public Text numberText;
	    public Text levelText;
	    public Image reputationIcon;
	    public Slider reputationBar;
	    public Sprite [] ReputationLevelIcons;

	    private GameModel _gmod;

	    // Use this for initialization
	    void Awake()
	    {
			_gmod = AmbitionApp.GetModel<GameModel>();
			AmbitionApp.Subscribe<PlayerReputationVO>(HandlePlayerReputation);
			_gmod.Reputation = _gmod.Reputation; // Elicit an event
	    }

	    void OnDestroy()
	    {
			AmbitionApp.Unsubscribe<PlayerReputationVO>(HandlePlayerReputation);
	    }

		private void HandlePlayerReputation(PlayerReputationVO vo)
	    {
	    	if (vo.Reputation > -20)
	    	{
				numberText.text = vo.Reputation.ToString("#,##0") + "/" + vo.ReputationMax.ToString("#,##0");
				levelText.text = vo.Title;
				reputationIcon.sprite = ReputationLevelIcons[vo.Level-1];
				reputationBar.value = (float)vo.Reputation / (float)vo.ReputationMax;
		    }
		    else
		    {
				AmbitionApp.SendMessage<string>(GameMessages.LOAD_SCENE,"Game_EndScreen");
		    }
	    }
	}
}