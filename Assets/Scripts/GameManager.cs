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

    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void RestartTask()
    {
        timer = 0f;
        UpdateTimerUI();

        objectsRemaining = 3;
        remainingText.text = "Objects Remaining: " + objectsRemaining.ToString();
        isTaskActive = true;

        AutoReturn[] puzzlePieces = FindObjectsOfType<AutoReturn>();
        foreach (AutoReturn piece in puzzlePieces)
        {
            piece.ResetObjectPosition();
        }

        Physics.SyncTransforms();
        PuzzleSocket[] allSockets = FindObjectsOfType<PuzzleSocket>();
        foreach (PuzzleSocket socket in allSockets)
        {
            socket.ResetSocket();
        }
    }
}
