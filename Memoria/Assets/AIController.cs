using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    public NavMeshAgent agent;
    private Animator animator;

    [Range(0, 100)] public float speed;
    [Range(1, 500)] public float walkRadius;

    public void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        if(agent != null)
        {
            speed = agent.speed;
            agent.SetDestination(RandomNavMeshLocation());
        }
    }

    public void FixedUpdate()
    {
        if(agent != null && agent.remainingDistance <= agent.stoppingDistance)
        {
            agent.SetDestination(RandomNavMeshLocation());
        }
        if(speed > 0)
        {
            animator.SetFloat("Forward", 1);
            animator.SetFloat("Running", 1);
        }

    }

    public Vector3 RandomNavMeshLocation()
    {
        Vector3 finalPosition = Vector3.zero;
        Vector3 randomPosition = Random.insideUnitSphere * walkRadius;
        randomPosition += transform.position;
        if(NavMesh.SamplePosition(randomPosition, out NavMeshHit hit, walkRadius, 1))
        {
            finalPosition = hit.position;
        }
        return finalPosition;

    }
}
