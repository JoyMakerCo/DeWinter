using UnityEngine;
using System.Collections;

public class ServantInventoryList : MonoBehaviour {

    public Servant selectedServant;
    public GameObject gossipInventoryButtonPrefab;
    public enum InventoryType {Personal, Available};
    public InventoryType inventoryType;

    // Use this for initialization
    void Start () {
        GenerateInventoryButtons();
        selectedServant = null;
    }

    public void GenerateInventoryButtons()
    {
        foreach (string k in GameData.servantDictionary.Keys)
        {
            Servant s = GameData.servantDictionary[k];
            if (inventoryType == InventoryType.Personal)
            {
                if (s.Hired())
                {
                    GameObject button = GameObject.Instantiate(gossipInventoryButtonPrefab);
                    ServantInventoryButton buttonStats = button.GetComponent<ServantInventoryButton>();
                    buttonStats.servant = s;
                    button.transform.SetParent(this.transform, false);
                    Debug.Log("Servant Button: " + s.NameAndTitle() + " is made!");
                }
            } else
            {
                if (!s.Hired() && s.Introduced())
                {
                    GameObject button = GameObject.Instantiate(gossipInventoryButtonPrefab);
                    ServantInventoryButton buttonStats = button.GetComponent<ServantInventoryButton>();
                    buttonStats.servant = s;
                    button.transform.SetParent(this.transform, false);
                    Debug.Log("Servant Button: " + s.NameAndTitle() + " is made!");
                }
            }
            
        }
    }

    public void ClearInventoryButtons()
    {
        foreach (Transform child in this.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
}
