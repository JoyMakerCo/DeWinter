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
    }

    public void CreateCancellationModal()
    {
        object[] objectStorage = new object[1];
        objectStorage[0] = DeWinter.DeWinterApp.GetModel<DeWinter.CalendarModel>().Today;
        sceneFader.gameObject.SendMessage("CreateCancellationPopUp", objectStorage);
    }

    public void GoToTheMerchant()
    {
        tabsController.WardrobeSelected();
    }
}

