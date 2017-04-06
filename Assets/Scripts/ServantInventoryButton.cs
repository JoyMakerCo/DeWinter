using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using DeWinter;

public class ServantInventoryButton : MonoBehaviour {

    public ServantVO servant;
    public Text nameText;
    public Image servantThumbnail;
    private Outline outline; // This is for highlighting buttons

    ServantInventoryList servantList;

    public Sprite camilleImage;

    List<Sprite> servantSpriteList = new List<Sprite>();

    void Start()
    {
        outline = this.GetComponent<Outline>();
        servantList = this.transform.parent.GetComponent<ServantInventoryList>();
        StockServantImageList();
        DisplayServant(servant);
    }

    void Update()
    {
        
        if (servantList.selectedServant == servant)
        {
            outline.effectColor = Color.yellow;
        }
        else
        {
            outline.effectColor = Color.clear;
        }
    }

    void DisplayServant(ServantVO s)
    {
        if (s != null)
        {
            nameText.text = s.NameAndTitle;
            servantThumbnail.sprite = servantSpriteList[0];
            Debug.Log("Our Servant is " + s.NameAndTitle);
        } else
        {
            Debug.Log("Don't have a servant for this button!");
        }
    }

    void StockServantImageList()
    {
        servantSpriteList.Add(camilleImage);
    }

    public void SelectServant()
    {
        servantList.selectedServant = servant;
    }
}
