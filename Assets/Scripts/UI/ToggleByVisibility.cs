using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ambition
{
    public class ToggleByVisibility : MonoBehaviour
    {
        public GameObject[] Objects;

        public void ToggleNext()
        {
            int count = Objects.Length;
            for (int i=0; i<count; ++i)
            {
                if (Objects[i]?.activeInHierarchy ?? false)
                {
                    for (int j=1; j<count; ++j)
                    {
                        if (Objects[(i+j)%count] != null)
                        {
                            Objects[i].SetActive(false);
                            Objects[(i + j) % count].SetActive(true);
                            return;
                        }
                    }
                }
            }
        }

        public void Toggle(GameObject obj)
        {
            foreach(GameObject togglable in Objects)
            {
                togglable?.SetActive(false);
            }
            obj.SetActive(true);
        }
    }
}
