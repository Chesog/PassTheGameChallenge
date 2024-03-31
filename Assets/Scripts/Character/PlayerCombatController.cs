using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{
    public List<AttackSO> combo;
    float timeBetweenCombo = 0.2f;
    private float lastClickedTime = 0;
    private float lastComboEnd = 0;
    private int comboCounter = 0;


    public void Attack(Animator animator)
    {
        if (Time.time - lastComboEnd > timeBetweenCombo && comboCounter <= combo.Count)
        {
            CancelInvoke(nameof(EndCombo));

            if (Time.time - lastClickedTime >=timeBetweenCombo)
            {
                animator.runtimeAnimatorController = combo[comboCounter].overrideController;
                animator.Play("Attack",0,0);
                comboCounter++;
                lastClickedTime = Time.time;
                if (comboCounter >= combo.Count)
                {
                    comboCounter = 0;
                }
                
            }
        }
    }

    public bool ExitAttack(Animator animator)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >0.9f && animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            Invoke(nameof(EndCombo),1);
            return true;
        }

        return false;
    }

    void EndCombo()
    {
        comboCounter = 0;
        lastComboEnd = Time.time;
    }
}