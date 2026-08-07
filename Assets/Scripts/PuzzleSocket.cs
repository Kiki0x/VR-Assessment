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
                if (rb != null) rb.isKinematic = true; 

                other.transform.position = transform.position;
                other.transform.rotation = transform.rotation;

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

    public void ResetSocket()
    {
        isSolved = false;
        GetComponent<MeshRenderer>().material = defaultMaterial;
    }
}
