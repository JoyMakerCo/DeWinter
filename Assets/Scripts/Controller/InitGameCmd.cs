using System;
using Core;

namespace Ambition
{
    public class InitGameCmd : ICommand
    {
        public void Execute()
        {
            App.Register<LocalizationSvc>();
            App.Register<ModelTrackingSvc>();
            App.Register<ModelSvc>();
            App.Register<MessageSvc>();
            App.Register<CommandSvc>();
            App.Register<UFlow.UFlowSvc>();
            App.Register<FactorySvc>();
            App.Register<RewardFactorySvc>();
            App.Register<RequirementsSvc>();
            App.Register<AssetBundleSvc>();

            AmbitionApp.RegisterModel<LocalizationModel>();
            AmbitionApp.RegisterModel<GameModel>();
            AmbitionApp.RegisterModel<FactionModel>();
            AmbitionApp.RegisterModel<InventoryModel>();
            AmbitionApp.RegisterModel<ServantModel>();
            AmbitionApp.RegisterModel<CalendarModel>();
            AmbitionApp.RegisterModel<CharacterModel>();
            AmbitionApp.RegisterModel<QuestModel>();
            AmbitionApp.RegisterModel<MapModel>();
            AmbitionApp.RegisterModel<ParisModel>();
            AmbitionApp.RegisterModel<ConsoleModel>();
            AmbitionApp.RegisterModel<IncidentModel>();
#if DEBUG
            AmbitionApp.Execute<LocalizeAssetsCmd>();
#endif
            AmbitionApp.RegisterCommand<SellItemCmd, ItemVO>(InventoryMessages.SELL_ITEM);
            AmbitionApp.RegisterCommand<BuyItemCmd, ItemVO>(InventoryMessages.BUY_ITEM);
            AmbitionApp.RegisterCommand<GrantRewardCmd, CommodityVO>();
            AmbitionApp.RegisterCommand<GrantRewardsCmd, CommodityVO[]>();
            AmbitionApp.RegisterCommand<CheckMilitaryReputationCmd, FactionVO>();
            AmbitionApp.RegisterCommand<IntroServantCmd, ServantVO>(ServantMessages.INTRODUCE_SERVANT);
            AmbitionApp.RegisterCommand<HireServantCmd, ServantVO>(ServantMessages.HIRE_SERVANT);
            AmbitionApp.RegisterCommand<FireServantCmd, ServantVO>(ServantMessages.FIRE_SERVANT);
            AmbitionApp.RegisterCommand<SelectDateCmd, DateTime>(CalendarMessages.SELECT_DATE);
            AmbitionApp.RegisterCommand<UpdateCalendarCmd>(CalendarMessages.UPDATE_CALENDAR);
            AmbitionApp.RegisterCommand<AdvanceDayCmd>(CalendarMessages.NEXT_DAY);
            AmbitionApp.RegisterCommand<AdjustFactionCmd, AdjustFactionVO>(FactionMessages.ADJUST_FACTION);
            AmbitionApp.RegisterCommand<EquipItemCmd, ItemVO>(InventoryMessages.EQUIP);
            AmbitionApp.RegisterCommand<GenerateOutfitCmd, ItemVO>(InventoryMessages.GENERATE_OUTFIT);
            AmbitionApp.RegisterCommand<UnequipItemCmd, ItemVO>(InventoryMessages.UNEQUIP);
            AmbitionApp.RegisterCommand<UnequipSlotCmd, ItemType>(InventoryMessages.UNEQUIP);
            AmbitionApp.RegisterCommand<AddLocationCmd, string>(ParisMessages.ADD_LOCATION);
            AmbitionApp.RegisterCommand<RemoveLocationCmd, string>(ParisMessages.REMOVE_LOCATION);
            AmbitionApp.RegisterCommand<GoToPartyCmd, PartyVO>(PartyMessages.GO_TO_PARTY);
            AmbitionApp.RegisterCommand<RestockMerchantCmd, DateTime>();
            AmbitionApp.RegisterCommand<PerilIncidentCmd, int>(GameConsts.PERIL);
            AmbitionApp.RegisterCommand<ResetGameCmd>(GameMessages.EXIT_MENU);
            AmbitionApp.RegisterCommand<CreateGossipCmd, FactionType>(InventoryMessages.CREATE_GOSSIP); 

            // Incidents
            AmbitionApp.RegisterCommand<UpdateIncidentsCmd>(CalendarMessages.UPDATE_CALENDAR);
            AmbitionApp.RegisterCommand<ScheduleIncidentCmd, IncidentVO>(CalendarMessages.SCHEDULE);
            AmbitionApp.RegisterCommand<CompleteIncidentCmd, IncidentVO>(CalendarMessages.CALENDAR_EVENT_COMPLETED);

            // Incidents
            AmbitionApp.RegisterCommand<UpdateIncidentsCmd>(CalendarMessages.UPDATE_CALENDAR);
            AmbitionApp.RegisterCommand<ScheduleIncidentCmd, IncidentVO>(CalendarMessages.SCHEDULE);
            AmbitionApp.RegisterCommand<CompleteIncidentCmd, IncidentVO>(CalendarMessages.CALENDAR_EVENT_COMPLETED);

            // Party
            AmbitionApp.RegisterCommand<InitPartyCmd, PartyVO>(PartyMessages.INITIALIZE_PARTY);
            AmbitionApp.RegisterCommand<AcceptInvitationCmd, PartyVO>(PartyMessages.ACCEPT_INVITATION);
            AmbitionApp.RegisterCommand<DeclineInvitationCmd, PartyVO>(PartyMessages.DECLINE_INVITATION);
            AmbitionApp.RegisterCommand<TargetGuestCmd, CharacterVO>(PartyMessages.TARGET_GUEST);
            AmbitionApp.RegisterCommand<SelectGuestCmd, CharacterVO>(PartyMessages.SELECT_GUEST);
            AmbitionApp.RegisterCommand<GuestSelectedCmd, CharacterVO>(PartyMessages.GUEST_SELECTED);
            AmbitionApp.RegisterCommand<GuestIgnoredCmd, CharacterVO>(PartyMessages.GUEST_IGNORED);
            AmbitionApp.RegisterCommand<CharmGuestCmd, CharacterVO>(PartyMessages.GUEST_CHARMED);
            AmbitionApp.RegisterCommand<OffendGuestCmd, CharacterVO>(PartyMessages.GUEST_OFFENDED);
            AmbitionApp.RegisterCommand<AmbushCmd, RoomVO>(PartyMessages.AMBUSH);
            AmbitionApp.RegisterCommand<FillHandCmd>(PartyMessages.FILL_REMARKS);
            AmbitionApp.RegisterCommand<RefillDrinkCmd>(PartyMessages.REFILL_DRINK);
            AmbitionApp.RegisterCommand<GrantRemarkCmd>(PartyMessages.FREE_REMARK);
            AmbitionApp.RegisterCommand<BurnCmd, int>(PartyMessages.BURN_REMARKS);
            AmbitionApp.RegisterCommand<DiscardCmd, RemarkVO>(PartyMessages.DISCARD);
            AmbitionApp.RegisterCommand<ReshuffleCmd, int>(PartyMessages.RESHUFFLE_REMARKS);
            AmbitionApp.RegisterCommand<DrawCmd, int>(PartyMessages.DRAW_REMARKS);
            AmbitionApp.RegisterCommand<DrawOneCmd>(PartyMessages.DRAW_REMARK);
            AmbitionApp.RegisterCommand<SetFashionCmd, PartyVO>(PartyMessages.PARTY_STARTED);
            //AmbitionApp.RegisterCommand<FactionTurnModifierCmd, PartyVO>(PartyMessages.PARTY_STARTED);
            AmbitionApp.RegisterCommand<RoomChoiceCmd, RoomVO>();
            //AmbitionApp.RegisterCommand<LeavePartyCmd>(PartyMessages.LEAVE_PARTY);
            AmbitionApp.RegisterCommand<EndPartyCmd>(PartyMessages.END_PARTY);
            AmbitionApp.RegisterCommand<ShowRoomCmd, IncidentVO>(PartyMessages.SHOW_ROOM);

            AmbitionApp.RegisterCommand<PayDayCmd, DateTime>();
            AmbitionApp.RegisterCommand<RestockMerchantCmd, DateTime>();
            AmbitionApp.RegisterCommand<CheckLivreCmd, int>(GameConsts.LIVRE);

            // Initially enabled for TUTORIAL
            AmbitionApp.RegisterCommand<StartTutorialCmd>(GameMessages.START_TUTORIAL);
            AmbitionApp.RegisterCommand<SkipTutorialCmd>(GameMessages.SKIP_TUTORIAL);

            // Audio
            AmbitionApp.RegisterCommand<PlaySoundCmd, FMODEvent>(AudioMessages.PLAY);

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
            
			AmbitionApp.RegisterRequirement(CommodityType.Chance, ChanceReq.Check);
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

            AmbitionApp.Execute<RegisterPartyControllerCmd>();
            AmbitionApp.Execute<RegisterEstateControllerCmd>();
            AmbitionApp.Execute<RegisterIncidentControllerCmd>();
            AmbitionApp.Execute<RegisterParisControllerCmd>();
            AmbitionApp.Execute<RegisterDayFlowControllerCommand>();

            //AmbitionApp.GetService<AssetBundleSvc>().Load(AssetBundleIDs.ON_LOAD, HandleLoaded);
        }

        private void HandleLoaded(UnityEngine.AssetBundle bundle)
        {

        }
    }
}
