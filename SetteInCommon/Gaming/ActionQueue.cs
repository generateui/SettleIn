using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SettleInCommon.Actions.InGame;

namespace SettleInCommon.Gaming
{
    public class ActionQueue: List<InGameAction>
    {

        public InGameAction Dequeue()
        {
            InGameAction action = this[0];
            this.RemoveAt(0);
            return action;
        }
        public void Enqueue(InGameAction action)
        {
            this.Add(action);
        }
        public InGameAction Peek()
        {
            return this[0];
        }
        public int Count
        {
            get
            {
                return base.Count;
            }
        }
        public bool IsExpected(InGameAction action, XmlGame game)
        {
            // Some incoming actions may not have a strict order. Those actions are:
            // -LooseCardsAction
            // -PickGoldAction

            // LooseCardsAction
            // If we have a loosecards action, it may come from any player
            LooseCardsAction loosecards = action as LooseCardsAction;
            if (loosecards != null)
            {
                // First action on the queue needs to be a loosecards action
                if (game.ActionsQueue.First().GetType() != typeof(LooseCardsAction))
                {
                    return false;
                }

                // If the loosecards action is contained within the queue, it is OK to proces it
                LooseCardsAction looseIt = game.ActionsQueue.OfType<LooseCardsAction>()
                    .Where(lca => lca.Sender == action.Sender)
                    .FirstOrDefault();

                if (looseIt == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }            
            
            // PickGoldAction
            // If we have a loosecards action, it may come from any player
            PickGoldAction pickGold = action as PickGoldAction;
            if (pickGold != null)
            {
                // First action on the queue needs to be a loosecards action
                if (game.ActionsQueue.First().GetType() != typeof(PickGoldAction))
                {
                    return false;
                }

                // If the loosecards action is contained within the queue, it is OK to proces it
                PickGoldAction pickIt = game.ActionsQueue.OfType<PickGoldAction>()
                    .Where(pga => pga.Sender == action.Sender)
                    .FirstOrDefault();

                if (pickIt == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

            // Grab the expected action
            InGameAction expected = game.ActionsQueue.Peek();

            // Sender & Type needs to correspond to match ecxpectation
            if (expected.Sender == action.Sender &&
                expected.GetType() == action.GetType())
            {
                // Type & Sender equals action on top of queue
                return true;
            }
            else
            {
                //Either sender or type is different, we do not expect this action to be executed
                return false;
            }
        }
    }
}
