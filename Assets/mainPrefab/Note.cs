using JetBrains.Annotations;
using UnityEngine;

public class Note : MonoBehaviour
{
    public float speed;
    public float Note_length;
    public float Note_pitch;

    public instrument instrument1;


    // Call this method to set up the note's properties
    public void Setup(float length, float startPitch, float pitchDelta, float pitchEnd)
    {
        // Adjust the visual length of the note
        AdjustLength(length);
        Note_length = length;

        // Set the pitch (could be position or other visual cue in your game)
        AdjustPitch(startPitch);
        Debug.Log("Within Setup: "+startPitch);
        Note_pitch = startPitch;
        // If you need to handle pitch delta or end pitch, add logic here
  
    }

    private void AdjustLength(float length)
    {
        // Assuming the length affects the x scale of the note
        Vector3 scale = transform.localScale;
        scale.x = length;
        transform.localScale = scale;
    }

    private void AdjustPitch(float pitch)
    {
        // Simple example: Adjust vertical position based on pitch
        // You might want to map this pitch value to a position more carefully
        Vector3 position = transform.position;
        position.y = MapStartPitchToPosition(pitch); // Assuming pitch directly translates to vertical position
        transform.position = position;
    }

    float MapStartPitchToPosition(float startPitch)
    {
        float minPitch = -165f; // StartPitch for the lowest note you want to display
        float maxPitch = 165f; // StartPitch for the highest note you want to display

        float minPosition = -4.8f; // Lowest position in camera view
        float maxPosition = 4.8f; // Highest position in camera view

        // Normalize startPitch to a 0-1 range
        float normalizedPitch = (startPitch - minPitch) / (maxPitch - minPitch);

        // Map the normalized startPitch to the camera's vertical range
        float positionY = Mathf.Lerp(minPosition, maxPosition, normalizedPitch);

        return positionY;
    }

    public void SetSpeedBasedOnTempo(float bpm)
    {
        float unitsPerBeat = 1; // Determine how many units you want the note to move per beat
        speed = (bpm / 60f) * unitsPerBeat; // Calculate speed based on BPM
    }


    void Update()
    {
        // Move the note across the screen
        transform.Translate(Vector3.left * speed * Time.deltaTime);

        // Check if the note has moved past the left edge of the camera's view
        if (Camera.main.WorldToViewportPoint(transform.position).x < -0.1) // 0 is the edge, so < -0.1 is a bit past the edge
        {
            Destroy(gameObject); // Destroy the note
        }
    }

    public void HandleHitStart()
    {
        // Handle the note being hit (play sound, visual feedback, etc.)
        Debug.Log("Note Pitch: when hit "+Note_pitch);
        GameManager.Instance.PlayTrumpetSound((int)(Note_pitch / 13.75 + 60));
    }
    public void HandleHitEnd()
    {
        // Handle the note being hit (play sound, visual feedback, etc.)
        GameManager.Instance.StopTrumpetSound();
    }

    public static float getScore()
    {
        float curscore = P1scoreboardScript.P1score + 1;

        return curscore;
    }
}



