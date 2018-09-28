/*
            rewardString = AmbitionApp.GetString("party.name.") + " is over. Here's what you got:";
        } else
        {
			rewardString = "Oh no! It appears that you had far too much to drink at the " + _model.Party.Name + ". You might not remember this but...";
			switch (_model.Party.blackOutEffect)
            {
                case "Reputation Loss":
				rewardString += "\nYou said some pretty embarassing things, which lost you " + _model.Party.blackOutEffectAmount.ToString("N0") + " Reputation.";
                    break;
                case "Faction Reputation Loss":
				rewardString += "\nYou said some pretty rude things about your host, which lost you " + _model.Party.blackOutEffectAmount.ToString("N0") + " Reputation with the " + _model.Party.Faction + ".";
                    break;
                case "Outfit Novelty Loss":
				rewardString += "\nYou spent most of the night talking to everyone about your Outfit. It may be safe to say that some were rather tired of hearing about it. Your " + outfit.Name + " has lost " + _model.Party.blackOutEffectAmount.ToString("N0") + " Novelty";
                    break;
                case "Outfit Ruined":
                    rewardString += "\nYou crashed into a waiter, who spilled wine all over you. Your " + outfit.Name + " has been ruined!";
                    break;
                case "Accessory Ruined":
                    rewardString += "\nYou dropped your " + inventory.Equipped[ItemConsts.ACCESSORY].Name + " in a fountain and nobody was able to find it. I'm afraid it was completely lost!";
                    break;
                case "Livre Lost":
                    rewardString += "\nWhen you got home last night, you didn't have your coin purse with you and you couldn't remember where you left it. You've lost " + Mathf.Abs(_model.Party.blackOutEffectAmount) + " Livres.";
                    break;
                case "New Enemy":
                    rewardString += "\nApparently someone bumped into you and you tore into them... verbally, that is. They were quite embarassed by the whole affair. It appears you have a new Enemy.";
                    break;
                case "Forgot All Gossip":
                    rewardString += "\nWhen you got home last night, you said that you had some Gossip you didn't want to forget. I grabbed something to write it all down with, but when I came back you were already... asleep. You seem to have forgotten all of the Gossip you acquired last night.";
                    break;
                case "No Effect":
                    rewardString += "\nYou had to leave the party early. Which is alright, at least you didn't do anything embarassing.";
                    break;
                case "Reputation Gain":
                    rewardString += "\nYou kept making all these ridiculous jokes, apparently you had everyone in stitches. You've gained " + _model.Party.blackOutEffectAmount.ToString("N0") + " Reputation.";
                    break;
                case "Faction Reputation Gain":
				rewardString += "\nYou said some absolutely hillarious things to the host. You two seemed to get along quite well. You've gained " + _model.Party.blackOutEffectAmount.ToString("N0") + " Reputation with the " + _model.Party.Faction + ".";
                    break;
                case "Livre Gained":
                    rewardString += "\nWhen you got home last night, you kept excitedly yelling about some kind of wager... I don't remember the details but I do know it involved a hot air balloon. You've gained " + _model.Party.blackOutEffectAmount.ToString("N0") + " Livres.";
                    break;
                case "New Gossip":
                    rewardString += "\nWhen you got home last night, you couldn't stop laughing about something you heard at the party. You even insisted that I write it down so you don't forget it. You've gained a new piece of Gossip.";
                    break;
                default: //Eliminated an Enemy
                    rewardString += "\nApparently one of your enemies almost fell down a flight of stairs and you grabbed them in the nick of time. They were so thankful that they apologized and ended their feud with you on the spot. You now have one less Enemy";
            */ 