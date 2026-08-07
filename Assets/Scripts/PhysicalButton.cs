using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PhysicalButton : MonoBehaviour
{
    public UnityEvent onPoke; // This lets us wire it in the Inspector just like a UI button!

    private bool isCooldown = false;

    void OnTriggerEnter(Collider other)
    {
        // Only trigger if we aren't in cooldown
        if (!isCooldown)
        {
            // Optional: You could check if(other.CompareTag("PlayerHand")) here, 
            // but for a quick assessment, any collision is fine!

            onPoke.Invoke(); // Trigger the GameManager function!

            // Add a tiny cooldown so we don't accidentally double-click
            isCooldown = true;
            Invoke("ResetCooldown", 1.0f);
        }
    }

    void ResetCooldown()
    {
        isCooldown = false;
    }
}
