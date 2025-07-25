using UnityEngine;

[RequireComponent(typeof(NearestPointOnSpline))]
public class CameraController : MonoBehaviour
{
    private NearestPointOnSpline nearestPointOnSpline;
    [SerializeField] private Player player;
    [SerializeField] private float tilt;
    [SerializeField] private float rotSpeed = 1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        nearestPointOnSpline = GetComponent<NearestPointOnSpline>();
    }

    // Update is called once per frame
    void Update()
    {
        //get vector the camera should face
        Vector3 dir = Vector3.MoveTowards(transform.forward, nearestPointOnSpline.NearestTangent(player.transform.position), rotSpeed * Time.deltaTime);
        dir.y = 0;
        //point the camera in the direction of the tangent
        transform.rotation = Quaternion.LookRotation(dir);
        transform.Rotate(new Vector3(tilt, 0, 0));
    }
}
