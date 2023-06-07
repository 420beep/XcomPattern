using System.Collections.Generic;
using UnityEngine;
namespace Ai
{
    public class Pathfind_lib
    {
        public static List<Waypoint> Raycast_Astar(Pathfind_agent agent, Waypoint destination, Waypoint start_wp, List<Waypoint> waypoints, float heuristic)
        {
            //=================================================================================
            Waypoint[] unvisited = new Waypoint[waypoints.Count];
            #region set initial array values
            for (int n = 0; n < waypoints.Count; n++)
            {
                unvisited[n] = waypoints[n];
            }
            #endregion
            List<Waypoint> visited = new List<Waypoint>(waypoints.Count);
            float[] distances = new float[unvisited.Length];
            #region set initial distances 9999
            //-------------------------
            for (int i = 0; i < distances.Length; i++)
            {
                distances[i] = 9999;
            }
            //set distances to be 9999 each
            #endregion

            #region visit first node
            int nodeiD = waypoints.IndexOf(start_wp);
            Waypoint current_node = start_wp;
            distances[nodeiD] = 0;
            visited.Add(current_node);
            unvisited[nodeiD] = null;
            float lastnodeDistance = 0;
            #endregion

            while (QueNotEmpty(unvisited))
            {
                //--------------------------------------------------------------------------
                //WHILE QUE NOT EMPTY.. FIND NEXT NODE, AND IMPROVE DISTANCES!!! PUSH NODE INTO VISITED, until array nullified
                int best_next_nodeID = 9999;
                float best_next_nodeDist = 9999;
                
                #region find best node to visit, update distances
                for (int i = 0; i < unvisited.Length; i++)
                {
                    Waypoint point = unvisited[i];
                    if (point == null) { continue; }
                    if (!current_node.connections.Contains(point)) { continue; }
                    Vector3 start = current_node.transform.position;
                    Vector3 end = point.transform.position;
                    //------------------------------------------------
                    //A* tack-on 
                    float Cost_score = (destination.transform.position - end).magnitude * heuristic;
                    #region improve estimate distance for each node  
                    //if connection possible, update best distance
                    float distance = point.cost + lastnodeDistance + Cost_score;
                    //UPDATE BEST DISTANCE 
                    if (distances[i] > distance) { distances[i] = distance; }
                    #endregion

                    #region cache best -next node 
                    if (distance < best_next_nodeDist)
                    {
                        best_next_nodeID = i; best_next_nodeDist = distance;
                    }
                    //====================NEXT NODE WE'D LIKE TO MOVE TOWARDS, IS CLOSEST TO END DESTINATION
                    #endregion
                    //------------------------------------------------

                }
                #endregion

                #region push node from unvisited, to visited!
                if (best_next_nodeID == 9999) { break; }
                Waypoint nextNode = unvisited[best_next_nodeID];

                visited.Add(nextNode);
                lastnodeDistance = (current_node.transform.position - nextNode.transform.position).magnitude;
                current_node = nextNode;
                unvisited[best_next_nodeID] = null;
                if (current_node == destination) { break; }
                #endregion
                //------break^--------------------------------------------------------------------
            }

            return visited;
        }
        static bool QueNotEmpty(Waypoint[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] != null) { return true; }
            }
            return false;
        }
        public static Waypoint Nearest_wp(Vector3 p,List<Waypoint>wps)
        {
            float bestdist = 9999;
            Waypoint bestWp = null;
            foreach(Waypoint pnt in wps)
            {
                float d = (pnt.transform.position - p).magnitude;
                if (d < bestdist) { bestdist = d; bestWp = pnt; }
            }
            return bestWp;
        }
        public static void connect_waypoints(List<Waypoint> points,int tilesize,int max_travelcost)
        {
            foreach(Waypoint point in points) { point.connections.Clear(); }
            foreach(Waypoint point in points)
            {
                foreach(Waypoint otherpoint in points)
                {
                    if (point == otherpoint||otherpoint.cost>max_travelcost) { continue; }
                    float D = (point.transform.position-otherpoint.transform.position).magnitude;
                    if (D <= tilesize + 0.25f) { point.connections.Add(otherpoint); }
                }
            }
        }
        #region visualization
        public static void Calculate_path_cost(Pathfind_agent agent, List<Waypoint> path)
        {
            agent.pathlenght = 0;

            for (int i = 1; i < path.Count; i++)
            {
                float distance = path[i].cost;
                agent.pathlenght += distance;
            }
        }
        public static void Debug_pathLine(List<Waypoint> points)
        {
            for (int i = 0; i < points.Count - 1; i++)
            {
                Vector3 start = points[i].transform.position;
                Vector3 end = points[i + 1].transform.position;

                Debug.DrawLine(start, end, Color.cyan);
            }
        }
        #endregion
    }
}