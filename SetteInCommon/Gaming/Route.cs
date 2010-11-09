using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SettleInCommon.Board;
using SettleInCommon.Gaming;
using SettleInCommon.Board.Hexes;

namespace SettleInCommon.Gaming
{
    /// 
    /// -represented by a list of HexPoints
    /// -EndPoint and StartPoint may be equal 
    /// -EndSide and StartSide may not be equal
    /// -total length in HexPoints equals length in HexSides + 1
    /// -has a startPoint (.First() or [0])
    /// -has an EndPoint (.Last() or [Count-1])
    /// -a route may become invalid by 
    ///      -building on a HexPoint in this list by opponent
    ///     -a town or city being blown up from owner of route (while connecting road+ship)
    ///      -any player (opponents as well current player),
    ///          modifying a route
    ///
    /// -Modifying a route means:
    ///      a player
    ///      -build a road, or
    ///      -build a ship, or
    ///      -moved a ship.
    ///
    /// -Each HexSide must be unique within the route
    /// -nothing changes when a city is build
    /// -Longest route award only changes owner when new owner has
    ///      a longer route after recalculation
    /// length of a route is always amount of hexpoints, minus one

    // -
    public class Route : List<RouteNode>
    {
        private bool IsEndPoint(HexPoint point, HexSide side, XmlGame game, GamePlayer player)
        {
            // create a list of opponents' cities+towns
            List<HexPoint> opponentsTownsCities = new List<HexPoint>();
            foreach (GamePlayer opponent in game.GetOpponents(player.XmlPlayer.ID))
                opponentsTownsCities.AddRange(opponent.GetTownsCities());

            // an opponent has a town or city the point.
            // Great, we have an end here, so return true.
            if (opponentsTownsCities.Contains(point))
            {
                return true;
            }
            else
            {
                //when the direction fails to connect, we have an end
                List<HexPoint> playerTownsCities = player.GetTownsCities();

                // opponent has no city/town at either point. Determine 
                // if we have a connection.
                // A connection is there when:
                // - road + road
                // - road + town/city + ship
                // - ship + ship
                // - ship + town/city + road
                if (player.Roads.Contains(side))
                // road
                {
                    // Get the two neighbouring points from the side
                    List<HexSide> otherSides1 = point.GetOtherSides(side);

                    // If any of both neighbours is contained within the list of roads,
                    // we have a connection.
                    if (player.Roads.Intersect(otherSides1).Count() > 0)
                        // road + road
                        return false;

                    // No road + road found. Check for road+ship, but only when we have a town/city
                    // on the point, which connects the road and ship.
                    if (playerTownsCities.Contains(side.HexPoint1))
                    {
                        // road + ship with city/town
                        if (player.Ships.Intersect(otherSides1).Count() > 0)
                            return false;
                    }
                }
                else
                // ship
                {
                    // Get the two neighbouring points from the side
                    List<HexSide> otherSides1 = point.GetOtherSides(side);
                    
                    // If any of both neighbours is contained within the list of ships,
                    // we have a connection.
                    if (player.Ships.Intersect(otherSides1).Count() > 0)
                        // ship + ship
                        return false;

                    // No ship + ship found. Check for ship + road, but only when we have a town/city
                    // on the point, which connects the ship and road.
                    if (playerTownsCities.Contains(point))
                    {
                        // ship + road with city/town
                        if (player.Roads.Intersect(otherSides1).Count() > 0)
                            return false;
                    }
                }
            }
            return true;
        }

        private bool IsSplit(HexPoint point, HexSide side, GamePlayer player)
        {
            bool isShip = player.Ships.Contains(side);
            bool isRoad = player.Roads.Contains(side);
            if (isShip && isRoad) { } // ugh

            if (player.GetTownsCities().Contains(point))
            {
                // we have a town/city, we just need three roads OR ships 
                // with the point in question
                return (from s in player.GetRoadsShips()
                        where s.HexPoint1.Equals(point) ||
                              s.HexPoint2.Equals(point)
                        select s).Count() == 3;
            }
            else
            {
                if (isShip)
                    return (from s in player.Ships
                            where s.HexPoint1.Equals(point) ||
                                  s.HexPoint2.Equals(point)
                            select s).Count() == 3;
                else
                    return (from s in player.Roads
                            where s.HexPoint1.Equals(point) ||
                                  s.HexPoint2.Equals(point)
                            select s).Count() == 3;
            }

        }

        private bool IsContained(List<HexPoint> route, HexPoint point1, HexPoint point2)
        {
            for (int i = 0; i < route.Count - 2; i++)
            {
                if (route[i].Equals(point1) && route[i + 1].Equals(point2) ||
                    route[i].Equals(point2) && route[i + 1].Equals(point1))
                    return true;
            }
            return false;
        }

        // 1.
        // calculate amount of ends
        // create all routepossibilities per "end to end"
        // 2. 
        // If no ends are found, but more then 4 roads/ships exist, a route
        // is a closed loop. Then:
        //      - look for a split in the loop. A closed loop can contain a split
        //          if the amount of roads+ships is more then 5 (the loop may contain
        //          both the starting towns). 
        //      -If no splits or ends exist, it means we have one closed loop, with both
        //          starting towns in it. 
        // calculate length of routes
        // return player with longest route
        //
        // New algorithm:
        // Try to make a possible route from each ship/road, until:
        //      an end is found
        //      a road/ship is found within the route
        //      the longest of the possible routes is the longest route of the player
        public Route CalculateALongestRoute(GamePlayer player, XmlGame game)
        {
            List<HexSide> roadsShips = player.GetRoadsShips();
            List<List<HexPoint>> routesWithEnd = new List<List<HexPoint>>();
            List<List<HexPoint>> routeswithoutEnd = new List<List<HexPoint>>();
            
            List<HexPoint> nonUniqueJoins = new List<HexPoint>();

            foreach (HexSide side in roadsShips)
            {
                nonUniqueJoins.Add(side.HexPoint1);
                nonUniqueJoins.Add(side.HexPoint2);
            }

            //make the list of hexpoints unique
            IEnumerable<HexPoint> uniqueJoins = nonUniqueJoins.Distinct();

            // create a new route possibility for each unique hexpoint
            foreach (HexSide side in roadsShips)
            {
                List<HexPoint> newRoute = new List<HexPoint>();
                List<HexPoint> newRoute2 = new List<HexPoint>();
                
                // each side creates 2 possibilities, with opposite directions to 
                // go to.
                newRoute.Add(side.HexPoint1);
                newRoute.Add(side.HexPoint2);
                newRoute2.Add(side.HexPoint2);
                newRoute2.Add(side.HexPoint1);
                routeswithoutEnd.Add(newRoute);
                routeswithoutEnd.Add(newRoute2);
            }
            int j = 0;

            // we iterate of over all routes which arent marked with an endpoint
            // yet. Eventuually only routes with endpoints will exist,
            // as a path eventually encounters a point where:
            //  -A town/city of an opponent is resided
            //  -Two roads/ships meeting following qualification:
            //      -allready present in the route
            //      -opponent has road/ship 
            //      -no player has road/ship on both locations
            //  -
            while (routeswithoutEnd.Count > 0)
            {
                j++;
                if (j > 100) { }
                //now we have three possibilities:
                // 1. last point is also an endpoint
                //    -close the route, add it to list of routes with end

                // 2. current point is a split
                //      pick a direction, copy route to list of routes without end
                //      proceed with new pass of check
                // 3. current point has a connection with 1 road or ship
                //      add currentpoint to the route
                //      proceed with new pass of the last point
                //
                // When the route already contains one road in the neighbours,
                // we can only proceed when we have a split
                // When the route contains both neighbours, we reached an end

                // grab a route from the list
                List<HexPoint> route = routeswithoutEnd[0];

                // the point used as starting point for the two lookahead points
                HexPoint lastPoint = route.Last();

                // the point where we came from
                HexPoint beforeLastPoint = route[route.Count - 2];
                
                // side where we came from
                HexSide side = new HexSide(lastPoint, beforeLastPoint);

                List<HexPoint> opponentsTownsCities = new List<HexPoint>();

                //create a list of opponent's towns and cities
                foreach (GamePlayer opponent in game.GetOpponents(player.XmlPlayer.ID))
                    opponentsTownsCities.AddRange(opponent.GetTownsCities());

                if (IsEndPoint(lastPoint, side, game, player) ||
                    opponentsTownsCities.Contains(lastPoint))
                // we have an endpoint here
                {
                    routesWithEnd.Add(route);
                    routeswithoutEnd.Remove(route);
                    continue;
                }
                if (IsSplit(lastPoint,side, player))
                // we encountered a split here.
                // possibilities:
                //  -one, or both of the sides are already contained within the route.
                //      -both: treat as end
                //      -one: pick other side to continue
                //      -zero: split the route
                {
                    List<HexPoint> newPoints = lastPoint.GetOtherNeighbours(lastPoint, beforeLastPoint);

                    bool firstIsContained = IsContained(route, lastPoint, newPoints[0]);
                    bool secondIsContained = IsContained(route, lastPoint, newPoints[1]);
                    if (firstIsContained && secondIsContained)
                    {
                        // we reached an end: both neighbours are already in the
                        // route, cannot proceed, close route
                        routesWithEnd.Add(route);
                        routeswithoutEnd.Remove(route);
                        continue;
                    }

                    if (firstIsContained)
                    {
                        route.Add(newPoints[1]);
                        continue;
                    }
                    if (secondIsContained)
                    {
                        route.Add(newPoints[0]);
                        continue;
                    }

                    //neither is contained, we encountered a valid split.

                    // the first point gets added to the splittedroute
                    List<HexPoint> splittedRoute = CopyList(route);
                    splittedRoute.Add(newPoints[0]);
                    routeswithoutEnd.Add(splittedRoute);

                    // the second point is the new lastpoint for the current route
                    route.Add(newPoints[1]);

                    continue;
                }

                //no split, no endpoint, no city/town of opponent present.
                // So, we have one ship or road in the neighbours.
                // road+ship: we need a town, then extend route
                // road+road or ship+ship: extend route
                // ship+road, we need a town, then extend route
                

                // Get the two neighbouring points from the side
                List<HexSide> otherSides = lastPoint.GetOtherSides(side);

                List<HexPoint> playerTownsCities = player.GetTownsCities();

                if (player.Roads.Contains(side))
                // road
                {
                    // If any of both neighbours is contained within the list of roads,
                    // we have a connection.
                    IEnumerable<HexSide> road = player.Roads.Intersect(otherSides);
                    if (road.Count() == 1)
                    // road + road
                    {
                        HexPoint newPoint = road.First().GetOtherPoint(lastPoint);
                        if (IsContained(route, lastPoint, newPoint))
                        {
                            routesWithEnd.Add(route);
                            routeswithoutEnd.Remove(route);
                            continue;
                        }
                        else
                        {
                            route.Add(newPoint);
                            continue;
                        }
                    }

                    // No road + road found. Check for road+ship, but only when we have a town/city
                    // on the point, which connects the road and ship.
                    if (playerTownsCities.Contains(lastPoint))
                    {
                        // road + ship with city/town
                        IEnumerable<HexSide> ship = player.Ships.Intersect(otherSides);
                        if (ship.Count() == 1)
                        {
                            HexPoint newPoint = ship.First().GetOtherPoint(lastPoint);
                            if (IsContained(route, lastPoint, newPoint))
                            {
                                routesWithEnd.Add(route);
                                routeswithoutEnd.Remove(route);
                                continue;
                            }
                            else
                            {
                                route.Add(newPoint);
                                continue;
                            }
                        }
                    }
                }
                else
                // ship
                {
                    // If any of both neighbours is contained within the list of roads,
                    // we have a connection.
                    IEnumerable<HexSide> ship = player.Ships.Intersect(otherSides);
                    if (ship.Count() == 1)
                    // road + road
                    {
                        HexPoint newPoint = ship.First().GetOtherPoint(lastPoint);
                        if (IsContained(route, lastPoint, newPoint))
                        {
                            routesWithEnd.Add(route);
                            routeswithoutEnd.Remove(route);
                            continue;
                        }
                        else
                        {
                            route.Add(newPoint);
                            continue;
                        }
                    }

                    // No road + road found. Check for road+ship, but only when we have a town/city
                    // on the point, which connects the road and ship.
                    if (playerTownsCities.Contains(lastPoint))
                    {
                        // road + ship with city/town
                        IEnumerable<HexSide> road = player.Roads.Intersect(otherSides);
                        if (road.Count() == 1)
                        {
                            HexPoint newPoint = road.First().GetOtherPoint(lastPoint);
                            if (IsContained(route, lastPoint, newPoint))
                            {
                                routesWithEnd.Add(route);
                                routeswithoutEnd.Remove(route);
                                continue;
                            }
                            else
                            {
                                route.Add(newPoint);
                                continue;
                            }
                        }
                    }
                }
            }

            //sweet! now we have a list of all possible roads. Pick the
            // first matching the maximum length;
            if (routesWithEnd.Count == 0) return null;
            List<HexPoint> f = (from route in routesWithEnd
                                where route.Count ==
                                    //length of longest road(s)
                                        (from r in routesWithEnd
                                         select r.Count).Max()
                                select route).First();
            Route result = new Route();

            if (f.Count > 5)
            {
                for (int i = 0; i < f.Count - 1; i++)
                {
                    RouteNode rn = new RouteNode(f[i], f[i + 1]);
                    //rn.
                    result.Add(rn);
                }
                return result;
            }
            else
            {
                // route length did not exceed 4 roads, so no route found.
                return null;
            }

        }


        // Split/Join
        // a split or join is a HexPoint where three ships or roads reside
        // of the player. On the HexPoint there can be no town/city of opponent.
        // 
        // Connection
        // A Connection is a set of two Splits/Joins, with intermediate 
        // HexSides 
        // - A route may have zero or more connections

        // Calculation:
        // = Create a list of Joins/splits
        // - each end calculates a path until it encounters an end
        // - each possibility is a unique sequence of splits
        // - when building a path, no HexPoint can be added tiwce
        // - 
        public Route CalculateLongestRoute(GamePlayer player, XmlGame game)
        {
            List<HexPoint> joins = GetJoins(player, game);
            List<HexPoint> ends = GetEndPoints(player, game);
            if (player.Roads.Count > 0) {}
            // make a possible route for each combination of ends.
            // -multiple combinations of equal start + end
            // -each unique possible route ends up being added twice,
            //      the first going from start to end, the second going
            //      from end to start
            //
            // when iterating over the list of endpoints,
            // - start by adding the (can be either, if both points 
            //     of hexside are endpoints) endpoint as starting point of
            //      the route
            // - end when we reached another endpoint, mark this as ending 
            //      endpoint of the route
            // - when encountered a join: 
            //      -copy the found route, add to list of routes
            //      -proceed with a side
            // - each copy in turn is followed untill an end is meet
            // this means the list will endup containing
            List<List<HexPoint>> routesWithEnd = new List<List<HexPoint>>();
            List<List<HexPoint>> routeswithoutEnd = new List<List<HexPoint>>();
            foreach (HexPoint end in ends)
            {
                // since the point is an end, we can safely lookup the end 
                // in the list of sides and get a unique side.
                HexPoint currentPointOnOtherSide = (from s in player.GetRoadsShips()
                                                   where s.HexPoint1.Equals(end) ||
                                                         s.HexPoint2.Equals(end)
                                                   select s).First().GetOtherPoint(end);

                List<HexPoint> newRoute = new List<HexPoint>();

                // start with a list of two points, a start and endpoint
                newRoute.Add(end);
                newRoute.Add(currentPointOnOtherSide);
                routeswithoutEnd.Add(newRoute);
            }
            //take a route, and process it.
            // when an end is recognized, it is put into the routesWithEnd list
            while (routeswithoutEnd.Count > 0)
            {
                //now we have three possibilities:
                // 1. last point is also an endpoint
                //    -close the route, add it to list of routes with end

                // 2. current point is a split
                //      pick a direction, copy route to list of routes without end
                //      proceed with new pass of check
                // 3. current point has a connection with 1 road or ship
                //      add currentpoint to the route
                //      proceed with new pass of the last point
                //
                // When the route already contains one road in the neighbours,
                // we can only proceed when we have a split
                // When the route contains both neighbours, we reached an end

                //grab a route from the list
                List<HexPoint> route = routeswithoutEnd[0];
                HexPoint lastPoint = route.Last();
                if (ends.Contains(lastPoint))
                // we have an endpoint here
                {
                    routesWithEnd.Add(route);
                    routeswithoutEnd.Remove(route);
                    continue;
                }
                if (joins.Contains(lastPoint))
                // we encountered a split here.
                {
                    List<HexPoint> splittedRoute = CopyList(route);
                    List<HexPoint> newPoints = lastPoint.GetNeighbours();
                    // now we have all three neighbours, remove the forelast 
                    // neighbour (this the other point of the last HexSide in
                    // the route)
                    newPoints.Remove(
                        //remove the point that matches the forelast point
                        newPoints.Find(
                        p => p.Equals(
                            //we always have routes with at least two points
                            route[route.Count - 2])));
                    bool firstContained = route.Contains(newPoints[0]);
                    bool seecondContained = route.Contains(newPoints[1]);

                    if (firstContained && seecondContained)
                    {
                        // we reached an end: both neighbours are already in the
                        // route, cannot proceed
                        routesWithEnd.Add(route);
                        routeswithoutEnd.Remove(route);
                        continue;
                    }
                    // we can do a safe OR operation here, since the AND operation 
                    // breaks the pass
                    if (firstContained || seecondContained)
                    {
                        //simply add the other point to the route
                        if (firstContained)
                        {
                            route.Add(newPoints[1]);
                            continue;
                        }
                        if (seecondContained)
                        {
                            route.Add(newPoints[2]);
                            continue;
                        }

                    }

                    // the first point gets added to the splittedroute
                    splittedRoute.Add(newPoints[0]);
                    routeswithoutEnd.Add(splittedRoute);

                    // the second point is the new lastpoint for the current route
                    route.Add(newPoints[1]);

                    continue;
                }
                // Here we should have only one road/ship on the new direction.
                // 
                // a connection:
                // - cannot be location of town/city of opponent
                //      (no need to check because we already checked the ends)
                // - cannot already be contained in the route
                // - when ship and road, player must have a town or city
                // - when ship+ship or road+road combination, 
                HexSide fromDirection = new HexSide(lastPoint, route[route.Count - 2]);
                bool startRoad = player.Roads.Contains(fromDirection);
                bool startShip = !startRoad;

                List<HexPoint> newPoints2 = lastPoint.GetNeighbours();
                // now we have all three neighbours, remove the forelast 
                // neighbour (this the other point of the last HexSide in
                // the route)
                newPoints2.Remove(
                    //remove the point that matches the forelast point
                    newPoints2.Find(
                    p => p.Equals(
                        //we always have routes with at least two points
                        route[route.Count - 2])));

                // we have an existing and nonexisting hexpoint in the list, remove
                // the nonexisting hexpoint
                List<HexSide> roadShips = player.GetRoadsShips();
                if (HexSideListContainsPoint(roadShips, newPoints2[0]))
                    newPoints2.Remove(newPoints2[1]);
                else
                    newPoints2.Remove(newPoints2[0]);

                // last man standing: the point and side where we are going to
                HexPoint lookAhead = newPoints2[0];
                HexSide LookAheadSide = new HexSide(lastPoint, lookAhead);

                List<HexSide> sidesRoute = ConvertToSideCollection(route);
                if (sidesRoute.Contains(LookAheadSide))
                {
                    // last side is already in this route, so we must end this 
                    // route. This is a route EndPoint
                    routesWithEnd.Add(route);
                    routeswithoutEnd.Add(route);
                    continue;
                }
                bool endRoad = player.Roads.Contains(LookAheadSide);
                bool endShip = !endRoad;

                if (startRoad && endRoad ||
                    startShip && endShip)
                    route.Add(lookAhead);
                if ((startShip && endRoad ||
                    startRoad && endShip) &&
                    player.GetTownsCities().Contains(lastPoint))
                    route.Add(lookAhead);
            }

            //sweet! now we have a list of all possible roads. Pick the
            // first matching the maximum length;
            if (routesWithEnd.Count == 0) return null;
            List<HexPoint> f= (from route in routesWithEnd
                    where route.Count ==
                        //length of longest road(s)
                            (from r in routesWithEnd
                             select r.Count).Max()
                    select route).First();
            Route result = new Route();

            for (int i = 0; i < f.Count - 1; i++)
            {
                RouteNode rn = new RouteNode(f[i], f[i + 1]);
                //rn.
                result.Add(rn);
            }

            return result;
        }

        private List<HexSide> ConvertToSideCollection(List<HexPoint> route)
        {
            List<HexSide> result = new List<HexSide>();

            for (int i = 0; i < route.Count - 1; i++)
                result.Add(new HexSide(route[i], route[i + 1]));

            return result;
        }

        private List<HexPoint> CopyList(List<HexPoint> toCopy)
        {
            List<HexPoint> result = new List<HexPoint>();

            foreach (HexPoint point in toCopy)
                result.Add(new HexPoint(point.Hex1, point.Hex2, point.Hex3));

            return result;
        }

        private bool HexSideListContainsPoint(List<HexSide> sides, HexPoint point)
        {
            return (from s in sides
                    where s.HexPoint1.Equals(point) ||
                          s.HexPoint2.Equals(point)
                    select s).Count() > 0;
        }

        /// <summary>
        /// Construct a list of ends from specified player
        /// </summary>
        ///  an "end" is a road or ship that meets the following criteria:
        /// -has one or two hexpoints with zero roads/ship of player on neighbouring hexisdes
        ///      -when checking road, ignore ships when no town/city
        ///      -when checking ship, ignore roads when no town/city
        /// -a hexpoint where a town/city of opponent resides
        /// -always connected to another end 
        /// <param name="player"></param>
        /// <param name="game"></param>
        /// <returns></returns>
        private List<HexPoint> GetEndPoints(GamePlayer player, XmlGame game)
        {
            List<HexPoint> endPoints = new List<HexPoint>();

            // create a list of opponents' cities+towns
            List<HexPoint> opponentsTownsCities = new List<HexPoint>();
            foreach (GamePlayer opponent in game.GetOpponents(player.XmlPlayer.ID))
                opponentsTownsCities.AddRange(opponent.GetTownsCities());

            List<GamePlayer> ops = game.GetOpponents(2);

            List<HexSide> playerRoadsShips = player.GetRoadsShips();

            foreach (HexSide side in playerRoadsShips)
            {
                // an opponent has a town or city on either point. 
                // Great, we have an end here, so add it so the list
                if (opponentsTownsCities.Contains(side.HexPoint1)) 
                {
                    endPoints.Add(side.HexPoint1);
                    continue;
                }
                if (opponentsTownsCities.Contains(side.HexPoint2)) 
                {
                    endPoints.Add(side.HexPoint2);
                    continue;
                }
                // we dont have 
                else
                {
                    //when either direction fails to connect, we have an end
                    List<HexPoint> playerTownsCities = player.GetTownsCities();
                    bool side1HasConnectionAndRoadOrShip = false;
                    bool side2HasConnectionAndRoadOrShip = false;

                    // opponent has no city/town at either point. Determine for
                    // each direction if we have a connection/
                    // A connection is there when:
                    // - road + road
                    // - road + town/city + ship
                    // - ship + ship
                    // - ship + town/city + road
                    if (player.Roads.Contains(side))
                    // road
                    {
                        List<HexSide> otherSides1 = side.HexPoint1.GetOtherSides(side);
                        IEnumerable<HexSide> roadsIntersection = player.Roads.Intersect(otherSides1);
                        if (roadsIntersection.Count() > 0)
                            // road + road
                            side1HasConnectionAndRoadOrShip = true;
                        if (playerTownsCities.Contains(side.HexPoint1))
                        {
                            IEnumerable<HexSide> shipsIntersection = player.Ships.Intersect(otherSides1);
                            // road + ship with city/town
                            if (shipsIntersection.Count() > 0)
                                side1HasConnectionAndRoadOrShip = true;
                        }

                        List<HexSide> otherSides2 = side.HexPoint2.GetOtherSides(side);
                        IEnumerable<HexSide> roadsIntersection2 = player.Roads.Intersect(otherSides2);
                        if (roadsIntersection2.Count() > 0)
                            // road + road
                            side2HasConnectionAndRoadOrShip = true;
                        if (playerTownsCities.Contains(side.HexPoint2))
                        {
                            IEnumerable<HexSide> shipsIntersection2 = player.Ships.Intersect(otherSides2);
                            // road + ship with city/town
                            if (shipsIntersection2.Count() > 0)
                                side2HasConnectionAndRoadOrShip = true;
                        }

                        if (!side1HasConnectionAndRoadOrShip)
                        {
                            endPoints.Add(side.HexPoint1);
                            continue;
                        }
                        if (!side2HasConnectionAndRoadOrShip)
                        {
                            endPoints.Add(side.HexPoint2);
                            continue;
                        }
                    }
                    else
                    // ship
                    {
                        List<HexSide> otherSides1 = side.HexPoint1.GetOtherSides(side);
                        if (player.Ships.Intersect(otherSides1).Count() > 0)
                            // ship + ship
                            side1HasConnectionAndRoadOrShip = true;
                        if (playerTownsCities.Contains(side.HexPoint1))
                            // ship + road with city/town
                            if (player.Roads.Intersect(otherSides1).Count() > 0)
                                side1HasConnectionAndRoadOrShip = true;

                        List<HexSide> otherSides2 = side.HexPoint2.GetOtherSides(side);
                        if (player.Ships.Intersect(otherSides2).Count() > 0)
                            // ship + ship
                            side1HasConnectionAndRoadOrShip = true;
                        if (playerTownsCities.Contains(side.HexPoint2))
                            // ship + road with city/town
                            if (player.Roads.Intersect(otherSides2).Count() > 0)
                                side2HasConnectionAndRoadOrShip = true;

                        // if either connection is not present, we have an end
                        if (!side1HasConnectionAndRoadOrShip)
                        {
                            endPoints.Add(side.HexPoint1);
                            continue;
                        }
                        if (!side2HasConnectionAndRoadOrShip)
                        {
                            endPoints.Add(side.HexPoint2);
                            continue;
                        }
                    }
                }
            }
            return endPoints;
        }

        //Create a list of joins/splits
        // 1. create a list of all HexPoints
        // 2. Make a new list with unique HexPoints
        // 3. If a hexPoint has no city/town of opponent,
        //      3.1 If HexPoint has a town/city of player
        //          3.1.1 Add with three ships/roads
        //      3.2 If HexPoint has no town/city of player
        //          3.2.1 add only when 3xship or 3x road
        //1. Create list off all hexPoints
        private List<HexPoint> GetJoins(GamePlayer player, XmlGame game)
        {
            List<HexPoint> joins = new List<HexPoint>();
            List<HexPoint> nonUniqueJoins = new List<HexPoint>();

            // create list of all HexPoints
            // duplicate items may occur since a HexSide consists or two HexPoints
            foreach (HexSide side in player.GetRoadsShips())
            {
                nonUniqueJoins.Add(side.HexPoint1);
                nonUniqueJoins.Add(side.HexPoint2);
            }

            //make the list of hexpoints unique
            List<HexPoint> uniqueJoins = new List<HexPoint>();

            foreach (HexPoint point in nonUniqueJoins)
                if (!uniqueJoins.Contains(point))
                    uniqueJoins.Add(point);
            //nonUniqueJoins.Distinct();

            // create a list of opponents' cities+towns
            List<HexPoint> opponentsTownsCities = new List<HexPoint>();
            foreach (GamePlayer opponent in game.GetOpponents(player.XmlPlayer.ID))
            {
                opponentsTownsCities.AddRange(opponent.Towns);
                opponentsTownsCities.AddRange(opponent.Cities);
            }

            foreach (HexPoint uniquePoint in uniqueJoins)
            {
                if (!opponentsTownsCities.Contains(uniquePoint))
                // sir, we may proceed when opponent does not have a town or city
                {
                    IEnumerable<HexSide> roadsAtPoint =
                        from road in player.Roads
                        where road.HexPoint1.Equals(uniquePoint) ||
                              road.HexPoint2.Equals(uniquePoint)
                        select road;

                    IEnumerable<HexSide> shipsAtPoint =
                        from ship in player.Ships
                        where ship.HexPoint1.Equals(uniquePoint) ||
                              ship.HexPoint2.Equals(uniquePoint)
                        select ship;

                    List<HexSide> playerRoadsShips = player.GetRoadsShips();
                    if (player.GetTownsCities().Contains(uniquePoint))
                    {
                        // A town/city present, any combination makes a join
                        // since road+town/city+ship connects
                        if (roadsAtPoint.Count() + shipsAtPoint.Count() == 3)
                            joins.Add(uniquePoint);
                    }
                    else
                    {
                        // no town/city present, only a join when we have 3x ship
                        // or 3x road, since road + ship does not connect without
                        // a town/city
                        if (roadsAtPoint.Count() == 3 || shipsAtPoint.Count() == 3)
                            joins.Add(uniquePoint);
                    }
                }
            }

            return joins;
        }

        //will create a list of trade routes
        // a trade route is a path between two hexpoints (city/town)
        // A trade route will always start at a town/city on
        //      starting territory, and end on a non-starting territory.
        //  Each unique combination of start+endpoints have a winner
        // - Has a winner, determined:
        //      -most ships in TradeRoute,
        //      -when even, earliest player to achieve TradeRoute wins it
        //      -Longest posible route is the route between two hexpoints 
        //          which is to be evaluated for winner determination
        //
        // Path
        //
        // a path is a sequence of hexpoints. 
        // a path may consist of ships and roads from both parties.
        // -never start with a road
        public List<List<HexPoint>> GetTradeRoutes(GamePlayer player, XmlGame game)
        {
            List<List<HexPoint>> tradeRoutesWithEnd = new List<List<HexPoint>>();
            List<List<HexPoint>> tradeRoutesWithoutEnd = new List<List<HexPoint>>();

            // loop through all towns/cities with at least a seahex
            foreach (HexPoint seaPoint in 
                from tc in player.GetTownsCities()
                where 
                    // constrain on at least one seahex
                    (game.Board.Hexes[tc.Hex1] is SeaHex ||
                    game.Board.Hexes[tc.Hex2] is SeaHex ||
                    game.Board.Hexes[tc.Hex3] is SeaHex)

                    &&

                    // constrain on startTown and SecondTownCity
                    // territories, to select only trade route starts
                    // starting and secondTownCity is never on border
                    // of two territories
                    player.StartingTerritories().Contains(
                        ((ITerritoryHex)
                        game.Board.Hexes[
                            (from hex in new List<HexLocation>() 
                                            { tc.Hex1, tc.Hex2, tc.Hex3 }
                            where game.Board.Hexes[hex] is ITerritoryHex
                            select hex).First()
                            ]).TerritoryID)
                select tc)
            {
                //make a list of ships next to the town from the player
                IEnumerable<HexSide> shipsBesideStart =
                    player.Ships.Intersect(seaPoint.GetSeaSides(game.Board));

                //add the freshly born possibilities to the list
                foreach (HexSide ship in shipsBesideStart)
                    tradeRoutesWithoutEnd.Add(
                           new List<HexPoint>() 
                                { seaPoint, ship.OtherPoint(seaPoint)});
                
            }
            //wee! we have now a list of starts. 
            // Make a list of possibilties
            // -If a possibility ends up at a town/city, a traderoute is present
            // -filter on start/end unicity of each route
            // -first player to own the uniqe route
            //
            while (tradeRoutesWithoutEnd.Count > 0)
            {
                // Possibilities:
                // - No ships/roads in lookahead hexsides, no towns/cities
                //      -we reached an end here
                // - WhenLookAhead HexPoint is:
                //      -town/city of any player,
                //      -on a territory other than the starting point of 
                //            the trade route VALIDATES
                //          -End the pass, add to TradeRoutes
                //       on same territory:
                //           -End reached, no Trade route
                // - current HexSide can be road of opponent, 
                //      Lookahead must be a road of same opponent
                // - current HexSide can be ship of opponent
                //      Lookahead must be a ships of same opponent
                // - current HexSide can be ship of player
                //      Lookahead must be a ship/road of any opponent, or ship
                //      of player

                // 2. one ship/road in lookahead hexsides
                //      proceed only when lookahead hexside is
                //      not a road from player (only ship player or ship+road opponent allowed)
                // 2. any combination of two ship/road in lookahead,
                //          we encountered a split
                //          Copy current route to routesWithoutEnd, 
                //          add one of both lookahead Hexside
                //          add other to current route;
            }
            
            //
            
            return null;
        }

        public void CreateTradeRoutes()
        { }
        private LinkedList<HexSide> _Route = new LinkedList<HexSide>();
        public int Length
        { get { throw new NotImplementedException(); } }
        public Route() { }

    }
}
