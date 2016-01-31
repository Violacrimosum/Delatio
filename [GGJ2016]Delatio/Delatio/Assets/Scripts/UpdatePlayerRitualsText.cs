using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UpdatePlayerRitualsText : MonoBehaviour {

    GameController gameController;
    Text text;

    public int player;
    public int line;

	// Use this for initialization
	void Start () {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        text = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        if(gameController.players[player].rituals[line] != null)
            text.text = gameController.players[player].rituals[line].ToString();
	}
}
