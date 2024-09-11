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
    public static AdministradorToques GetSingleton()
    {
        return SingletonGameManager;
    }
    #endregion

    #region Variables
    public InputActionAsset inputs;
    public Material materialPlataforma;
    public Material materialSeleccionado;
    private InputAction toque;
    private InputAction posicionToque;
    private Camera mainCam;
    public delegate void PlataformaTocada(GameObject plataforma);
    private AdministradorUI administradorUI;
    private AdministradorTorres administradorTorres;
    private GameManager gameManager;
    #endregion

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
        gameManager = GameManager.GetSingleton();
        administradorUI = AdministradorUI.GetSingleton();
        administradorTorres = AdministradorTorres.GetSingleton();
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
            if (hit.transform.gameObject.tag == "Plataforma" && !administradorUI.menuTorres.activeSelf)
            {
                if (gameManager.IsGamePause) return;
                var lstPlatform = GameObject.FindGameObjectsWithTag("Plataforma");
                foreach (var objplatform in lstPlatform) 
                { 
                    objplatform.GetComponent<Renderer>().material = materialPlataforma;
                }
                hit.transform.gameObject.GetComponent<Renderer>().material = materialSeleccionado;
                administradorUI.MostrarMenuTorres();
                administradorTorres.plataformaSeleccionada = hit.transform.gameObject;
                var lstBotonesTorres = GameObject.FindObjectsByType<BotonTorre>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID);
                var lstTorresHijas = administradorTorres.plataformaSeleccionada.transform;
                foreach (var objBotonesTorres in lstBotonesTorres)
                {
                    objBotonesTorres.TorreSeleccionada = false;
                    for (var i = 0; i < lstTorresHijas.childCount && !objBotonesTorres.TorreSeleccionada; i++)
                    {
                        objBotonesTorres.TorreSeleccionada = lstTorresHijas.GetChild(i).gameObject.GetComponent<_Tower>().NombreTorre == objBotonesTorres.NombreTorre;
                    }
                }
            }
        }
        else
        {
            Debug.Log("No hubo hit en el raycast");
        }
    }
}