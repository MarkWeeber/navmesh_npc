using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NPCRandomCourse : MonoBehaviour
{
    [SerializeField] private float minCourseRange = 5f;
    [SerializeField] private float maxCourseRange = 15f;
    [SerializeField] private float minRestSeconds = 12f;
    [SerializeField] private float maxRestSeconds = 19f;
    [SerializeField] private bool waitAtLaunch = false;

    private bool targetReached;
    private bool launched = false;
    private float distnaceToTarget = 0f;
    private NavMeshAgent agent;
    private NavMeshPath navMeshPath;
    private float setCourseTime;


    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        navMeshPath = new NavMeshPath();
        targetReached = false;
    }

    private void Update()
    {
        CircleCourse();
        if (targetReached)
        {
            
        }
    }

    private void CircleCourse()
    {
        distnaceToTarget = Vector3.Distance(this.transform.position, agent.destination);
        if(distnaceToTarget == 1)
        {
            targetReached = true;
        }
        else
        {
            targetReached = false;
        }
    }

    private bool RandomPoint(Vector3 center, float minRange, float maxRange, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere.normalized * Random.Range(minRange, maxRange);
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, agent.areaMask))
            {
                if (agent.CalculatePath(hit.position, navMeshPath) && navMeshPath.status == NavMeshPathStatus.PathComplete)
                {
                    result = hit.position;
                    return true;
                }
            }
        }
        result = Vector3.zero;
        return false;
    }
}
