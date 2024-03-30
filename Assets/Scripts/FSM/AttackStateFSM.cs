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
 
        attack = false;
        character.animator.applyRootMotion = true;
        timePassed = 0f;
        character.animator.SetTrigger("Attack");
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
 
        timePassed += Time.deltaTime;
        clipLength = character.animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        clipSpeed = character.animator.GetCurrentAnimatorStateInfo(0).speed;
 
        if (timePassed >= clipLength / clipSpeed && attack)
        {
            stateMachine.ChangeState(character.attacking);
            Attack();
        }
        if (timePassed >= clipLength / clipSpeed)
        {
            stateMachine.ChangeState(character.standing);
            character.animator.SetTrigger("normal");
        }
        if (attackAction.triggered)
        {
            stateMachine.ChangeState(character.attacking);
            Attack();
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
