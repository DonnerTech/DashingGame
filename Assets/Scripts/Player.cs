using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Scripting;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    private CharacterController controller;
    [SerializeField] private float speed = 2.0f;
    [SerializeField] private float drag = 0.9f;
    [SerializeField] private float verticalPosition = 0f;

    private Transform model;
    private Vector3 direction;
    private Vector3 velocity;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();
        model = transform.GetChild(0);
    }

    void FixedUpdate()
    {
        transform.position.Set(transform.position.x, verticalPosition, transform.position.z);

        velocity += direction * speed;
        velocity *= drag;
        controller.Move(velocity);

        // direction = Vector3.zero;
    }

    //Remaps input to move in the direction the camera faces
    private Vector3 InputCameraRemap(Vector3 input)
    {

        float rotation = Camera.main.transform.rotation.eulerAngles.y;

        return Quaternion.AngleAxis(rotation, Vector3.up) * input;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        direction = new Vector3(input.x, 0, input.y);
        direction = InputCameraRemap(direction);
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        Vector3 lookDir = new Vector3(input.x, 0, input.y);

        //face the player model in the look direction
        if (lookDir != Vector3.zero)
        {
            model.forward = InputCameraRemap(lookDir.normalized);
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {

    }

    public void OnShoot(InputAction.CallbackContext context)
    {

    }
}
