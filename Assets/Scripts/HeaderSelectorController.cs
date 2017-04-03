using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;

public class HeaderSelectorController : MonoBehaviour {

    public Animator animator;
	
	public void CalendarSelected()
    {
        animator.SetBool("Wardrobe", false);
        animator.SetBool("Estate", false);
        animator.SetBool("Journal", false);
        animator.SetBool("Calendar", true);
    }

    public void WardrobeSelected()
    {
        animator.SetBool("Calendar", false);
        animator.SetBool("Estate", false);
        animator.SetBool("Journal", false);
        animator.SetBool("Wardrobe", true);
    }

    public void EstateSelected()
    {
        animator.SetBool("Calendar", false);
        animator.SetBool("Wardrobe", false);
        animator.SetBool("Journal", false);
        animator.SetBool("Estate", true);
    }

    public void JournalSelected()
    {
        animator.SetBool("Calendar", false);
        animator.SetBool("Wardrobe", false);
        animator.SetBool("Estate", false);
        animator.SetBool("Journal", true);
    }
}
