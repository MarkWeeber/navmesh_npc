using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestNavMeshAgent : MonoBehaviour
{
    [SerializeField] private Transform target;
    private NavMeshAgent agent;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    
    private void Update()
    {
        agent.SetDestination(target.position);
    }
}
