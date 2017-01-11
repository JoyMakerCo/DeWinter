using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ReputationTracker : MonoBehaviour {
    public Text numberText;
    public Text toolTipText;
    private int[] reputationLevelRequirements;

    
    // Use this for initialization
    void Start()
    {
        reputationLevelRequirements = new int[11];
        StockReputationLevelRequirements();
        UpdateReputation();
        DefeatCheck();
    }

    void Update()
    {
        UpdateReputation();
    }

    public void UpdateReputation()
    {
        numberText.text = PlayerReputationLevel() + "(" + GameData.reputationCount.ToString("#,##0") + ")";                     
    }

    private void StockReputationLevelRequirements()
    {
        reputationLevelRequirements[0] = 10;
        reputationLevelRequirements[1] = 20;
        reputationLevelRequirements[2] = 40;
        reputationLevelRequirements[3] = 80;
        reputationLevelRequirements[4] = 160;
        reputationLevelRequirements[5] = 320;
        reputationLevelRequirements[6] = 640;
        reputationLevelRequirements[7] = 1080;
        reputationLevelRequirements[8] = 2160;
        reputationLevelRequirements[9] = 4320;
        reputationLevelRequirements[10] = 8640;
    }

    public int PlayerReputationLevel()
    {
        int i = 0;
        while (GameData.reputationCount > reputationLevelRequirements[i])
        {
            i++;
        }
        GameData.playerReputationLevel = i;
        return GameData.playerReputationLevel;
    }

    void DefeatCheck()
    {
        //If your Reputation drops to 0 or below then you lose (for now)
        if (GameData.reputationCount <= 0)
        {
            GameData.playerVictoryStatus = "Reputation Loss";
            LevelManager man = GameObject.Find("LevelManager").GetComponent<LevelManager>();
            man.LoadLevel("Game_EndScreen");
        }
    }
}
