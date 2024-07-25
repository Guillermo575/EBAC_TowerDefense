using UnityEngine;
public class _DamageElement : MonoBehaviour
{
    public int damage = 1;
    protected GameManager gameManager;
    void Start()
    {
        gameManager = GameManager.GetManager();
    }
}