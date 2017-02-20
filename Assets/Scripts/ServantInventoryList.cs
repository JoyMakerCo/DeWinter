using UnityEngine;
using System.Collections.Generic;
using DeWinter;

public class ServantInventoryList : MonoBehaviour
{
    public ServantVO selectedServant;
    public GameObject gossipInventoryButtonPrefab;
    public enum InventoryType {Personal, Available};
    public InventoryType inventoryType;

    // Use this for initialization
    void Start ()
    {
        GenerateInventoryButtons();
        selectedServant = null;
    }

    public void GenerateInventoryButtons()
    {
    	ClearInventoryButtons();
		List<ServantVO> Servants = (inventoryType == InventoryType.Personal)
			? DeWinterApp.GetModel<ServantModel>().Hired.Values
			: DeWinterApp.GetModel<ServantModel>().Introduced;

		foreach(ServantVO s in Servants)
    	{
    		MakeButton(s);
    	}
    }

    public void ClearInventoryButtons()
    {
        foreach (Transform child in this.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    private GameObject MakeButton(ServantVO s)
    {
		GameObject button = GameObject.Instantiate(gossipInventoryButtonPrefab);
        ServantInventoryButton buttonStats = button.GetComponent<ServantInventoryButton>();
        buttonStats.servant = s;
        button.transform.SetParent(this.transform, false);
        Debug.Log("Servant Button: " + s.NameAndTitle + " is made!");
    }
}
