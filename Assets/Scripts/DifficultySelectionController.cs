using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultySelectionController : MonoBehaviour {

    public GameObject normalButton;
    public GameObject hardButton;
    
    public Text difficultyDescriptionText;

    // Use this for initialization
    void Start()
    {
        SelectNormal();
    }

    public void SelectNormal()
    {
        normalButton.GetComponent<Image>().color = Color.black;
        hardButton.GetComponent<Image>().color = Color.gray;
        difficultyDescriptionText.text = "Ideal for new Players, this difficulty setting will provide a decent challenge but also let you sit back and experience the story."
                                    + "\n\nRemember: You might not get the ending you want at first, but that's alright. Surprises are part of the fun!";
    }
}
