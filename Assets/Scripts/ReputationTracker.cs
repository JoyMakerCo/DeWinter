using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ReputationTracker : MonoBehaviour {
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

    // Use this for initialization
    void Start()
    {
        StockReputationLevelIcons();
        UpdateReputation();
        DefeatCheck();
    }

    void Update()
    {
        UpdateReputation();
    }

    public void UpdateReputation()
    {
        PlayerReputationLevel();
        int playerReputationLevel = GameData.playerReputationLevel;
        numberText.text = shownReputationValue(playerReputationLevel) + "/" + shownReputationNextLevelValue(playerReputationLevel);
        levelText.text = GameData.reputationLevels[playerReputationLevel].Name;
        reputationIcon.sprite = reputationLevelIconArray[playerReputationLevel];
        reputationBar.value = shownReputationValue(playerReputationLevel) / shownReputationNextLevelValue(playerReputationLevel);                     
    }

    public void PlayerReputationLevel()
    {
        int i = 0;
        while (GameData.reputationCount >= GameData.reputationLevels[i].RequiredReputation())
        {
            i++;
        }
        GameData.playerReputationLevel = i-1;
    }

    void DefeatCheck()
    {
        //If your Reputation drops to -20 or below then you lose (for now)
        if (GameData.reputationCount <= -20)
        {
            GameData.playerVictoryStatus = "Reputation Loss";
            LevelManager man = GameObject.Find("LevelManager").GetComponent<LevelManager>();
            man.LoadLevel("Game_EndScreen");
        }
    }

    void StockReputationLevelIcons()
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

    int shownReputationValue(int repLevel)
    {
        if(repLevel == 1)
        {
            return GameData.reputationCount;
        } else
        {
            int repValue = GameData.reputationCount - GameData.reputationLevels[repLevel].RequiredReputation();
            return repValue;
        }
    }

    int shownReputationNextLevelValue(int repLevel)
    {
        if(repLevel == 1)
        {
            return GameData.reputationLevels[repLevel].RequiredReputation();
        } else
        {
            int nextLevelValue = GameData.reputationLevels[repLevel + 1].RequiredReputation() - GameData.reputationLevels[repLevel].RequiredReputation();
            return nextLevelValue;
        }

    }
}
