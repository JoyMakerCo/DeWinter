using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using FMOD.Studio;

namespace Ambition
{
    public class PartySoundMediator : MonoBehaviour
    {
        private const float DELAY_TIME = 3.00f;

        public AmbientPartySFXCollection AmbientSFXCollection;
        public PartyMusicCollection MusicCollection;
        public GuestSFXCollection GuestBarkCollection;

        MapModel model = AmbitionApp.GetModel<MapModel>();
        PartyModel partyModel = AmbitionApp.GetModel<PartyModel>();

        private FMODEvent _partyMusic;
        private IEnumerator _coroutine;
        private bool _partyStarted = false;

        void Awake()
        {
            AmbitionApp.Subscribe(PartyMessages.SHOW_ROOM, ConversationMusicStart);
            AmbitionApp.Subscribe(PartyMessages.END_CONVERSATION, ConversationVictorySound);
            AmbitionApp.Subscribe(PartyMessages.FLEE_CONVERSATION, ConversationDefeatSound);
            AmbitionApp.Subscribe<CharacterVO>(PartyMessages.GUEST_REACTION_POSITIVE, GuestPositiveReaction);
            AmbitionApp.Subscribe<CharacterVO>(PartyMessages.GUEST_REACTION_NEUTRAL, GuestNeutralReaction);
            AmbitionApp.Subscribe<CharacterVO>(PartyMessages.GUEST_REACTION_NEGATIVE, GuestNegativeReaction);
            AmbitionApp.Subscribe<CharacterVO>(PartyMessages.GUEST_CHARMED, GuestCharmedReaction);
            AmbitionApp.Subscribe<CharacterVO>(PartyMessages.GUEST_OFFENDED, GuestPutOffReaction);
            AmbitionApp.Subscribe<CharacterVO>(PartyMessages.GUEST_REACTION_BORED, GuestBoredReaction);
            AmbitionApp.Subscribe(PartyMessages.END_PARTY, LeaveParty);
            AmbitionApp.Subscribe(PartyMessages.SHOW_MAP, HandleShowMap);
            AmbitionApp.Subscribe<RoomVO>(MapMessage.GO_TO_ROOM, HandleRoomSFX);
            _partyMusic = MusicCollection.GetFMODEvent(partyModel.Party.Faction.ToString());
        }

        void OnDestroy()
        {
            AmbitionApp.Unsubscribe(PartyMessages.SHOW_ROOM, ConversationMusicStart);
            AmbitionApp.Unsubscribe(PartyMessages.END_CONVERSATION, ConversationVictorySound);
            AmbitionApp.Unsubscribe(PartyMessages.FLEE_CONVERSATION, ConversationDefeatSound);
            AmbitionApp.Unsubscribe<CharacterVO>(PartyMessages.GUEST_REACTION_POSITIVE, GuestPositiveReaction);
            AmbitionApp.Unsubscribe<CharacterVO>(PartyMessages.GUEST_REACTION_NEUTRAL, GuestNeutralReaction);
            AmbitionApp.Unsubscribe<CharacterVO>(PartyMessages.GUEST_REACTION_NEGATIVE, GuestNegativeReaction);
            AmbitionApp.Unsubscribe<CharacterVO>(PartyMessages.GUEST_CHARMED, GuestCharmedReaction);
            AmbitionApp.Unsubscribe<CharacterVO>(PartyMessages.GUEST_OFFENDED, GuestPutOffReaction);
            AmbitionApp.Unsubscribe<CharacterVO>(PartyMessages.GUEST_REACTION_BORED, GuestBoredReaction);
            AmbitionApp.Unsubscribe(PartyMessages.SHOW_MAP, HandleShowMap);
            AmbitionApp.Unsubscribe<RoomVO>(MapMessage.GO_TO_ROOM, HandleRoomSFX);
        }

        //This is for room SFX, necessary if we have indoor and outdoor rooms that the player is traversing on the map
        private void HandleRoomSFX(RoomVO room)
        {
            AmbitionApp.SendMessage(AudioMessages.PLAY_AMBIENT, AmbientSFXCollection.GetFMODEvent(/*room.Indoors*/ true, (int)partyModel.Party.Size));
        }

        private void HandleShowMap()
        {
            if (!_partyStarted)
            {
                SetParam(_partyMusic, "Conversation Start", 0);
                SetParam(_partyMusic, "WIN", 0);
                SetParam(_partyMusic, "LOSE", 0);
                AmbitionApp.SendMessage(AudioMessages.PLAY_MUSIC, _partyMusic);
                _partyStarted = true;
            } 
        }

        //This is is for conversation music, sfx is handled via the HandleRoomSFX method
        public void ConversationMusicStart()
        {
            if(_coroutine != null) StopCoroutine(_coroutine);
            //Set the parameters for the Music
            SetParam(_partyMusic, "Conversation Start", 1);
            SetParam(_partyMusic, "WIN", 0);
            SetParam(_partyMusic, "LOSE", 0);
            AmbitionApp.SendMessage(AudioMessages.PLAY_MUSIC, _partyMusic);
        }

        public void ConversationVictorySound()
        {
            SetParam(_partyMusic, "WIN", 1);
            AmbitionApp.SendMessage(AudioMessages.PLAY_MUSIC, _partyMusic);
            _coroutine = DelayedPartyMapMusic(DELAY_TIME);
            StartCoroutine(_coroutine); //This has to be handled in a coroutine or the check brings the whole game to a screeching halt
        }

        public void ConversationDefeatSound()
        {
            SetParam(_partyMusic, "LOSE", 1);
            AmbitionApp.SendMessage(AudioMessages.PLAY_MUSIC, _partyMusic);
            _coroutine = DelayedPartyMapMusic(DELAY_TIME);
            StartCoroutine(_coroutine); //This has to be handled in a coroutine or the check brings the whole game to a screeching halt
        }

        private IEnumerator DelayedPartyMapMusic(float duration)
        {
            //Internal Timer
            float normalizedTime = 0;
            while (normalizedTime <= 1f)
            {
                normalizedTime += Time.deltaTime / duration;
                yield return null;
            }
            //Actually handle the FMOD Event
            SetParam(_partyMusic, "Conversation Start", 0);
            SetParam(_partyMusic, "WIN", 0);
            SetParam(_partyMusic, "LOSE", 0);
            AmbitionApp.SendMessage(AudioMessages.PLAY_MUSIC, _partyMusic);
        }

        public void GuestPositiveReaction(CharacterVO guest)
        {
            FMODEvent selectedBark = GuestBarkCollection.GetFMODEvent(guest.Gender, "positive"); 
            AmbitionApp.SendMessage(AudioMessages.PLAY, selectedBark);
        }

        public void GuestNeutralReaction(CharacterVO guest)
        {
            FMODEvent selectedBark = GuestBarkCollection.GetFMODEvent(guest.Gender, "neutral");
            AmbitionApp.SendMessage(AudioMessages.PLAY, selectedBark);
        }

        public void GuestNegativeReaction(CharacterVO guest)
        {
            FMODEvent selectedBark = GuestBarkCollection.GetFMODEvent(guest.Gender, "negative");
            AmbitionApp.SendMessage(AudioMessages.PLAY, selectedBark);
        }

        public void GuestCharmedReaction(CharacterVO guest)
        {
            FMODEvent selectedBark = GuestBarkCollection.GetFMODEvent(guest.Gender, "charmed");
            AmbitionApp.SendMessage(AudioMessages.PLAY, selectedBark);
        }

        public void GuestPutOffReaction(CharacterVO guest)
        {
            FMODEvent selectedBark = GuestBarkCollection.GetFMODEvent(guest.Gender, "negative"); //Still waiting on a Put Off Sound Effect
            AmbitionApp.SendMessage(AudioMessages.PLAY, selectedBark);
        }

        public void GuestBoredReaction(CharacterVO guest)
        {
            FMODEvent selectedBark = GuestBarkCollection.GetFMODEvent(guest.Gender, "bored");
            AmbitionApp.SendMessage(AudioMessages.PLAY, selectedBark);
        }

        public void LeaveParty()
        {
            AmbitionApp.SendMessage(AudioMessages.STOP_MUSIC);
        }

        private void SetParam(FMODEvent e, string id, int value)
        {
            if (e.Parameters != null)
            {
                int index = Array.FindIndex(e.Parameters, p => p.Name == id);
                if (index >= 0) e.Parameters[index].Value = value;
            }
        }
    }
}
