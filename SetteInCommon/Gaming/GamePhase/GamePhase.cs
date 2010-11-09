using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SettleInCommon.Actions.InGame;
using System.Runtime.CompilerServices;
using SettleInCommon.Gaming.TurnPhases;
using System.Runtime.Serialization;

namespace SettleInCommon.Gaming.GamePhases
{
    /// <summary>
    /// Implementation of state pattern for the gamephase. 
    /// </summary>
    [DataContract]
    public class GamePhase
    {
        TurnPhase _TurnPhase = new BeforeDiceRollTurnPhase();
        protected List<Type> _AllowedActions = new List<Type>();

        public GamePhase()
        {
            AddActions();
        }

        protected virtual void AddActions()
        {
        }

        public virtual Type EndAction()
        {
            throw new NotImplementedException();
        }

        public virtual void Start(XmlGame game)
        {
            throw new NotImplementedException();
        }

        public virtual void ProcessAction(InGameAction inGameAction, XmlGame game)
        {
            // base class should implement
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns true if passed action is allowed to be performed in current
        /// game phase
        /// </summary>
        /// <param name="inGameAction"></param>
        /// <returns></returns>
        public virtual bool IsAllowed(InGameAction inGameAction, XmlGame game)
        {
            return _AllowedActions.Contains(inGameAction.GetType());
        }

        public virtual GamePhase Next(XmlGame game)
        {
            throw new NotImplementedException();

        }
    }
}
