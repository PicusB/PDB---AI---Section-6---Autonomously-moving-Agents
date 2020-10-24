using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bot : MonoBehaviour
{
    NavMeshAgent agent;
    public GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Seek(Vector3 targetLocation)
    {
        agent.SetDestination(targetLocation);
    }

    void Flee(Vector3 targetLocation)
    {
        Vector3 fleeLocation = transform.position - (targetLocation - transform.position);
        agent.SetDestination(fleeLocation);
    }

    // Update is called once per frame
    void Update()
    {
        Flee(target.transform.position);
    }
}
