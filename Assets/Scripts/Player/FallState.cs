using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallState : State
{
    bool grounded;

    float gravityValue;
    float playerSpeed;

    Vector3 airVelocity;

    public FallState(Character _character, StateMachine _stateMachine) : base(_character, _stateMachine)
    {
        character = _character;
        stateMachine = _stateMachine;
    }

    public override void Enter()
    {
        base.Enter();

        grounded = false;
        gravityValue = character.gravityValue;
        playerSpeed = character.playerSpeed;
        gravityVelocity.y = 0;

        //character.controller.height = character.JumpColliderHeight;
        //character.controller.center = new Vector3(0f, 1 , 0f);

        character.animator.SetFloat("speed", 0);
        character.animator.SetTrigger("fall");
    }

    public override void Exit()
    {
        base.Exit();
        character.animator.SetTrigger("move");
        //character.controller.height = character.normalColliderHeight;
        //character.controller.center = new Vector3(0f, character.normalColliderHeight / 2f, 0f);
    }

    public override void HandleInput()
    {
        base.HandleInput();

        input = moveAction.ReadValue<Vector2>();
        velocity = new Vector3(input.x, 0, 0);

        velocity = velocity.x * character.cameraTransform.right.normalized + velocity.z * character.cameraTransform.forward.normalized;
        velocity.y = 0f;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (grounded)
        {

            stateMachine.ChangeState(character.landing);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if (!grounded)
        {

            //velocity = character.playerVelocity;
            airVelocity = new Vector3(input.x, 0, 0);

            //velocity = velocity.x * character.cameraTransform.right.normalized + velocity.z * character.cameraTransform.forward.normalized;
            //velocity.y = 0f;
            airVelocity = airVelocity.x * character.cameraTransform.right.normalized + airVelocity.z * character.cameraTransform.forward.normalized;
            airVelocity.y = 0f;
            character.controller.Move(gravityVelocity * Time.deltaTime + (airVelocity * character.airControl + velocity * (1 - character.airControl)) * playerSpeed * Time.deltaTime);


            if (velocity.magnitude > 0)
            {
                character.transform.rotation = Quaternion.Slerp(character.transform.rotation, Quaternion.LookRotation(velocity), character.rotationDampTime);
            }

        }

        gravityVelocity.y += gravityValue * Time.deltaTime;
        grounded = character.controller.isGrounded;
    }


}
