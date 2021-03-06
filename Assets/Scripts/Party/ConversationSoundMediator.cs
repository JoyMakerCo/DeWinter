﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using FMOD.Studio;

namespace Ambition
{
    public class ConversationSoundMediator : MonoBehaviour
    {
        public GuestSFXCollection GuestCollection;
        public PartyMusicCollection MusicCollection;
        PartyModel partyModel = AmbitionApp.GetModel<PartyModel>();
    }
}
