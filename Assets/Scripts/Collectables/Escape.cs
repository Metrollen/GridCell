using System;
using TMPro;
using UnityEngine;

public class Escape : MonoBehaviour
{
    public GameObject Player;
    
    public CollectableCount CollectableCount;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Unit"))
        {
            Destroy(gameObject);
            CollectableCount.OnEscape();
            Destroy(Player);
        }
    }
}
