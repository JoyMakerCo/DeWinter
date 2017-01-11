using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FireBackRemarkSlotButton : MonoBehaviour {

    public FireBackRemarkSlot myHostRemarkSlot;
    public WorkTheHostManager workManager;
    public int slot;

    public Image remarkSlotImage;
    Image dispositionIcon;

	//Done
	void Start () {
        dispositionIcon = this.transform.Find("DispositionIcon").GetComponent<Image>();
        remarkSlotImage = this.transform.Find("TargetImage").GetComponent<Image>();
    }

    void Update()
    {
        if (myHostRemarkSlot.lockedInState == 1)
        {
            remarkSlotImage.color = Color.black;
        }
        dispositionIcon.color = DispositionImageColor();
    }

    Color DispositionImageColor()
    {
            return GameData.dispositionList[myHostRemarkSlot.dispositionInt].color;
    }

    public void HostRemarkSelected()
    {
        workManager.HostRemarkSelected(slot);
    }

    public void HostRemarkHighlighting()
    {
        workManager.HostRemarkSlotHighlighting(slot);
    }

    public void HostRemarkUnhighlighting()
    {
        remarkSlotImage.color = Color.white;
    }
}

