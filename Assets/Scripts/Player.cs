using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

[RequireComponent(typeof(NavMeshAgent))]
public class Player : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 2.0f;
    [SerializeField] private float drag = 0.9f;
    [SerializeField] private float dashDistance;
    [SerializeField] private float maxVelocity;

    [Header("Direction")]
    [SerializeField] private float rotSpeed = 1f;

    private Transform model;
    private Vector3 heading = Vector3.zero;
    private Vector3 facing = Vector3.forward;
    private Vector3 velocity;
    private Vector3 lastPosition;

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
        navMeshAgent.ResetPath();

        // calculate the current velocity
        velocity = transform.position - lastPosition;
        // apply the players input
        velocity += InputCameraRemap(heading) * speed;
        velocity *= drag;
        // clamp the velocity
        if (velocity.magnitude > maxVelocity)
            velocity = velocity.normalized * maxVelocity;

        // save the current position and move the player
        lastPosition = transform.position;
        navMeshAgent.Move(velocity);

        //rotate the player
            model.forward = Vector3.MoveTowards(InputCameraRemap(facing), model.forward, rotSpeed * Time.fixedDeltaTime);
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
        // print(context);
        // print(context.GetType());
        if (context.performed)
        {
            print("Dash");

            Vector3 dashDestination = transform.position + InputCameraRemap(heading.normalized) * dashDistance;

            navMeshAgent.SetDestination(dashDestination);
            navMeshAgent.CalculatePath(dashDestination, navMeshAgent.path);
            if (navMeshAgent.pathStatus == NavMeshPathStatus.PathComplete)
            {
                // navMeshAgent.SetDestination(dashDestination);

                transform.position = dashDestination;
            }
        }
    }

    public void OnShoot(InputAction.CallbackContext context)
    {

    }
}
