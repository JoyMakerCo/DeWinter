using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Ambition
{
	public class PierreQuestManager : MonoBehaviour
	{
	    Text questNameText;
	    Text questFlavorText;
	    public PierreQuestInventoryList pierreQuestInventoryList;

	    // Use this for initialization
	    void Start () {
	        questNameText = this.GetComponent<Text>();
	        questFlavorText = this.transform.Find("PierreQuestFlavorText").GetComponent<Text>();
	    }

	    // Update is called once per frame
	    void Update()
	    {
	        if (pierreQuestInventoryList.selectedQuest != -1)
	        {
	            PierreQuest displayQuest = GameData.pierreQuestInventory[pierreQuestInventoryList.selectedQuest];
	            questNameText.text = displayQuest.Name;
	            questFlavorText.text = displayQuest.FlavorText();
	        }
	        else
	        {
	            questNameText.text = "";
	            questFlavorText.text = "";
	        }
	    }
	}
}