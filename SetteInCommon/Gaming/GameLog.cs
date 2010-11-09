using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SettleInCommon.Actions.InGame;
using System.Collections.ObjectModel;
using SettleInCommon.Actions.TurnActions;

namespace SettleInCommon.Gaming
{
    public class GameLog : ObservableCollection<InGameAction>
    {
        private int? _GameStarterID=null;

        /// <summary>
        /// Gets the RollDiceActions of the current round.
        /// A round end when a RolledSameAction is encountered
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>
        public List<RollDiceAction> GetCurrentRoundRolls(XmlGame game)
        {
            List<RollDiceAction> result = new List<RollDiceAction>();

            //We run the like a stack, first to examine is Last in the list
            for (int i = Count -1; i > 0; i--)
            {
                // we break the loop when we encountered a rolledsame action
                RolledSameAction rolledSame = this[i] as RolledSameAction;
                if (rolledSame != null)
                    break;

                // When we encounter a RollDiceAction, add it to the list
                RollDiceAction rda = this[i] as RollDiceAction;
                if (rda != null)
                    result.Add(rda);

                //We always have maximum of PlayerCount RollDiceAction
                if (result.Count == game.Players.Count)
                    break;
            }

            return result;
        }

        public bool FirstPlayerIsDetermined(XmlGame game)
        {
            List<RollDiceAction> rolledDices = GetCurrentRoundRolls(game);
            var x = from r in rolledDices
                    where r.Dice == (from rd in rolledDices
                                     select rd.Dice).Max()
                    select r;

            // the player with highest dice is determined when we
            // have only one result
            if (x.Count()== 1)
            {
                _GameStarterID = x.First().Sender;
            }
            return x.Count() == 1;
        }

        public int? GameStarterID
        {
            get { return _GameStarterID; }
        }


    }
}
