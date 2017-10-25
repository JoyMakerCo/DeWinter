using System;
using UnityEngine;
using System.Collections.Generic;
using Ambition;

public class ServantInventoryList : MonoBehaviour
{
    public ServantVO selectedServant;
    public GameObject gossipInventoryButtonPrefab;
    public enum InventoryType {Personal, Available};
    public InventoryType inventoryType;
	private ServantModel _model;

    // Use this for initialization
    void Awake ()
    {
		_model = AmbitionApp.GetModel<ServantModel>();
        GenerateInventoryButtons();
        selectedServant = null;
    }

    public void GenerateInventoryButtons()
    {
    	ClearInventoryButtons();
		ServantVO[] servants = (inventoryType == InventoryType.Personal)
			? Array.FindAll(_model.Servants, s=>s.Hired)
			: Array.FindAll(_model.Servants, s=>s.Introduced);

		 Array.ForEach(servants, MakeButton);
    }

    public void ClearInventoryButtons()
    {
        foreach (Transform child in this.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    private void MakeButton(ServantVO s)
    {
		GameObject button = GameObject.Instantiate(gossipInventoryButtonPrefab);
        ServantInventoryButton buttonStats = button.GetComponent<ServantInventoryButton>();
        buttonStats.servant = s;
        button.transform.SetParent(this.transform, false);
        Debug.Log("Servant Button: " + s.NameAndTitle + " is made!");
    }
}
