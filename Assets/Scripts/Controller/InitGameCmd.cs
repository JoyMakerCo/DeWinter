using System;
using Core;

namespace Ambition
{
    public class InitGameCmd : ICommand<PlayerConfig>
	{
        public void Execute(PlayerConfig config)
        {
            AmbitionApp.RegisterModel<LocalizationModel>();
			AmbitionApp.RegisterModel<GameModel>();
			AmbitionApp.RegisterModel<FactionModel>();
			AmbitionApp.RegisterModel<InventoryModel>();
			AmbitionApp.RegisterModel<ServantModel>();
			AmbitionApp.RegisterModel<CalendarModel>();
			AmbitionApp.RegisterModel<PartyModel>();
			AmbitionApp.RegisterModel<CharacterModel>();
			AmbitionApp.RegisterModel<QuestModel>();
			AmbitionApp.RegisterModel<MapModel>();
            AmbitionApp.RegisterModel<ConversationModel>();
            AmbitionApp.RegisterModel<ParisModel>();

            // Initialize Selected Player
            AmbitionApp.GetModel<GameModel>().PlayerName = config.name;
            CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();
            Array.ForEach(config.Incidents, i => calendar.Schedule(i.Incident));

            AmbitionApp.RegisterCommand<SellItemCmd, ItemVO>(InventoryMessages.SELL_ITEM);
			AmbitionApp.RegisterCommand<BuyItemCmd, ItemVO>(InventoryMessages.BUY_ITEM);
			AmbitionApp.RegisterCommand<GrantRewardCmd, CommodityVO>();
            AmbitionApp.RegisterCommand<GrantRewardsCmd, CommodityVO[]>();
            AmbitionApp.RegisterCommand<CheckMilitaryReputationCmd, FactionVO>();
            AmbitionApp.RegisterCommand<DegradeOutfitCmd>(PartyMessages.END_PARTY);
			AmbitionApp.RegisterCommand<IntroServantCmd, ServantVO>(ServantMessages.INTRODUCE_SERVANT);
			AmbitionApp.RegisterCommand<HireServantCmd, ServantVO>(ServantMessages.HIRE_SERVANT);
			AmbitionApp.RegisterCommand<FireServantCmd, ServantVO>(ServantMessages.FIRE_SERVANT);
			AmbitionApp.RegisterCommand<QuitCmd>(GameMessages.QUIT_GAME);
			AmbitionApp.RegisterCommand<GoToRoomCmd, RoomVO>(MapMessage.GO_TO_ROOM);
            AmbitionApp.RegisterCommand<InvokeMachineCmd, string>(PartyMessages.START_PARTY, "PartyController");
			AmbitionApp.RegisterCommand<UpdatePartyCmd, PartyVO>();
			AmbitionApp.RegisterCommand<SelectDateCmd, DateTime>(CalendarMessages.SELECT_DATE);
            AmbitionApp.RegisterCommand<AdvanceDayCmd>(CalendarMessages.NEXT_DAY);
            AmbitionApp.RegisterCommand<CreateEnemyCmd, string>(GameMessages.CREATE_ENEMY);
			AmbitionApp.RegisterCommand<AdjustFactionCmd, AdjustFactionVO>(FactionConsts.ADJUST_FACTION);
			AmbitionApp.RegisterCommand<EquipItemCmd, ItemVO>(InventoryMessages.EQUIP);
			AmbitionApp.RegisterCommand<UnequipItemCmd, ItemVO>(InventoryMessages.UNEQUIP);
			AmbitionApp.RegisterCommand<UnequipSlotCmd, string>(InventoryMessages.UNEQUIP);
            AmbitionApp.RegisterCommand<AddLocationCmd, string>(ParisMessages.ADD_LOCATION);
            AmbitionApp.RegisterCommand<RemoveLocationCmd, string>(ParisMessages.REMOVE_LOCATION);
            AmbitionApp.RegisterCommand<InvokeMachineCmd, string>(IncidentMessages.START_INCIDENT, "IncidentController");
            AmbitionApp.RegisterCommand<GoToPartyCmd, PartyVO>(PartyMessages.GO_TO_PARTY);

            // Party
            AmbitionApp.RegisterCommand<TargetGuestCmd, GuestVO>(PartyMessages.TARGET_GUEST);
            AmbitionApp.RegisterCommand<SelectGuestCmd, GuestVO>(PartyMessages.SELECT_GUEST);
            AmbitionApp.RegisterCommand<EnemyAttackCmd, EnemyVO>(PartyMessages.GUEST_SELECTED);
            AmbitionApp.RegisterCommand<GuestSelectedCmd, GuestVO>(PartyMessages.GUEST_SELECTED);
            AmbitionApp.RegisterCommand<GuestIgnoredCmd, GuestVO>(PartyMessages.GUEST_IGNORED);
            AmbitionApp.RegisterCommand<CharmGuestCmd, GuestVO>(PartyMessages.GUEST_CHARMED);
            AmbitionApp.RegisterCommand<OffendGuestCmd, GuestVO>(PartyMessages.GUEST_OFFENDED);
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
			AmbitionApp.RegisterCommand<FactionTurnModifierCmd, PartyVO>(PartyMessages.PARTY_STARTED);
			AmbitionApp.RegisterCommand<RoomChoiceCmd, RoomVO>();
			AmbitionApp.RegisterCommand<EndPartyCmd>(PartyMessages.LEAVE_PARTY);

            AmbitionApp.RegisterCommand<PayDayCmd, DateTime>();
			AmbitionApp.RegisterCommand<RestockMerchantCmd, DateTime>();
			AmbitionApp.RegisterCommand<CheckLivreCmd, int>(GameConsts.LIVRE);

            // Paris
            AmbitionApp.RegisterCommand<RestAtHomeCmd>(ParisMessages.REST);
            AmbitionApp.RegisterCommand<RestockMerchantCmd, DateTime>();

            // Initially enabled for TUTORIAL
            AmbitionApp.RegisterCommand<StartTutorialCmd>(GameMessages.START_TUTORIAL);
			AmbitionApp.RegisterCommand<SkipTutorialCmd>(GameMessages.SKIP_TUTORIAL);

            // Rewards
            AmbitionApp.RegisterReward<LivreReward>(CommodityType.Livre);
            AmbitionApp.RegisterReward<RepReward>(CommodityType.Reputation);
            AmbitionApp.RegisterReward<GossipReward>(CommodityType.Gossip);
            AmbitionApp.RegisterReward<EnemyReward>(CommodityType.Enemy);
            AmbitionApp.RegisterReward<ItemReward>(CommodityType.Item);
            AmbitionApp.RegisterReward<ServantReward>(CommodityType.Servant);
            AmbitionApp.RegisterReward<MessageReward>(CommodityType.Message);
            AmbitionApp.RegisterReward<IncidentReward>(CommodityType.Incident);
            AmbitionApp.RegisterReward<LocationReward>(CommodityType.Location);
            AmbitionApp.RegisterReward<PartyReward>(CommodityType.Party);

            AmbitionApp.Execute<RegisterPartyControllerCmd>();
            AmbitionApp.Execute<RegisterConversationControllerCmd>();
            AmbitionApp.Execute<RegisterEstateControllerCmd>();
            AmbitionApp.Execute<RegisterIncidentControllerCmd>();
            AmbitionApp.Execute<RegisterGuestActionControllerCmd>();
            AmbitionApp.Execute<RegisterParisControllerCmd>();

            AmbitionApp.InvokeMachine("EstateController");
        }
	}
}
