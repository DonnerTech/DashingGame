using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{
    public NavMeshAgent root;
    [SerializeField] private Vector3 worldDirection;
    [SerializeField] private float recoil;
    [SerializeField] private float refireRate;
    [SerializeField] private float spread;
    [SerializeField] private FiringType firingType;
    [SerializeField] private int clipSize;
    private enum FiringType
    {
        DoubleAction,
        SemiAuto,
        FullAuto
    }

    private bool fireWeapon;
    private float refireRate_timer = 0;
    private int roundsLeft;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // LateUpdate is called once per frame
    void LateUpdate()
    {
        refireRate_timer -= Time.deltaTime;

        //if clip is empty
        if (roundsLeft == 0)
        {
            if (firingType == FiringType.SemiAuto)
            {
                //wait for reload
                return;
            }
            if (firingType == FiringType.FullAuto)
            {
                // reload weapon automatically
                roundsLeft = clipSize;
            }
        }

        if (refireRate_timer <= 0 && fireWeapon)
            {
                roundsLeft--;
                //TODO: fire projectile

                // apply recoil
                if (root != null)
                {
                    root.Move(root.transform.InverseTransformDirection(-worldDirection.normalized * recoil));
                }

                refireRate_timer = refireRate;
            }

        if (firingType == FiringType.DoubleAction)
        {
            fireWeapon = false;
        }
    }

    public void FireWeapon(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            fireWeapon = true;
        }
        else if(context.canceled)
        {
            fireWeapon = false;
        }
    }

    public void Reload(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            roundsLeft = clipSize;
        }
    }

    public void FireWeapon()
    {
        fireWeapon = true;
    }
}
