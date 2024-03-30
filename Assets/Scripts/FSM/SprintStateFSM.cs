using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprintStateFSM : StateFSM
{
    private float gravityValue;
    private float playerSpeed;
    
    private bool grounded;
    private bool sprint;
    private bool sprintJump;
    
    private Vector3 currentVelocity;
    private Vector3 cVelocity;
    public SprintStateFSM(Character _character, StateMachine _stateMachine) : base(_character, _stateMachine)
    {
        character = _character;
        stateMachine = _stateMachine;
    }
    
    public override void Enter()
    {
        base.Enter();

        sprint = false;
        sprintJump = false;
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
        
        input = moveAction.ReadValue<Vector2>();
        velocity = new Vector3(input.x, 0.0f,input.y);
        
        velocity = velocity.x * character.cameraTransform.right.normalized +
                   velocity.z * character.cameraTransform.forward.normalized;
        velocity.y = 0.0f;

        if (sprintAction.triggered || input.sqrMagnitude == 0.0f)
            sprint = false;
        else
            sprint = true;

        if (jumpAction.triggered)
            sprintJump = true;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        
        if (sprint)
            character.animator.SetFloat("speed",input.magnitude + 0.5f,character.speedDampTime,Time.deltaTime);
        else
            stateMachine.ChangeState(character.standing);
        
        //if (sprintJump)
        //    stateMachine.ChangeState(character.sprintJumping);
        if (sprintJump)
            stateMachine.ChangeState(character.jumping);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        gravityVelocity.y += gravityValue * Time.deltaTime;
        grounded = character.controller.isGrounded;

        if (grounded && gravityVelocity.y < 0.0f)
            gravityVelocity.y = 0.0f;

        currentVelocity = Vector3.SmoothDamp(currentVelocity,velocity *2, ref cVelocity,character.velocityDampTime);

        character.controller.Move(currentVelocity * (Time.deltaTime * playerSpeed) + gravityVelocity * Time.deltaTime);
        
        if (velocity.magnitude > 0)
            character.transform.rotation = Quaternion.Slerp(character.transform.rotation,Quaternion.LookRotation(velocity),character.rotationDampTime);
    }
}
