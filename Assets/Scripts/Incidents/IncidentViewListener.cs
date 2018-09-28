using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ambition
{
    public class IncidentViewListener : MonoBehaviour
    {
        public GameObject IncidentViewPrefab;

        void Awake()
        {
            AmbitionApp.Subscribe<IncidentVO>(IncidentMessages.START_INCIDENT, HandleIncident);
            AmbitionApp.Subscribe(IncidentMessages.END_INCIDENTS, HandleEndIncident);
        }

        private void OnDestroy()
        {
            AmbitionApp.Unsubscribe<IncidentVO>(IncidentMessages.START_INCIDENT, HandleIncident);
            AmbitionApp.Unsubscribe(IncidentMessages.END_INCIDENTS, HandleEndIncident);
        }

        private void HandleIncident(IncidentVO incident)
        {
            if (incident != null && transform.childCount == 0)
            {
                Instantiate(IncidentViewPrefab, transform);
                AmbitionApp.SendMessage(IncidentMessages.START_INCIDENT, incident);
            }
        }

        void HandleEndIncident()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
