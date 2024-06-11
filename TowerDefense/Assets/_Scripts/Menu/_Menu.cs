using UnityEngine;
public class _Menu : MonoBehaviour
{
    #region Variables
    protected MenuManager menuManager;
    #endregion

    #region Start
    protected virtual void Start()
    {
        menuManager = MenuManager.GetManager();
    }
    #endregion
}