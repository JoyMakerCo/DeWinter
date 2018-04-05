using UnityEngine;
using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using Core;
using Util;

namespace Ambition
{
	public enum IncidentSetting { Party, Night, Intro };

	public class IncidentModel : IModel, IInitializable
	{
		private IncidentCollection _config;

		public Dictionary<IncidentSetting, IncidentVO[]> eventInventories;

		public IncidentVO FindEvent(string ID)
		{
			if (_config != null)
			{
				return Array.Find(_config.Incidents, e=>e.Name.Contains(ID));
			}
			return null;
		}

		public IncidentVO FindEvent(string ID, IncidentSetting setting)
		{
			if (_config != null)
			{
				return Array.Find(_config.Incidents, e=>e.Setting == setting && e.Name.Contains(ID));
			}
			return null;
		}


		private IncidentVO _incident;
		public IncidentVO Incident
		{
			get { return _incident; }
			set {
				_incident = value;
				AmbitionApp.SendMessage<IncidentVO>(_incident);
				if (value == null && _moment != null) Moment = null;
			}
		}

		private MomentVO _moment;
		public MomentVO Moment
		{
			get { return _moment; }
			set {
				_moment = _incident != null ? value : null;
				AmbitionApp.SendMessage<MomentVO>(_moment);
				if (_moment != null)
				{
					int index = Array.IndexOf(_incident.Moments, _moment);
					TransitionVO[] transitions = Array.FindAll(_incident.Transitions, t=>t.Index == index);
					AmbitionApp.SendMessage<TransitionVO[]>(transitions);
				}
			}
		}

		public int EventChance=20;

	    public void Initialize()
	    {
			_config = Resources.Load<IncidentCollection>("IncidentConfig");
			// eventInventories = new Dictionary<EventSetting, EventVO[]>();
			// eventInventories.Add(EventSetting.Intro, StockIntroInventory());
			// eventInventories.Add(EventSetting.Party, StockPartyInventory());
			// eventInventories.Add(EventSetting.Night, StockNightInventory());
	    }
/*
		public EventVO[] StockIntroInventory()
		{
			List<EventVO> events = new List<EventVO>();
			//---- Yvette's Story----
			events.Add(new EventVO(1, "Yvette’s Story - The Beginning",
				//Stage 0
				new EventStage("The carriage rumbles and bounces along the cobbles as you approach the Capitol. It’s taken over a week to get here, even with what little luggage you have. You’ve already finished all of your books and Armand's letter is your only entertainment, other than staring out the window.",
	                new EventOption("<Read Armand’s Letter>", 1),
	                new EventOption("<Look Out the Window>", 3),
	                new EventOption("<To the Coachman> “Let’s go! I don’t have all day!”", 4)),
	           
	            //Stage 1
	            new EventStage("You must have read the brief letter over a hundred times, but you still find new details to puzzle over." +
		            "\n\n‘My Dearest Yvette,"+
		            "\n\nI know you must still be in shock from your parents’ passing…’" + 
		            "\nYou’re also still in shock from your tiny inheritance. Your older siblings received the lion’s share of the family fishing business." +
		            "\n\n‘I know it may be sudden, but I want to invite you to live with me in the Capitol.’" +
		            "\nYou were engaged to Armand, before he moved north to the Capitol to pursue his dreams of changing the Country. You mailed love letters soaked in perfume, he replied with volumes about how much he missed you. The son of a Baron, falling in love with someone like you. It didn’t feel real sometimes.",
	                new EventOption("<Continue>", 2)),

	            //Stage 2
	            new EventStage("‘I can’t wait for you to see it. It’s a place unlike anything back home, a bustling place where anyone can be anything." +
		            "\n\nJe t'embrasse," +
		            "\n\nArmand’" +
		            "\n\nThat was something you were looking forward to. A chance to be something more." +
		            "\nBaroness Yvette… it’s got a ring to it.",
		                new EventOption("<Look Out the Window>", 3),
		                new EventOption("<To the Coachman> “Can we pick up the pace, please?”", 4)),

	            //Stage 3
	            new EventStage("You crane your neck out the window to peer at the Capitol. So much of it is exactly as Armand described it. The streets are a riot of color and energy as people go about their days. Elegant persons of fashion share the streets with beggars in rags." + 
	            "\n\nIt’s all such a far cry from back home. Though part of you is worried by such unfamiliar surroundings, you mostly feel excited.This place is to be the stage for your new life.",
	                new EventOption("<Read Armand’s Letter>", 1),
	                new EventOption("<To the Coachman> “Can we pick up the pace, please?”", 4)),

	            //Stage 4
	            new EventStage("The coachman obediently flicks at the reins and you lurch back in your seat as the carriage surges forward. The city blurs by as you wind your way towards where Armand is staying. When you arrive at your new home you’re eager to leave the carriage, as tired and rumpled as you are." + 
	            "\n\nArmand never described his living conditions much, but he’s the son of a Baron and his family was always well to-do, especially compared to yours. You’ve spent a lot of time in that carriage wondering about the home you’d spend your new life in." +
	            "\n\nStill, you didn’t expect this...",
	                new EventOption("<Continue>", 5)),

	            //Stage 5
	            new EventStage("Calling the house ‘rustic’ would imply that you were out in the country, not in the center of a city. The neighborhood is nice enough, but you’re worried that if you breathe too hard on the building, it’ll fall over. You look around but Armand is nowhere to be found. A well kept woman in the attire of a maid opens the door and approaches you." +
	            "\n\n“Ah, good evening! I assume you must be Madame Yvette! My name is Camille. I have a message for you, from Monsieur Armand. He cannot join us right now.”",
	                new EventOption("<Take the message>", 6),
	                new EventOption("“Are you sure this is the right building? I pictured something more… upright.”", 7),
	                new EventOption("“He’s not here? Oh God, the rats in there ate him, didn’t they?”", 8),
	                new EventOption("“Well, this looks awful. Camille, give me the short version.”", 11)),

                //Stage 6
	            new EventStage("You instantly recognize the handwriting as Armand’s, though it looks like the letter was written in a hurry." +
	            "\n\n‘My Dearest Yvette," +
	            "\n\nI know you were looking forward to us being together, but circumstances have forced me to abandon my old life, even my name. I cannot tell you much, as I do not want to endanger your life. Please make use of my old home, the rent has been paid through the month, though you will still need to pay Camille every week." +
	            "\n\nI’m sorry," +
	            "\n\nArmand’",
	                new EventOption("“Camille, what happened to Armand? Is he alright?”", 9),
	                new EventOption("“This better be a joke.”", 10)),

	             //Stage 7
	             new EventStage("“Don’t worry Madame, the building is fine, I grew up in one just like it!” Camille replies cheerily, regarding the ageing structure with some fondness." +
	             "\n\nSomehow, you don’t find yourself particularly reassured.",
	                new EventOption("<Read the Message>", 6)),

	            //Stage 8
	            new EventStage("“Impossible, Madame! I’ve taken care of all the rats myself.” Camille replies, emphasizing her point with a playful stomp of her foot." + 
	            "\n\nEyeing Camille's worn shoes, you have a feeling that while the rats didn’t go quietly, they're definitely gone.",
	                new EventOption("<Read the Message>", 6)),

	            //Stage 9
	            new EventStage("“I don’t know Madame. Monsieur Armand packed up and left a few days ago, all in a great hurry. I know he was involved in politics but I never asked about the specifics. He seemed worried but I’m sure he’ll be fine!” Camille’s facial expression doesn’t show a lot of confidence." + 
	            "\n\nCamille suddenly brightens up again. “As you’re alone, perhaps you should go out and meet some new people! You might find someone who could help you find your way in the city. I know a place nearby that may help you do just that.”",
	                new EventOption("<Start the Tutorial> “Thanks Camille, I’ll take all the help I can get”", 13),
	                new EventOption("<Skip the Tutorial> “No Camille. I’ll take care of this myself”", 12)),

	            //Stage 10
	            new EventStage("Camille shifts awkwardly from foot to foot." +
	            "\n\n“I’m afraid not Madame. Monsieur Armand packed up and left a few days ago, all in a great hurry. I’ve spent the last few days preparing the house for your arrival.”" +
	            "\n\nCamille suddenly brightens up again. “This is the perfect time to go out and meet some new people! You might find someone who could help you find your way in the city. I know a place nearby that may help you do just that.”",
	                new EventOption("<Start the Tutorial> “Thanks Camille, I’ll take all the help I can get”", 13),
	                new EventOption("<Skip the Tutorial> “No Camille. I’ll take care of this myself”", 12)),

	             //Stage 11
	             new EventStage("Camille stares at you, a little startled by your brusqueness." +
	             "\n\n“Um… Monsieur Armand isn’t here and he’s not coming back. He’s paid the rent through the month. You still need to pay me every week.”" + 
	             "\n\nShe shifts awkwardly from foot to foot." + 
	             "\n\n“As you are alone now, perhaps you should go meet some new people. You might find someone who could help you find your way in the city.”",
	                new EventOption("<Start the Tutorial> “Thanks Camille, I’ll take all the help I can get”", 13),
	                new EventOption("<Skip the Tutorial> “No Camille. I’ll take care of this myself”", 12)),

				//Stage 12
				new EventStage("“Are you sure Madamme?” Camille asks tentatively." +
		            "\n\n<If it is your first time playing the game, we do not advise skipping the tutorial Party. It will teach you a lot of useful things you'll need to play the game.>",
		                new EventOption("<Start the Tutorial> “On second thought, I could use a little help", 13),
		                new EventOption("<Skip the Tutorial> “I'm Sure”", -1)),

                //Stage 13 (Tutorial step)
	            new EventStage("", new RewardVO(RewardType.Message, GameMessages.START_TUTORIAL))));
       		return events.ToArray();
		}

	    //This is all the Party Events
	    // Priority Weight, "Event Title", Event Stage 0, Event Stage 1, etc...
		public EventVO[] StockPartyInventory()
	    {
	    	List<EventVO> parties = new List<EventVO>();
	        //---- Event 1 ---- An Insult
	        parties.Add(new EventVO(1, "An Insult",
				// Stage 0
	            new EventStage("Someone bumps into you from behind, and you feel a sudden chill as their drink spills down your back." 
	                         + "\n'Out of my way!' someone yells shrilly. You turn around to find a rather intoxicated woman, freshly emptied drink still in her hands. Two of her friends are trying to guide her outside for some fresh air."
	                         + "\nHer friends mumble some half hearted apologies about your dress before their charge interjects 'I don't see the problem. I didn't ruin anything. I would never be seen in a dress that tacky.' Her friends giggle at the remark."
	                         + "\n\n-You have lost 25 Reputation",
	                         new RewardVO[] { new RewardVO(RewardType.Reputation, -25) },

	                new EventOption("'Then wear it next time, because I don't think anyone wants to see you either.'", 1, 2, 2, 1),
	                new EventOption("'Haha... yes, very funny...'", 3, 4, 4, 1),
	                new EventOption("'I don't have to put up with this.' <Walk Away>", 5)),

				// Stage 1
	            new EventStage("The other guests hush for a moment before errupting in laughter. It seems like your comment was perfectly timed. The drunk guest and her compatriots skulk away."
	                         + "\n\n-You have gained 100 Reputation",
							new RewardVO[] { new RewardVO(RewardType.Reputation, 100) },

	                new EventOption("Return to the Party <End Event>", -1)),

				// Stage 2
	            new EventStage("The room erupts into laughter, but you see something in the drunken woman's eyes. It looks like your retort cut deeper than you thought. She glares at you, her eyes boiling with hate." 
	                         + "\n'I'll remember this.' she hisses at you under her breath before stalking away, a little more composed than before, but only a little."
	                         + "\n\n-You have a new Enemy"                       
	                         + "\n-You have gained 50 Reputation",
							new RewardVO[] { new RewardVO(RewardType.Reputation, 50),
											new RewardVO(RewardType.Enemy, null, 1)},

	                new EventOption("Return to the Party <End Event>", -1)),

	             // Stage 3
	            new EventStage("One of the friends quietly thanks you for being a good sport. The drunk guest continues her slurred tirade to nobody in particular. You walk away, searching for a damp cloth to blot your dress with."
	                         + "\n\n-You have regained 15 Reputation",
							new RewardVO(RewardType.Reputation, 15),

	                new EventOption("Return to the Party <End Event>", -1)),

				// Stage 4
	            new EventStage("One of the friends perks up. 'It is funny isn't it? Just like how it's funny that the wine actually makes your dress look better.'" 
	                         + "\nLooking around, you notice that some of the once sympathetic faces have turned to quiet laughter. You walk away, searching for a damp cloth to blot your dress with."
	                         + "\n\n-You have lost 15 Reputation",
							new RewardVO(RewardType.Reputation, -15),

	                new EventOption("Return to the Party <End Event>", -1)),

				// Stage 5
	            new EventStage("You walk away, searching for a damp cloth to blot your dress with. Behind you, you can hear the trio snickering at you.",
	                new EventOption("Return to the Party <End Event>", -1))));

	        //---- Event 2 ---- A Time for Gossip
	        parties.Add(new EventVO(1, "A Time for Gossip",

	        	// Stage 0
	            new EventStage("You run across a circle of ladies in a back corner, talking amongst themselves and laughing carefully behind their fans. As you get closer, you can overhear them gossiping about the Duchess of Anemour." 
	                         + "\nThere are rumors going around about her and the Baron of Baden. The fact that the Duchess is actually here makes the dicussion that much more delicious.",

	                new EventOption("Join in on the fun", 1, 1, 2, 1),
	                new EventOption("Don't get involved", 4)),
	            
				// Stage 1
	            new EventStage("You work your way seamlessly into the group and provide some new fodder for their mockery, generating laughter and murmurs of approval. After a few minutes everyone in the cicle goes their seperate ways with new rumors and jabs to tell their friends." //Stage 1
	                         + "\n\n-You have gained 50 Reputation",
							new RewardVO(RewardType.Reputation, 50),
	                new EventOption("Return to the Party <End Event>", -1)),

				// Stage 2
	            new EventStage("You join the conversation and launch into a miniature tirade of the Duchess's various failings and romantic misadventures."
	                         + "\nIn fact, you get so wrapped up in your performance that it takes a moment to realize that nobody in the circle is laughing anymore." 
	                         + "\nThey're just staring silently over your shoulder.",
	                new EventOption("'...She's right behind me, isn't she?'", 3)),

				// Stage 3
	            new EventStage("You're correct. She IS right behind you. This point is driven home when the Duchess starts screaming in your ear." //Stage 3
	                         + "\nYou turn to defend yourself from her verbal barrage, but can barely get a word in amongst her retaliation." 
	                         + "\nBy the time you turn back to the group, desperate for some support, you find that they've already vanished to other portions of the party. The Duchess storms away, leaving a trail of confused and upset party guests in her wake."
	                         + "\nThere is a slight ringing in your ears."
	                         + "\n\n-You have a new Enemy" 
	                         + "n-You have lost 25 Reputation",
							new RewardVO[] { new RewardVO(RewardType.Reputation, -25),
											new RewardVO(RewardType.Enemy, null, 1)},

	                new EventOption("'Well, that could have gone better.' <End Event>", -1)),

				// Stage 4
	            new EventStage("Talking about someone who's actually at the party seems like a recipe for disaster, so you choose to avoid their circle." 
	                         + "\nPerhaps they'll talk about you next.",
	                new EventOption("Return to the Party <End Event>", -1))
	            ));

	        //---- Event 3 ----
	        parties.Add(new EventVO(1, "Unattended Valuables",

				// Stage 0
	            new EventStage("While ascending the stairs up to the party you notice a small, silver statue of a duck left out on a short table." //Stage 0
	                         + "It would easily fit inside your bag and a quick glance shows that you're alone on the stairs.",
	                new EventOption("Take the statue", 1, 1, 2, 1),
	                new EventOption("Leave it alone", 3)),

				// Stage 1
	            new EventStage("With a single, swift movement you scoop up the statue and covertly place it in your bag."
	                         + "\nAnother glance about confirms that nobody was around to witness your indiscretion, though the extra weight in your bag reminds you of your new acquisition."
	                         + "\n\nYou have gained 100 Livres.",
							new RewardVO(RewardType.Livre, 100),

	                new EventOption("Return to the Party <End Event>", -1)),

				// Stage 2
	            new EventStage("As you reach out for the statue, the hem of your sleeve gets caught on the corner of the table. You knock the statue to the floor with a heavy 'thud'." //Stage 2
	                         + "\nWhile kneeling down to retrieve your prize, you notice the shoes of a servant out of the corner of your eye. Looking up at them suddenly and manage to stammer some sort of apology and return the item to its place on the table."
	                         + "\nThey know exactly what you were trying to do and you're sure the host will hear about this." 
	                         + "\n\nYou've just lost 150 Reputation.",
						new RewardVO(RewardType.Reputation, -150),

	                new EventOption("Return to the Party <End Event>", -1)),

				// Stage 3
	            new EventStage("You decide against taking the statue, petty theivery doesn't really suit you. "
	                         + "\nFor now, at least.",
	                new EventOption("Return to the Party <End Event>", -1))
	            ));

	        //---- Event 4 ---- A Friendly Wager
	        // To Do: Rewrite this to be Roulette? Bet on Red or Black, Use actual Roulette Odds, add an open better stage for the 4 options, could actually allow multiple rounds of betting with circular stage references
	        parties.Add(new EventVO(1, "A Friendly Wager", 

				// Stage 0
	            new EventStage("You are drawn to a side room by shouts of excitement and the clink of heavy coins. A handsome man, his face flush with drink, approaches you."
	                         + "\n'Hello there! How do you feel about a game of dice? We've been playing all day and a new player might liven things up!'" 
	                         + "\nHe smiles cheekily before leaning in closer 'How does 50 Livres sound?'",
	                new EventOption("'I'm game for a litte wager.' <Bet 50 Livres>", 1,1,2,1),
	                new EventOption("'That's all? Please, let's make this interesting.' <Bet 100 Livres>", 3,1,4,1),
	                new EventOption("'Sorry, I'm not interested.'", 5)),

				// Stage 1
	            new EventStage("The dice turn up your way and you can't help but smile as the grumbling betters shovel the scattering of coins over to you."
						+ "\n\nYou have gained 50 Livres.",
							new RewardVO(RewardType.Livre, 50),
	                new EventOption("'Thanks for the money, Gentlemen!' <End Event>", -1)),

				// Stage 2
	            new EventStage("You grimace as the dice don't turn up in your favor. One of the men at a table gives you a knowing wink as he takes your money away. "
	                         + "'Better Luck next time, eh?'"
							+ "\n\nYou have lost 50 Livres.",
							new RewardVO(RewardType.Livre, -50),
	                new EventOption("'Oh well...' <End Event>", -1)),

				// Stage 3
	            new EventStage("The dice turn up your way and you can't help but smile as the grumbling betters shovel the small pile of coins over to you."
							+ "\n\nYou have gained 100 Livres.",
							new RewardVO(RewardType.Livre, 100),
	                new EventOption("'Thanks for the money, Gentlemen!' <End Event>", -1)),

				// Stage 4
	            new EventStage("You grimace as the dice don't turn up in your favor. You suppress the urge to reach out for your coins as one of the men at a table takes your sizable amount of money away. "
	                         + "\n'Better Luck next time, eh?'"
	                         + "\n\nYou have lost 100 Livres.",
							new RewardVO(RewardType.Livre, -100),
	                new EventOption("'Ugh!' <End Event>", -1)),

				// Stage 5
	            new EventStage("There are some grumbles of distaste as you walk away, but you feel better for not trusting your precious funds to chance.",
	                new EventOption("Return to the Party <End Event>", -1))
	            ));

	        //---- Event 5 ---- That Can't Be Them!
	        parties.Add(new EventVO(1, "That Can't Be Them!",

	            //Stage 0
	            new EventStage("You come across a small crowd fawning over a man in splendid finery. He gestures expansively with his cane. None of this is unusual, until you hear someone refer to him as 'Baron de Fay'." 
	                         + "\nThat's not Baron de Fay, it can't be."
	                         + "\nAs you get closer, your suspicions are confirmed. The Baron is reclusive, yes, but you've read enough to know that this man definitely isn't him. They're an impostor!",
	                new EventOption("'That man's not the Baron! Who are you?'", 1),
	                new EventOption("'Excusez moi, may I have moment with the Baron?' <Pull Him Away>", 2),
	                new EventOption("'Ugh, fools...' <Walk Away>", 5)),

	            //Stage 1
	            new EventStage("The small crowd parts at one side to let you in. The looks on their faces show a mix of disgust and confusion. You almost doubt yourself until you look in the Baron's eyes. For a split second you see a flash of fear."
	                        + "\n'That's quite a bold claim Madamme!' he laughs, puffing up to his previous stature again 'Are you sure you haven't been overserved? If you'd like, I could help you get some air.'"
	                        + "\nHe gestures with his cane to a quiet corner of the room. It looks like he wants to discuss something with you in private.",
	                new EventOption("'No, I'm not drunk! Why is your sigil wrong, Baron?'", 4),
	                new EventOption("'Maybe I do need some air...'<Go With Him>", 2)),

	            //Stage 2
	            new EventStage("As you walk together towards a quiet corner of the room, you tap your finger silently on the sigil decorating the top of the Baron's cane. His eyes widen and he quickly redjusts his grip to cover the incorrect sigil. Once your alone, he checks over his shoulder. His accent suddenly drops to a much more common tone."
	                         + "'Alright, what's it gonna' take to make this problem go away?' He asks flatly. You hear the clink of coins as one of his hands rifles through his pockets.",
	                new EventOption("'I don't know... let's see what you got'", 4)),

	            //Stage 3
	            new EventStage("The crowd drops silent in confusion. The Baron looks uncertainly at you."
	                         + "\n'Your cane has your family crest on it' you continued 'But it's wrong, the real Baron de Fay has a bear on his crest, not a wolf.'"
	                         + "\nAll attention shifts to the head of his gaudy cane. The Baron's hand moves to cover it unsuccesfully."
	                         + "\n'This cane? Why it was a gift from a good friend, he must have made a mistake...' he stammers."
	                         + "\n'But, an hour ago you told me it was your father's' one of the other Guests interjects. A grumble of discontent grows among the crowd."
	                         + "\n'Ah, I forgot something very important... over there.' the fake Baron gestures vaguely in a direction before scurrying away. He shoots a look back at you. This isn't over." 
	                         + "\n\n-You have gained 75 Reputation." 
	                         + "\n-You have a new Enemy",
	                         new RewardVO[]{ new RewardVO(RewardType.Reputation, 75),
											new RewardVO(RewardType.Enemy, null, 1) },
	                new EventOption("Return to the Party <End Event>", -1)),

	            //Stage 4
	            new EventStage("He narrows his eyes at you before rummaging through his pockets, muttering some words that are truly not befitting a man of his supposed title.'"
	                         + "\nHe drops a significant pouch of coins in your hand. 'Take this, you vulture, it's all I have.' He glances over at the crowd of social climbers you had left behind, who are growing more antsy by the second. 'Now leave me alone, I've got a room to work.'"
	                         + "\n\n-You have gained 150 Livres.",
							new RewardVO(RewardType.Livre, 150),
	                new EventOption("'I'm glad we could come to an understanding' <End Event>", -1)),

	            //Stage 5
	            new EventStage("You leave them alone. If those social climbers want to waste their time on a fake, then so be it. You have real schmoozing to do.",
	                new EventOption("Return to the Party <End Event>", -1))));

	        //---- Event 6 ---- A Downtrodden Guest
	        parties.Add(new EventVO(1, "A Downtrodden Guest",
	            //Stage 0
	            new EventStage("In a hallway between rooms you're distracted by the sound of muffled sobs. A quick survey reveals a younger woman, sitting on a chair up against the wall. She blots at her eyes with a handkerchief between sobs."
	                        + "\nOther guests at the party pass by, studiously ignoring her plight, as is the polite thing to do in high society.",
	                new EventOption("'Excusez-moi, are you alright?'", 1),
	                new EventOption("'I'm sure she's fine...' <Ignore Her>", 4)),

	            //Stage 1
	            new EventStage("She looks up at you. Between her tears and the constant blotting with the handkerchief, her makeup is a complete mess. She can't be older than sixteen." 
	                        + "\n'I'm sorry... I'm here because my mother insisted and suddenly I was alone and I was just... saying all the wrong things and everyone looked so mad... I don't know what came over me, I was breathing really fast and I needed to sit down'"
	                        + "\n'I know mother is going to find out, and she's going to be so mad. I promised not to embarass her. I just...' Her eyes go wide and you can see a fresh set of tears forming. Passers-by are starting to stare.'",
	                new EventOption("'Oh! Uh... There, there, you're going to be just fine.", 2,1,3,1),
	                new EventOption("'You look like you need some confidence. Here, this is what I use.' <Give Her Your Wine>", 5),
	                new EventOption("'I see you've got this all figured out, I should go...' <Leave Her>", 6)),

	            //Stage 2
	            new EventStage("You sit down on a chair next to her and spend the next hour comforting her, giving her advice and generally trying to rebuild her confidence. It works for the most part. She still doesn't want to be here, but at least she's able to wait it out."
	                        + "\nAs you get up to leave, you you notice a few strangers nodding approvingly."
	                        + "\n\n-You have gained 50 Reputation.",
						new RewardVO(RewardType.Reputation, 50),
	            new EventOption("Return to the Party < End Event > ", -1)),

	            //Stage 3
	            new EventStage("You lean in to try comforting her but she immediately pulls away and begins sobbing harder. A few half-hearted attempts to calm her down fail and you decide to leave the girl to her feelings."
	                        + "\nSome nearby guests gawk at you, as if they had actually planned on doing something about all this themselves."
	                        + "\n\n-You have lost 50 Reputation.",
							new RewardVO(RewardType.Reputation, -50),
		                new EventOption("Return to the Party <End Event>", -1)),

	            //Stage 4
	            new EventStage("You leave her alone and keep walking. It isn't long before the sounds of her sobs grow distant then quiet. You shrug to yourself. After all, you have your own problems to solve.",
	                new EventOption("Return to the Party <End Event>", -1)),

	            //Stage 5
	            new EventStage("She reaches for your glass and quickly gulps the whole thing down. She reels back, making a sour face."
	                        + "\n'Ugh, that is awful!' She spits, eyeing your glass suspiciously. 'How does my mother drink so much of that stuff?'"
	                        + "\nIt'll take some time for your wine to take effect, but the shock of the taste seems to have distracted her."
	                        + "\nYou talk for a few more minutes and try to give her some advice on how to survive these things. When you finally walk away you notice a few strangers nodding approvingly."
	                        + "\n\n-You have gained 50 Reputation.",
							new RewardVO(RewardType.Reputation, 50),
		                new EventOption("Return to the Party <End Event>", -1)),

	            //Stage 6
	            new EventStage("As soon as you turn your back to her a new fit of tears begins. A few faces around you stare accusingly, as if this mess is somehow your fault."
	                        + "\nYou hurry away quickly, before more people recognize you."
	                        + "\n\n-You have lost 30 Reputation.",
							new RewardVO(RewardType.Reputation, -30),
		                new EventOption("Return to the Party <End Event>", -1))));

	        //---- Event 7 ---- A Pressing Concern
	        parties.Add(new EventVO(1, "A Pressing Concern",
	            //Stage 0
	            new EventStage("This room is empty save for a small crowd in the center, where an extremely loud woman is venting her grievances at everyone within reach."
	                        + "\n“That Pierre!' she rails, gesturing so furiously with her wine glass that you fear for the apparel of everyone around her. “That little man and his wretched rag 'Le Mercure' has done nothing but slander my good name for months.”"
	                        + "\n“Worse yet!' she continues, 'He can't even invent his own lies! He pays other people to rumor monger for him!”"
	                        + "\nYou feel yourself cringe involuntarily when she notices you. 'You there! What do you think of all of this? What should be done?”"
	                        + "\nThe crowd murmurs amongst itself. Something certainly SHOULD be done.",
	                new EventOption("“You're right! They have no right to go spreading lies, something should be done!'", 1),
	                new EventOption("“I happen to like 'Le Mercure'. It's a rag, but at least it's fun.'", 2),
	                new EventOption("“What? Me? Sorry, I have a pressing engagement... over there' <Avoid Conversation>", 3,1,4,1)),

	            //Stage 1
	            new EventStage("Everyone nods in agreement for a while. She breaks the silence to continue “The little rat Pierre, insinuating that I was seeing the Baron of Baden behind my husband's back. As if there weren't so many other men that were far more desirable.”"
	                        + "\nYou can feel the room freeze for a moment. A few sets of eyes in the crowd widen. Did she mean to say that?",
	                new EventOption("“Absolutely, completely preposterous!”", 5),
	                new EventOption("“What? I mean, do tell...”", 6)),

	            //Stage 2
	            new EventStage("“Fun?” she spits angrily “That fun rag you enjoy so much dared to accuse me of infidelity, and with the Baron of Baden, no less!”"
	                        + "\n“As if I'd even see that boor for tea” she grumbles, mostly to herself. Suddenly, one of her eyebrows cocks fiercely. “You wouldn't happen to know anything about that, would you? Pierre gets his trash from somewhere and you appear to be his biggest fan.”",
	                new EventOption("“Me? How can I gossip about you? I've barely met you.”", 7),
	                new EventOption("“Accusing a total stranger? Wow. I don't think even Pierre could make this up.”", 8,1,9,1)),

	            //Stage 3
	            new EventStage("As you turn to leave, you feel the eyes of the entire crowd focus on you. You supress a grimace as social pressure forces you to stay."
	                        + "“No no, it's quite alright...” the loud woman says, even though it isn't. “What do you think should be done about this situation?”",
	                new EventOption("“The press has no right to go spreading lies, something should be done!”", 1),
	                new EventOption("“Actually, I like 'Le Mercure'. It's a rag, but at least it's fun.”", 2)),

	            //Stage 4
	            new EventStage("You hike up your skirts and flee the room as fast as politely possible. The loud woman quickly loses interest in your retreating back and returns to talking about herself.",
	                new EventOption("'That was close...” <End Event>", -1)),

	            //Stage 5
	            new EventStage("Your comment defuses the sudden tension in the room. Shortly, everyone takes turns decrying someone who isn't there to defend themselves and they soon disperse happily, their moral duties fulfilled."
	                        + "\n\n-You have gained 10 Reputation.",
					new RewardVO(RewardType.Reputation, 10),
	                new EventOption("“That was easier than I expected” <End Event>", -1)),

	            //Stage 6
	            new EventStage("“Oh my yes! There are so many to pick from!'. She launches into a description of nearly every single eligible male in the Royal Court, and some who are already spoken for. These descriptions range from the mild to the downright salacious. At no point in all of this does she mention her own husband. Members of the crowd glance from side to side, not sure if they're the only ones hearing this right now."
	                        + "\n“Don't you see?' She asks, flush with exasperation, among other things. 'Pierre has slandered my good name.'"
	                        + "\nThe crowd awkwardly nods in agreement and begins to disperse."
	                        + "\n\n-You have gained a new piece of Gossip",
					new RewardVO(RewardType.Gossip, null, 1),
	                new EventOption("“Let's do this again some time” <End Event>", -1)),

	            //Stage 7
	            new EventStage("The eyes of the crowd return to the loud woman. You are right, after all, how COULD you be that particular rumor monger?"
	                    + "\n“Well, I guess that's true, it can't be you,' she demures, staring into her glass as she swirls it. “However, I still cannot forgive you for supporting that terrible newspaper!'"
	                    + "She turns and suddenly leaves in a huff, her anger finally satisfied. The crowd disperses, whispering about your love for such a scandalous publication, even though they probably read it too."
	                    + "\n\n-You have lost 10 Reputation",
						new RewardVO(RewardType.Reputation, -10),
	            new EventOption("“Well, that was something else...” <End Event>", -1)),

	             //Stage 8
	            new EventStage("“Well, you would know, wouldn't you?' she says icily. Without warning, she snatches a glass of water from a nearby table and throws it in your face."
	                    + "\nBy the time you finish drying your face she is already gone, as is most of the crowd. You get the feeling that you've somehow confirmed your guilt to them, even though it wasn't specifically true."
	                    + "\n\n-You have lost 20 Reputation",
						new RewardVO(RewardType.Reputation, -20),
	            new EventOption("“At least it wasn't wine. I like this outfit...” <End Event>", -1)),

	            //Stage 9
	            new EventStage("The room hisses as the crowd collectively sucks in the air between their teeth. It sounds like that particular barb struck quite deep. Without another word she spins around in a swirl of skirts and sleeves."
	                    + "\nAs she storms off you can feel the anger radiating off of her. Doesn't matter though, you're still right and she's still a jerk. A glance around the crowd confirms that they seem to think the same."
	                    + "\n\n-You have gained 20 Reputation",
						new RewardVO(RewardType.Reputation, 20),
	            new EventOption("“Yeah, I thought so!” <End Event>", -1))));

	        return parties.ToArray();
	    }

	    //This is all the Night Events
		public EventVO[] StockNightInventory()
	    {
	    	List<EventVO> night = new List<EventVO>();
			//---- Event 1 ---- A Gift?
	        night.Add(new EventVO(1, "A Gift?",
	            //Stage 0
	            new EventStage("“Madamme, there's a man outside and he's very insistent about seeing you.”"
	                         + "\nThe man in question appears to be wearing a footman's uniform and is carrying a small package."
	                         + "\n“Bonsoir Madamme!' he yells over your handmaiden's shoulder 'I serve an admirer who's eye you caught at the last party. He wishes to remain anonymous for now, but wants you to have this token of his affection. May I come in? It's quite drafty outside.”",
	                new EventOption("“Please, let him in”", 1, 75, 2, 25),
	                new EventOption("“Tell your master that I don't take gifts in the middle of the night from stangers. Good night.”", 3),
	                new EventOption("Bodyguard", "“Hansel, would you mind searching this man?” <Use Bodyguard>", 4, 75, 5, 25)),
	            //Stage 1
	            new EventStage("With a glance back at you, your handmaiden steps aside to let the main in. He bows before presenting a package wrapped in fine paper." 
	                         + "\nOpening the package reveals a jeweled brooch. Before you can ask any more questions, the footman is already out your front door and heading for the street." 
	                         + "\n\n-You have gained 100 Livres.",
							new RewardVO(RewardType.Livre, 100),
	                new EventOption("“Well that's nice, I wonder who this mystery admirer is...” <End Event>", -1)),
	            //Stage 2
	            new EventStage("Your handmaiden steps off to the side to let him in. As you step forward to take the package from his hands, you notice the grubby clothes under his footman's coat."
	                         + "\nWithout warning the man bursts forward and shoves you to the ground. In the chaos he snatches a valuable painting from your wall and runs out the door."
	                         + "\n\n-You have lost 50 Livres.",
							new RewardVO(RewardType.Livre, -50),
	                new EventOption("“Looks like I need two things, a stiff drink and a bodyguard.” <End Event>", -1)),
	            //Stage 3
	            new EventStage("The man at the door remains extremely insistent but after a while your handmaiden manages to shoo him away." 
	                         + "\nThe identity of your admirer is still a mystery, but it's nice to know that one is still out there.",
	                new EventOption("Go back to bed <End Event>.", -1)),
	            //Stage 4
	            new EventStage("Hansel approaches the footman and pats him down, searching for any sort of concealed danger. After a few moments, Hansel gives a nod of approval and lets the man inside. The footman bows before presenting a package wrapped in fine paper."
	                     + "\nOpening the package reveals a jeweled brooch. Before you can ask any more questions, the footman is already out your front door and heading for the street."
	                     + "\n\n-You have gained 100 Livres.",
							new RewardVO(RewardType.Livre, 100),
	            new EventOption("“Well that's nice, I wonder who this mystery admirer is...” <End Event>", -1)),
	            //Stage 5
	            new EventStage("Hansel approaches the footman, who begins to shake like a leaf. It doesn't take long for your bodyguard to reveal the grubby clothes under the footman's coat."
	                         + "\nWithout a word the man dashes away into the street. Hansel turns to you and shrugs."
	                         + "\n“A petty thief looking for easy marks. Could have at least worked on his disguise a little more.”",
	                new EventOption("“Hansel, this is exactly why I keep you around.” <End Event>", -1))));
	        //---- Event 2 ---- A Little Too Much Merriment
	        night.Add(new EventVO(1, "A Little Too Much Merriment",
	            //Stage 0
	            new EventStage("When you leave the party you find your carriage waiting for you, but as your coachman opens the door, you smell brandy on his breath. A lot of it. He notices your change in expression."
	                         + "\n“Think nothing of it Madamme! Me and the other coachmen simply had a little party of our own,” he slurs, giggling to himself. “I'll get us home just fine.”",
	                new EventOption("“Sleep it off here, I'll have someone send for a sober driver.” <Spend 15 Livres>", 1),
	                new EventOption("“Fine, just be careful.” <Go With This Coachman>", 2,3,3,1)),
	            //Stage 1
	            new EventStage("A new carriage with a new driver shows up in half an hour. It's inconvenient but still safer than the alternative."
	                         + "\nYou arrive home, safe and sound."
	                         + "\n\n-You have lost 15 Livres.",
						new RewardVO(RewardType.Livre, -15),
	                new EventOption("“Finally! Off to bed.” <End Event>", -1)),
	            //Stage 2
	            new EventStage("You hop into your carriage and your coachman flicks the reins once, then again. Soon, you're speeding home, the horses' hooves thundering on the cobbles."
	                         + "\nThe carriage is going much too fast when one of the wheels hits a loose set of cobbles in the road. You hold on for dear life as the carriage lurches to the side and a wheel gets caught in the gutter. You hear a series of high pitched cracks as the gilded spokes snap." 
	                         + "\nWhen you finally come to a halt, the carriage is tilting precariously to one side. Thankfully, nobody is hurt, but it's going to cost quite a bit to fix that wheel."
	                         + "\n\n-You have lost 50 Livres.",
							new RewardVO(RewardType.Livre, -50),
	                new EventOption("“I feel like I should have seen this coming.” <End Event>", -1)),
	            //Stage 3
	            new EventStage("You don't care enough to spare the coin or the time to find another way home at this time of night. You hop in your carriage and with a flick of the coachman's reins it lurches forward."
	                        +"\nThe journey is slow going, as your coachman is overly cautious, but it's better than the alternative. Eventually you arrive home, safe and sound.",
	                new EventOption("“Finally! Off to bed.” <End Event>", -1))));
	        //---- Event 3 ---- The Company That We Keep
	        night.Add(new EventVO(1, "The Company That We Keep",
	            //Stage 0
	            new EventStage("Late at night, while you're getting ready for bed, you wander into a guest bedroom that doesn't look like it's been used in months. A thin layer of dust covers everything, which is strange given how fastidious Camille is about cleanliness."
	                        + "\nYou bring Camille upstairs to show her the room. She puts down her duster and thinks deeply, “Monsiuer Armand told me never told me to never enter that room, even to clean it, so I never did! He even told me to make sure you never went in.” She shrugs, as if these mysterious orders were the most natural thing in the world."
	                        + "\n“However,” Camille continues “You're the one paying me now, not Armand, so...”"
	                        + "\n\nWithout finishing her statement, Camille steps off to the side, leaving the door wide open.",
	                new EventOption("“If Armand didn't want me to search this room then he should have kept living here.” <Search the Room>", 1),
	                new EventOption("“Hmmm... Armand probably wanted this place to stay undisturbed for a reason.” <Leave the Room Alone>", 2)),
	            //Stage 1
	            new EventStage("Candle in hand, you enter the room and look around. The guest room has a table in the center covered in ink stained papers. Underneath the papers you find a sketch book filled with notes and drawings of various people. Hanging over the mantle is a painting, but something just doesn't feel right about it.",
	                new EventOption("“Camille, who are these people?” <Investigate the Sketch Book>", 3),
	                new EventOption("“Something is odd about this painting...” <Investigate the Painting>", 4),
	                new EventOption("“Now I REALLY think Armand wanted this place to stay undisturbed for a reason.” <Leave the Room Alone>", 2)),
	            //Stage 2
	            new EventStage("You leave the room alone for now, noting to yourself that you should bring this place up with Armand, should you ever see him again.",
	                new EventOption("“I have enough to deal with as is, I don't need this too.” <End Event>", -1)),
	            //Stage 3
	            new EventStage("You hold the sketchbook up to the light and slowly page through it. The book itself is filled with fragments of notes, written in charcoal. However, none of these notes are written in Armand's handwriting. Worse yet, most of the notes are smudged. Without any context, it's hard to make sense of them at all."
	                        + "\nMore interesting is all the sketches in the book of Armand, gathered with several other people you don't recognize. The drawings don't look deliberate, someone must sketched them out idly during boring parts of whatever these meetings were. " 
	                        + "You show the images to Camille."
	                        + "\n“Ah yes, these people were often Armand's guests, though they never gave me their names. They always held these midnight meetings here for some kind of political group but they always were very secretive about the details.”",
	                new EventOption("“Something is odd about this painting...” <Investigate the Painting>", 4),
	                new EventOption("“I think I've seen everything that I need to see” <Leave the Room>", 5)),
	            //Stage 4
	            new EventStage("Just looking at the painting gives you chills. It depicts Armand and several other men and women circled around a dead man laid out on a table. Why anyone would comission such a grim piece is beyond you."
	                        + "\n\nWhat's even stranger about the painting though, is that you're sure that you've seen the dead man in the painting before, and alive! You can't remember his name, but you're sure that you've seen him before.",
	                new EventOption("“Camille, who are these people?” <Investigate the Sketch Book>", 3),
	                new EventOption("“I think I've seen everything that I need to see” <Leave the Room>", 5)),
	            //Stage 5
	            new EventStage("Investigating the room seems to have only left you with further questions. Still, if you find Armand then perhaps more of this will be made clear.",
	                new EventOption("“That's enough of all this. Time to head to bed.” <End Event>", -1))));
	        //---- Event 4 ---- The Help On Hard Times
	        night.Add(new EventVO(1, "The Help On Hard Times",
	            //Stage 0
	            new EventStage("Camile approaches you as you're getting ready to go to bed. You can tell she's nervous."
	                        + "\n“Madamme, I don't mean to disturb you but...” she wrings the front of her apron before continuing “My dear sister is sick and she needs to see a doctor, but we haven't the money.”"
	                        + "\nShe swallows hard and stares at the floor. “Is there any way I can get an advance on my pay? I'd need around 50 Livres.”",
	                new EventOption("“Yes, I think we can do that.” <Advance 50 Livres>", 1),
	                new EventOption("“Camille, your family is in trouble, just take the money.” <Pay Her 50 Livres>", 2),
	                new EventOption("“I'm sorry Camille, I can't spare that right now.”", 3)),
	            //Stage 1
	            new EventStage("Camille looks up, genuinely surprised before recomposing herself. “Thank you Madamme, I assure you that you'll not reget this kindness.”"
	                        + "\n“Is there anything else I can take care of for you?”"
	                        + "\n\n-You have lost 50 Livres.",
							new RewardVO(RewardType.Livre, -50),
	                new EventOption("“No, thank you Camille. You are dismissed.” <End Event>", -1)),
	            //Stage 2
	            new EventStage("Camille's eyes go wide like saucer plates and she opens her mouth to speak but nothing comes out for a while."
	                        + "\n“Madamme, thank you so very much! I can't believe this. My sister will be so happy, once she sees the doctor, of course.” She pauses after a few giddy moments and recomposes herself. “Is there anything else I can do for you?”"
	                        + "\n\n-You have lost 50 Livres.",
							new RewardVO(RewardType.Livre, -50),

	                new EventOption("“No, thank you Camille. You are dismissed.” <End Event>", -1)),

	            //Stage 3
	            new EventStage("Camille deflates when she hears this. “I understand Madamme, I'm sorry for bothering you.” She closes the door to your room and returns to her remaining duties before the end of her shift.",
	                new EventOption("“She's smart, I'm sure she'll figure something out.” <End Event>.", -1))));
	        //---- Event 5 ---- Apolitical Asylum
	        night.Add(new EventVO(1, "Apolitical Asylum",
	            //Stage 0
	            new EventStage("A sudden crash downstairs rouses you from your slumber. In a flash, Camille is in your room, firepoker in hand."
	                    + "\n“Madamme, I believe we have an intruder,” she whispers, nodding significantly to a small marble statue near your bed."
	                    + "\nYou follow Camille down the stairs, statue in hand, to find that a young man wearing the colors of the revolutionaries has crashed through your window."
	                    + "\nYou hear the whistle of the marshals, doubtlessly in hot pursuit. The young revolutionary slumps to his knees. He's bleeding."
	                    + "\n\nHe looks up at you “Madamme, my name is Isaac, please help me.”",
	                new EventOption("“What are you doing here?”", 1),
	                new EventOption("“Quick! Get to the attic and stay quiet!”", 2,1,3,1),
	                new EventOption("“Guards! Guards! He's in here!”", 4)),
	            //Stage 1
	            new EventStage("“Do you live here? Mon Dieu, I thought this place was abandoned,” he gasps. You're not sure if the insult against your home was intentional."
	                    + "\nIsaac winces as he clutches the wound on his ribs. “I was hanging posters decrying the King when the marshals spotted me. I managed to get away but they're still after me. I need a place to hide.”"
	                    + "\nYou can hear the marshals getting closer. “Please! You have to help me!”",
	                new EventOption("“Fine, get to the attic and stay quiet!”", 2,1,3,1),
	                new EventOption("“Guards! Guards! He's in here!”", 4)),
	            //Stage 2
	            new EventStage("Isaac leans on Camille as she helps him hobble up the stairs towards the attic. You slowly put down the statue and watch the door, waiting for the marhsals to come bursting through at any second. They don't."
	                    + "\nYou wait on edge for an hour, but they never show. They must have passed by your home entirely."
	                    + "\n\nThe next morning, at breakfast, Isaac joins you in the dining room, sporting some fresh bandages. He bows awkwardly to you when he enters."
	                    + "\n“Merci beaucoup Madamme. If it weren't for you I'd be in a jail cell right now, or worse. You're a true friend of the people and I'll make sure everyone knows it.” He bows again before leaving out the back door."
	                    + "\n\n-You have gained 50 Reputation with the Third Estate",
						new RewardVO(RewardType.Faction, "Third Estate", 50),

	                new EventOption("“As far as mornings after go, that was pretty good.” <End Event>", -1)),
	            //Stage 3
	            new EventStage("Camille escorts Isaac up the stairs towards the attic. He's barely out of sight before you hear a sharp knock on the door."
	                    + "\nAn authoritative voice booms out “Open up in the name of the King!”"
	                    + "\nYou open the door to find a trio of armed marshals, breathing heavily. They look shocked to see you."
	                    + "\n“Pardon Madamme. We are searching for a vagrant who has committed crimes against the King. We believe he is hiding in this...” the leader of the Marshalls pauses to survey your home. “...structure.”"
	                    + "\nYou remind yourself to nag Camille about finding some carpenters to fix this place up a little. “I must remind you, Madamme,” the marshal continues “That harboring fugitives is a very serious crime. May we come in?”" 
	                    + "\nThat last question doesn't sound like a question at all.",
	                new EventOption("“Absolutely Officers! Allow me to show you around my humble 'structure', we'll start here on the ground floor.”", 6,1,7,1),
	                new EventOption("“Monsieurr, a man has forced his way into my home and barricaded himself in the attic.”", 5)),
	            //Stage 4
	            new EventStage("Isaac's eyes go wide as you start shouting for the marshals. He tries to run but trips and spills across your floor. An urgent fist pounds at the door, which you rush to open. The marshalls burst into the room and aprehend the young revolutionary."
	                    + "\n“Merci beaucoup Madamme, your actions helped bring this vile criminal to justice. Your King and Country extend their gratitude.” Isaac stares at you accusingly as they marshalls frog march him out the door. You have a feeling that he'll make sure everyone knows where your loyalties lie."
	                    + "\n\n-You have gained 50 Reputation with the Crown" 
	                    + "\n-You have lost 50 Reputation with the Third Estate",
						new RewardVO[]{new RewardVO(RewardType.Faction, "Crown", 50),
										new RewardVO(RewardType.Faction, "Third Estate", -50)},
	                new EventOption("“Another victory for justice. Now, about fixing that window...” <End Event>", -1)),
	            //Stage 5
	            new EventStage("Hearing your words, the marshals silently ready their weapons and stalk up your stairs towards the attic."
	                    + "\nMoments later you hear the crash and scuffle of a fight. It isn't long before the marshals are roughly dragging the wounded Isaac down your stairs." 
	                    + "\n“Madamme, your King and Country extend their gratitude,” the leader of the marchals says as he looks you up and down. He's certain you hid Isaac, but you also gave him up. Isaac stares at you accusingly before he's shoved out the door." 
	                    + "\n\n-You have gained 30 Reputation with the Crown"
	                    + "\n-You have lost 50 Reputation with the Third Estate",
					new RewardVO[]{ new RewardVO(RewardType.Faction, "Crown", 30),
									new RewardVO(RewardType.Faction, "Third Estate", -50)},

	                new EventOption("“Sorry Isaac, it was either you or me.” <End Event>.", -1)),
	            //Stage 6
	            new EventStage("You give the marshals an excruciatingly detailed tour of your home, starting with the kitchen and first floor. That should give Camille enough time to hide Isaac and tend to his wounds. By the time you finish showing them the second floor, it's been over an hour. The marshals are equally bored and furious."
	                    + "\nAfter your 'tour' the marshals perform their own sweep of the house and come up with nothing. As the marshals stalk out the door their leader eyes you suspiciously. He knows you've hidden Isaac, but he has no proof." 
	                    + "\n\nThe next morning, at breakfast, Isaac joins you in the dining room, sporting some fresh bandages. He bows awkwardly to you when he enters."
	                    + "\n“Merci beaucoup Madamme. If it weren't for you I'd be in a jail cell right now, or worse. You're took an enormous risk to help me, I'll never forget that. All of my brothers and sisters of the Third Estate will never forget that either.” He bows again before leaving out the back door."
	                    + "\n\n-You have gained 70 Reputation with the Third Estate"
	                    + "\n-You have lost 50 Reputation with the Crown",
						new RewardVO[]{ new RewardVO(RewardType.Faction, "Crown", -50),
										new RewardVO(RewardType.Faction, "Third Estate", 70)},
	                new EventOption("“As far as mornings after go, that was pretty good.” <End Event>.", -1)),

	            //Stage 7
	            new EventStage("You start to give the marshals a tour of the first floor, in order to give Camille enough time to hide Isaac properly. However, as you lead them into the kitchen, one of the marshals notices something."
	                    + "\n“Madamme, why is there a statue on the floor in the living room?” The marshal asks, kneeling next to your previously improvised weapon. The marshal's gaze then falls on your broken window and the glass shards littering the floor."
	                    + "\n\n“He's here!” the leader of the marshals hisses through his teeth as he glares at you. “Tear this place apart, I want him found!”"
	                    + "\nYour home is filled with the sound of scraping and clattering as the marshals roughly search your home. It isn't long before they're dragging the wounded Isaac down your stairs."
	                    + "\n“Why was this criminal hiding in the attic with your maid?” the leader of the marshals asks, his hand resting on his spare manacles." 
	                    + "\nIsaac suddenly interjects “She was my hostage! I forced them to hide me here!” the marshals beat him until he's quiet. The leader of the marshals looks you up and down then leaves with Isaac. His story is just plausible enough, but the marshals still suspect you. Isaac looks at you one last time before he's shoved at the door. He knows you tried to save him."
	                    + "\n\n-You have gained 10 Reputation with the Third Estate"
	                    + "\n-You have lost 70 Reputation with the Crown",
						new RewardVO[]{ new RewardVO(RewardType.Faction, "Crown", -70),
										new RewardVO(RewardType.Faction, "Third Estate", 10)},
	                new EventOption("<End Event>.", -1))));

	        //---- Event 6 ---- This Shall Not Stand!
	        //The press is talking shit about you! Worst of all, it's mostly true! Getting it to stop makes you an enemy. Failing to Make it stop gets you a scandal
	        night.Add(new EventVO(1, "This Shall Not Stand!",
	        //Stage 0
	        new EventStage("Camille hands brings in the morning paper along with your breakfast. You nearly spit out your tea when read the social pages. The social pages are full of gossip about you and the worst part is, it's mostly true!"
	                    + "\nWorst of all, it's mostly true! The pages of 'Le Mercure' read like a list of criminal charges against you written by some disgruntled socialite."
	                    + "\n\nIf you don't do something about this then people might start to believe that this drivel is actually true. It mostly is, but you can't have them believing it!"
	                    + "\n\n-You have lost 15 Reputation",
						new RewardVO(RewardType.Reputation, -15),

	            new EventOption("“Camille, bring Pierre to me, now. I don't care where you find him or how you get him.”", 1),
	            new EventOption("“I'm not going to dignify this trash with a response.”", 4),
	            new EventOption("Bodyguard", "“Hansel, Pierre is lying about me. Please make him understand how upset I am.” <Use Bodyguard>", 9)),

	        //Stage 1
	        new EventStage("Later, around lunch time, Camille escorts Pierre into your living room. Judging from the state of his appearance, Pierre has been celebrating their most recent release quite thoroughly."
	                    + "\n\n“Good afternoon Madamme!” Pierre greets you with his smile of crooked teeth. “I came as soon as I could. Your maid was quite... insistent. What can I do for you today?”",
	            new EventOption("“Pierre, after all I've done for you, how could you print this trash about me?”", 2),
		            new EventOption("“Either you stop this garbage Pierre, or you'll regret even knowing my name.”", 3)),
		        //Stage 2
		        new EventStage("Pierre appears stung by your reproach, his face expressing an approximation of regret. “Madamme, please! I know it seems hurtful now, but we are just printing what people want to read about.”"
		                    + "\nA sly smile forms on his lips. “Is it really our fault that the only thing people want to discuss is you?”"
		                    + "\nPierre appears to be relieved that you didn't call him in just to threaten him. He probably gets a lot of that in his line of work. Not that he doesn't deserve it.",
		            new EventOption("“A sweet sentiment Pierre, but you know this is unfair. At least balance out the negative with some good”", 5, 3, 6, 1),
		            new EventOption("“There are lies in this paper Pierre and you will retract them, now.”", 7, 1, 8, 3)),
		        //Stage 3
		        new EventStage("The social columnist leans back in his chair at your threat, his smile vanishes. “Now, I can see why the good Madamme would be upset,” Pierre admits, holding up his hands in a placating gesture."
		                    + "\n“I'd like to make ammends, I'm just not exactly sure what you want.”"
		                    + "\nPierre uses a handkerchief monogrammed with someone else's initials to mop a trickle of sweat from his brow. You seem to have him on the run.",
		            new EventOption("“Please Pierre, do me a favor here. At least balance out the negative with some good”.", 5, 1, 6, 3),
		            new EventOption("“There are lies in this paper Pierre and you will retract them, now.”", 7, 3, 8, 1)),
		        //Stage 4
		        new EventStage("A person of your stature needn't concern themselves with the trivial postings of a second rate newspaper. At least, that's what you tell yourself."
		                    + "\n\nUnfortunately, your fellow socialites aren't nearly as mature as you'd hoped them to be. It's not long before you hear of people openly discussing the distressing things being said about you in the press."
		                    + "\n\n-You have lost 15 more Reputation",
							new RewardVO(RewardType.Reputation, -15),
		            new EventOption("“Ugh... children, all of them!” <End Event>", -1)),
		        //Stage 5
		        new EventStage("A special evening edition of 'Le Mercure' is released, including updates to the social pages, of all things. You smile as you notice Pierre attempt to highlight your various positive qualities."
		                    + "\nIt'll be obvious to any regular reader that you're behind this sudden change of tone, but that's alright. The ability to manipulate the press has a social cachet all of its own."
		                    + "\n\n-You have gained 45 Reputation",
							new RewardVO(RewardType.Reputation, 45),
		            new EventOption("“I'm glad Pierre was willing to listen to a little reason.” <End Event>", -1)),
		        //Stage 6
		        new EventStage("A special evening edition of 'Le Mercure' is released, including updates to the social pages, of all things. You grimace as you notice that Pierre has decided to pay homage to your virtues, with statements that are obviously lies."
		                    + "\nPierre barely bothered to do his research. Now, in addition to being bad-mouthed by the press, they're saying positive things that can't possibly be true. This will further cement people's newfound negative opinions of you."
		                    + "\n\n-You have lost 30 more Reputation",
							new RewardVO(RewardType.Reputation, -30),
		            new EventOption("“That's the last time I try being nice.” <End Event>", -1)),
		        //Stage 7
		        new EventStage("Camille returns from her errands with a special evening edition of 'Le Mercure', including updates to the social pages. It seems like Pierre has taken your threat to heart and retracted several things said about you, even some of the things that are true!"
		                    + "\nIt'll be obvious to anyone that you're behind this sudden series of retractions, but that's alright. The ability to bully the press has a social cachet all of its own."
		                    + "\n\n-You have gained 45 Reputation",
							new RewardVO(RewardType.Reputation, 45),
		            new EventOption("“I'm glad Pierre understands not to get on my bad side.” <End Event>", -1)),

		        //Stage 8
		        new EventStage("Camille returns from her errands with a special evening edition of 'Le Mercure', including updates to the social pages. You scowl as you read Pierre's excessively exacting retractions. Only the lies were retracted and absolutely nothing else."
		                    + "\nNot only is it obvious that you ordered the retractions, the specificity of the retractions confirm that every other thing about you is completely true."
		                    + "\n\n-You have lost 30 more Reputation",
							new RewardVO(RewardType.Reputation, -30),
		            new EventOption("“Well, that's the last time I try bullying the press.” <End Event>.", -1)),
		        //Stage 9
		        new EventStage("Hansel grunts and heads out the door, his face a mask of menace. Later, Camille returns from her errands with a special evening edition of 'Le Mercure', including updates to the social pages. It seems like, whatever Hansel said, or threatened to do, to Pierre has cause a real change of heart. The Newspaper has retracted several things they said about you, even some of the things that are true!"
		                    + "\nIt'll be obvious to anyone that you're behind this sudden series of retractions, but that's alright. The ability to bully the press has a social cachet all of its own."
		                    + "\n\n-You have gained 45 Reputation",
							new RewardVO(RewardType.Reputation, 45),
		            new EventOption("“Hansel is always so good at explaining things to people.” <End Event>.", -1))));

		        //---- Event 7 ---- Man of God, Feet of Clay
		        night.Add(new EventVO(1, "Man of God, Feet of Clay",
		        //Stage 0
		        new EventStage("You hear a knock at the back door during breakfast. Camille answers it and waves you over after a brief conversation."
		                    + "\nAt the back door you find a priest wearing simple robes that have obviously seen better days. He's grimy and barefoot, but smiles warmly. He holds out an empty plate."
		                    + "\n\n“Madamme, do you perchance have spare scraps for a man sworn to poverty?”",
		            new EventOption("“Pardon, a vow of poverty? I've never seen one of those.”", 1),
		            new EventOption("“I think we can spare something.” <Costs 15 Livres>", 2),
		            new EventOption("“Scraps? No, come inside and have a proper meal.” <Costs 25 Livres>", 3),
		            new EventOption("“Sorry Father, I don't have anything to spare.”", 4)),
		        //Stage 1
		        new EventStage("“They're not as common as they used to be,” the priest admits, glancing significantly to the steeple of a cathedral rising above the city skyline. If you so remember, they just had the roof redone in copper." 
		                    + "\n“However, I am in no position to cast stones.” The priest shuffles his bare feet on the cobbles “In a previous life, I was bishop who would rather spend our donations enriching myself than on helping the poor. The cardinals noticed my indiscretions and punished me as such.”"
		                    + "\n“Now I must spend the next 5 years depending on the charity of others, much like those I shunned.” He stares into his empty bowl. “Admittedly, it has not been going well, as of late. Nobody has anything to spare anymore. Do you?”",
		            new EventOption("“I think we can spare something.” <Costs 15 Livres>", 2),
		            new EventOption("“You haven't eaten in days, have you? Come inside and have a proper meal.” <Costs 25 Livres>", 3),
		            new EventOption("“Sorry Father, just like everyone else, I don't have anything to spare.”", 4)),
		        //Stage 2
		        new EventStage("The priest eagerly takes the scraps and left overs from this morning's breakfast. “Thank you again, my child. I will pray on your behalf.”"
		                    + "\nHe leaves much more happily than before, his bowl laden with provisions, albiet second hand ones."
		                    + "\n\n-You have lost 15 Livres."
		                    + "\n-You have gained 20 Reputation with the Church.",
							new RewardVO[]{ new RewardVO(RewardType.Faction, "Church", 20),
											new RewardVO(RewardType.Livre, -15)},
		            new EventOption("“Thank you Father, good luck and good day!” <End Event>", -1)),
		        //Stage 3
		        new EventStage("The priest's eyes widen as you invite him inside to share breakfast with you. He stops to pray over his food before beginning."
		                    + "\nThe man turns out to be a much more intriguing conversation partner than you would have expected. His previous life as a wealthy bishop and his new life as a beggar have given him a full perspective of the world."
		                    + "\nFinally, he takes his leave, along with a small parcel of leftovers. “Thank you, my child. I will pray for you and ensure that others do the same.”"
		                    + "\n\n-You have lost 25 Livres."
		                    + "\n-You have gained 40 Reputation with the Church.", 
							new RewardVO[]{ new RewardVO(RewardType.Faction, "Church", 40),
											new RewardVO(RewardType.Livre, -25)},
		            new EventOption("“Thank you Father! Good luck and good day!” <End Event>.", -1)),
		        //Stage 4
		        new EventStage("The priest nods along with your reply as he stares into his empty bowl. “Thank you for your time, Madamme. I understand that we live in trying times.”"
		                    + "\nYou watch him turn away and return to wandering the streets.",
		            new EventOption("“Well, he took a vow of poverty. This is what poverty looks like.” <End Event>.", -1))));

	        return night.ToArray();
		}
		*/
	}
}