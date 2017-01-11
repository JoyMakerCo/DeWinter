using UnityEngine;
using System.Collections;

public class NoOutfitModal : MonoBehaviour {

    private SceneFadeInOut sceneFader;
    private MainScreenTabsController tabsController;

    void Start()
    {
        sceneFader = GameObject.Find("ScreenFader").GetComponent<SceneFadeInOut>();
        tabsController = GameObject.Find("MainScreenTabsContainer").GetComponent<MainScreenTabsController>();
    }

    public void Dismiss()
    {
        Destroy(transform.parent.gameObject);
        GameData.activeModals--;
    }

    public void CreateCancellationModal()
    {
        object[] objectStorage = new object[1];
        Day cancellationDay = GameData.calendar.monthList[GameData.currentMonth].SelectDayByInt(GameData.currentDay);
        objectStorage[0] = cancellationDay; //Day
        sceneFader.gameObject.SendMessage("CreateCancellationPopUp", objectStorage);
    }

    public void GoToTheMerchant()
    {
        tabsController.WardrobeSelected();
    }
}

