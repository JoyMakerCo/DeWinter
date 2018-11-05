using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeaderSelectorController : MonoBehaviour {

    public Animator ShadowAnimator;
    public Animator HeaderAnimator;
	
	public void CalendarSelected()
    {
        ShadowAnimator.SetBool("Wardrobe", false);
        ShadowAnimator.SetBool("Estate", false);
        ShadowAnimator.SetBool("Journal", false);
        ShadowAnimator.SetBool("Calendar", true);
        HeaderAnimator.SetBool("Wardrobe", false);
        HeaderAnimator.SetBool("Estate", false);
        HeaderAnimator.SetBool("Journal", false);
        HeaderAnimator.SetBool("Calendar", true);
    }

    public void WardrobeSelected()
    {
        ShadowAnimator.SetBool("Calendar", false);
        ShadowAnimator.SetBool("Estate", false);
        ShadowAnimator.SetBool("Journal", false);
        ShadowAnimator.SetBool("Wardrobe", true);
        HeaderAnimator.SetBool("Calendar", false);
        HeaderAnimator.SetBool("Estate", false);
        HeaderAnimator.SetBool("Journal", false);
        HeaderAnimator.SetBool("Wardrobe", true);
    }

    public void EstateSelected()
    {
        ShadowAnimator.SetBool("Calendar", false);
        ShadowAnimator.SetBool("Wardrobe", false);
        ShadowAnimator.SetBool("Journal", false);
        ShadowAnimator.SetBool("Estate", true);
        HeaderAnimator.SetBool("Calendar", false);
        HeaderAnimator.SetBool("Wardrobe", false);
        HeaderAnimator.SetBool("Journal", false);
        HeaderAnimator.SetBool("Estate", true);
    }

    public void JournalSelected()
    {
        ShadowAnimator.SetBool("Calendar", false);
        ShadowAnimator.SetBool("Wardrobe", false);
        ShadowAnimator.SetBool("Estate", false);
        ShadowAnimator.SetBool("Journal", true);
        HeaderAnimator.SetBool("Calendar", false);
        HeaderAnimator.SetBool("Wardrobe", false);
        HeaderAnimator.SetBool("Estate", false);
        HeaderAnimator.SetBool("Journal", true);
    }
}
