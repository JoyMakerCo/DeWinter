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

        void Awake()
        {
            AmbitionApp.Subscribe(PartyMessages.SHOW_ROOM, ConversationSoundStart);
            AmbitionApp.Subscribe(PartyMessages.END_CONVERSATION, ConversationVictorySound);
            AmbitionApp.Subscribe(PartyMessages.FLEE_CONVERSATION, ConversationDefeatSound);
            AmbitionApp.Subscribe<GuestVO>(PartyMessages.GUEST_REACTION_POSITIVE, GuestPositiveReaction);
            AmbitionApp.Subscribe<GuestVO>(PartyMessages.GUEST_REACTION_NEUTRAL, GuestNeutralReaction);
            AmbitionApp.Subscribe<GuestVO>(PartyMessages.GUEST_REACTION_NEGATIVE, GuestNegativeReaction);
            AmbitionApp.Subscribe<GuestVO>(PartyMessages.GUEST_REACTION_CHARMED, GuestCharmedReaction);
            AmbitionApp.Subscribe<GuestVO>(PartyMessages.GUEST_REACTION_PUTOFF, GuestPutOffReaction);
            AmbitionApp.Subscribe<GuestVO>(PartyMessages.GUEST_REACTION_BORED, GuestBoredReaction);
            AmbitionApp.Subscribe(PartyMessages.LEAVE_PARTY, LeaveParty);
            StartPartyMapSound(); // Because it starts on the map by default, it seems like Show Map isn't called at the very beginning, so we have to call it on Awake()
        }

        void OnDestroy()
        {
            AmbitionApp.Unsubscribe(PartyMessages.SHOW_ROOM, ConversationSoundStart);
            AmbitionApp.Unsubscribe(PartyMessages.END_CONVERSATION, ConversationVictorySound);
            AmbitionApp.Unsubscribe(PartyMessages.FLEE_CONVERSATION, ConversationDefeatSound);
            AmbitionApp.Unsubscribe<GuestVO>(PartyMessages.GUEST_REACTION_POSITIVE, GuestPositiveReaction);
            AmbitionApp.Unsubscribe<GuestVO>(PartyMessages.GUEST_REACTION_NEUTRAL, GuestNeutralReaction);
            AmbitionApp.Unsubscribe<GuestVO>(PartyMessages.GUEST_REACTION_NEGATIVE, GuestNegativeReaction);
            AmbitionApp.Unsubscribe<GuestVO>(PartyMessages.GUEST_REACTION_CHARMED, GuestCharmedReaction);
            AmbitionApp.Unsubscribe<GuestVO>(PartyMessages.GUEST_REACTION_PUTOFF, GuestPutOffReaction);
            AmbitionApp.Unsubscribe<GuestVO>(PartyMessages.GUEST_REACTION_BORED, GuestBoredReaction);
        }

        public void StartPartyMapSound()
        {
            AmbitionApp.SendMessage(AudioMessages.PLAY_MUSIC, MusicCollection.GetFMODEvent(true, partyModel.Party.Faction));
            AmbitionApp.SendMessage(AudioMessages.PLAY_AMBIENTSFX, AmbientSFXCollection.GetFMODEvent(model.Room.Indoors, (int)partyModel.Party.Importance));
        }

        public void ConversationSoundStart()
        {
            _currentConversationMusic = MusicCollection.GetFMODEvent(false, partyModel.Party.Faction);
            //Set the parameters to zero, because sometimes they get stuck in the victory or lose state after the instance is completed, which fucks it up the next time that track is used
            _currentConversationMusic.Parameters[_currentConversationMusic.GetParameterIndexByName("WIN")].Value = 0;
            _currentConversationMusic.Parameters[_currentConversationMusic.GetParameterIndexByName("LOSE")].Value = 0;
            AmbitionApp.SendMessage(AudioMessages.PLAY_MUSIC, _currentConversationMusic);
            AmbitionApp.SendMessage(AudioMessages.PLAY_AMBIENTSFX, AmbientSFXCollection.GetFMODEvent(model.Room.Indoors, (int)partyModel.Party.Importance));
        }

        public void ConversationVictorySound()
        {
            FMODEvent victoryEvent = _currentConversationMusic;
            victoryEvent.Parameters[victoryEvent.GetParameterIndexByName("WIN")].Value = 1; //We're already in the right song, now we just need to make it end appropriately
            AmbitionApp.SendMessage(AudioMessages.PLAY_MUSIC, victoryEvent);
            //We have to queue the next track up, instead of an instant switch, or the map music takes over before the victory chime of the conversation track
            AmbitionApp.SendMessage(AudioMessages.QUEUE_MUSIC, MusicCollection.GetFMODEvent(true, partyModel.Party.Faction));
        }

        public void ConversationDefeatSound()
        {
            FMODEvent defeatEvent = _currentConversationMusic;
            defeatEvent.Parameters[defeatEvent.GetParameterIndexByName("LOSE")].Value = 1; //We're already in the right song, now we just need to make it end appropriately
            AmbitionApp.SendMessage(AudioMessages.PLAY_MUSIC, defeatEvent);
            //We have to queue the next track up, instead of an instant switch, or the map music takes over before the defeat chime of the conversation track
            AmbitionApp.SendMessage(AudioMessages.QUEUE_MUSIC, MusicCollection.GetFMODEvent(true, partyModel.Party.Faction));
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
    }
}
