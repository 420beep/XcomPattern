using System.Collections.Generic;
using UnityEngine;
namespace Ai
{
    public class Pathfind_agent : MonoBehaviour
    {
        private Ai_data data;
        public List<Waypoint> path;
        public Waypoint destination;
        public float Astar_heuristic;
        //agent will prefer to check paths first, if they are closer to goal.. (heuristic)
        public float pathlenght;
        [ContextMenu("calculate path")]
        public void Set_path()
        {
            List<Waypoint> wps = data.waypoints;
            if (wps == null) { Debug.LogWarning("waypoint list was found null..returning"); return; }
            if (wps.Count == 0) { Debug.LogWarning("no waypoints in list..returning"); return; }
            Waypoint position = Pathfind_lib.Nearest_wp(this.transform.position, wps);
            Pathfind_lib.Calculate_path_cost(this, wps);//WHENEVER AGENT MOVES, UPDATE PATHCOST..
            path = Pathfind_lib.Raycast_Astar(this, destination, position, wps, Astar_heuristic);
        }
        private void Update()
        {
            Pathfind_lib.Debug_pathLine(path);
        }
        private void Start()
        {
            data = FindObjectOfType<Ai_data>();
            if (data == null) { Destroy(this); Debug.LogWarning("agent removed, Ai_data component must be added to scene first!!"); }
        }
    }
}
