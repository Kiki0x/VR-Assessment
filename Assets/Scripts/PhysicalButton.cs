using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PhysicalButton : MonoBehaviour
{
    public UnityEvent onPoke;

    private bool isCooldown = false;

    void OnTriggerEnter(Collider other)
    {
        if (!isCooldown)
        {
            onPoke.Invoke(); 

            isCooldown = true;
            Invoke("ResetCooldown", 1.0f);
        }
    }

    void ResetCooldown()
    {
        isCooldown = false;
    }
}
