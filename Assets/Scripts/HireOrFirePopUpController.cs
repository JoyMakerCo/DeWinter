using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HireOrFirePopUpController : MonoBehaviour {

    public Servant servant;

    void HireOrFire()
    {
        if (servant.Hired())
        {
            servant.Fire();
        } else
        {
            servant.Hire();
        }
    }
}
