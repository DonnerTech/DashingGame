using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Splines;

public class LinearPingPongEnemyMovement : MonoBehaviour
{
    [SerializeField] private NearestPointOnSpline nearestPointOnSpline;

    private NavMeshAgent navMeshAgent;
    [SerializeField] private float speed;
    private float targetDistance;

    private Vector3 oldPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = speed;

        NearestPointOnSpline.SplinePointData splineData = nearestPointOnSpline.GetNearestData(transform.position);
        targetDistance = Vector3.Distance(transform.position, splineData.position);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // get the nearest point on the spline
        NearestPointOnSpline.SplinePointData splineData = nearestPointOnSpline.GetNearestData(transform.position);


        // switch directions when an obsticle was encoutnered
        if (Vector3.Distance(oldPos, transform.position) < 0.001)
        {
            speed = -speed;
        }

        // stay x distance from the spline while traveling along it

        Vector3 newPos = transform.position + splineData.tangent.normalized * speed;

        float distance = Vector3.Distance(newPos, splineData.position);
        float error = targetDistance - distance;

        Vector3 correction = (newPos - splineData.position).normalized * error;

        newPos += correction;

        oldPos = transform.position;
        navMeshAgent.Move((newPos - transform.position));

    }
}
