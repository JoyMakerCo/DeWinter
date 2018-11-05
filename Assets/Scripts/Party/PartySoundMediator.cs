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
        public AmbientPartySFXCollection AmbientSFXCollection;
        public PartyMusicCollection MusicCollection;
        public GuestSFXCollection GuestBarkCollection;

        MapModel model = AmbitionApp.GetModel<MapModel>();
        PartyModel partyModel = AmbitionApp.GetModel<PartyModel>();

        private FMODEvent _currentConversationMusic;
        private bool _partyStarted = false;

        void Awake()
        {
            AmbitionApp.Subscribe(PartyMessages.SHOW_ROOM, ConversationSoundStart);
            AmbitionApp.Subscribe(PartyMessages.END_CONVERSATION, ConversationVictorySound);
            AmbitionApp.Subscribe(PartyMessages.FLEE_CONVERSATION, ConversationDefeatSound);
            AmbitionApp.Subscribe<GuestVO>(PartyMessages.GUEST_REACTION_POSITIVE, GuestPositiveReaction);
            AmbitionApp.Subscribe<GuestVO>(PartyMessages.GUEST_REACTION_NEUTRAL, GuestNeutralReaction);
            AmbitionApp.Subscribe<GuestVO>(PartyMessages.GUEST_REACTION_NEGATIVE, GuestNegativeReaction);
            AmbitionApp.Subscribe<GuestVO>(PartyMessages.GUEST_CHARMED, GuestCharmedReaction);
            AmbitionApp.Subscribe<GuestVO>(PartyMessages.GUEST_OFFENDED, GuestPutOffReaction);
            AmbitionApp.Subscribe<GuestVO>(PartyMessages.GUEST_REACTION_BORED, GuestBoredReaction);
            AmbitionApp.Subscribe(PartyMessages.LEAVE_PARTY, LeaveParty);
            AmbitionApp.Subscribe(PartyMessages.SHOW_MAP, HandleShowMap);
            AmbitionApp.Subscribe<RoomVO>(MapMessage.GO_TO_ROOM, HandleRoom);
        }

        void OnDestroy()
        {
            AmbitionApp.Unsubscribe(PartyMessages.SHOW_ROOM, ConversationSoundStart);
            AmbitionApp.Unsubscribe(PartyMessages.END_CONVERSATION, ConversationVictorySound);
            AmbitionApp.Unsubscribe(PartyMessages.FLEE_CONVERSATION, ConversationDefeatSound);
            AmbitionApp.Unsubscribe<GuestVO>(PartyMessages.GUEST_REACTION_POSITIVE, GuestPositiveReaction);
            AmbitionApp.Unsubscribe<GuestVO>(PartyMessages.GUEST_REACTION_NEUTRAL, GuestNeutralReaction);
            AmbitionApp.Unsubscribe<GuestVO>(PartyMessages.GUEST_REACTION_NEGATIVE, GuestNegativeReaction);
            AmbitionApp.Unsubscribe<GuestVO>(PartyMessages.GUEST_CHARMED, GuestCharmedReaction);
            AmbitionApp.Unsubscribe<GuestVO>(PartyMessages.GUEST_OFFENDED, GuestPutOffReaction);
            AmbitionApp.Unsubscribe<GuestVO>(PartyMessages.GUEST_REACTION_BORED, GuestBoredReaction);
            AmbitionApp.Unsubscribe(PartyMessages.SHOW_MAP, HandleShowMap);
            AmbitionApp.Unsubscribe<RoomVO>(MapMessage.GO_TO_ROOM, HandleRoom);
        }

        //This is for room SFX, necessary if we have indoor and outdoor rooms that the player is traversing on the map
        private void HandleRoom(RoomVO room)
        {
            AmbitionApp.SendMessage(AudioMessages.PLAY_AMBIENTSFX, AmbientSFXCollection.GetFMODEvent(room.Indoors, (int)partyModel.Party.Importance));
        }

        private void HandleShowMap()
        {
            //To Do: an If Else for queueing the track
            if (!_partyStarted)
            {
                AmbitionApp.SendMessage(AudioMessages.PLAY_MUSIC, MusicCollection.GetFMODEvent(true, partyModel.Party.Faction));
                _partyStarted = true;
            } else
            {
                //We have to queue the next track up, instead of an instant switch, or the map music takes over before the victory chime of the conversation track
                AmbitionApp.SendMessage(AudioMessages.QUEUE_MUSIC, MusicCollection.GetFMODEvent(true, partyModel.Party.Faction));
            }
        }

        //This is is for conversation music and SFX
        public void ConversationSoundStart()
        {
            _currentConversationMusic = MusicCollection.GetFMODEvent(false, partyModel.Party.Faction);
            //Set the parameters to zero, because sometimes they get stuck in the victory or lose state after the instance is completed, which fucks it up the next time that track is used
            SetParam(_currentConversationMusic, "WIN", 0);
            SetParam(_currentConversationMusic, "LOSE", 0);
            AmbitionApp.SendMessage(AudioMessages.PLAY_MUSIC, _currentConversationMusic);
            AmbitionApp.SendMessage(AudioMessages.PLAY_AMBIENTSFX, AmbientSFXCollection.GetFMODEvent(model.Room.Indoors, (int)partyModel.Party.Importance));
        }

        public void ConversationVictorySound()
        {
            SetParam(_currentConversationMusic, "WIN", 1);
            AmbitionApp.SendMessage(AudioMessages.PLAY_MUSIC, _currentConversationMusic);

        }

        public void ConversationDefeatSound()
        {
            SetParam(_currentConversationMusic, "LOSE", 1);
            AmbitionApp.SendMessage(AudioMessages.PLAY_MUSIC, _currentConversationMusic);
        }

        public void GuestPositiveReaction(GuestVO guest)
        {
            FMODEvent selectedBark = GuestBarkCollection.GetFMODEvent(guest.Gender, "positive"); 
            AmbitionApp.SendMessage(AudioMessages.PLAY_ONESHOTSFX, selectedBark);
        }

        public void GuestNeutralReaction(GuestVO guest)
        {
            FMODEvent selectedBark = GuestBarkCollection.GetFMODEvent(guest.Gender, "neutral");
            AmbitionApp.SendMessage(AudioMessages.PLAY_ONESHOTSFX, selectedBark);
        }

        public void GuestNegativeReaction(GuestVO guest)
        {
            FMODEvent selectedBark = GuestBarkCollection.GetFMODEvent(guest.Gender, "negative");
            AmbitionApp.SendMessage(AudioMessages.PLAY_ONESHOTSFX, selectedBark);
        }

        public void GuestCharmedReaction(GuestVO guest)
        {
            FMODEvent selectedBark = GuestBarkCollection.GetFMODEvent(guest.Gender, "charmed");
            AmbitionApp.SendMessage(AudioMessages.PLAY_ONESHOTSFX, selectedBark);
        }

        public void GuestPutOffReaction(GuestVO guest)
        {
            FMODEvent selectedBark = GuestBarkCollection.GetFMODEvent(guest.Gender, "negative"); //Still waiting on a Put Off Sound Effect
            AmbitionApp.SendMessage(AudioMessages.PLAY_ONESHOTSFX, selectedBark);
        }

        public void GuestBoredReaction(GuestVO guest)
        {
            FMODEvent selectedBark = GuestBarkCollection.GetFMODEvent(guest.Gender, "bored");
            AmbitionApp.SendMessage(AudioMessages.PLAY_ONESHOTSFX, selectedBark);
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
