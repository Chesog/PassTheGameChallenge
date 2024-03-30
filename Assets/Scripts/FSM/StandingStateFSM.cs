using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandingStateFSM : StateFSM
{
    private float gravityValue;
    private float playerSpeed;

    private bool jump;
    private bool crouch;
    private bool grounded;
    private bool sprint;
    private bool drawWeapon;
    private bool attack;

    private Vector3 currentVelocity;
    private Vector3 cVelocity;

    public StandingStateFSM(Character _character, StateMachine _stateMachine) : base(_character, _stateMachine)
    {
        character = _character;
        stateMachine = _stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
        jump = false;
        crouch = false;
        sprint = false;
        attack = false;

        input = Vector2.zero;
        velocity = Vector3.zero;
        currentVelocity = Vector3.zero;
        gravityVelocity.y = 0.0f;

        playerSpeed = character.playerSpeed;
        grounded = character.controller.isGrounded;
        gravityValue = character.gravityValue;
    }

    public override void HandleInput()
    {
        base.HandleInput();

        if (jumpAction.triggered)
            jump = true;
        if (crouchAction.triggered)
            crouch = true;
        if (sprintAction.triggered)
            sprint = true;
        //if (drawWeaponAction.triggered)
        //    drawWeapon = true;

        if (attackAction.triggered)
            attack = true;

        input = moveAction.ReadValue<Vector2>();
        velocity = new Vector3(input.x, 0, input.y);

        velocity = velocity.x * character.cameraTransform.right.normalized +
                   velocity.z * character.cameraTransform.forward.normalized;
        velocity.y = 0.0f;

        velocity = velocity.normalized;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        character.animator.SetFloat("speed", input.magnitude, character.speedDampTime, Time.deltaTime);

        if (sprint)
            stateMachine.ChangeState(character.sprinting);
        if (jump)
            stateMachine.ChangeState(character.jumping);
        if (crouch)
            stateMachine.ChangeState(character.crouching);
        if (attack)
            stateMachine.ChangeState(character.attacking);
        if (drawWeapon)
        {
            stateMachine.ChangeState(character.combatting);
            character.animator.SetTrigger("drawWeapon");
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        gravityVelocity.y += gravityValue * Time.deltaTime;
        grounded = character.controller.isGrounded;

        if (grounded && gravityVelocity.y < 0.0f)
            gravityVelocity.y = 0.0f;

        currentVelocity = Vector3.SmoothDamp(currentVelocity, velocity, ref cVelocity, character.velocityDampTime);
        character.controller.Move(currentVelocity * Time.deltaTime * playerSpeed + gravityVelocity * Time.deltaTime);

        if (velocity.sqrMagnitude > 0)
            character.transform.rotation = Quaternion.Slerp(character.transform.rotation,
                Quaternion.LookRotation(velocity), character.rotationDampTime);
    }

    public override void Exit()
    {
        base.Exit();

        gravityVelocity.y = 0.0f;
        character.playerVelocity = new Vector3(input.x, 0.0f, input.y);

        if (velocity.sqrMagnitude > 0)
            character.transform.rotation = Quaternion.LookRotation(velocity);
    }
}