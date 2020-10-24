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

    void Pursue()
    {
        Vector3 targetDirection = target.transform.position - transform.position;

        float relativeHeading = Vector3.Angle(transform.forward, transform.TransformVector(target.transform.forward));
        float toTarget = Vector3.Angle(transform.forward, transform.TransformVector(targetDirection));



        if ((toTarget>90 && relativeHeading <20) || target.GetComponent<Drive>().currentSpeed < 0.01f)
        {
            Seek(target.transform.position);
            return;
        }
        float lookAhead = targetDirection.magnitude / (agent.speed + target.GetComponent<Drive>().currentSpeed);
        Vector3 seekTarget = target.transform.position + target.transform.forward * lookAhead;
        Seek(seekTarget);
        Debug.DrawLine(transform.position, seekTarget);
        
    }

    // Update is called once per frame
    void Update()
    {
        Pursue();
    }
}
