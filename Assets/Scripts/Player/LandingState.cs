using UnityEngine;

public class LandingState:State
{
    //float timePassed;
    //float landingTime;
    //bool jump;



    public LandingState(Character _character, StateMachine _stateMachine) : base(_character, _stateMachine)
	{
		character = _character;
		stateMachine = _stateMachine;
	}

    public override void Enter()
	{
		base.Enter();
        //jump = false;

        //timePassed = 0f;
        character.animator.SetTrigger("land");
        //landingTime = character.LandDelay;

    }

    public override void HandleInput()
    {
        base.HandleInput();

        /*if (jumpAction.triggered)
        {
            jump = true;
        }*/

        input = moveAction.ReadValue<Vector2>();
        velocity = new Vector3(input.x, 0, 0);

        velocity = velocity.x * character.cameraTransform.right.normalized + velocity.z * character.cameraTransform.forward.normalized;
        velocity.y = 0f;

    }



    public override void LogicUpdate()
    {
        
        base.LogicUpdate();


        /*if (jump)
        {
            character.animator.SetTrigger("move");
            stateMachine.ChangeState(character.jumping);
        }
*/
        //Debug.Log(velocity.magnitude);

        /*        if(velocity.magnitude > 0f)
                {
                    character.animator.SetTrigger("move");
                    stateMachine.ChangeState(character.standing);
                }
                else
                {*/
        //if (timePassed > landingTime)
            {
                character.animator.SetTrigger("move");
                stateMachine.ChangeState(character.standing);
            }
        //}

        
        //timePassed += Time.deltaTime;
    }



}

