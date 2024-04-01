using System;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Character : MonoBehaviour
{
    public UnityEvent PlayerDeath;
    public UnityEvent PlayerReceiveDamage;

    [Header("Controls")]
    public float playerHealth = 50.0f; // 5.0f
    public float currentHeaalth { get; set; }

    public float playerSpeed = 5.0f; // 5.0f
    public float crouchSpeed = 2.0f; // 2.0f
    public float sprintSpeed = 7.0f; // 7.0f
    public float jumpHeight = 0.8f; // 0.8f
    public float gravityMultiplayer = 2.0f; // 2.0f
    public float rotationSpeed = 5.0f; // 5.0f
    public float crouchColliderHeight = 1.35f; // 1.35f

    [Header("Animation Smoothing")]
    [Range(0, 1)]
    [SerializeField]
    public float speedDampTime = 0.1f; // 0.1f
    [Range(0, 1)]
    [SerializeField]
    public float velocityDampTime = 0.9f; // 0.9f
    [Range(0, 1)]
    [SerializeField]
    public float rotationDampTime = 0.2f; // 0.2f
    [Range(0, 1)]
    [SerializeField]
    public float airControl = 0.5f; // 0.5f
    [Range(0, 100)]
    [SerializeField]
    public float attackRad = 5.0f; // 0.5f

    public StateMachine movementSM;
    public StandingStateFSM standing;
    public JumpingStateFSM jumping;
    public CrouchingStateFSM crouching;
    public LandingStateFSM landing;
    public SprintStateFSM sprinting;
    public SprintJumpStateFSM sprintJumping;
    public CombatState combatting;
    public AttackStateFSM attacking;

    [HideInInspector] public float gravityValue = -9.81f;
    [HideInInspector] public float normalCollderHeight;
    [HideInInspector] public CharacterController controller;
    [HideInInspector] public PlayerCombatController combatController;
    [HideInInspector] public PlayerInput playerInput;
    [FormerlySerializedAs("CameraTransform")][HideInInspector] public Transform cameraTransform;
    public Animator animator;
    [HideInInspector] public Vector3 playerVelocity;
    public AnimatorController animatorController;

    private void Start()
    {
        currentHeaalth = playerHealth;

        controller = GetComponent<CharacterController>();
        combatController = GetComponent<PlayerCombatController>();
        playerInput = GetComponent<PlayerInput>();
        cameraTransform = Camera.main.transform;

        movementSM = new StateMachine();
        standing = new StandingStateFSM(this, movementSM);
        jumping = new JumpingStateFSM(this, movementSM);
        crouching = new CrouchingStateFSM(this, movementSM);
        landing = new LandingStateFSM(this, movementSM);
        sprinting = new SprintStateFSM(this, movementSM);
        sprintJumping = new SprintJumpStateFSM(this, movementSM);
        combatting = new CombatState(this, movementSM);
        attacking = new AttackStateFSM(this, movementSM);

        movementSM.Initialize(standing);

        normalCollderHeight = controller.height;
        gravityValue *= gravityMultiplayer;
    }

    private void Update()
    {
        movementSM.currentState?.HandleInput();
        movementSM.currentState?.LogicUpdate();
    }

    private void FixedUpdate()
    {
        movementSM.currentState?.PhysicsUpdate();

        float moveMouseX = Input.GetAxis("Mouse X");
        transform.Rotate(0f, moveMouseX, 0f);
    }

    public void ReceiveDamage(float damage)
    {
        currentHeaalth -= damage;
        PlayerReceiveDamage.Invoke();
        if (currentHeaalth <=  0)
        {
            PlayerDeath.Invoke();
        }
    }
}