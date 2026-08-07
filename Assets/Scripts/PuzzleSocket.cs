using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;

public class PuzzleSocket : MonoBehaviour
{
    public string correctTag;
    public Material successMaterial;
    private Material defaultMaterial;

    [HideInInspector]
    public bool isSolved = false;

    void Start()
    {
        // Remember the transparent material for when we hit Restart
        defaultMaterial = GetComponent<MeshRenderer>().material;
    }

    void OnTriggerStay(Collider other)
    {
        if (isSolved) return;

        if (other.CompareTag("Tag_Cube") || other.CompareTag("Tag_Sphere") || other.CompareTag("Tag_Cylinder"))
        {
            Grabbable grabbable = other.GetComponent<Grabbable>();

            if (grabbable != null && grabbable.SelectingPointsCount > 0) return;

            if (other.CompareTag(correctTag))
            {
                isSolved = true;

                Rigidbody rb = other.GetComponent<Rigidbody>();
                if (rb != null) rb.isKinematic = true; // Freeze gravity

                other.transform.position = transform.position;
                other.transform.rotation = transform.rotation;

                // SAFELY DISABLE ALL GRABBING (Instead of Destroying)
                MonoBehaviour[] scripts = other.GetComponentsInChildren<MonoBehaviour>();
                foreach (MonoBehaviour s in scripts)
                {
                    if (s.GetType().Name == "Grabbable" || s.GetType().Name.Contains("Interactable"))
                    {
                        s.enabled = false;
                    }
                }

                GetComponent<MeshRenderer>().material = successMaterial;

                if (GameManager.Instance != null) GameManager.Instance.ObjectPlaced();
            }
        }
    }

    // GameManager will call this when you click Restart
    public void ResetSocket()
    {
        isSolved = false;
        GetComponent<MeshRenderer>().material = defaultMaterial;
    }
}