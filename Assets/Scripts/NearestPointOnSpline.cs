using System;
using UnityEngine;
using UnityEngine.Splines;


public class NearestPointOnSpline : MonoBehaviour
{
    [SerializeField] private SplineContainer splineContainer;

    public struct SplinePointData
    {
        public Vector3 position;
        public Vector3 tangent;
        public float cameraFov;
    }

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

    public SplinePointData GetNearestData(Vector3 position)
    {
        Spline spline = splineContainer[0];

        Unity.Mathematics.float3 nearest;
        float t;

        //Finds a position near a point
        SplineUtility.GetNearestPoint(spline, position, out nearest, out t, resolution: 4, iterations: 2);
        Debug.DrawLine(position, nearest, Color.blue);
        // returns the tangent to the position on the spline
        float zoom = Camera.main.fieldOfView;

        // attempt to get data "CameraFOV" stored in the spline
        if (spline.TryGetFloatData("CameraFOV", out SplineData<float> zoomData))
        {
            // convert from normalized distance to knot index
            // (halfway between knot 1 and 2 would return 1.5)
            float index = spline.ConvertIndexUnit(t, PathIndexUnit.Normalized, PathIndexUnit.Knot);

            // get the data at the knot index, which is interpolated between knots using lerp
            zoom = zoomData.Evaluate(spline, index, PathIndexUnit.Knot, InterpolatorUtility.SmoothStepFloat);
        }

        SplinePointData splinePointData = new()
        {
            position = nearest,
            tangent = spline.EvaluateTangent(t),
            cameraFov = zoom
        };

        return splinePointData;
    }
}
