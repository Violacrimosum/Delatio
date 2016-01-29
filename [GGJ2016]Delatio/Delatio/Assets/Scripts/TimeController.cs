using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimeController : MonoBehaviour {

    GameController gameController;
    Text text;

	// Use this for initialization
	void Start () {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        text = GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {

        text.text = "" + Mathf.Floor((8 + (gameController.GetTime() / 36))).ToString("#0") + ":" + Mathf.Floor(((gameController.GetTime() % 36) / 0.60f)).ToString("00");
	}
}
