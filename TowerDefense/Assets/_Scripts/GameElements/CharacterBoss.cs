using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class CharacterBoss : MonoBehaviour
{
    public GameObject objetivo;
    void Start()
    {
        GetComponent<NavMeshAgent>().SetDestination(objetivo.transform.position);
    }
    void Update()
    {
        
    }
}