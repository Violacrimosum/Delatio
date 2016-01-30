using UnityEngine;
using MonsterLove.StateMachine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Player : StateBehaviour {

    public PlayerParameters.PlayerNumber playerNum;

    private float playerSpeed;

    private float drinkTime;
    private float lookTime;

    private float drinkTimer;
    private float lookTimer;

    private GameObject interactiveObject = null;

    private Rigidbody2D controller;
    private BoxCollider2D collider;

    private Vector2 input;
    public enum PlayerStates
    {
        WALK,
        DRINK,
        LOOK,
        IDLE
    }

	// Use this for initialization
	void Start () {
        

        controller = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();

        Initialize<PlayerStates>();
        ChangeState(PlayerStates.IDLE);
	}

    /* ==========================
    * ----- IDLE state ------
    * When the level is started or restarted    
    */
    private void IDLE_Enter()
    {

    }
    private void IDLE_Update()
    {
        if (this.interactiveObject != null)
        {
            if (Input.GetAxisRaw("Interact_" + playerNum) > 0)
            {
                ChangeState(PlayerStates.LOOK, StateTransition.Safe);
            }
            else if (Input.GetAxisRaw("Interact_" + playerNum) < 0)
            {
                ChangeState(PlayerStates.DRINK, StateTransition.Safe);
            }
        }
        if (input.x != 0 || input.y != 0)
        {
            ChangeState(PlayerStates.WALK, StateTransition.Safe);
        }  
    }
    private void IDLE_Exit()
    {

    }

    /* ==========================
   * ----- WALK state ------
   * When the level is started or restarted    
   */
    private void WALK_Enter()
    {

    }
    private void WALK_Update()
    {
        if (this.interactiveObject != null)
        {
            if (Input.GetAxisRaw("Interact_" + playerNum) > 0)
            {
                ChangeState(PlayerStates.LOOK, StateTransition.Safe);
            }
            else if (Input.GetAxisRaw("Interact_" + playerNum) < 0)
            {
                ChangeState(PlayerStates.DRINK, StateTransition.Safe);
            }
        }
    }
    private void WALK_Exit()
    {

    }

    /* ==========================
   * ----- DRINK state ------
   * When the level is started or restarted    
   */
    private void DRINK_Enter()
    {
        drinkTimer = 0;
        controller.velocity = new Vector2(0, 0);
    }
    private void DRINK_Update()
    {
        drinkTimer += Time.deltaTime;

        if (drinkTimer >= drinkTime)
        {
            ChangeState(PlayerStates.IDLE, StateTransition.Safe);
        }
    }
    private void DRINK_Exit()
    {

    }

    /* ==========================
   * ----- LOOK state ------
   * When the level is started or restarted    
   */
    private void LOOK_Enter()
    {
        lookTimer = 0;
        controller.velocity = new Vector2(0, 0);
    }
    private void LOOK_Update()
    {
        lookTimer += Time.deltaTime;

        if (lookTimer >= lookTime)
        {
            ChangeState(PlayerStates.IDLE, StateTransition.Safe);
        }
    }
    private void LOOK_Exit()
    {

    }
    public void InitPlayer(float playerSpeed, float drinkTime, float lookTime)
    {
        this.playerSpeed = playerSpeed;
        this.drinkTime = drinkTime;
        this.lookTime = lookTime;
    }

	// Update is called once per frame
	void Update () {

        if (GameEvent.GameStarting)
        {
            _MoveGhost();
        }
        else 
        {
            if (!collider.enabled)
               collider.enabled = true;

            input = new Vector2(Input.GetAxisRaw("Horizontal_"+playerNum), Input.GetAxisRaw("Vertical_"+playerNum));

            if (stateMachine.GetState().Equals(PlayerStates.WALK) || stateMachine.GetState().Equals(PlayerStates.IDLE))
                controller.velocity = input * ((GameEvent.GameEnding)?playerSpeed*0.3f:playerSpeed);
        }
	}

    private void _MoveGhost()
    {
        if (collider.enabled)
            collider.enabled = false;
        Vector2 ghostInput = new Vector2(1, 0);
        controller.velocity = ghostInput * (playerSpeed * 0.25f);
    }

    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "InteractiveObject")
        {
            this.interactiveObject = other.gameObject;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "InteractiveObject" && this.interactiveObject == other.gameObject)
        {
            this.interactiveObject = null;
        }
    }
   /* void OnTriggerStay(Collider other)
    {
        Debug.Log("Something has entered this zone.");
    }*/
   /* void OnCollisionEnter2D(Collision2D collision)
    {
       
        if (collision.gameObject.tag == "Player")
        {
            if (stateMachine.GetState().Equals(PlayerStates.IDLE) || stateMachine.GetState().Equals(PlayerStates.LOOK) || stateMachine.GetState().Equals(PlayerStates.DRINK))
                controller.isKinematic = true;
        }
    } */
}
