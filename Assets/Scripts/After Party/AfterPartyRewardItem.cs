using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AfterPartyRewardItem : MonoBehaviour
{
    public Image IconImage;
    public Text TypeText;
    public Text FluffText;

    public string Text;
    public string Fluff;
    public int Value;
    public bool ShowValue = true;

    public void UpdateView()
    {
        TypeText.text = ShowValue ? Value.ToString("### ###") + " " + Text : Text;
        FluffText.enabled = !string.IsNullOrEmpty(Fluff);
        FluffText.text = Fluff;
    }
}
