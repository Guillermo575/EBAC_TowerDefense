using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatManager : MonoBehaviour
{
    GameManager gameManager;
    void Start()
    {
        gameManager = GameManager.GetSingleton();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (!gameManager.IsLevelCleared)
            {
                gameManager.GameLevelCleared();
            }
        }
    }
}
