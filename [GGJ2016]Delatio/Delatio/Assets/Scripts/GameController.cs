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

    public Player p1;

    public CanvasGroup ScreenFader;
    public float FadeTime = 3f;
    public float GameEndTime = 10f;

    private float GameTimer = 0f;

    private float FadeTimer = 0f;

    public float GetTime()
    {
        return GameTimer;
    }

	// Use this for initialization
	void Start () {

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

        Vector3 p1Pos = EntryPoint.transform.position;
        p1Pos.z = 0;
        p1Pos.x -= 3;
        p1.transform.position = p1Pos;
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
}
