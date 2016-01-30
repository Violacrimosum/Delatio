using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerInfoViewer : MonoBehaviour {

    public int playerNum;

    GameController gameController;
    Text scoreText;

	// Use this for initialization
	void Start () {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        scoreText = GetComponentInChildren<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        scoreText.text = "Score : " + gameController.GetScoreFromPlayer(playerNum);
	}
}
