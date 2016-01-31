using UnityEngine;
using MonsterLove.StateMachine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Player : StateBehaviour {

    public PlayerParameters.PlayerNumber playerNum;

    public GameController.Ritual[] rituals;

    private int score = 0;
    private int mistakes = 0;

    private float playerSpeed;

    private float drinkTime;
    private float lookTime;
    private float reportTime;
    private float tauntTime;

    private float drinkTimer;
    private float lookTimer;
    private float reportTimer;
    private float tauntTimer;

    private GameObject interactiveObject = null;

    private Rigidbody2D controller;
    private BoxCollider2D collider;

    private GameController.Report pendingReport = null;

    public int reportResult = 0;
    private Vector2 input;
    public enum PlayerStates
    {
        WALK,
        DRINK,
        LOOK,
        IDLE,
        REPORT,
        TAUNT
    }

	// Use this for initialization
	void Start () {
        

        controller = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();

        Initialize<PlayerStates>();
        ChangeState(PlayerStates.IDLE);
	}

    public void GettingReported()
    {
        this.score -= 200;
        if (score < 0)
        {
            score = 0;
        }
    }

    public GameController.Ritual ValidateCurrentRitual(float GameTime)
    {
        PlayerParameters.Interaction interaction = PlayerParameters.Interaction.TAKE;

        if(GetState().Equals("LOOK")){
            interaction = PlayerParameters.Interaction.LOOK;
        }
        GameController.Ritual ritual = new GameController.Ritual(GameTime, 
            PlayerParameters.InteractiveObject.COFFEE_MACHINE, 
            interaction);

        return ritual;
    }

    public bool IsPlayerRitual(GameController.Ritual ritual){
        return false;
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
        
        if (input.x == 0 && input.y == 0)
        {
            ChangeState(PlayerStates.IDLE, StateTransition.Safe);
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
        score += 100;
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
        score += 50;
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
    public void InitPlayer(float playerSpeed, float drinkTime, float lookTime, float reportTime, float tauntTime)
    {
        this.playerSpeed = playerSpeed;
        this.drinkTime = drinkTime;
        this.lookTime = lookTime;

        this.reportTime = reportTime;
        this.tauntTime = tauntTime;

        score = 0;
        mistakes = 0;
    }

    /* ==========================
   * ----- REPORT state ------
   * When the level is started or restarted    
   */
    private void REPORT_Enter()
    {
        reportTimer = 0;
        controller.velocity = new Vector2(0, 0);

        if (playerNum.Equals("P1"))
        {
            GameEvent.P1Ritual = this.pendingReport;
            this.pendingReport = null;
        }
        else if(playerNum.Equals("P2")){
            GameEvent.P2Ritual = this.pendingReport;
            this.pendingReport = null;
        }
        else if(playerNum.Equals("P3")){
            GameEvent.P3Ritual = this.pendingReport;
            this.pendingReport = null;
        }
        else if(playerNum.Equals("P4")){
            GameEvent.P4Ritual = this.pendingReport;
            this.pendingReport = null;
        }
    }
    private void REPORT_Update()
    {
        reportTimer += Time.deltaTime;

        if (reportTimer >= reportTime)
        {
            ChangeState(PlayerStates.IDLE, StateTransition.Safe);
        }
    }
    private void REPORT_Exit()
    {
        if (reportResult == 1)
        {
            this.score += 200;
        }
        else if (reportResult == -1)
        {
            this.score -= 200;
            if (score < 0)
            {
                score = 0;
            }
            this.mistakes++;
        }
        reportResult = 0;
    }

    /* ==========================
   * ----- TAUNT state ------
   * When the level is started or restarted    
   */
    private void TAUNT_Enter()
    {
        tauntTimer = 0;
        controller.velocity = new Vector2(0, 0);
    }
    private void TAUNT_Update()
    {
        tauntTimer += Time.deltaTime;

        if (tauntTimer >= tauntTime)
        {
            ChangeState(PlayerStates.IDLE, StateTransition.Safe);
        }

    }
    private void TAUNT_Exit()
    {

    }

    public void InitPlayer(float playerSpeed, float drinkTime, float lookTime, float reportTime)
    {
        this.playerSpeed = playerSpeed;
        this.drinkTime = drinkTime;
        this.lookTime = lookTime;

        score = 0;
        mistakes = 0;
    }

	// Update is called once per frame
	void Update () {

        //Debug.Log(playerNum + " " + stateMachine.GetState());
        
        if (GameEvent.GameStarting)
        {
            MoveGhost();
        }
        else 
        {
            if (!collider.enabled)
               collider.enabled = true;

            input = new Vector2(Input.GetAxisRaw("Horizontal_"+playerNum), Input.GetAxisRaw("Vertical_"+playerNum));

            if (stateMachine.GetState().Equals(PlayerStates.WALK) || stateMachine.GetState().Equals(PlayerStates.IDLE))
            {
                controller.velocity = input * ((GameEvent.GameEnding) ? playerSpeed * 0.3f : playerSpeed);
                CheckReport();
            }
                
        }
	}

    private void CheckReport()
    {

       if (Input.GetButtonDown("ReportP1_" + playerNum))
        {
            if (playerNum == PlayerParameters.PlayerNumber.P1)
            {
                Debug.Log("Taunt");
                ChangeState(PlayerStates.TAUNT, StateTransition.Safe);
            }
            else
            {
                Debug.Log(playerNum + " is reporting P1");
                this.pendingReport = new GameController.Report(playerNum, PlayerParameters.PlayerNumber.P1);
                ChangeState(PlayerStates.REPORT, StateTransition.Safe);
            }
        }
        else if (Input.GetButtonDown("ReportP2_" + playerNum))
        {
            if (playerNum == PlayerParameters.PlayerNumber.P2)
            {
                Debug.Log("Taunt");
                ChangeState(PlayerStates.TAUNT, StateTransition.Safe);
            }
            else
            {
                Debug.Log(playerNum + " is reporting P2");
                this.pendingReport = new GameController.Report(playerNum, PlayerParameters.PlayerNumber.P2);
                ChangeState(PlayerStates.REPORT, StateTransition.Safe);
            }
        }
        else if (Input.GetButtonDown("ReportP3_" + playerNum))
        {
            if (playerNum == PlayerParameters.PlayerNumber.P3)
            {
                Debug.Log("Taunt");
                ChangeState(PlayerStates.TAUNT, StateTransition.Safe);
            }
            else
            {
                Debug.Log(playerNum + " is reporting P3");
                this.pendingReport = new GameController.Report(playerNum, PlayerParameters.PlayerNumber.P3);
                ChangeState(PlayerStates.REPORT, StateTransition.Safe);
            }
        }
        else if (Input.GetButtonDown("ReportP4_" + playerNum))
        {
            if (playerNum == PlayerParameters.PlayerNumber.P4)
            {
                Debug.Log("Taunt");
                ChangeState(PlayerStates.TAUNT, StateTransition.Safe);
            }
            else
            {
                Debug.Log(playerNum + " is reporting P4");
                this.pendingReport = new GameController.Report(playerNum, PlayerParameters.PlayerNumber.P4);
                ChangeState(PlayerStates.REPORT, StateTransition.Safe);
            }
        }
    }

    private void MoveGhost()
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

    public int GetScore()
    {
        return this.score;
    }

    public int GetMistakes()
    {
        return this.mistakes;
    }

}
