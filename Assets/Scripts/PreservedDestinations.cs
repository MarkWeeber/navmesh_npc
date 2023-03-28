using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class PreservedDestinations : MonoBehaviour
{
    [SerializeField] private List<NPCPreservation> locations;

    private void Start()
    {
        foreach (NPCRandomCourse item in GetComponentsInChildren<NPCRandomCourse>().ToList())
        {
            locations.Add(new NPCPreservation() {initialized = false, destination = Vector3.zero, npc = item });
        }
    }

    public bool AddLocation(NPCRandomCourse npc, Vector3 destination, float minDistance)
    {
        bool ans = false;
        if(locations.Where(t => t.initialized = true).Where(u => Vector3.Distance(u.destination, destination) < minDistance).Count() == 0)
        {
            NPCPreservation searchedNPC = locations.FirstOrDefault(t => t.npc == npc);
            if (searchedNPC != null)
            {
                searchedNPC.initialized = true;
                searchedNPC.destination = destination;
            }
            ans = true;
        }
        return ans;
    }
}

[System.Serializable]
public class NPCPreservation
{
    public bool initialized;
    public NPCRandomCourse npc;
    public Vector3 destination;
}