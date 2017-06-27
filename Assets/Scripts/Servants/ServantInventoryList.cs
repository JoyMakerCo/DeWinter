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
    void Start ()
    {
		_model = AmbitionApp.GetModel<ServantModel>();
        GenerateInventoryButtons();
        selectedServant = null;
    }

    public void GenerateInventoryButtons()
    {
    	ClearInventoryButtons();
		List<ServantVO> servants;
		if (inventoryType == InventoryType.Personal)
		{
			servants = new List<ServantVO>(_model.Hired.Values);
		}
		else
		{
			servants = new List<ServantVO>();
			foreach(List<ServantVO> intros in _model.Introduced.Values)
			{
				servants.AddRange(intros);
			}
		}

		servants.ForEach(MakeButton);
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
