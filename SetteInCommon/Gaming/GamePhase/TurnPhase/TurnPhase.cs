using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SettleInCommon.Actions.InGame;
using SettleInCommon.Actions.TurnActions;
using System.Runtime.Serialization;

namespace SettleInCommon.Gaming.TurnPhases
{
    [DataContract]
    public class TurnPhase
    {
        protected List<Type> _AllowedActions = new List<Type>();

        public TurnPhase()
        {
            AddAllowedActions();
        }
        protected virtual void AddAllowedActions()
        {
        }

        public virtual TurnPhase ProcessAction(InGameAction action, XmlGame game)
        {
            throw new NotImplementedException();
        }

        public virtual bool AllowedAction(InGameAction inGameAction, XmlGame game)
        {
            if (_AllowedActions.Contains(inGameAction.GetType()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public virtual TurnPhase Next()
        {
            throw new NotImplementedException();
        }

    }
}
