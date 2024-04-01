using System;
using System.Collections;
using UnityEngine;

namespace Enemy
{
    public class Enemy_Controller : MonoBehaviour, IHealth
    {
        public Animator animator;
        private float currentHeaalth { get; set; }

       public float MaxHealth;
        public float detectionRadius = 5f;
        public LayerMask characterControllerLayer;
        public float moveSpeed = 3f;
        public float attackRange = 1.5f;
        public float attackCooldown = 1f;
        public int attackDamage = 10; 

        private Transform target;
        private Coroutine movementCoroutine;
        private bool isAttacking;
        private float lastAttackTime;
        
        private void Start()
        {
           animator.SetFloat("Blend", 0);
           currentHeaalth = MaxHealth;
        }

        private void Update()
        {
            if (isDeath()) return;
            Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, characterControllerLayer);
          
            if (colliders.Length > 0)
            {
                target = colliders[0].transform;
                if (movementCoroutine == null)
                    movementCoroutine = StartCoroutine(MoveTowardsTargetCoroutine());
                else if (isAttacking)
                    StopCoroutine(movementCoroutine);

                if (!isAttacking && Vector3.Distance(transform.position, target.position) <= attackRange)
                {
                    Attack();
                }
            }
            else
            {
                target = null;
                if (movementCoroutine != null && !isAttacking)
                {
                    StopCoroutine(movementCoroutine);
                    movementCoroutine = null;
                }
            }
        }

        private void Attack()
        {
            if (Time.time - lastAttackTime >= attackCooldown)
            {
                animator.SetFloat("Blend",0);       
                Debug.Log("Attacking " + target.name + " for " + attackDamage + " damage.");
                animator.SetTrigger("Attack");
                target.GetComponent<Character>().ReceiveDamage(attackDamage);
                lastAttackTime = Time.time;
                StartCoroutine(AttackCooldownCoroutine());
            }
        }
        private IEnumerator AttackCooldownCoroutine()
        {
            isAttacking = true;
            yield return new WaitForSeconds(attackCooldown);
            isAttacking = false;
        }
        public bool isDeath()
        {
            return currentHeaalth < 0;
        }

        public void ReceiveDamage(float damage)
        {
            currentHeaalth -= damage;
            if (isDeath())
            {
                animator.SetTrigger("Die");
                animator.SetFloat("Blend",0);
                StopAllCoroutines();
                
                this.enabled = false;

            }
            else
            {
                animator.SetTrigger("Hit");
            }
        }

        public float GetMaxHealth()
        {
            return MaxHealth;
        }

        public float GetCurrentHealth()
        {
            return currentHeaalth;
        }

        private IEnumerator MoveTowardsTargetCoroutine()
        {
            animator.SetFloat("Blend",1);
            while (!isAttacking)
            {
                Vector3 direction = (target.position - transform.position).normalized;
                Debug.Log(direction);
                transform.Translate(direction * moveSpeed * Time.deltaTime);
                transform.LookAt(target);

                yield return null;
            }
            Debug.Log("MoveEnded");
        }
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, detectionRadius);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }
}

public interface IHealth
{
    public bool isDeath();
    public void ReceiveDamage(float damage);
    public float GetMaxHealth();
    public float GetCurrentHealth();
}

public interface IAttack
{
    public bool canAttack();
    public void Attack();
}