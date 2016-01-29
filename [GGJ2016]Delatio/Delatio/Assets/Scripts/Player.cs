using UnityEngine;
using MonsterLove.StateMachine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : StateBehaviour {

    public PlayerValue.PlayerNumber playerNum;

    public float playerSpeed = 5f;

    private Rigidbody2D controller;
    private BoxCollider2D collider;

    public enum PlayerStates
    {
        ENTER,
        WALK,
        IDLE
    }

    

	// Use this for initialization
	void Start () {
        Initialize<PlayerStates>();
        ChangeState(PlayerStates.IDLE);

        controller = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
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
            Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal_P1"), Input.GetAxisRaw("Vertical_P1"));

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
}
