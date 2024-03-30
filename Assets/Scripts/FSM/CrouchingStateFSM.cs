using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CrouchingStateFSM : StateFSM
{
    private float playerSpeed;
    private float gravityValue;

    private bool belowCeiling;
    private bool crouchHeld;
    private bool grounded;

    private Vector3 currentVelocity;

    public CrouchingStateFSM(Character _character, StateMachine _stateMachine) : base(_character, _stateMachine)
    {
        character = _character;
        stateMachine = _stateMachine;
    }

    public override void Enter()
    {
        base.Enter();

        character.animator.SetTrigger("crouch");
        belowCeiling = false;
        crouchHeld = false;
        gravityVelocity.y = 0.0f;

        playerSpeed = character.crouchSpeed;

        character.controller.height = character.crouchColliderHeight;
        character.controller.center = new Vector3(0.0f, character.crouchColliderHeight / 2.0f, 0.0f);

        grounded = character.controller.isGrounded;
        gravityValue = character.gravityValue;
    }

    public override void HandleInput()
    {
        base.HandleInput();

        if (crouchAction.triggered)
                crouchHeld = true;

        input = moveAction.ReadValue<Vector2>();
        velocity = new Vector3(input.x, 0.0f, input.y);

        velocity = velocity.x * character.cameraTransform.right.normalized +
                   velocity.z * character.cameraTransform.forward.normalized;
        velocity.y = 0.0f;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        character.animator.SetFloat("speed", input.magnitude, character.speedDampTime, Time.deltaTime);

        if (crouchHeld)
            stateMachine.ChangeState(character.standing);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        belowCeiling = CheckCollisionOverlap(character.transform.position + Vector3.up * character.normalCollderHeight);
        gravityVelocity.y += gravityValue * Time.deltaTime;
        grounded = character.controller.isGrounded;

        if (grounded && gravityVelocity.y < 0)
            gravityVelocity.y = 0.0f;

        currentVelocity = Vector3.Lerp(currentVelocity, velocity, character.velocityDampTime);
        character.controller.Move(currentVelocity * Time.deltaTime * playerSpeed + gravityVelocity * Time.deltaTime);

        if (velocity.magnitude > 0)
            character.transform.rotation = Quaternion.Slerp(character.transform.rotation,
                Quaternion.LookRotation(velocity), character.rotationDampTime);
    }

    public bool CheckCollisionOverlap(Vector3 targetPosition)
    {
        int layerMask = 1 << 8;
        layerMask = ~layerMask;
        RaycastHit hit;

        Vector3 direction = targetPosition - character.transform.position;
        if (Physics.Raycast(character.transform.position, direction, out hit, character.normalCollderHeight, layerMask))
        {
            Debug.DrawRay(character.transform.position, direction * hit.distance, Color.yellow);
            return true;
        }
        else
        {
            Debug.DrawRay(character.transform.position, direction * character.normalCollderHeight, Color.white);
            return false;
        }
    }

    public override void Exit()
    {
        base.Exit();

        character.controller.height = character.normalCollderHeight;
        character.controller.center = new Vector3(0.0f, character.normalCollderHeight / 2.0f, 0.0f);

        gravityVelocity.y = 0.0f;

        character.playerVelocity = new Vector3(input.x, 0.0f, input.y);
        character.animator.SetTrigger("move");
    }
}