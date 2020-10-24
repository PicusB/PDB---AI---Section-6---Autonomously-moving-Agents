using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bot : MonoBehaviour
{
    NavMeshAgent agent;
    public GameObject target;
    public float wanderRadius = 10;
    public float wanderDistance = 20;
    public float wanderJitter = 1;
    Vector3 wanderTarget = Vector3.zero;

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

    void Wander()
    {
        wanderTarget += new Vector3(Random.Range(-1.0f, 1.0f) * wanderJitter, transform.position.y, Random.Range(-1.0f, 1.0f) * wanderJitter);
        wanderTarget.Normalize();
        wanderTarget *= wanderRadius;

        Vector3 targetLocal = wanderTarget += new Vector3(0f, 0f, wanderDistance);
        Vector3 targetWorld = gameObject.transform.InverseTransformVector(targetLocal);
        Seek(targetWorld);
    }

    void Evade()
    {
        Vector3 targetDirection = target.transform.position - transform.position;
        float lookAhead = targetDirection.magnitude / (agent.speed + target.GetComponent<Drive>().currentSpeed);
        Vector3 seekTarget = target.transform.position + target.transform.forward * lookAhead;
        Flee(seekTarget);
        Debug.DrawLine(transform.position, seekTarget, Color.red);
    }

    // Update is called once per frame
    void Update()
    {
        Wander();
    }
}
