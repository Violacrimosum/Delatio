using UnityEngine;
using MonsterLove.StateMachine;
using System.Collections;

public class GameController : StateBehaviour
{
    public enum GameStates
    {
        START,
        PLAY,
        END
    }

    public GameObject EntryPoint;

    public Player[] players;

    public PlayerParameters PlayerParameters;

    public CanvasGroup ScreenFader;
    public float FadeTime = 3f;
    public float GameEndTime = 50f;

    private float GameTimer = 0f;

    private float FadeTimer = 0f;

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
        ChangeState(GameStates.START);
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
        
    }
    private void PLAY_Update()
    {
        GameTimer += Time.deltaTime;

        if (GameTimer > GameEndTime)
        {
            ChangeState(GameStates.END, StateTransition.Safe);
        }
    }
    private void PLAY_Exit()
    {

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

    public class Report
    {
        PlayerParameters.PlayerNumber source, target;

        float gameTime;
        PlayerParameters.InteractiveObject usedObject;
        PlayerParameters.Interaction type;

    }
}
