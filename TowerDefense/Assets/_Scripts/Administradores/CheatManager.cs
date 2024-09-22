using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**
 * @file
 * @brief Puedes saltarte niveles, se desactiva antess 
 */
public class CheatManager : MonoBehaviour
{
    /** @hidden */
    GameManager gameManager;
    /** @hidden */
    void Start()
    {
        gameManager = GameManager.GetSingleton();
    }
    /**
     * F1: Finaliza automaticamente el nivel
     */
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