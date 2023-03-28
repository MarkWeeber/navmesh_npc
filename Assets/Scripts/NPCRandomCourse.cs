using UnityEngine;
using UnityEngine.AI;
using System.Linq;

[RequireComponent(typeof(NavMeshAgent))]
public class NPCRandomCourse : MonoBehaviour
{
    [SerializeField] private float minCourseRange = 5f;
    [SerializeField] private float maxCourseRange = 15f;
    [SerializeField] private float minRestSeconds = 3f;
    [SerializeField] private float maxRestSeconds = 5f;
    [SerializeField] private bool waitAtLaunch = true;

    private bool targetReached;
    private float setCourseTimer;
    private float distnaceToTarget = 0f;
    private NavMeshAgent agent;
    private NavMeshPath navMeshPath;
    private Vector3 destination;
    private float[] distanceToTargetArray;
    private int arrayIndex = 0;
    private PreservedDestinations preservedDestinations;

    private void Start()
    {
        preservedDestinations = GetComponentInParent<PreservedDestinations>();
        agent = GetComponent<NavMeshAgent>();
        navMeshPath = new NavMeshPath();
        distanceToTargetArray = new float[3] { -1, -1, -1 };
        if (waitAtLaunch)
        {
            setCourseTimer = Random.Range(minRestSeconds, maxRestSeconds);
        }
        else
        {
            setCourseTimer = 0;
        }
    }

    private void Update()
    {
        CheckIfDestinationIsReached();
        if (targetReached)
        {
            if(setCourseTimer > 0)
            {
                setCourseTimer -= Time.deltaTime;
            }
            else
            {
                SetNextCourse();
                setCourseTimer = Random.Range(minRestSeconds, maxRestSeconds);
            }
        }
    }

    private void SetNextCourse()
    {
        if (RetreiveRandomPoint(this.transform.position, minCourseRange, maxCourseRange, out destination))
        {
            agent.SetDestination(destination);
        }
    }

    private void CheckIfDestinationIsReached()
    {
        distnaceToTarget = Vector3.Distance(this.transform.position, agent.destination);
        if(arrayIndex == distanceToTargetArray.Length)
        {
            arrayIndex = 0;
        }
        distanceToTargetArray[arrayIndex] = distnaceToTarget;
        arrayIndex++;
        if(distanceToTargetArray.Distinct().Count() == 1)
        {
            targetReached = true;
        }
        else
        {
            targetReached = false;
        }
    }

    private bool RetreiveRandomPoint(Vector3 center, float minRange, float maxRange, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere.normalized * Random.Range(minRange, maxRange);
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, agent.areaMask))
            {
                if (agent.CalculatePath(hit.position, navMeshPath) && navMeshPath.status == NavMeshPathStatus.PathComplete)
                {
                    if (!OmitSpaces.PositionIsWithinOmitSpace(hit.position))
                    {
                        if (preservedDestinations.AddLocation(this, hit.position, agent.radius * 2.5f))
                        {
                            result = hit.position;
                            return true;
                        }
                    }
                }
            }
        }
        result = Vector3.zero;
        return false;
    }
}
