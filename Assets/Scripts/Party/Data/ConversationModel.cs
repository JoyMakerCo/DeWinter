using System;
using System.Collections.Generic;
using Core;

namespace Ambition
{
    public class ConversationModel : Model
    {
        public int FreeRemarkCounter;
        //TODO: Temp, until buffs are figured out
        public bool ItemEffect;
        public bool Repartee;
        public int RemarksBought;

        private PartyModel _model;
        private MapModel _map;

        private int _round;
        public int Round
        {
            get { return _round; }
            set {
                _round = value;
                AmbitionApp.SendMessage<int>(PartyMessages.ROUND, _round);
            }
        }

        public PartyVO Party => _model.Party;

        private RemarkVO _remark;
        public RemarkVO Remark
        {
            get { return _remark;  }
            set {
                _remark = value;
                AmbitionApp.SendMessage(_remark);
            }
        }

        private RemarkVO[] _remarks;
        public RemarkVO[] Remarks
        {
            get { return _remarks; }
            set { AmbitionApp.SendMessage(_remarks = value); }
        }

        //public CharacterVO[] Guests => Room?.Guests;
        //public RoomVO Room => _map.Room;

        public ConversationModel()
        {
            _model = AmbitionApp.GetModel<PartyModel>();
            _map = AmbitionApp.GetModel<MapModel>();
            Round = 0;
            Remark = null;
            FreeRemarkCounter = _model.FreeRemarkCounter;
            Repartee = false;
            RemarksBought = 0;
            _remarks = new RemarkVO[_model.HandSize];
        }
    }
}
