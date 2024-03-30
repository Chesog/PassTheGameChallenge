using UnityEngine;
using UnityEngine.InputSystem;

public class StateFSM
{
    public Character character;
    public StateMachine stateMachine;

    protected Vector3 gravityVelocity;
    protected Vector3 velocity;
    protected Vector2 input;

    public InputAction moveAction;
    public InputAction lookAction;
    public InputAction jumpAction;
    public InputAction crouchAction;
    public InputAction sprintAction;
    public InputAction drawWeaponAction;
    public InputAction attackAction;

    public StateFSM( Character _character , StateMachine _stateMachine)
    {
        character = _character;
        stateMachine = _stateMachine;

        moveAction = character.playerInput.actions["Move"];
        //lookAction = character.playerInput.actions["Look"];
        jumpAction = character.playerInput.actions["Jump"];
        crouchAction = character.playerInput.actions["Crouch"];
        sprintAction = character.playerInput.actions["Sprint"];
        drawWeaponAction = character.playerInput.actions["DrawWeapon"];
        attackAction = character.playerInput.actions["Attack"];
    }

    public virtual void Enter() {Debug.Log("Enter State : " + this.ToString());}
    public virtual void HandleInput() {}
    public virtual void LogicUpdate() {}
    public virtual void PhysicsUpdate() {}
    public virtual void Exit() {}
}
