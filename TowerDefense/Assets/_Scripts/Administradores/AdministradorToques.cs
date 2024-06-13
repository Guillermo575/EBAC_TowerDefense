using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
public class AdministradorToques : MonoBehaviour
{
    #region Singleton
    private static AdministradorToques SingletonGameManager;
    private AdministradorToques()
    {
    }
    private void CreateSingleton()
    {
        if (SingletonGameManager == null)
        {
            SingletonGameManager = this;
        }
        else
        {
            Debug.LogError("Ya existe una instancia de esta clase");
        }
    }
    public static AdministradorToques GetManager()
    {
        return SingletonGameManager;
    }
    #endregion

    public InputActionAsset inputs;
    private InputAction toque;
    private InputAction posicionToque;
    private Camera mainCam;
    public delegate void PlataformaTocada(GameObject plataforma);
    public event PlataformaTocada EnPlataformaTocada;
    private void OnEnable()
    {
        TouchSimulation.Enable();
        inputs.Enable();
        toque = inputs.FindAction("Toque");
        posicionToque = inputs.FindAction("PosicionToque");
        toque.performed += Toque;
    }
    private void OnDisable()
    {
        inputs.Disable();
        TouchSimulation.Disable();
        toque.performed -= Toque;
    }
    private void Awake()
    {
        CreateSingleton();        
    }
    private void Start()
    {
        mainCam = Camera.main;
    }
    private void Toque(InputAction.CallbackContext obj)
    {
        Vector2 poseToque2D = posicionToque.ReadValue<Vector2>();
        Vector3 poseToque3D = new Vector3(poseToque2D.x, poseToque2D.y, mainCam.farClipPlane);
        Ray rayoPantalla = mainCam.ScreenPointToRay(poseToque3D);
        Debug.Log($"La pantalla fue tocada en la posicion { poseToque2D }");
        RaycastHit hit;
        if (Physics.Raycast(rayoPantalla, out hit, Mathf.Infinity))
        {
            Debug.Log(hit.transform.gameObject.name);
            if (hit.transform.gameObject.tag == "Plataforma")
            {
                Debug.Log("Plataforma tocada");
                if (EnPlataformaTocada != null)
                {
                    EnPlataformaTocada(hit.transform.gameObject);
                }
            }
        }
        else
        {
            Debug.Log("No hubo hit en el raycast");
        }
    }
}