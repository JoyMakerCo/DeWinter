using System;
using Core;

namespace Ambition
{
    public class InitGameCmd : ICommand
    {
        public void Execute()
        {
            // Model, Message, Command, and Loc services registered in StartupInitBehavior
            App.Register<UFlow.UFlowSvc>();
            App.Register<FactorySvc>();
            App.Register<RewardFactorySvc>();
            App.Register<RequirementsSvc>();
            App.Register<ModelSvc>();

            GameModel game = AmbitionApp.RegisterModel<GameModel>();
            AmbitionApp.RegisterModel<LocalizationModel>();
            AmbitionApp.RegisterModel<FactionModel>();
            AmbitionApp.RegisterModel<InventoryModel>();
            AmbitionApp.RegisterModel<ServantModel>();
            CalendarModel calendar = AmbitionApp.RegisterModel<CalendarModel>();
            AmbitionApp.RegisterModel<CharacterModel>();
            AmbitionApp.RegisterModel<QuestModel>();
            AmbitionApp.RegisterModel<PartyModel>();
            AmbitionApp.RegisterModel<ParisModel>();
            IncidentModel incidents = AmbitionApp.RegisterModel<IncidentModel>();
#if DEBUG
            AmbitionApp.RegisterModel<ConsoleModel>();
#endif

            AmbitionApp.RegisterCommand<LoadPartyCmd, string>(PartyMessages.LOAD_PARTY);
            AmbitionApp.RegisterCommand<ScheduleOccasionCmd, OccasionVO>(CalendarMessages.SCHEDULE);
            AmbitionApp.RegisterCommand<SchedulePartyCmd, PartyVO>(CalendarMessages.SCHEDULE);
            AmbitionApp.RegisterCommand<ScheduleIncidentCmd, IncidentVO>(CalendarMessages.SCHEDULE);
            AmbitionApp.RegisterCommand<TransitionInputCmd, TransitionVO>(IncidentMessages.TRANSITION);
            AmbitionApp.RegisterCommand<SellItemCmd, ItemVO>(InventoryMessages.SELL_ITEM);
            AmbitionApp.RegisterCommand<SellGossipCmd, ItemVO>(InventoryMessages.SELL_GOSSIP);
            AmbitionApp.RegisterCommand<PeddleGossipCmd, ItemVO>(InventoryMessages.PEDDLE_GOSSIP);
            AmbitionApp.RegisterCommand<BuyItemCmd, ItemVO>(InventoryMessages.BUY_ITEM);
            AmbitionApp.RegisterCommand<DeleteItemCmd, ItemVO>(InventoryMessages.DELETE_ITEM);

            AmbitionApp.RegisterCommand<GrantRewardCmd, CommodityVO>();
            AmbitionApp.RegisterCommand<GrantRewardsCmd, CommodityVO[]>();
            AmbitionApp.RegisterCommand<SetCommodityCmd, CommodityVO>(CommodityMessages.SET_COMMODITY);
            AmbitionApp.RegisterCommand<CheckMilitaryReputationCmd, FactionVO>();
            AmbitionApp.RegisterCommand<IntroServantCmd, ServantVO>(ServantMessages.INTRODUCE_SERVANT);
            AmbitionApp.RegisterCommand<HireServantCmd, ServantVO>(ServantMessages.HIRE_SERVANT);
            AmbitionApp.RegisterCommand<FireServantCmd, ServantVO>(ServantMessages.FIRE_SERVANT);
            AmbitionApp.RegisterCommand<SelectDateCmd, DateTime>(CalendarMessages.SELECT_DATE);
            AmbitionApp.RegisterCommand<AdvanceDayCmd>(CalendarMessages.NEXT_DAY);
            AmbitionApp.RegisterCommand<AdjustFactionCmd, AdjustFactionVO>(FactionMessages.ADJUST_FACTION);
            AmbitionApp.RegisterCommand<SetFactionValuesCmd, AdjustFactionVO>(FactionMessages.SET_FACTION);
            AmbitionApp.RegisterCommand<EquipItemCmd, ItemVO>(InventoryMessages.EQUIP);
            AmbitionApp.RegisterCommand<GenerateOutfitCmd, ItemVO>(InventoryMessages.GENERATE_OUTFIT);
            AmbitionApp.RegisterCommand<UnequipItemCmd, ItemVO>(InventoryMessages.UNEQUIP);
            AmbitionApp.RegisterCommand<UnequipItemSlotCmd, ItemType>(InventoryMessages.UNEQUIP);
            AmbitionApp.RegisterCommand<AddLocationCmd, string>(ParisMessages.ADD_LOCATION);
            AmbitionApp.RegisterCommand<RemoveLocationCmd, string>(ParisMessages.REMOVE_LOCATION);
            AmbitionApp.RegisterCommand<GoToPartyCmd, PartyVO>(PartyMessages.GO_TO_PARTY);
            AmbitionApp.RegisterCommand<PerilIncidentCmd, int>(GameConsts.PERIL);
            AmbitionApp.RegisterCommand<CreateGossipCmd, GossipRewardSpec>(InventoryMessages.CREATE_GOSSIP);
            AmbitionApp.RegisterCommand<RestockMerchantCmd>(InventoryMessages.RESTOCK_MERCHANT);
            AmbitionApp.RegisterCommand<ShowHeaderCmd, string>(GameMessages.SHOW_HEADER);
            AmbitionApp.RegisterCommand<SetHeaderTitleCmd, string>(GameMessages.SHOW_HEADER);

            // MENU
            AmbitionApp.RegisterCommand<SaveGameCmd>(GameMessages.SAVE_GAME);
            AmbitionApp.RegisterCommand<OpenSavedGameDialog>(GameMessages.LOAD_GAME);
            AmbitionApp.RegisterCommand<LoadSavedGameCmd, string>(GameMessages.LOAD_GAME);
            AmbitionApp.RegisterCommand<ResetGameCmd>(GameMessages.EXIT_GAME);
            AmbitionApp.RegisterCommand<QuitGameCmd>(GameMessages.QUIT_GAME);

            // Party
            AmbitionApp.RegisterCommand<InitPartyCmd, PartyVO>(PartyMessages.INITIALIZE_PARTY);
            AmbitionApp.RegisterCommand<SetFashionCmd, PartyVO>(PartyMessages.PARTY_STARTED);
            //AmbitionApp.RegisterCommand<FactionTurnModifierCmd, PartyVO>(PartyMessages.PARTY_STARTED);
            AmbitionApp.RegisterCommand<RoomChoiceCmd, RoomVO>();
            AmbitionApp.RegisterCommand<SelectIncidentsCmd, string[]>(PartyMessages.SELECT_INCIDENTS);
            AmbitionApp.RegisterCommand<ShowRoomCmd, string>(PartyMessages.SHOW_ROOM);

            AmbitionApp.RegisterCommand<PayDayCmd, DateTime>();
            AmbitionApp.RegisterCommand<CheckLivreCmd, int>(GameConsts.LIVRE);

            AmbitionApp.RegisterCommand<InitPartyCmd, PartyVO>(PartyMessages.INITIALIZE_PARTY);
            AmbitionApp.RegisterCommand<AcceptInvitationCmd, PartyVO>(PartyMessages.ACCEPT_INVITATION);
            AmbitionApp.RegisterCommand<DeclineInvitationCmd, string>(PartyMessages.DECLINE_INVITATION);


            // Audio
            AmbitionApp.RegisterCommand<PlaySoundCmd, FMODEvent>(AudioMessages.PLAY);

            // Paris
            AmbitionApp.RegisterCommand<EnableDiscoverableLocationsCmd>(ParisMessages.ENABLE_DISCOVERABLE_LOCATIONS);
            AmbitionApp.RegisterCommand<DisableDiscoverableLocationsCmd>(ParisMessages.DISABLE_DISCOVERABLE_LOCATIONS);
            AmbitionApp.RegisterCommand<ChooseLocationCmd, LocationVO>(ParisMessages.GO_TO_LOCATION);

            // Rewards
            AmbitionApp.RegisterReward<LivreReward>(CommodityType.Livre);
            AmbitionApp.RegisterReward<RepReward>(CommodityType.Reputation);
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
            AmbitionApp.RegisterReward<FactionAllegianceReward>(CommodityType.FactionAllegiance);
            AmbitionApp.RegisterReward<FactionPowerReward>(CommodityType.FactionPower);
            AmbitionApp.RegisterReward<ActiveQuestReward>(CommodityType.ActiveQuest);

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
            AmbitionApp.RegisterRequirement(CommodityType.FactionAllegiance, FactionAllegianceReq.Check);
            AmbitionApp.RegisterRequirement(CommodityType.FactionPower, FactionPowerReq.Check);
            AmbitionApp.RegisterRequirement(CommodityType.Exhaustion, ExhaustionReq.Check);
            AmbitionApp.RegisterRequirement(CommodityType.ActiveQuest, ActiveQuestReq.Check);
            AmbitionApp.RegisterRequirement(CommodityType.Incident, IncidentReq.Check);
            AmbitionApp.RegisterRequirement(CommodityType.OutfitReaction, OutfitReactionReq.Check);

            AmbitionApp.Execute<RegisterPartyControllerCmd, string>(FlowConsts.PARTY_CONTROLLER);
            AmbitionApp.Execute<RegisterEstateControllerCmd, string>(FlowConsts.ESTATE_CONTROLLER);
            AmbitionApp.Execute<RegisterIncidentControllerCmd, string>(FlowConsts.INCIDENT_CONTROLLER);
            AmbitionApp.Execute<RegisterParisControllerCmd, string>(FlowConsts.PARIS_CONTROLLER);
            AmbitionApp.Execute<RegisterDayFlowControllerCommand, string>(FlowConsts.DAY_FLOW_CONTROLLER);

            //AmbitionApp.GetService<AssetBundleSvc>().Load(AssetBundleIDs.ON_LOAD, HandleLoaded);
        }
    }
}
