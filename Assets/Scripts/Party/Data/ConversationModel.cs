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

        public PartyVO Party { get { return _model.Party; } }

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

        private List<RemarkVO> _remarks = new List<RemarkVO>();
        public List<RemarkVO> Remarks
        {
            get { return _remarks; }
            set
            {
                _remarks = value;
                AmbitionApp.SendMessage<List<RemarkVO>>(Remarks);
            }
        }

        private void HandleClearRemark(GuestVO guest)
        {
            Remarks.Remove(Remark);
            AmbitionApp.SendMessage<List<RemarkVO>>(Remarks);
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
        }
    }
}
