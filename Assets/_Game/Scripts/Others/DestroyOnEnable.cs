using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnEnable : MonoBehaviour
{
    [SerializeField] private float timeToDestroy;
    private void OnEnable()
    {
        Invoke(nameof(DestroyByTime), timeToDestroy);
    }

    private void DestroyByTime()
    {
        Destroy(gameObject);
    }
}
