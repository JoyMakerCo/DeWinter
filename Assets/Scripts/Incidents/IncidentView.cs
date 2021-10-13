using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

namespace Ambition
{
    public class IncidentView : SceneView, ISubmitHandler, IPointerClickHandler, IAnalogInputHandler
    {
        private const string ACTIVE = "Active"; //corresponds with Animation Parameter
        private float BG_TIMER = .25f;

        public Text descriptionText;
        public Text SpeakerName;
        public AvatarCollection Avatars;
        public AvatarView Character1;
        public AvatarView Character2;
        public IncidentButton[] _buttons;
        public CanvasGroup[] _tooltips;

        [SerializeField] Image _hitTarget;
        [SerializeField] Animator _animator;
        [SerializeField] FMODEvent AdvanceSound;
        [SerializeField] GameObject Ellipse;
        [SerializeField] Image _background;

        private IncidentVO _incident;
        private List<TransitionVO> _transitions = new List<TransitionVO>();
        private int _index = -1;
        private bool _showTooltip = false;


        public void HandleInput(Vector2 [] sticks)
        {
            int count = _transitions.Count;
            int index = -1;
            bool showTooltip = _showTooltip;
            if (count > 0)
            {
                float x = 0, y = 0;
                foreach (Vector2 stick in sticks)
                {
                    x += stick.x;
                    y += stick.y;
                }
                if (x != 0 || y != 0)
                {
                    showTooltip = x > 0;
                    if (y > 1) y = 1;
                    else if (y < -1) y = -1;
                    index = count > 1
                        ? (int)Math.Round((-y + 1f) / (_buttons.Length + 1 - count))
                        : count < 1
                        ? -1
                        : 0;
                }
            }
            if (index != _index || showTooltip != _showTooltip)
            {
                _index = index;
                _showTooltip = showTooltip;
                EventSystem.current.SetSelectedGameObject(_index < 0 ? null : _buttons[_index].gameObject);
                for (int i = count - 1; i >= 0; --i)
                {
                    _tooltips[i].alpha = (_showTooltip && i == _index) ? 1 : 0;
                }
            }
        }

        public void Cancel()
        {
#if UNITY_STANDALONE
            AmbitionApp.OpenDialog(DialogConsts.GAME_MENU);
#endif
        }

        public void Submit()
        {
            if (_transitions == null) return;
            if (_transitions.Count <= 1) HandleTransition(-1);
            else if (_index >= 0) HandleTransition(_index);
        }

        public void OnPointerClick(PointerEventData eventData) => HandleTransition(-1);

        public void HandleTransition(int transitionIndex)
        {
            _index = -1;
            if (_transitions == null) return;
            if (_transitions.Count == 0)
            {
                AmbitionApp.SendMessage<TransitionVO>(IncidentMessages.TRANSITION, null);
            }
            else
            {
                TransitionVO trans = _transitions.Count == 1
                    ? _transitions[0]
                    : transitionIndex >= 0 && transitionIndex < _transitions.Count
                    ? _transitions[transitionIndex]
                    : null;
                if (trans != null)
                {
                    AmbitionApp.SendMessage(IncidentMessages.TRANSITION, trans);
                    AmbitionApp.SendMessage(AudioMessages.PLAY, AdvanceSound);
                }
            }
        }

        void Awake()
        {
            AmbitionApp.Subscribe<MomentVO>(HandleMoment);
            AmbitionApp.Subscribe<IncidentVO>(IncidentMessages.START_INCIDENT, HandleIncident);
            AmbitionApp.Subscribe<string>(IncidentMessages.END_INCIDENT, HandleEndIncident);
            AmbitionApp.Subscribe<TransitionVO[]>(HandleTransitions);
        }

        void Start()
        {
            IncidentModel model = AmbitionApp.GetModel<IncidentModel>();
            _incident = model.Incident;
            _background.sprite = model.RestoreSavedState();
            HandleMoment(model.Moment);

            if (_incident != null)
            {
                int index = Array.IndexOf(_incident.Nodes, model.Moment);
                TransitionVO[] transitions = _incident.GetLinks(index);
                HandleTransitions(transitions);
            }
        }

        void OnDestroy()
        {
            AmbitionApp.Unsubscribe<MomentVO>(HandleMoment);
            AmbitionApp.Unsubscribe<IncidentVO>(IncidentMessages.START_INCIDENT, HandleIncident);
            AmbitionApp.Unsubscribe<string>(IncidentMessages.END_INCIDENT, HandleEndIncident);
            AmbitionApp.Unsubscribe<TransitionVO[]>(HandleTransitions);
        }

        private void HandleIncident(IncidentVO incident) => _incident = incident;

        private void HandleEndIncident(string incidentID)
        {
            if (incidentID == _incident?.ID)
            {
                _incident = null;
                _animator.SetBool(ACTIVE, false);
                AmbitionApp.SendMessage(GameMessages.HIDE_HEADER);
            }
        }

        private void HandleMoment(MomentVO moment)
        {
            if (_incident == null) AmbitionApp.SendMessage(GameMessages.HIDE_HEADER);
            else
            {
                AmbitionApp.SendMessage(GameMessages.SHOW_HEADER, _incident.ID + ".name");
                if (_incident?.Nodes != null && moment != null)
                {
                    int index = Array.IndexOf(_incident.Nodes, moment);
                    if (index >= 0)
                    {
                        descriptionText.text = AmbitionApp.Localize(_incident.ID + ".node." + index.ToString());
                        if (moment.Background != null && !moment.Background.Equals(_background.sprite))
                        {
                            if (index > 0)
                            {
                                StopAllCoroutines();
                                StartCoroutine(SwapBG(moment.Background));
                            }
                            else _background.sprite = moment.Background;
                        }
                        Character1.Avatar = Avatars.GetAvatar(moment.Character1.AvatarID);
                        Character1.Pose = moment.Character1.Pose;
                        Character1.Play(moment.Character1.Motion);
                        Character2.Avatar = Avatars.GetAvatar(moment.Character2.AvatarID);
                        Character2.Pose = moment.Character2.Pose;
                        Character2.Play(moment.Character2.Motion);

                        AmbitionApp.SendMessage(AudioMessages.PLAY, moment.OneShotSFX);
                        if (moment.Music.Name.Length > 0)
                            AmbitionApp.SendMessage(AudioMessages.PLAY_MUSIC, moment.Music);
                        if (moment.AmbientSFX.Name.Length > 0)
                            AmbitionApp.SendMessage(AudioMessages.PLAY_AMBIENT, moment.AmbientSFX);

                        SpeakerName.enabled = (moment != null && moment.Speaker != SpeakerType.None);
                        switch (moment.Speaker)
                        {
                            case SpeakerType.Player:
                                SpeakerName.text = AmbitionApp.GetModel<GameModel>().PlayerName;
                                break;
                            case SpeakerType.Character1:
                                SpeakerName.text = moment.Character1.Name;
                                break;
                            case SpeakerType.Character2:
                                SpeakerName.text = moment.Character2.Name;
                                break;
                        }
                    }
                }
            }
        }

        void HandleTransitions(TransitionVO[] transitions)
        {
            int index = -1;
            int max;
            string loc = null;
            bool show;

            _transitions.Clear();
            foreach (TransitionVO trans in transitions)
            {
                if (AmbitionApp.CheckRequirements(trans.Requirements))
                {
                    if (!trans.xor)
                    {
                        _transitions.Add(trans);
                    }
                    else if (index < 0)
                    {
                        index = _transitions.Count;
                        _transitions.Add(trans);
                    }
                    else if (_transitions[index].Requirements.Length == 0)
                    {
                        _transitions[index] = trans;
                    }    
                }
            }

            max = _transitions.Count;

            for (int i = _buttons.Length-1; i >= 0; --i)
            {
                show = i < max;
                if (show)
                {
                    index = Array.IndexOf(_incident.LinkData, _transitions[i]);
                    loc = AmbitionApp.Localize(_incident.ID + ".link." + index.ToString());
                    show = !string.IsNullOrEmpty(loc);
                }
                _buttons[i].gameObject.SetActive(show);
                if (show) _buttons[i].SetTransition(_transitions[i], loc);
            }

            _hitTarget.raycastTarget = _transitions.Count < 2;
            Ellipse.SetActive(_transitions.Count < 2);
            _animator.SetBool(ACTIVE, true);
        }

        private IEnumerator SwapBG(Sprite target)
        {
            float mult = 1f / BG_TIMER;
            Color startColor = _background.color;
            for (float t= BG_TIMER; t>0; t -= Time.deltaTime)
            {
                _background.color = Color.Lerp(Color.black, startColor, t * mult);
                yield return null;
            }
            _background.sprite = target;
            for (float t = 0f; t < BG_TIMER; t += Time.deltaTime)
            {
                _background.color = Color.Lerp(Color.black, Color.white, t * mult);
                yield return null;
            }
            _background.color = Color.white;
        }
    }
}
