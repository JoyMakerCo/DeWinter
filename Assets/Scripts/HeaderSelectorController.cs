using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;

public class HeaderSelectorController : MonoBehaviour {


    public Animator animator;
	
	public void CalendarSelected()
    {
        animator.SetTrigger("Calendar");
    }

    public void WardrobeSelected()
    {
        animator.SetTrigger("Wardrobe");
    }
}
