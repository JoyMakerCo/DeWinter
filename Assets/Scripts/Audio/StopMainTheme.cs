using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopMainTheme : MonoBehaviour {

    public FMODUnity.StudioEventEmitter eventEmitter;
	
    public void Stop()
    {
        eventEmitter.Stop();
    }
}
