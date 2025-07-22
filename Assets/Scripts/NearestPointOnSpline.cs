using UnityEngine;
using UnityEngine.Splines;


public class NearestPointOnSpline : MonoBehaviour
{
    [SerializeField] private SplineContainer splineContainer;

    public Vector3 NearestTangent(Vector3 position)
    {
        Spline spline = splineContainer[0];

        Unity.Mathematics.float3 nearest;
        float index;

        //Finds a position near a point
        SplineUtility.GetNearestPoint(spline, position, out nearest, out index, resolution: 4, iterations: 2);
        Debug.DrawLine(position, nearest, Color.blue);
        // returns the tangent to the position on the spline
        return spline.EvaluateTangent(index);
    }
}
