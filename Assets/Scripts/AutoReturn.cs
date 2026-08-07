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
        gameObject.SetActive(false);

        transform.position = startPos;
        transform.rotation = startRot;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        MonoBehaviour[] scripts = GetComponentsInChildren<MonoBehaviour>(true);
        foreach (MonoBehaviour s in scripts)
        {
            if (s.GetType().Name == "Grabbable" || s.GetType().Name.Contains("Interactable"))
            {
                s.enabled = true;
            }
        }
        gameObject.SetActive(true);
    }
}
