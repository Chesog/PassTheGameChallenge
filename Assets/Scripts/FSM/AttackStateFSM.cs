using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackStateFSM : StateFSM
{
    float timePassed;
    float clipLength;
    float clipSpeed;
    bool attack;
    
    public AttackStateFSM(Character _character, StateMachine _stateMachine) : base(_character, _stateMachine)
    {
        character = _character;
        stateMachine = _stateMachine;
    }
 
    public override void Enter()
    {
        base.Enter();
        character.animator.applyRootMotion = true;
        attack = false;
        timePassed = 0f;
        character.combatController.Attack(character.animator);
        character.animator.Play("Attack",0,0);
        character.animator.SetFloat("Blend", 0f);
        Attack();
    }
 
    public override void HandleInput()
    {
        base.HandleInput();
 
        if (attackAction.triggered)
        {
            attack = true;
        }
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
 
       
        
        if (attack)
        {
            stateMachine.ChangeState(character.attacking);
        }
        else if (character.combatController.ExitAttack(character.animator))
        {
            stateMachine.ChangeState(character.standing);
           
        }
     
 
    }

    private void Attack()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(character.transform.position, character.attackRad);
        foreach (var enemy in hitEnemies)
        {
            if (enemy.CompareTag("Enemy"))
            {
                enemy.GetComponent<PushController>().Push(character.transform.position);
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
        character.animator.applyRootMotion = false;
    }
}
