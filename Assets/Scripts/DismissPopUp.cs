using UnityEngine;
using System.Collections;

public class DismissPopUp : MonoBehaviour {

    public void Dismiss()
    {
        Destroy(transform.parent.gameObject);
    }
}