using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CameraController : MonoBehaviour
{
    [Header("Room Positions")]
    public Transform[] roomPositions; // Assign 5 room points
    
    [Header("Story Content")]
    public string[] storyTexts; // 5 story pieces
    
    [Header("UI References")]
    public Text storyText; // or TextMeshProUGUI
    public Button previousButton;
    public Button nextButton;
    
    [Header("Settings")]
    public float transitionSpeed = 2f;
    
    private int currentRoom = 0;
    private bool isTransitioning = false;
    
    void Start()
    {
        // Start hidden
        transform.position = roomPositions[0].position;
        transform.rotation = roomPositions[0].rotation;
    }
    
    public void StartStory()
    {
        currentRoom = 0;
        UpdateStory();
    }
    
    public void NextRoom()
    {
        if(currentRoom < roomPositions.Length - 1 && !isTransitioning)
        {
            currentRoom++;
            StartCoroutine(TransitionToRoom(currentRoom));
        }
    }
    
    public void PreviousRoom()
    {
        if(currentRoom > 0 && !isTransitioning)
        {
            currentRoom--;
            StartCoroutine(TransitionToRoom(currentRoom));
        }
    }
    
    IEnumerator TransitionToRoom(int roomIndex)
    {
        isTransitioning = true;
        DisableButtons();
        
        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;
        Vector3 targetPos = roomPositions[roomIndex].position;
        Quaternion targetRot = roomPositions[roomIndex].rotation;
        
        float elapsed = 0f;
        float duration = 1f / transitionSpeed;
        
        while(elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            
            // Smooth interpolation
            t = Mathf.SmoothStep(0, 1, t);
            
            transform.position = Vector3.Lerp(startPos, targetPos, t);
            transform.rotation = Quaternion.Slerp(startRot, targetRot, t);
            
            yield return null;
        }
        
        transform.position = targetPos;
        transform.rotation = targetRot;
        
        UpdateStory();
        isTransitioning = false;
    }
    
    void UpdateStory()
    {
        if(storyText != null && currentRoom < storyTexts.Length)
        {
            storyText.text = storyTexts[currentRoom];
        }
        
        // Update button states
        previousButton.interactable = (currentRoom > 0);
        nextButton.interactable = (currentRoom < roomPositions.Length - 1);
    }
    
    void DisableButtons()
    {
        previousButton.interactable = false;
        nextButton.interactable = false;
    }
}
