using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Waypoints : MonoBehaviour
{
    public static Waypoints instance;
    [SerializeField] private List<Vector3> waypoints;
    [SerializeField] private Vector3 stepping = Vector3.one;
    [SerializeField] private LayerMask omitMask = 0;
    private void Awake()
    {
        instance = this;
        if (waypoints.Count == 0)
        {
            BuildWaypoints();    
        }
    }

    private void BuildWaypoints()
    {
        waypoints = new List<Vector3>();
        NavMeshAgent navMeshAgent = GetComponent<NavMeshAgent>();
        BoxCollider boxCollider = GetComponent<BoxCollider>();
        List<BoxCollider> omitSpaces = FindGameObjectsWithLayer<BoxCollider>(omitMask);
        Vector3 searhAreaStart = new Vector3(
            transform.position.x + boxCollider.center.x - boxCollider.size.x / 2f,
            transform.position.y + boxCollider.center.y - boxCollider.size.y / 2f,
            transform.position.z + boxCollider.center.z - boxCollider.size.z / 2f
        );
        Vector3 searchAreaEnd = new Vector3(
            transform.position.x + boxCollider.center.x + boxCollider.size.x / 2f,
            transform.position.y + boxCollider.center.y + boxCollider.size.y / 2f,
            transform.position.z + boxCollider.center.z + boxCollider.size.z / 2f
        );
        NavMeshPath navMeshPath = new NavMeshPath();
        for (float x = searhAreaStart.x; x <= searchAreaEnd.x; x += stepping.x)
        {
            for (float z = searhAreaStart.z; z <= searchAreaEnd.z; z += stepping.z)
            {
                for (float y = searhAreaStart.y; y <= searchAreaEnd.y; y += stepping.y)
                {
                    Vector3 checkingPosition = new Vector3(x, y, z);
                    if (navMeshAgent.CalculatePath(checkingPosition, navMeshPath) && navMeshPath.status == NavMeshPathStatus.PathComplete)
                    {                        
                        bool _valid = true;
                        foreach (BoxCollider _item in omitSpaces)
                        {
                            if (_item.bounds.Contains(checkingPosition))
                            {
                                _valid = false;
                            }
                        }
                        if(_valid)
                        {
                            waypoints.Add(checkingPosition);
                            y += navMeshAgent.height;
                        }
                    }
                }
            }
        }
    }

    private List<T> FindGameObjectsWithLayer<T>(LayerMask targetMask)
    {
        List<T> ans = new List<T>();
        if (omitMask != 0)
        {
            GameObject[] allObjects = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[];
            foreach (GameObject _object in allObjects)
            {
                if ((targetMask & (1 << _object.layer)) != 0)
                {
                    ans.Add(_object.GetComponent<T>());
                }
            }
        }
        return ans;
    }

    public static Vector3 GetNextRandomDestination(Vector3 currentDestination)
    {
        Vector3 ans = Vector3.zero;
        return ans;
    }

}
