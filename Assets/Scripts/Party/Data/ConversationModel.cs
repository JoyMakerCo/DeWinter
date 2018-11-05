using System;
using System.Collections.Generic;
using Core;

namespace Ambition
{
    public class ConversationModel : IModel, Util.IInitializable
    {
        public int FreeRemarkCounter;
        //TODO: Temp, until buffs are figured out
        public bool ItemEffect;
        public bool Repartee;
        public int RemarksBought;

        private MapModel _map;
        private PartyModel _model;

        private int _round;
        public int Round
        {
            get { return _round; }
            set {
                _round = value;
                AmbitionApp.SendMessage<int>(PartyMessages.ROUND, _round);
            }
        }

        public Queue<RemarkVO> Deck;
        public List<RemarkVO> Discard;
        public int MaxDeckSize
        {
            get
            {
                int total = Remarks == null ? 0 : Array.FindAll(Remarks, r => r != null).Length;
                return Deck.Count + Discard.Count + total;
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

        public RoomVO Room
        {
            get { return _map.Room;  }
        }

        public GuestVO[] Guests
        {
            get { return Room?.Guests;  }
            set
            {
                if (Room != null)
                {
                    Room.Guests = value;
                    AmbitionApp.SendMessage(value);
                }
            }
        }

        public void Initialize()
        {
            _map = AmbitionApp.GetModel<MapModel>();
            _model = AmbitionApp.GetModel<PartyModel>();
            _remarks = new RemarkVO[_model.HandSize];
        }
    }
}
