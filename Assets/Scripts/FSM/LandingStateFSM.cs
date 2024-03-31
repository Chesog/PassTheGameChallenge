using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingStateFSM : StateFSM
{
    private float timePassed;
    private float landingTime;
    public LandingStateFSM(Character _character, StateMachine _stateMachine) : base(_character, _stateMachine)
    {
        character = _character;
        stateMachine = _stateMachine;
    }
    
    public override void Enter()
    {
        base.Enter();
        timePassed = 0.0f;
        character.animator.SetTrigger("Jump");
        character.animator.SetFloat("Blend",1);
        landingTime = 0.5f;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (timePassed > landingTime)
        {
            character.animator.SetTrigger("move");
            stateMachine.ChangeState(character.standing);
        }

        timePassed += Time.deltaTime;
        Debug.Log("timePassed : " + timePassed);
    }
}
