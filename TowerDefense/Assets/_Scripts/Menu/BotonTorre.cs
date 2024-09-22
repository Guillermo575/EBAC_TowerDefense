using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/**
 * @file
 * @brief Boton que sirve para instanciar las torres
 */
public class BotonTorre : MonoBehaviour
{
    private GameManager gameManager;
    public string NombreTorre;
    public int Costo;
    [HideInInspector]public bool TorreSeleccionada;
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
        boton.interactable = gameManager.GetRecursos() >= Costo && !TorreSeleccionada;
        var colors = boton.colors;
        var disabledColor = colors.disabledColor;
        disabledColor = TorreSeleccionada ? Color.green : Color.red;
        colors.disabledColor = disabledColor;
        boton.colors = colors;
    }
}
