using UnityEngine;
using System.Collections;

namespace Ambition
{
    public class DebugOnlyView : MonoBehaviour
    {
        void Awake()
        {
#if DEBUG
            Destroy(this);
#else
            Destroy(gameObject);
#endif
        }
    }
}
