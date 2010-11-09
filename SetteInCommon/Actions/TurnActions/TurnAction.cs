using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;
using SettleInCommon.Actions.InGame;
using SettleInCommon.Gaming;
using SettleInCommon.User;
using SettleInCommon.Board;

namespace SettleInCommon.Actions.TurnActions
{
    /// <summary>
    /// A Trunaction typically calls a method of the Game object. This should ensure synchronous
    /// state of the game among client and server
    /// </summary>
    [DataContract]
    [KnownType(typeof(BuildRoadAction))]
    [KnownType(typeof(BuildShipAction))]
    [KnownType(typeof(BuyDevcardAction))]
    [KnownType(typeof(ClaimVictoryAction))]
    [KnownType(typeof(EndTurnAction))]
    [KnownType(typeof(MoveShipAction))]
    [KnownType(typeof(TradeOfferAction))]
    [KnownType(typeof(BuildCityAction))]
    [KnownType(typeof(PlaceRobberPirateAction))]
    [KnownType(typeof(BuildTownAction))]
    [KnownType(typeof(PlacePortAction))]
    [KnownType(typeof(PlayDevcardAction))]
    [KnownType(typeof(RobPlayerAction))]
    [KnownType(typeof(RollDiceAction))]
    [KnownType(typeof(RollVolcanoDiceAction))]
    [KnownType(typeof(SwapChitAction))]
    [KnownType(typeof(TradeBankAction))]
    [KnownType(typeof(TradePlayerAction))]
    public abstract class TurnAction : InGameAction
    {
        /// <summary>
        /// Removes data to ensure users cannot cheat
        /// (before sending an action from server to client, this data is removed)
        /// </summary>
        public virtual void NullifyData() { }

        protected int? _TurnID;

        [DataMember]
        public int? TurnID
        {
            get { return _TurnID; }
            set { _TurnID = value; }
        }

        public override string ToString()
        {
            return _Message;
        }

        public override bool IsValid(XmlGame game)
        {
            if (!base.IsValid(game)) return false;

            if (game.PlayerOnTurn.XmlPlayer.ID != Sender)   
            {
                _InvalidMessage = "A turnaction should always originate from the player on turn";
                return false;
            }

            return true;
        }

        /// <summary>
        /// Derivers should call base method
        /// TODO: check all derivers if they call baseclass method
        /// </summary>
        /// <param name="xmlGame"></param>
        public override void PerformTurnAction(XmlGame xmlGame)
        {
            _TurnID = xmlGame.CurrentTurn;

            base.PerformTurnAction(xmlGame);
        }
    }
}
