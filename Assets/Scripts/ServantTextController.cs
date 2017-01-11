using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ServantTextController : MonoBehaviour {

    private Text textField;
    public string servantType;

    void Start () {
        textField = this.GetComponent<Text>();
        //If the Servant has been introduced then their description doesn't show up
        if (GameData.servantDictionary[servantType].Introduced())
        {
            textField.color = Color.white;
        } else
        {
            textField.color = Color.clear;
        }
    }
}
