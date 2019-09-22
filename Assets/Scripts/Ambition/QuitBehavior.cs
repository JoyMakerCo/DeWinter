using UnityEngine;
using System.Collections;

namespace Ambition
{
    public class QuitBehavior : MonoBehaviour
    {
        public void Quit()
        {
            UnityEngine.Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}
