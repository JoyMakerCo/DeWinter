using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectionController : MonoBehaviour {

    // ---- The Buttons in the List ----
    public GameObject button0;
    public Text button0Text;
    public GameObject button1;
    public Text button1Text;
    public GameObject button2;
    public Text button2Text;
    public GameObject button3;
    public Text button3Text;

    // ---- The Lists ----
    List<GameObject> buttonList = new List<GameObject>();
    List<Text> buttonTextList = new List<Text>();
    List<PlayerCharacter> characterList = new List<PlayerCharacter>();

    // ---- Used when Characters are Selected ----
    PlayerCharacter selectedCharacter;
    public Text characterDescriptionText;

	// Use this for initialization
	void Start () {
        StockCharacterList();
        StockButtonList();
        StockButtonTextList();
        DisplayButtonText();
        SelectCharacter(0);
	}

    public void SelectCharacter(int buttonNumber)
    {
        selectedCharacter = characterList[buttonNumber];
        for (int i = 0; i < buttonList.Count; i++)
        {
            if(i == buttonNumber)
            {
                buttonList[i].GetComponent<Image>().color = Color.black;
            } else
            {
                buttonList[i].GetComponent<Image>().color = Color.white;
            }
        }
        characterDescriptionText.text = selectedCharacter.description;
    }

    void StockButtonList()
    {
        buttonList.Add(button0);
        buttonList.Add(button1);
        buttonList.Add(button2);
        buttonList.Add(button3);
    }

    void StockButtonTextList()
    {
        buttonTextList.Add(button0Text);
        buttonTextList.Add(button1Text);
        buttonTextList.Add(button2Text);
        buttonTextList.Add(button3Text);
    }

    void DisplayButtonText()
    {
        for(int i = 0; i < buttonList.Count; i++)
        {
            buttonTextList[i].text = characterList[i].name;
        }
    }

    void StockCharacterList()
    {
        // ---- Character 0, Yvette) ----
        characterList.Add(new PlayerCharacter("Yvette", "A young woman from a small fishing village to the South, Yvette came to Paris with hopes of marrying in the the nobility. Charming and ambitious, she is balanced and perfect for new Players."
            + "\n\n-Balanced Character"
            + "\n-No Advantages or Disadvantages"));
        // ---- Character 1, Yvette) ----
        characterList.Add(new PlayerCharacter("Karoline", "From Northern Europe, Karoline is bubbly, insecure and prone to drink."
            + "\n\n-This Character is not yet implemented"));
        // ---- Character 2, Yvette) ----
        characterList.Add(new PlayerCharacter("Félicité", "Born in Haiti and recently arrvied in Paris, Félicité will have to contend both with mundane social challenges, but with contentions surrounding her mixed heritage. Not recommended for new players."
            + "\n\n-This Character is not yet implemented"));
        // ---- Character 3, Yvette) ----
        characterList.Add(new PlayerCharacter("Donatien", "A mysterious and handsome man, many rumors swirl around Donatien and his past, including several scandalous ones concerning his... choice of pleasures."
            + "\n\n-This Character is not yet implemented"));
    }
}
