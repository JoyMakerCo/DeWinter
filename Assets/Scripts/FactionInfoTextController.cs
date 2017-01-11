using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FactionInfoTextController : MonoBehaviour {

    public Text crownInfo;
    public Text churchInfo;
    public Text militaryInfo;
    public Text bourgeoisieInfo;
    public Text revolutionInfo;

    public int testTheWatersCost;

    public bool availableSpymasterTestTheWaters;
    public bool availableTestTheWaters;

	// Use this for initialization
	void Start () {
        if (GameData.servantDictionary["Spymaster"].Hired())
        {
            availableSpymasterTestTheWaters = true;
        } else
        {
            availableSpymasterTestTheWaters = false;
        }
        availableTestTheWaters = true;
        IncrementKnowledgeTimers();
        UpdateInfo();
	}

    void UpdateInfo()
    {
        crownInfo.text = "- The Royalty and members of high ranking nobility alligned with them" +
                        "\n- Like Expensive but Modest Clothes" +
                        "\n- Power: " + GameData.factionList["Crown"].knownPower + " " + ConvertKnowledgeTimer(GameData.factionList["Crown"].powerKnowledgeTimer);
        churchInfo.text = "- The Clergy and those alligned with them" +
                        "\n- Like Vintage and Modest Clothes" +
                        "\n- Allegiance: " + GameData.factionList["Church"].knownAllegiance + " " + ConvertKnowledgeTimer(GameData.factionList["Church"].allegianceKnowledgeTimer) +
                        "\n- Power: " + GameData.factionList["Church"].knownPower + " " + ConvertKnowledgeTimer(GameData.factionList["Church"].powerKnowledgeTimer); 
        militaryInfo.text = "- The Generals and Troops of the armed forces and those alligned with them" +
                        "\n- Couldn't care less about your clothes" +
                        "\n- Allegiance: " + GameData.factionList["Military"].knownAllegiance + " " + ConvertKnowledgeTimer(GameData.factionList["Military"].allegianceKnowledgeTimer) +
                        "\n- Power: " + GameData.factionList["Military"].knownPower + " " + ConvertKnowledgeTimer(GameData.factionList["Military"].powerKnowledgeTimer);
        bourgeoisieInfo.text = "- The newly wealthy Mercantile class" +
                        "\n- Like clothes that are Luxurious and Racy" +
                        "\n- Allegiance: " + GameData.factionList["Bourgeoisie"].knownAllegiance + " " + ConvertKnowledgeTimer(GameData.factionList["Bourgeoisie"].allegianceKnowledgeTimer) +
                        "\n- Power: " + GameData.factionList["Bourgeoisie"].knownPower + " " + ConvertKnowledgeTimer(GameData.factionList["Bourgeoisie"].powerKnowledgeTimer);
        revolutionInfo.text = "- The unhappy Common Man along with the Academics and Artists alligned with them" +
                        "\n - Like clothes that are Vintage and Racy" +
                        "\n- Power: " + GameData.factionList["Revolution"].knownPower + " " + ConvertKnowledgeTimer(GameData.factionList["Revolution"].powerKnowledgeTimer);
    }

    public void TestTheWatersPower(string faction)
    {
        if (availableSpymasterTestTheWaters)
        {
            availableSpymasterTestTheWaters = false;
            GameData.factionList[faction].CalculateKnownPower();
            UpdateInfo();
        } else if (availableTestTheWaters && GameData.moneyCount > testTheWatersCost)
        {
            availableTestTheWaters = false;
            GameData.moneyCount -= testTheWatersCost;
            GameData.factionList[faction].CalculateKnownPower();
            UpdateInfo();
        }
        else
        {
            Debug.Log("");
        }
    }

    public void TestTheWatersAllegiance(string faction)
    {
        if (availableSpymasterTestTheWaters)
        {
            availableSpymasterTestTheWaters = false;
            GameData.factionList[faction].CalculateKnownAllegiance();
            UpdateInfo();
        }
        else if(availableTestTheWaters && GameData.moneyCount > testTheWatersCost)
        {
            availableTestTheWaters = false;
            GameData.moneyCount -= testTheWatersCost;
            GameData.factionList[faction].CalculateKnownAllegiance();
            UpdateInfo();
        } else
        {
            Debug.Log("");
        }
    }

    void IncrementKnowledgeTimers()
    {
        //No Allegiance Timers for Crown and Revolution in here because they don't need to have them
        GameData.factionList["Crown"].powerKnowledgeTimer++;
        GameData.factionList["Church"].powerKnowledgeTimer++;
        GameData.factionList["Church"].allegianceKnowledgeTimer++;
        GameData.factionList["Military"].powerKnowledgeTimer++;
        GameData.factionList["Military"].allegianceKnowledgeTimer++;
        GameData.factionList["Bourgeoisie"].powerKnowledgeTimer++;
        GameData.factionList["Bourgeoisie"].allegianceKnowledgeTimer++;
        GameData.factionList["Revolution"].powerKnowledgeTimer++;
    }

    string ConvertKnowledgeTimer(int timer)
    {
        switch (timer) {
            case 0:
                return "(As of Today)";
            case 1:
                return "(As of Yesterday)";
            default:
                return "(As of " + timer + " Days Ago)";
        }     
    }
}
