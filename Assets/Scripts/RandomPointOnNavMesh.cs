using UnityEngine;
using UnityEngine.AI;

public class RandomPointOnNavMesh : MonoBehaviour
{
    [SerializeField] private float minRange = 5.0f;
    [SerializeField] private float maxRange = 20.0f;
    private NavMeshAgent agent;
    private NavMeshPath navMeshPath;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        navMeshPath = new NavMeshPath();
    }

    private bool RandomPoint(Vector3 center, float minRange, float maxRange, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere.normalized * Random.Range(minRange, maxRange);
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, (agent != null) ? agent.areaMask : NavMesh.AllAreas))
            {
                if (agent != null)
                {
                    if (agent.CalculatePath(hit.position, navMeshPath) && navMeshPath.status == NavMeshPathStatus.PathComplete)
                    {
                        result = hit.position;
                        return true;
                    }
                }
                else
                {
                    result = hit.position;
                    return true;
                }
            }
        }
        result = Vector3.zero;
        return false;
    }

    private void Update()
    {
        Vector3 point;
        if (RandomPoint(transform.position, minRange, maxRange, out point))
        {
            Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f);
        }
    }
}