using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AccessoryInventorySizeTracker : MonoBehaviour {
    private Text myText;

    // Use this for initialization
    void Start()
    {
        myText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        myText.text = AccessoryInventory.personalInventory.Count.ToString() + "/" + AccessoryInventory.personalInventoryMaxSize.ToString();
    }
}
