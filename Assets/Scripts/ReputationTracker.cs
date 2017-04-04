using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace DeWinter
{
	public class ReputationTracker : MonoBehaviour
	{
	    public Text numberText;
	    public Text levelText;
	    public Image reputationIcon;
	    public Slider reputationBar;

	    public Sprite reputationLevel0Icon;
	    public Sprite reputationLevel1Icon;
	    public Sprite reputationLevel2Icon;
	    public Sprite reputationLevel3Icon;
	    public Sprite reputationLevel4Icon;
	    public Sprite reputationLevel5Icon;
	    public Sprite reputationLevel6Icon;
	    public Sprite reputationLevel7Icon;
	    public Sprite reputationLevel8Icon;
	    public Sprite reputationLevel9Icon;

	    Sprite[] reputationLevelIconArray = new Sprite[10];
	    private GameModel _gmod;

	    // Use this for initialization
	    void Awake()
	    {
			_gmod = DeWinterApp.GetModel<GameModel>();
	        StockReputationLevelIcons();
			DeWinterApp.Subscribe<PlayerReputationVO>(HandlePlayerReputation);

			PlayerReputationVO vo = new PlayerReputationVO(GameData.reputationCount,  GameData.playerReputationLevel);
			HandlePlayerReputation(vo);
	    }

	    void OnDestroy()
	    {
			DeWinterApp.Unsubscribe<PlayerReputationVO>(HandlePlayerReputation);
	    }

		private void HandlePlayerReputation(PlayerReputationVO vo)
	    {
	    	if (vo.Reputation > -20)
	    	{
				ReputationLevel repLvl = _gmod.ReputationLevels[vo.Level + 1];
				int nextLvl = repLvl.Reputation;
				numberText.text = vo.Reputation.ToString("#,##0") + "/" + nextLvl.ToString("#,##0");
				levelText.text = _gmod.ReputationLevels[vo.Level].Title;
		        reputationIcon.sprite = reputationLevelIconArray[vo.Level];
		        reputationBar.value = (float)vo.Reputation / (float)nextLvl;
		    }
		    else
		    {
				DeWinterApp.SendMessage<string>(GameMessages.LOAD_SCENE,"Game_EndScreen");
		    }
	    }

		private void StockReputationLevelIcons()
		{
		    reputationLevelIconArray[0] = reputationLevel0Icon;
		    reputationLevelIconArray[1] = reputationLevel1Icon;
		    reputationLevelIconArray[2] = reputationLevel2Icon;
		    reputationLevelIconArray[3] = reputationLevel3Icon;
		    reputationLevelIconArray[4] = reputationLevel4Icon;
		    reputationLevelIconArray[5] = reputationLevel5Icon;
		    reputationLevelIconArray[6] = reputationLevel6Icon;
		    reputationLevelIconArray[7] = reputationLevel7Icon;
		    reputationLevelIconArray[8] = reputationLevel8Icon;
		    reputationLevelIconArray[9] = reputationLevel9Icon;
		}
	}
}