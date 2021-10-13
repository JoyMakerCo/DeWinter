using System;
using Core;

namespace Ambition
{
    public class InitGameCmd : ICommand
    {
        public void Execute()
        {
            Util.RNG.Randomize();
            // Model, Message, Command, and Loc services registered in StartupInitBehavior
            App.Register<UFlow.UFlowSvc>();
            App.Register<FactorySvc>();
            App.Register<RewardFactorySvc>();
            App.Register<RequirementsSvc>();
            App.Register<ModelSvc>();

            CalendarModel calendar = AmbitionApp.RegisterModel<CalendarModel>();
            AmbitionApp.RegisterModel<LocalizationModel>();
            AmbitionApp.RegisterModel<ParisModel>();
            AmbitionApp.RegisterModel<GossipModel>();
            IncidentModel incidents = AmbitionApp.RegisterModel<IncidentModel>();
#if DEBUG
            AmbitionApp.RegisterModel<ConsoleModel>();
#endif

            // MENU
            AmbitionApp.RegisterCommand<AutosaveCmd>(GameMessages.AUTOSAVE);
            AmbitionApp.RegisterCommand<SaveGameCmd>(GameMessages.SAVE_GAME);
            AmbitionApp.RegisterCommand<ResetGameCmd>(GameMessages.EXIT_GAME);

            AmbitionApp.RegisterCommand<SchedulePartyCmd, PartyVO>(CalendarMessages.SCHEDULE);
            AmbitionApp.RegisterCommand<ScheduleIncidentCmd, IncidentVO>(CalendarMessages.SCHEDULE);
            AmbitionApp.RegisterCommand<ScheduleRendezvousCmd, RendezVO>(CalendarMessages.SCHEDULE);
            AmbitionApp.RegisterCommand<TransitionInputCmd, TransitionVO>(IncidentMessages.TRANSITION);
            AmbitionApp.RegisterCommand<SellItemCmd, ItemVO>(InventoryMessages.SELL_ITEM);
            AmbitionApp.RegisterCommand<SellGossipCmd, GossipVO>(InventoryMessages.SELL_GOSSIP);
            AmbitionApp.RegisterCommand<PeddleInfluenceCmd, GossipVO>(InventoryMessages.PEDDLE_INFLUENCE);
            AmbitionApp.RegisterCommand<BuyItemCmd, ItemVO>(InventoryMessages.BUY_ITEM);
            AmbitionApp.RegisterCommand<DeleteItemCmd, ItemVO>(InventoryMessages.DELETE_ITEM);
            AmbitionApp.RegisterCommand<InitPartyCmd, PartyVO>(PartyMessages.INITIALIZE_PARTY);

            AmbitionApp.RegisterCommand<GrantRewardCmd, CommodityVO>();
            AmbitionApp.RegisterCommand<GrantRewardsCmd, CommodityVO[]>();
            AmbitionApp.RegisterCommand<CheckMilitaryReputationCmd, FactionVO>();
            AmbitionApp.RegisterCommand<IntroServantCmd, string>(ServantMessages.INTRODUCE_SERVANT);
            AmbitionApp.RegisterCommand<HireServantCmd, string>(ServantMessages.HIRE_SERVANT);
            AmbitionApp.RegisterCommand<FireServantCmd, string>(ServantMessages.FIRE_SERVANT);
            AmbitionApp.RegisterCommand<FireServantTypeCmd, ServantType>(ServantMessages.FIRE_SERVANT);
            AmbitionApp.RegisterCommand<SelectDateCmd, DateTime>(CalendarMessages.SELECT_DATE);
            AmbitionApp.RegisterCommand<UpdateGossipModelCmd, CalendarModel>(CalendarMessages.UPDATE_CALENDAR);
            AmbitionApp.RegisterCommand<GenerateInvitationsCmd, CalendarModel>(CalendarMessages.UPDATE_CALENDAR);
            AmbitionApp.RegisterCommand<GenerateRendezvousCmd, CalendarModel>(CalendarMessages.UPDATE_CALENDAR);
            AmbitionApp.RegisterCommand<PoliticalIncidentCmd, CalendarModel>(CalendarMessages.UPDATE_CALENDAR);
            AmbitionApp.RegisterCommand<UpdateStandingsCmd>(FactionMessages.UPDATE_STANDINGS);
            AmbitionApp.RegisterCommand<EquipItemCmd, ItemVO>(InventoryMessages.EQUIP);
            AmbitionApp.RegisterCommand<UnequipItemCmd, ItemVO>(InventoryMessages.UNEQUIP);
            AmbitionApp.RegisterCommand<UnequipItemSlotCmd, ItemType>(InventoryMessages.UNEQUIP);
            AmbitionApp.RegisterCommand<GoToPartyCmd, PartyVO>(PartyMessages.GO_TO_PARTY);
            AmbitionApp.RegisterCommand<CreateGossipCmd, GossipVO>(InventoryMessages.CREATE_GOSSIP);
            AmbitionApp.RegisterCommand<CompleteQuestCmd, QuestVO>(QuestMessages.COMPLETE_QUEST);
            AmbitionApp.RegisterCommand<FailQuestCmd, QuestVO>(QuestMessages.QUEST_FAILED);
            AmbitionApp.RegisterCommand<FailLastQuestCmd>(QuestMessages.QUEST_FAILED);
            AmbitionApp.RegisterCommand<UpdateMerchantCmd>(InventoryMessages.UPDATE_MERCHANT);
            AmbitionApp.RegisterCommand<StartTutorialCmd, string>(TutorialMessages.START_TUTORIAL);
            AmbitionApp.RegisterCommand<EndTutorialCmd, string>(TutorialMessages.END_TUTORIAL);

            // Party
            AmbitionApp.RegisterCommand<RoomChoiceCmd, RoomVO>();
            AmbitionApp.RegisterCommand<ShowRoomCmd, string>(PartyMessages.SHOW_ROOM);

            AmbitionApp.RegisterCommand<IncreaseExhaustionCmd>(GameMessages.ADD_EXHAUSTION);
            AmbitionApp.RegisterCommand<ApplyExhaustionPenaltyCmd>(GameMessages.EXHAUSTION_EFFECT);
            AmbitionApp.RegisterCommand<ApplyOutfitEffectCmd>(GameMessages.OUTFIT_EFFECT);
            AmbitionApp.RegisterCommand<RestAtHomeCmd>(ParisMessages.REST);
            AmbitionApp.RegisterCommand<CheckPerilCmd, int>(GameConsts.PERIL);

            AmbitionApp.RegisterCommand<AcceptInvitationCmd, PartyVO>(PartyMessages.ACCEPT_INVITATION);
            AmbitionApp.RegisterCommand<DeclineInvitationCmd, PartyVO>(PartyMessages.DECLINE_INVITATION);

            AmbitionApp.RegisterCommand<AcceptRendezvousCmd, RendezVO>(PartyMessages.ACCEPT_INVITATION);
            AmbitionApp.RegisterCommand<DeclineRendezvousCmd, RendezVO>(PartyMessages.DECLINE_INVITATION);

            // Paris
            AmbitionApp.RegisterCommand<SelectDailiesCmd, string[]>(ParisMessages.SELECT_DAILIES);
            AmbitionApp.RegisterCommand<ChooseRendezvousCmd, string>(ParisMessages.CHOOSE_LOCATION);
            AmbitionApp.RegisterCommand<ChooseLocationCmd, string>(ParisMessages.CHOOSE_LOCATION);
            AmbitionApp.RegisterCommand<CalculateRendezvousResponseCmd, RendezVO>(RendezvousMessages.CREATE_RENDEZVOUS_RESPONSE);

            // REWARDS
            AmbitionApp.RegisterReward<LivreReward>(CommodityType.Livre);
            AmbitionApp.RegisterReward<GossipReward>(CommodityType.Gossip);
            AmbitionApp.RegisterReward<ItemReward>(CommodityType.Item);
            AmbitionApp.RegisterReward<ServantReward>(CommodityType.Servant);
            AmbitionApp.RegisterReward<MessageReward>(CommodityType.Message);
            AmbitionApp.RegisterReward<IncidentReward>(CommodityType.Incident);
            AmbitionApp.RegisterReward<LocationReward>(CommodityType.Location);
            AmbitionApp.RegisterReward<PartyReward>(CommodityType.Party);
            AmbitionApp.RegisterReward<CredReward>(CommodityType.Credibility);
            AmbitionApp.RegisterReward<PerilReward>(CommodityType.Peril);
            AmbitionApp.RegisterReward<FavorReward>(CommodityType.Favor);
            AmbitionApp.RegisterReward<FactionAllegianceReward>(CommodityType.Allegiance);
            AmbitionApp.RegisterReward<FactionPowerReward>(CommodityType.Power);
            AmbitionApp.RegisterReward<LiaisonReward>(CommodityType.Liaison);
            AmbitionApp.RegisterReward<MiscReward>(CommodityType.Misc);
            AmbitionApp.RegisterReward<TutorialReward>(CommodityType.Tutorial);
            AmbitionApp.RegisterReward<CharacterReward>(CommodityType.Character);
            AmbitionApp.RegisterReward<RendezvousOutfitReward>(CommodityType.RendezvousOutfit);
            AmbitionApp.RegisterReward<QuestReward>(CommodityType.Quest);
            AmbitionApp.RegisterReward<ExhaustionReward>(CommodityType.Exhaustion);
            AmbitionApp.RegisterReward<DateReward>(CommodityType.Date);

            // REQUIREMENTS
            AmbitionApp.RegisterRequirement(CommodityType.Chance, ChanceReq.Check);
            AmbitionApp.RegisterRequirement(CommodityType.Random, ChanceReq.Check);
            AmbitionApp.RegisterRequirement(CommodityType.Livre, LivreReq.Check);
            AmbitionApp.RegisterRequirement(CommodityType.Credibility, CredReq.Check);
            AmbitionApp.RegisterRequirement(CommodityType.Peril, PerilReq.Check);
            AmbitionApp.RegisterRequirement(CommodityType.Date, DateReq.Check);
            AmbitionApp.RegisterRequirement(CommodityType.Item, ItemReq.Check);
            AmbitionApp.RegisterRequirement(CommodityType.Location, LocationReq.Check);
            AmbitionApp.RegisterRequirement(CommodityType.Servant, ServantReq.Check);
            AmbitionApp.RegisterRequirement(CommodityType.Favor, FavorReq.Check);
            AmbitionApp.RegisterRequirement(CommodityType.Allegiance, FactionAllegianceReq.Check);
            AmbitionApp.RegisterRequirement(CommodityType.Power, FactionPowerReq.Check);
            AmbitionApp.RegisterRequirement(CommodityType.Exhaustion, ExhaustionReq.Check);
            AmbitionApp.RegisterRequirement(CommodityType.Quest, ActiveQuestReq.Check);
            AmbitionApp.RegisterRequirement(CommodityType.Incident, IncidentReq.Check);
            AmbitionApp.RegisterRequirement(CommodityType.OutfitReaction, OutfitReactionReq.Check);
            AmbitionApp.RegisterRequirement(CommodityType.Liaison, LiaisonReq.Check);
            AmbitionApp.RegisterRequirement(CommodityType.Misc, MiscReq.Check);
            AmbitionApp.RegisterRequirement(CommodityType.Rendezvous, RendezvousReq.Check);
            AmbitionApp.RegisterRequirement(CommodityType.RendezvousFavor, RendezvousFavorReq.Check);
            AmbitionApp.RegisterRequirement(CommodityType.RendezvousOutfit, RendezvousOutfitReq.Check);
            AmbitionApp.RegisterRequirement(CommodityType.Character, CharacterReq.Check);

            AmbitionApp.UFlow.Register<GameFlow>(FlowConsts.GAME_CONTROLLER);
            AmbitionApp.UFlow.Register<PartyFlow>(FlowConsts.PARTY_CONTROLLER);
            AmbitionApp.UFlow.Register<EstateFlow>(FlowConsts.ESTATE_CONTROLLER);
            AmbitionApp.UFlow.Register<IncidentFlow>(FlowConsts.INCIDENT_CONTROLLER);
            AmbitionApp.UFlow.Register<ParisFlow>(FlowConsts.PARIS_CONTROLLER);
            AmbitionApp.UFlow.Register<DayFlow>(FlowConsts.DAY_FLOW_CONTROLLER);
            AmbitionApp.UFlow.Register<RendezvousFlow>(FlowConsts.RENDEZVOUS_CONTROLLER);

            //AmbitionApp.GetService<AssetBundleSvc>().Load(AssetBundleIDs.ON_LOAD, HandleLoaded);
            AmbitionApp.Game.Initialized = true;
        }
    }
}
