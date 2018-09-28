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

        public PartyVO Party => _model.Party;

        public int Confidence
        {
            get { return _model.Confidence; }
            set { _model.Confidence = value; }
        }

        private RemarkVO _remark;
        public RemarkVO Remark
        {
            get { return _remark;  }
            set {
                _remark = value;
                AmbitionApp.SendMessage<RemarkVO>(_remark);
            }
        }

        private RemarkVO[] _remarks;
        public RemarkVO[] Remarks
        {
            get { return _remarks; }
            set
            {
                AmbitionApp.SendMessage<RemarkVO[]>(_remarks = value);
            }
        }

        public RoomVO Room
        {
            get { return _map.Room;  }
        }


        public GuestVO[] Guests
        {
            get { return _map.Room.Guests; } 
            set {
                _map.Room.Guests = value;
                AmbitionApp.SendMessage<GuestVO[]>(value);
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
