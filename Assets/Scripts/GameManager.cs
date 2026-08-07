using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI References")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI remainingText;

    private int objectsRemaining = 3;
    private float timer = 0f;
    private bool isTaskActive = true;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Update()
    {
        if (isTaskActive)
        {
            timer += Time.deltaTime;
            UpdateTimerUI();
        }
    }

    void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(timer / 60F);
        int seconds = Mathf.FloorToInt(timer - minutes * 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void ObjectPlaced()
    {
        objectsRemaining--;
        remainingText.text = "Objects Remaining: " + objectsRemaining.ToString();

        if (objectsRemaining <= 0)
        {
            TaskCompleted();
        }
    }

    void TaskCompleted()
    {
        isTaskActive = false;
        Debug.Log("GameManager confirms Task Completed!");
    }

    // Link this to your Reset Button
    public void ResetScene()
    {
        // This reloads the entire level, resetting everything.
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void RestartTask()
    {
        // 1. Reset the timer and counts
        timer = 0f;
        UpdateTimerUI();

        objectsRemaining = 3;
        remainingText.text = "Objects Remaining: " + objectsRemaining.ToString();
        isTaskActive = true;

        // 2. Teleport the pieces
        AutoReturn[] puzzlePieces = FindObjectsOfType<AutoReturn>();
        foreach (AutoReturn piece in puzzlePieces)
        {
            piece.ResetObjectPosition();
        }

        // THE MAGIC LINE: Force Unity to update all colliders INSTANTLY 
        // so the sockets don't accidentally grab the objects back!
        Physics.SyncTransforms();

        // 3. Reset Sockets
        PuzzleSocket[] allSockets = FindObjectsOfType<PuzzleSocket>();
        foreach (PuzzleSocket socket in allSockets)
        {
            socket.ResetSocket();
        }
    }
}
