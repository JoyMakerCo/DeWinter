using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
    public class GuestRemarkResultView : GuestViewMediator
    {
        public Animator Animator;
        public Text Text;

        private PartyModel _partyModel;
        private ConversationModel _conversationModel;

        private RemarkVO _remark;
        private Dictionary<string, string> _dialogSubstitutions = new Dictionary<string, string>();

        // Use this for initialization
        void Awake()
        {
            _partyModel = AmbitionApp.GetModel<PartyModel>();
            _conversationModel = AmbitionApp.GetModel<ConversationModel>();
            AmbitionApp.Subscribe<RemarkVO>(HandleRemark);
            AmbitionApp.Subscribe<CharacterVO>(PartyMessages.GUEST_SELECTED, HandleSelected);
            AmbitionApp.Subscribe<CharacterVO>(PartyMessages.GUEST_REACTION_ALREADYCHARMED, HandleAlreadyCharmed);
            AmbitionApp.Subscribe<CharacterVO>(PartyMessages.GUEST_CHARMED, HandleCharmed);
            AmbitionApp.Subscribe<CharacterVO>(PartyMessages.GUEST_REACTION_BORED, HandleBored);
            AmbitionApp.Subscribe<CharacterVO>(PartyMessages.GUEST_LEFT, HandleLeft);
            InitGuest();
        }

        private void OnDestroy()
        {
            AmbitionApp.Unsubscribe<RemarkVO>(HandleRemark);
            AmbitionApp.Unsubscribe<CharacterVO>(PartyMessages.GUEST_SELECTED, HandleSelected);
            AmbitionApp.Unsubscribe<CharacterVO>(PartyMessages.GUEST_REACTION_ALREADYCHARMED, HandleAlreadyCharmed);
            AmbitionApp.Unsubscribe<CharacterVO>(PartyMessages.GUEST_CHARMED, HandleCharmed);
            AmbitionApp.Unsubscribe<CharacterVO>(PartyMessages.GUEST_REACTION_BORED, HandleBored);
            AmbitionApp.Unsubscribe<CharacterVO>(PartyMessages.GUEST_LEFT, HandleLeft);
        }

        protected override void HandleGuest(CharacterVO guest)
        {
            //To Do: What the fuck do I put in here?
        }

        private void HandleRemark(RemarkVO remark)
        {
            _remark = remark;
        }

        private void HandleSelected(CharacterVO guest)
        {
//            if (guest.State == GuestState.Offended) return; //Ignore guests that are gone
            string key = _key(_remark, guest);

            if (key == PartyConstants.DISLIKE && _conversationModel != null && _conversationModel.ItemEffect)
            {
                _conversationModel.ItemEffect = false;
                return;
            }

            RemarkResult result = _partyModel.RemarkResults[key];

            //These values aren't just getting established, they can also be updated, which is why we're not using Dictionary.Add()
            _dialogSubstitutions["$OPINIONAMOUNT"] = _intSubstitution(result.OpinionMax);
            _dialogSubstitutions["$REMARKAMOUNT"] = _intSubstitution(result.Remarks);
/*
            if (_remark != null && guest != null && Guest == guest)
            {
                if (_remark.Interest == Guest.Like)
                {
                    if(guest.State != GuestState.Charmed) //This stops the action from overriding the charmed message put forward via "Already Charmed"
                    {
                        Text.text = _setStrings("conversations.remarks_results.opinion_change", _dialogSubstitutions);
                        Text.text += "\n" + _setStrings("conversations.remarks_results.remarks_change", _dialogSubstitutions);
                        Animator.SetTrigger("Positive");
                    }
                }
                else if (_remark.Interest == Guest.Dislike)
                {
                    if (guest.State == GuestState.Bored) //While a disliked remark can make a guest bored, that's not always the case, so we have to check
                    {
                        Text.text = _setStrings("conversations.remarks_results.opinion_change", _dialogSubstitutions);
                        Text.text += "\n" + _setStrings("conversations.remarks_results.remarks_change", _dialogSubstitutions);
                        Text.text += "\n" + localizationModel.GetString("conversation.remarks_results.bored");
                        Animator.SetTrigger("Negative");
                    } else if (guest.State == GuestState.PutOff)
                    {
                        Text.text = _setStrings("conversations.remarks_results.opinion_change", _dialogSubstitutions);
                        Text.text += "\n" + _setStrings("conversations.remarks_results.remarks_change", _dialogSubstitutions);
                        Text.text += "\n" + localizationModel.GetString("conversation.remarks_results.put_off_status");
                        Animator.SetTrigger("Negative");
                    } else if (guest.State == GuestState.Charmed)
                    {
                        Text.text += _setStrings("conversations.remarks_results.remarks_change", _dialogSubstitutions);
                        Animator.SetTrigger("Negative");
                    }
                    else
                    {
                        Text.text = _setStrings("conversations.remarks_results.opinion_change", _dialogSubstitutions);
                        Text.text += "\n" + _setStrings("conversations.remarks_results.remarks_change", _dialogSubstitutions);
                        Animator.SetTrigger("Negative");
                    }
                }
                else
                {
                    if (guest.State != GuestState.Charmed) //This stops the action from overriding the charmed message put forward via "Already Charmed"
                    {
                        Text.text = _setStrings("conversations.remarks_results.opinion_change", _dialogSubstitutions);
                        Animator.SetTrigger("Neutral");
                    }
                }
            }
            */
        }

        private void HandleAlreadyCharmed(CharacterVO guest)
        {
            if (guest != null && Guest == guest)
            {
                Text.text = _setStrings("conversations.remarks_results.charmed_status", _dialogSubstitutions);
                Animator.SetTrigger("Charmed");
            }
        }

        private void HandleCharmed(CharacterVO guest)
        {
            if (guest != null && Guest == guest)
            {
                RemarkResult result = _partyModel.RemarkResults[_key(_remark, guest)];

                //These values aren't just getting established, they can also be updated, which is why we're not using Dictionary.Add()
                _dialogSubstitutions["$OPINIONAMOUNT"] = _intSubstitution(result.OpinionMax);
                _dialogSubstitutions["$REMARKAMOUNT"] = _intSubstitution(_partyModel.CharmedRemarkBonus);

                Text.text = _setStrings("conversations.remarks_results.opinion_change", _dialogSubstitutions);
                Text.text += "\n" + _setStrings("conversations.remarks_results.remarks_change", _dialogSubstitutions);
                Text.text += "\n" + _setStrings("conversations.remarks_results.charmed_status", _dialogSubstitutions);
                Animator.SetTrigger("Charmed");
            }
        }

        private void HandleBored(CharacterVO guest)
        {
            if (guest != null && Guest == guest)
            {
                _dialogSubstitutions["$OPINIONAMOUNT"] = _intSubstitution(_partyModel.BoredomPenalty);
                _dialogSubstitutions["$REMARKAMOUNT"] = _intSubstitution(_partyModel.BoredomRemarkPenalty);
/*                if (guest.State == GuestState.PutOff)
                {
                    Text.text = _setStrings("conversations.remarks_results.opinion_change", _dialogSubstitutions);
                    Text.text += "\n" + _setStrings("conversations.remarks_results.remarks_change", _dialogSubstitutions);
                    Text.text += "\n" + _setStrings("conversations.remarks_results.put_off_status", _dialogSubstitutions);
                    Animator.SetTrigger("Negative");
                } else if (guest.State == GuestState.Offended)
                {
                    HandleLeft(guest);
                }
                else
                {
                    Text.text = _setStrings("conversations.remarks_results.opinion_change", _dialogSubstitutions);
                    Text.text += "\n" + _setStrings("conversations.remarks_results.remarks_change", _dialogSubstitutions);
                    Text.text += "\n" + _setStrings("conversations.remarks_results.bored_status", _dialogSubstitutions);
                    Animator.SetTrigger("Bored");
                }
*/            }
        }

        //Used when a guest leaves the conversation (not 'left' the direction)
        private void HandleLeft(CharacterVO guest)
        {
            if (guest != null && Guest == guest)
            {
                _dialogSubstitutions["$REMARKAMOUNT"] = _intSubstitution(_partyModel.OffendedRemarkPenalty);
                Text.text = _setStrings("conversations.remarks_results.remarks_change", _dialogSubstitutions);
                Text.text += "\n" + _setStrings("conversations.remarks_results.offended_status", _dialogSubstitutions);
                Animator.SetTrigger("Negative");
            }
        }

        private string _key(RemarkVO remark, CharacterVO guest)
        {
            return null;
/*            return remark.Interest == guest.Like
                        ? PartyConstants.LIKE
                        : _remark.Interest == guest.Dislike
                        ? PartyConstants.DISLIKE
                        : PartyConstants.NEUTRAL;
*/                      
        }

        private string _intSubstitution(int value)
        {
            if(value >= 0)
            {
                return "+" + value.ToString();
            } else
            {
                return value.ToString();
            }
        }

        private string _setStrings(string dialogString, Dictionary<string, string> dictionary)
        {
            string str = AmbitionApp.GetString(dialogString, dictionary);
            if (str != null && Text != null) return str;
            else return AmbitionApp.Localize(dialogString);
        }
    }
}
