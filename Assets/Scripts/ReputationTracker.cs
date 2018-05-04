using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Core;

namespace Ambition
{
	public class ReputationTracker : MonoBehaviour
	{
	    public Text numberText;
	    public Text levelText;
	    public Image reputationIcon;
	    public Slider reputationBar;
	    public Sprite [] ReputationLevelIcons;

	    private GameModel _model;

	    // Use this for initialization
	    void Awake()
	    {
			_model = AmbitionApp.GetModel<GameModel>();
			AmbitionApp.Subscribe<ReputationVO>(HandlePlayerReputation);
			_model.Reputation = _model.Reputation; // Elicit an event
	    }

	    void OnDestroy()
	    {
			AmbitionApp.Unsubscribe<ReputationVO>(HandlePlayerReputation);
	    }

		private void HandlePlayerReputation(ReputationVO vo)
	    {
				numberText.text = vo.Reputation.ToString("#,##0") + "/" + vo.ReputationMax.ToString("#,##0");
				levelText.text = AmbitionApp.GetModel<LocalizationModel>().GetString("reputation." + vo.Level);
				reputationIcon.sprite = ReputationLevelIcons[vo.Level-1];
				reputationBar.value = (float)vo.Reputation / (float)vo.ReputationMax;
	    }
	}
}
