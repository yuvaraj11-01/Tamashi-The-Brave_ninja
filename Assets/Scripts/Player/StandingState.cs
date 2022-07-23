using UnityEngine;

public class StandingState: State
{  
    float gravityValue;
    bool jump;   
    bool crouch;
    Vector3 currentVelocity;
    bool grounded;
    float playerSpeed;
    bool GroundCheck;
    float timePassed;
    float landingTime;
    bool falling;

    //bool drawWeapon;

    Vector3 cVelocity;

    public StandingState(Character _character, StateMachine _stateMachine) : base(_character, _stateMachine)
	{
		character = _character;
		stateMachine = _stateMachine;
	}

    public override void Enter()
    {
        base.Enter();

        jump = false;
        crouch = false;
        GroundCheck = true;
        //drawWeapon = false;
        falling = false;
        input = Vector2.zero;
        
        currentVelocity = Vector3.zero;
        gravityVelocity.y = 0;

        velocity = character.playerVelocity;
        playerSpeed = character.playerSpeed;
        grounded = character.controller.isGrounded;
        gravityValue = character.gravityValue;

        timePassed = 0f;
        landingTime = character.LandDelay;
    }

    public override void HandleInput()
    {
        base.HandleInput();

        if (jumpAction.triggered && timePassed > landingTime)
        {
            jump = true;
		}
		if (crouchAction.triggered)
		{
            crouch = true;
		}


/*		if (drawWeaponAction.triggered)
		{
            drawWeapon = true;
        }*/

        input = moveAction.ReadValue<Vector2>();
        velocity = new Vector3(input.x, 0,0);

        velocity = velocity.x * character.cameraTransform.right.normalized + velocity.z * character.cameraTransform.forward.normalized;
        velocity.y = 0f;
     
    }
    
    public override void LogicUpdate()
    {
        base.LogicUpdate();

        character.animator.SetFloat("speed", Mathf.Abs(input.x), character.speedDampTime, Time.deltaTime);

        if (jump )
        {
            stateMachine.ChangeState(character.jumping);
        }
		if (crouch)
		{
            stateMachine.ChangeState(character.crouching);
        }
        timePassed += Time.deltaTime;
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        gravityVelocity.y += gravityValue * Time.deltaTime;
        grounded = character.controller.isGrounded;

        if (grounded && gravityVelocity.y < 0)
        {
            gravityVelocity.y = 0f;
        }

        GroundCheck = CheckCollisionOverlap(character.transform.position + Vector3.down * character.normalColliderHeight);





        currentVelocity = Vector3.SmoothDamp(currentVelocity, velocity,ref cVelocity, character.velocityDampTime);
        character.controller.Move(currentVelocity * Time.deltaTime * playerSpeed + gravityVelocity * Time.deltaTime);
  
		if (velocity.sqrMagnitude>0)
		{
            character.transform.rotation = Quaternion.Slerp(character.transform.rotation, Quaternion.LookRotation(velocity),character.rotationDampTime);
        }
        
    }

    public override void Exit()
    {
        base.Exit();

        gravityVelocity.y = 0f;
        character.playerVelocity = new Vector3(input.x, 0, input.y);

        if (velocity.sqrMagnitude > 0)
        {
            character.transform.rotation = Quaternion.LookRotation(velocity);
        }
    }

    public bool CheckCollisionOverlap(Vector3 targetPositon)
    {
        int layerMask = 1 << 8;
        layerMask = ~layerMask;
        RaycastHit hit;

        Vector3 direction = targetPositon - character.transform.position;
        if (Physics.Raycast(character.transform.position, direction, out hit, character.normalColliderHeight+2, layerMask))
        {
            Debug.DrawRay(character.transform.position, direction * hit.distance, Color.yellow);
            return true;
        }
        else
        {
            Debug.DrawRay(character.transform.position, direction * character.normalColliderHeight, Color.white);
            return false;
        }
    }

}
