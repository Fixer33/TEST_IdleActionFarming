using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CharacterControl : MonoBehaviour
{
    [SerializeField] private FloatingJoystick joystick;
    [SerializeField] private Transform mesh;
    [SerializeField] private Animator animator;
    [SerializeField] private float movementSpeed = 1;
    [SerializeField] private GameObject weaponObject;

    public bool _blockMovement = false;

    private Rigidbody _rigidbody;
    private int _updateInterval = 3;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        HandleMovementAndRotation();
        if (Time.frameCount % _updateInterval == 0)
        {
            HandleAnimation();
        }
    }

    #region Movement and Rotation
    private Vector3 _direction;
    private void HandleMovementAndRotation()
    {
        if (_blockMovement)
        {
            return;
        }

        _direction = new Vector3(joystick.Horizontal, 0, joystick.Vertical);
        _direction.Normalize();

        MoveCharacter(_direction);
        RotateMesh(_direction);
    }

    private void MoveCharacter(Vector3 direction)
    {
        Vector3 target = transform.position + (direction * movementSpeed / 10);
        _rigidbody.MovePosition(target);
    }

    private void RotateMesh(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            mesh.forward = direction;
        }
    }
    #endregion

    #region Animation
    private void HandleAnimation()
    {
        BlockMovementOnActionAnimation();
        HandleIdleRunChanging();
    }

    private void BlockMovementOnActionAnimation()
    {
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        if (state.IsTag("Action"))
        {
            _blockMovement = true;
        }
        else
        {
            _blockMovement = false;

            if (weaponObject.activeSelf)
                weaponObject.SetActive(false);
        }
    }

    private void HandleIdleRunChanging()
    {
        if (_blockMovement)
            return;

        bool isRunning = animator.GetBool("isRunning");
        bool noInput = joystick.Horizontal == 0 && joystick.Vertical == 0;
        if (noInput)
        {
            //Idle
            if (isRunning == false)
                return;

            animator.SetBool("isRunning", false);
        }
        else
        {
            //Run
            if (isRunning == true)
                return;

            animator.SetBool("isRunning", true);
        }
    }

    public void AnimateVegetationCut()
    {
        _blockMovement = true;
        weaponObject.SetActive(true);
        animator.SetTrigger("cutVegetation");
    }

    public Animator GetAnimator()
    {
        return animator;
    } 
    #endregion
}
