using Core;
using System.Collections.Generic;
using UFlow;

namespace Ambition
{
    public class RegisterIncidentControllerCmd : ICommand
    {
        private string FLOW_ID = "IncidentController";
        private string _lastState = null;
        private List<string> _registeredStates = new List<string>();

        public void Execute()
        {
            Decision<CheckIncidentLink>("StartIncidentDecision", "StartIncidents", "NoIncidents");
            State<StartIncidentState>("StartIncident");
            State<MomentState>("Moment");
            State<TransitionInput>("Transition");
            _lastState = null;
            Link("Transition", "Moment");
            State<MessageInputState>("EndIncidentInput", IncidentMessages.END_INCIDENT);
            Link("Moment", "EndIncidentInput");
            State<EndIncidentState>("EndIncident");
            Decision<CheckIncidentLink>("CheckNextIncident", "NextIncident", "EndIncidents");
            Link("NextIncident", "StartIncident");

            AmbitionApp.BindState<FadeOutInState>(FLOW_ID, "NextIncident");
            AmbitionApp.BindState<LoadSceneState>(FLOW_ID, "StartIncidents", SceneConsts.INCIDENT_SCENE);
        }

        private void State(string StateID)
        {
            if (!_registeredStates.Contains(StateID))
            {
                AmbitionApp.RegisterState(FLOW_ID, StateID);
                _registeredStates.Add(StateID);
            }
            Link(_lastState, StateID);
        }

        private void State<S>(string StateID, params object[] parameters) where S:UState, new()
        {
            if (!_registeredStates.Contains(StateID))
            {
                AmbitionApp.RegisterState<S>(FLOW_ID, StateID, parameters);
                _registeredStates.Add(StateID);
            }
            Link(_lastState, StateID);
        }

        private void Decision<L>(string StateID, string Yes, string No) where L : ULink, new()
        {
            State(StateID);
            if (!_registeredStates.Contains(Yes))
            {
                AmbitionApp.RegisterState(FLOW_ID, Yes);
                _registeredStates.Add(Yes);
            }
            if (!_registeredStates.Contains(No))
            {
                AmbitionApp.RegisterState(FLOW_ID, No);
                _registeredStates.Add(No);
            }
            Link<L>(StateID, Yes);
            Link(StateID, No);
            _lastState = Yes;
        }

        private void Link(string state0, string state1)
        {
            if (!string.IsNullOrEmpty(state0) && !string.IsNullOrEmpty(state1))
                AmbitionApp.RegisterLink(FLOW_ID, state0, state1);
            _lastState = state1;
        }

        private void Link<L>(string state0, string state1) where L : ULink, new()
        {
            if (!string.IsNullOrEmpty(state0) && !string.IsNullOrEmpty(state1))
                AmbitionApp.RegisterLink<L>(FLOW_ID, state0, state1);
            _lastState = state1;
        }
    }
}
