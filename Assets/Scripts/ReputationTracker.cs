using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ReputationTracker : MonoBehaviour {
    public Text numberText;
    public Text toolTipText;
    
    // Use this for initialization
    void Start()
    {
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

    public int PlayerReputationLevel()
    {
        int i = 0;
        while (GameData.reputationCount > GameData.reputationLevels[i].RequiredReputation())
        {
            i++;
        }
        GameData.playerReputationLevel = i-1;
        return GameData.playerReputationLevel;
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
}
