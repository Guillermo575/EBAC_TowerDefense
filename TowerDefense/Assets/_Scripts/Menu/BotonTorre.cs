using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BotonTorre : MonoBehaviour
{
    private GameManager gameManager;
    public int Costo;
    private Button boton;
    void Start()
    {
        gameManager = GameManager.GetSingleton();
        boton = this.GetComponent<Button>();
    }
    void Update()
    {
        HabilitarBoton();
    }
    private void HabilitarBoton()
    {
        gameManager = GameManager.GetSingleton();
        boton = this.GetComponent<Button>();
        boton.interactable = gameManager.GetRecursos() >= Costo;
    }
}
