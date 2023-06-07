using UnityEngine;
using System.Collections.Generic;
namespace Ai
{
    public class Waypoint : MonoBehaviour
    {
        public float cost;
        public List<Waypoint> connections = new List<Waypoint>();
        private void Update()
        {
            foreach(Waypoint point in connections)
            {
                if(point == null) { continue; }
                Debug.DrawLine(this.transform.position, point.transform.position, new Color(0, 255, 0, 0.25f));
            }
        }
        private void Start()
        {
            this.transform.gameObject.name = "COST: " + cost;
            Ai_data data = FindObjectOfType<Ai_data>();
            if (data != null) { data.waypoints.Add(this); }
        }
        private void OnDestroy()
        {
            Ai_data data = FindObjectOfType<Ai_data>();
            if (data != null) { data.waypoints.Remove(this); }
        }
    }
}