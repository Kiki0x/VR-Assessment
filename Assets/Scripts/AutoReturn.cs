using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoReturn : MonoBehaviour
{
    private Vector3 startPos;
    private Quaternion startRot;

    void Start()
    {
        startPos = transform.position;
        startRot = transform.rotation;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Floor")
        {
            ResetObjectPosition();
        }
    }

    public void ResetObjectPosition()
    {
        // 1. THE NUKE: Turn the entire object off instantly. 
        // This forces Unity and Meta to completely drop it and erase its physics memory.
        gameObject.SetActive(false);

        // 2. Now that it is asleep, we can teleport it with zero resistance.
        transform.position = startPos;
        transform.rotation = startRot;

        // 3. Reset the physics gravity
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        // 4. Turn the grabbing scripts back on
        MonoBehaviour[] scripts = GetComponentsInChildren<MonoBehaviour>(true);
        foreach (MonoBehaviour s in scripts)
        {
            if (s.GetType().Name == "Grabbable" || s.GetType().Name.Contains("Interactable"))
            {
                s.enabled = true;
            }
        }

        // 5. Wake it back up on the table!
        gameObject.SetActive(true);
    }
}