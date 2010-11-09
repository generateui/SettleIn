using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Media3D;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;

using SettleInCommon.Board.Hexes;
using SettleIn.Engine.Pieces;
using SettleInCommon.Actions.InGame;
using SettleInCommon.Actions.TurnActions;
using SettleInCommon.Gaming;
using SettleInCommon.Gaming.GamePhases;

namespace SettleIn.Engine.ViewPort.Behaviour
{
    /// <summary>
    /// User interaction behaviour 
    /// Pattern: State
    /// </summary>
    public abstract class BoardVisualBehaviour
    {
        protected InGameAction _OriginatingAction;
        protected HexVisual _OldMouseOverHex;

        public InGameAction OriginatingAction
        {
            get { return _OriginatingAction; }
            set { _OriginatingAction = value; }
        }


        public virtual HitTestFilterBehavior HittestFilter(DependencyObject o)
        {
            if (o.GetType() == typeof(MapEditorViewPort3D)) return HitTestFilterBehavior.Continue;
            if (o.GetType() == typeof(HexVisual)) return HitTestFilterBehavior.Continue;
            return HitTestFilterBehavior.Continue;
        }

        public virtual void SetStartState(BoardVisual board)
        {

        }

        public virtual void RemoveStartState(BoardVisual board)
        {

        }

        public virtual BehaviourResult Clicked(RayMeshGeometry3DHitTestResult rayMeshResult, BoardVisual board) 
        {
            return BehaviourResult.NoSuccess;
        }

        /// <summary>
        /// Default behaviour: highlight hex
        /// </summary>
        /// <param name="rayMeshResult"></param>
        public virtual void Moved(RayMeshGeometry3DHitTestResult rayMeshResult, BoardVisual board)
        {
            if (rayMeshResult != null)
            {
                HexVisual hex = rayMeshResult.VisualHit as HexVisual;
                if (hex != null)
                {
                    if (_OldMouseOverHex != null)
                        _OldMouseOverHex.Selected = false;
                    hex.Selected = true;

                    _OldMouseOverHex = hex;
                }
            }
        }

        /// <summary>
        /// Returns interaction behaviour for the 3D viewport depending on a game action
        /// </summary>
        /// Pattern: Factory method
        /// <param name="action"></param>
        /// <param name="game"></param>
        /// <returns></returns>
        public static BoardVisualBehaviour CreateBehaviour(InGameAction action, XmlGame game)
        {
            PlaceRobberPirateAction placerobberPirate = action as PlaceRobberPirateAction;
            if (placerobberPirate != null)
                return new PlaceRobberBehaviour() { OriginatingAction = action };

            BuildCityAction buildCity = action as BuildCityAction;
            if (buildCity != null)
                return new BuildCityBehaviour(false) { OriginatingAction = action };

            BuildTownAction buildTown = action as BuildTownAction;
            if (buildTown != null)
            {
                bool isStart;
                if (game.Phase.GetType() == typeof(PlayTurnsGamePhase))
                    isStart = false;
                else
                    isStart = true;
                return new BuildTownBehaviour(isStart) { OriginatingAction = action };
            }

            BuildRoadAction buildRoad = action as BuildRoadAction;
            if (buildRoad != null)
                return new BuildRoadBehaviour() 
                { 
                    StartingTownOrCity = buildRoad.OriginatingTownOrCity,
                    OriginatingAction = action 
                };

            BuildShipAction buildShip = action as BuildShipAction;
            if (buildShip != null)
                return new BuildShipBehaviour() { OriginatingAction = action };

            TradeBankAction tba = action as TradeBankAction;
            if (tba != null)
                return new ShowPointsBehaviour();

            ClaimVictoryAction claimWin = action as ClaimVictoryAction;
            if (claimWin != null)
            {
                return new TestBehaviour();
            }

            MoveShipAction moveShip = action as MoveShipAction;
            if (moveShip != null)
            {
                return new MoveShipBehaviour()
                {
                    OriginatingAction = moveShip
                };
            }

            RollDiceAction rollDice = action as RollDiceAction;
            if (rollDice != null)
            {
                return new ShowResourcesGainedBehaviour()
                {
                    OriginatingAction=rollDice
                };
            }
            return null;
        }

        public virtual string Name
        {
            get
            {
                return this.GetType().Name;
            }
        }
    }
}
