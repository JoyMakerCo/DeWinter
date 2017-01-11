using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TopRowButtonColor : MonoBehaviour {

    private Image myImage;

    void Start()
    {
        myImage = this.GetComponent<Image>();
    }

    public void ColorSelected()
    {
        BroadcastMessage("ColorNotSelected");
        myImage.color = Color.black;  
    }

    public void ColorNotSelected()
    {
        //Debug.Log("Color Not Selected!");
        myImage.color = Color.white;
    }
}
