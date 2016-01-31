using UnityEngine;
using MonsterLove.StateMachine;
using System.Collections;

public class GameController : StateBehaviour
{
    public enum GameStates
    {
        START,
        PLAY,
        INIT,
        HABITS,
        PAUSE,
        SCORE,
        END,
        TOSTART
    }

    public GameObject EntryPoint;

    public Player[] players;

    public PlayerParameters PlayerParameters;

    public CanvasGroup ScreenFader;
    public CanvasGroup HabitsCanvas;
    public CanvasGroup TimerCanvas;
    public CanvasGroup PlayerInfoCanvas;

    public float FadeTime = 5f;
    public float GameEndTime = 50f;

    private float GameTimer = 0f;

    private float FadeTimer = 0f;

    public void GenerateRituals()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                players[i].rituals[j] =  GenerateRitual(j);
            }
                
        }
    }

    public float GetTime()
    {
        return GameTimer;
    }

    public int GetScoreFromPlayer(int i)
    {
        return this.players[i-1].GetScore();
    }

    public int GetMistakesFromPlayer(int i)
    {
        return this.players[i-1].GetMistakes();
    }

	// Use this for initialization
	void Start () {

        for (int i = 0; i < players.Length; i++)
        {
            players[i].InitPlayer(PlayerParameters.PlayerSpeed, PlayerParameters.DrinkTime, PlayerParameters.LookTime, PlayerParameters.ReportTime, PlayerParameters.TauntTime);
        }

        Initialize<GameStates>();
        ChangeState(GameStates.INIT);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

     /* ==========================
    * ----- Starting state ------
    * When the level is started or restarted    
    */
    private void START_Enter()
    {
        GameTimer = 0;

        GameEvent.GameStarting = true;

        ScreenFader.alpha = 1;
        FadeTimer = 0f;

        Vector3 playerPos = EntryPoint.transform.position;
        playerPos.z = 0;
        playerPos.x -= 3;
        playerPos.y += 1.5f;

        for (int i = 0; i < players.Length; i++)
        {
            players[i].transform.position = playerPos;
            playerPos.y -= 1;
        }
            
    }
    private void START_Update()
    {
        FadeTimer += Time.deltaTime;

        ScreenFader.alpha = Mathf.Lerp(1, 0, (FadeTimer / FadeTime));

        if (ScreenFader.alpha <= 0.05f)
        {
            ChangeState(GameStates.PLAY, StateTransition.Safe);
        }
    }
    private void START_Exit()
    {
        GameEvent.GameStarting = false;
        ScreenFader.alpha = 0;
    }

    /* ==========================
    * ----- Playing state ------
    * When the level is started or restarted    
    */
    private void PLAY_Enter()
    {
        PlayerInfoCanvas.GetComponent<Animator>().SetBool("IsVisible", true);
    }
    private void PLAY_Update()
    {
        GameTimer += Time.deltaTime;

        if (GameEvent.P1Ritual != null)
        {

        }
        if (GameTimer > GameEndTime)
        {
            ChangeState(GameStates.END, StateTransition.Safe);
        }
    }
    private void PLAY_Exit()
    {
        PlayerInfoCanvas.GetComponent<Animator>().SetBool("IsVisible", false);
    }

    /* ==========================
   * ----- Ending state ------
   * When the level is started or restarted    
   */
    private void END_Enter()
    {
        GameEvent.GameEnding = true;
        ScreenFader.alpha = 0;
        FadeTimer = 0f;
    }
    private void END_Update()
    {
        FadeTimer += Time.deltaTime;

        ScreenFader.alpha = Mathf.Lerp((FadeTimer / FadeTime), 1, 0);

        if (ScreenFader.alpha >= 0.95f)
        {
            ChangeState(GameStates.START, StateTransition.Safe);
        }
    }
    private void END_Exit()
    {
        GameEvent.GameEnding = false;
    }

    /* ==========================
    * ----- INIT state ------
    * When the level is started or restarted    
    */
    private void INIT_Enter()
    {
        GenerateRituals();
        ScreenFader.alpha = 1;
        HabitsCanvas.alpha = 1;
        FadeTimer = 0f;

        Vector3 playerPos = EntryPoint.transform.position;
        playerPos.z = 0;
        playerPos.x -= 3;
        playerPos.y += 1.5f;

        for (int i = 0; i < players.Length; i++)
        {
            players[i].transform.position = playerPos;
            playerPos.y -= 1;
        }
    }
    private void INIT_Update()
    {
        FadeTimer += Time.deltaTime;

        ScreenFader.alpha = Mathf.Lerp(1, 0, (FadeTimer / FadeTime));

        if (ScreenFader.alpha <= 0.05f)
        {
            ChangeState(GameStates.HABITS, StateTransition.Safe);
        }
    }
    private void INIT_Exit()
    {
        ScreenFader.alpha = 0;
    }

    /* ==========================
    * ----- HABITS state ------
    * When the level is started or restarted    
    */
    private void HABITS_Enter()
    {

    }
    private void HABITS_Update()
    {
        if (Input.GetAxisRaw("Interact_P1") < 0 &&
            Input.GetAxisRaw("Interact_P2") < 0 &&
            Input.GetAxisRaw("Interact_P3") < 0 &&
        Input.GetAxisRaw("Interact_P4") < 0) 
        {
            ChangeState(GameStates.TOSTART, StateTransition.Safe);
        }

        if(Input.GetButton("ReportP1_P1")){
            Debug.Log("P1 List");
            HabitsCanvas.GetComponent<Animator>().SetInteger("PlayerRituals", 1);
        }
        else if(Input.GetButton("ReportP2_P2")){
            Debug.Log("P2 List");
            HabitsCanvas.GetComponent<Animator>().SetInteger("PlayerRituals", 2);
        }
        else if(Input.GetButton("ReportP3_P3")){
            Debug.Log("P3 List");
            HabitsCanvas.GetComponent<Animator>().SetInteger("PlayerRituals", 3);
        }
        else if(Input.GetButton("ReportP4_P4")){
            Debug.Log("P4 List");
            HabitsCanvas.GetComponent<Animator>().SetInteger("PlayerRituals", 4);
        }
        else{
             Debug.Log("Idle");
             HabitsCanvas.GetComponent<Animator>().SetInteger("PlayerRituals", 0);
        }
    }
    private void HABITS_Exit()
    {

    }

    /* ==========================
   * ----- TOSTART state ------
   * When the level is started or restarted    
   */
    private void TOSTART_Enter()
    {
        ScreenFader.alpha = 0;
        FadeTimer = 0f;
    }
    private void TOSTART_Update()
    {
        FadeTimer += Time.deltaTime;

        ScreenFader.alpha = Mathf.Lerp((FadeTimer / FadeTime), 1, 0);

        if (ScreenFader.alpha >= 0.95f)
        {
            ChangeState(GameStates.START, StateTransition.Safe);
        }
    }
    private void TOSTART_Exit()
    {
    }

    public void CheckResultOfReport(Report report, bool success)
    {
        switch (report.source)
        {
            case global::PlayerParameters.PlayerNumber.P1:
                players[0].reportResult = (success) ? 1 : -1;
                break;
            case global::PlayerParameters.PlayerNumber.P2:
                players[1].reportResult = (success) ? 1 : -1;
                break;
            case global::PlayerParameters.PlayerNumber.P3:
                players[2].reportResult = (success) ? 1 : -1;
                break;
            case global::PlayerParameters.PlayerNumber.P4:
                players[3].reportResult = (success) ? 1 : -1;
                break;
        }
    }

    public void FillReportRitual(Report report)
    {
        switch (report.target)
        {
            case global::PlayerParameters.PlayerNumber.P1 :
                if(players[0].GetState().Equals("DRINK") ||
                    players[0].GetState().Equals("LOOK"))
                {
                    report.ritual = players[0].ValidateCurrentRitual(GameTimer);

                    if (players[0].IsPlayerRitual(report.ritual))
                    {
                        players[0].GettingReported();
                        CheckResultOfReport(report, true);
                    }
                    else
                    {
                        CheckResultOfReport(report, false);
                    }
                }
                break;
            case global::PlayerParameters.PlayerNumber.P2:

                break;
            case global::PlayerParameters.PlayerNumber.P3:

                break;
            case global::PlayerParameters.PlayerNumber.P4:

                break;
        }
    }

    public Ritual GenerateRitual(int i)
    {
        float gameTime = (GameEndTime / 3) / 2 + (GameEndTime / 3) * (i - 1);
        PlayerParameters.Interaction type = PlayerParameters.Interaction.LOOK;

        if (Random.Range(0, 1) >= 0.5f)
        {
            type = PlayerParameters.Interaction.TAKE;
        }

        PlayerParameters.InteractiveObject usedObject = PlayerParameters.InteractiveObject.COFFEE_MACHINE;

        float random = Random.Range(0, 3);

        if (random >= 2)
        {
            usedObject = PlayerParameters.InteractiveObject.COMPUTER;
        }
        else if (random >= 1)
        {
            usedObject = PlayerParameters.InteractiveObject.WINDOW;
        }

        Ritual ritual = new Ritual(gameTime, usedObject, type);
        return ritual;
    }

    public class Report
    {
        public PlayerParameters.PlayerNumber source, target;

        public Ritual ritual;

        public Report(PlayerParameters.PlayerNumber source, PlayerParameters.PlayerNumber target)
        {
            this.source = source;
            this.target = target;
        }
    }

    public class Ritual
    {
        float gameTime;
        PlayerParameters.InteractiveObject usedObject;
        PlayerParameters.Interaction type;

        public Ritual(float gameTime, PlayerParameters.InteractiveObject usedObject, PlayerParameters.Interaction type)
        {
            this.gameTime = gameTime;
            this.usedObject = usedObject;
            this.type = type;
        }

        
        public string ToString()
        {
            return "" + Mathf.Floor((8 + (gameTime / 36))).ToString("#0") + ":" + Mathf.Floor(((gameTime % 36) / 0.60f)).ToString("00")+
                " : " + GetCorrespondingString();
        }

        public string GetCorrespondingString()
        {
            switch (usedObject)
            {
                case PlayerParameters.InteractiveObject.COFFEE_MACHINE :
                    if (type.Equals(PlayerParameters.Interaction.LOOK))
                        return "Look at the coffee machine";
                    else
                        return "Drink a coffee at the coffee machine";
                case PlayerParameters.InteractiveObject.COMPUTER :
                    if (type.Equals(PlayerParameters.Interaction.LOOK))
                        return "Look at computer";
                    else
                        return "Work on a computer";
                case PlayerParameters.InteractiveObject.WINDOW :
                    if (type.Equals(PlayerParameters.Interaction.LOOK))
                        return "Look through the window";
                    else
                        return "Open/Close a window";
                default:
                    return "And that's a bug !!";
            }


        }
    }
}
