using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

[RequireComponent(typeof(NavMeshAgent))]
public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 2.0f;
    [SerializeField] private float drag = 0.9f;
    [SerializeField] private float verticalPosition = 0f;

    private Transform model;
    private Vector3 heading = Vector3.zero;
    private Vector3 facing = Vector3.forward;
    private Vector3 velocity;

    public NavMeshAgent navMeshAgent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // navMeshAgent = GetComponent<NavMeshAgent>();
        // navMeshAgent.updatePosition = false;
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;

        model = transform.GetChild(0);
    }

    void FixedUpdate()
    {
        // transform.position = new Vector3(transform.position.x, verticalPosition, transform.position.z);

        // move the player by their input
        velocity += InputCameraRemap(heading) * speed;
        velocity *= drag;

        navMeshAgent.nextPosition = transform.position;
        if (navMeshAgent.CalculatePath(transform.position + velocity, navMeshAgent.path))
        {
            navMeshAgent.Move(velocity);
        }

        //rotate the player by their input
        model.forward = Vector3.Lerp(InputCameraRemap(facing), model.forward,1 - Vector3.Angle(InputCameraRemap(facing), model.forward)/120);
    }

    //Remaps input to move in the direction the camera faces
    private Vector3 InputCameraRemap(Vector3 input)
    {

        float rotation = Camera.main.transform.rotation.eulerAngles.y;

        return Quaternion.AngleAxis(rotation, Vector3.up) * input;
    }

    //recieve heading inputs
    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        heading = new Vector3(input.x, 0, input.y);
    }

    //recieve facing inputs
    public void OnLook(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        Vector3 lookDir = new Vector3(input.x, 0, input.y);

        //face the player model in the look direction
        if (lookDir != Vector3.zero)
        {
            facing = lookDir.normalized;
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {

    }

    public void OnShoot(InputAction.CallbackContext context)
    {

    }
}
