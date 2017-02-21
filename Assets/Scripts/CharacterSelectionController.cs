using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectionController : MonoBehaviour {

    public GameObject yvetteButton;
    public GameObject karolinaButton;
    public GameObject feliciteButton;
    public GameObject donatienButton;

    public Text characterDescriptionText;

	// Use this for initialization
	void Start () {
        SelectYvette();
	}

    public void SelectYvette()
    {
        yvetteButton.GetComponent<Image>().color = Color.black;
        karolinaButton.GetComponent<Image>().color = Color.gray;
        feliciteButton.GetComponent<Image>().color = Color.gray;
        donatienButton.GetComponent<Image>().color = Color.gray;
        characterDescriptionText.text = "A young woman from a small fishing village to the South, Yvette came to Paris with hopes of marrying in the the nobility. Charming and ambitious, she is balanced and perfect for new Players."
            + "\n-Balanced Character"
            + "\n-No Advantages or Disadvantages";
    }
}
