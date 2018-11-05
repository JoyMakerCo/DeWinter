using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PierreAndSpeechBubbleMediator : MonoBehaviour {

    public Sprite PierreNeutral;
    public Sprite PierreHappy;

    public Image PierreAvatar;
    public GameObject SpeechBubble;
    public Text SpeechBubbleText;
    
	// Use this for initialization
	void Awake() {
        HideSpeechBubble();
	}

    void HideSpeechBubble()
    {
        SpeechBubble.SetActive(false);
    }

    void PierreQuestCheck()
    {

    }
}
