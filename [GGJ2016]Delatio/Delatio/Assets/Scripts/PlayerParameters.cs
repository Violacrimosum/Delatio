using UnityEngine;
using System.Collections;

[System.Serializable]
public class PlayerParameters {

    public enum PlayerNumber
    {
        P1 = 1,
        P2 = 2,
        P3 = 3,
        P4 = 4
    }

    public float PlayerSpeed = 7f;

    public float DrinkTime = 1.5f;
    public float LookTime = 1.75f;
    
}
