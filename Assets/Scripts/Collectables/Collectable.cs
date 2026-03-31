using System;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public static event Action OnCollected;
    public static int total;

    void Awake() => total++;
    

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Unit"))
        {
            OnCollected?.Invoke();
            Destroy(gameObject);
            Debug.Log("Collected");
        }
    }
}
