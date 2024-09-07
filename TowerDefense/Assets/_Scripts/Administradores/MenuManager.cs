using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
public class MenuManager : MonoBehaviour
{
    #region Singleton
    private static MenuManager SingletonMenuManager;
    private MenuManager()
    {
    }
    private void CreateSingleton()
    {
        if (SingletonMenuManager == null)
        {
            SingletonMenuManager = this;
        }
        else
        {
            Debug.LogError("Ya existe una instancia de esta clase");
        }
    }
    public static MenuManager GetSingleton()
    {
        return SingletonMenuManager;
    }
    #endregion

    #region Variables
    public List<GameObject> lstMenuTree;
    public Opciones opciones;
    public MenuConfirmar menuConfirmar;
    public GameObject menuInicial;
    public GameObject menuOpciones;
    public GameObject menuPausa;
    public GameObject menuFinNivel; //Ganar
    public GameObject menuFinJuego; //Perder
    public GameObject menuCreditos; //Perder
    #endregion

    #region Start
    private void Awake()
    {
        CreateSingleton();
    }
    void Start()
    {
        lstMenuTree = new List<GameObject>();
        if(menuInicial.activeSelf) lstMenuTree.Add(menuInicial);
    }
    void Update()
    {
    }
    #endregion

    #region Menus
    public void BackMenu()
    {
        if (lstMenuTree.Count > 1)
        {
            var objBack = lstMenuTree[lstMenuTree.Count - 2];
            var objActual = lstMenuTree.Last();
            lstMenuTree.Remove(objActual);
            objActual.SetActive(false);
            objBack.SetActive(true);
        }
    }
    public void ShowMenu(GameObject objMenu)
    {
        if (objMenu != null)
        {
            SetActiveCanvas();
            lstMenuTree.Add(objMenu);
            objMenu.SetActive(true);
        }
    }
    public void SetActiveCanvas(bool value = false)
    {
        var lst = lstMenuTree;
        foreach (var x in lst)
        {
            if (x.gameObject != null && x.gameObject.activeSelf)
            {
                x.gameObject.SetActive(value);
            }
        }
    }
    public void DeleteMenuTree()
    {
        SetActiveCanvas(false);
        lstMenuTree = new List<GameObject>();
    }
    #endregion
}