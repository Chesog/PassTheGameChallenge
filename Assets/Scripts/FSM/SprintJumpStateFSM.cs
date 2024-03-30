using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprintJumpStateFSM : StateFSM
{
    private float timePassed;
    private float jumpTime;
    
    public SprintJumpStateFSM(Character _character, StateMachine _stateMachine) : base(_character, _stateMachine)
    {
        character = _character;
        stateMachine = _stateMachine;
    }
    
    public override void Enter()
    {
        base.Enter();

        character.animator.applyRootMotion = true;
        timePassed = 0.0f;
        character.animator.SetTrigger("sprintJump");
        Jump();

        jumpTime = 1.0f;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (timePassed > jumpTime)
        {
            character.animator.SetTrigger("move");
            stateMachine.ChangeState(character.sprinting);
        }

        timePassed += Time.deltaTime;
    }
    
    private void Jump()
    {
        gravityVelocity.y += Mathf.Sqrt(character.jumpHeight * -3.0f * character.gravityValue);
    }

    public override void Exit()
    {
        base.Exit();
        character.animator.applyRootMotion = false;
    }
}
