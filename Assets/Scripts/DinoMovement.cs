using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DinoMovement : MonoBehaviour
{
    [SerializeField] float jumpForce = 7f;
    [SerializeField] float horizontalJumpForce = 2f;  // Add this for horizontal force
    [SerializeField] AudioClip jumpSound; // assign your jump sound clip in the Inspector
    [SerializeField] float jumpDelay = 0.2f;   // Time (in seconds) before action registers
    [SerializeField] float soundDelay = 0.2f;  // Time (in seconds) to delay the jump sound

    private Rigidbody2D rb;
    private AudioSource audioSource;
    private AudioClip source;
    private bool isGrounded = false;
    public bool hasCollectedCoinThisJump = false;
    public bool isSoundDelay = false;

    [Header("Tone")]
    [SerializeField] float frequency = 440f;     // Hz
    [SerializeField] float duration = 0.10f;    // seconds
    [SerializeField] float fadeTime = 0.005f;   // seconds (5 ms)
    AudioSource src;
    AudioClip clip;


    void Awake()
    {
        src = GetComponent<AudioSource>();
        clip = MakeToneClip(frequency, duration, fadeTime);
    }

    void Start()
    {
        if (SceneManager.GetActiveScene().name != "GamePlaytrialv3") {
            isSoundDelay = GameData.isDelay;
            print(SceneManager.GetActiveScene().name);
            print(GameData.isDelay);
        }
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>(); // Make sure an AudioSource component is attached to this GameObject
    }

    void Update()
    {
        float jd = 0;
        int totalFrameDelay = 0;
        // Allow jump only if the player is grounded
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            if (Time.timeSinceLevelLoad > 30)
            {
                // --- MODIFIED SECTION START ---

                // 1. Define the target frame rate
                const float frameRate = 30.0f;

                // 2. Create a weighted distribution for the offset.
                //    In this array, '0' appears four times, while '-1' and '1' appear once.
                //    This makes '0' significantly more likely to be chosen.
                int[] weightedOffsets = { 0, 0, 0, 0, 0, 0 };

                // 3. Randomly choose an offset from the weighted array
                int frameOffset = weightedOffsets[Random.Range(0, weightedOffsets.Length)];

                // 4. Set the base frame delay and add the chosen offset
                totalFrameDelay = 6 + frameOffset; // Result is 5, 6, or 7

                // 5. Calculate the delay in seconds
                jd = totalFrameDelay / frameRate;

                // --- MODIFIED SECTION END ---
            }

            // Prevent further jumps until landing
            isGrounded = false;

            if (isSoundDelay)
            {
                // Log the calculated delay in milliseconds and frames for clarity
                Debug.Log($"Delay: {jd * 1000:F2} ms ({totalFrameDelay} frames)");
                Invoke("PlayBeep", jd);
                Invoke("ExecuteJump", jd);
            }
            else
            {
                Debug.Log($"Delay: {jd * 1000:F2} ms ({totalFrameDelay} frames)");
                PlayBeep();
                Invoke("ExecuteJump", jd);
            }

            GameData.jumps.Add(new GameData.JumpRecord(Time.timeSinceLevelLoad));
        }
    }

    void ExecuteJump() {
        rb.velocity = new Vector2(rb.velocity.x+horizontalJumpForce, jumpForce);
    }

    void PlayBeep()
    {
        src.PlayOneShot(clip);        // no Stop() needed ¡V let the clip finish
    }


// When the player collides with an object tagged as "Ground", they¡¦re considered grounded
private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            hasCollectedCoinThisJump = false;  // Reset here
        }
    }

    // Optionally, you can use OnCollisionExit2D to clear the flag when leaving the ground.
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    //---------------------------------------------------------------------
    AudioClip MakeToneClip(float freq, float len, float fade)
    {
        int sampleRate = AudioSettings.outputSampleRate;
        int totalSamples = Mathf.CeilToInt(len * sampleRate);
        float[] buffer = new float[totalSamples];

        for (int i = 0; i < totalSamples; i++)
        {
            float t = i / (float)sampleRate;           // seconds
            float envelope = 1f;

            // Simple linear fade-in / fade-out
            if (t < fade) envelope = t / fade;
            else if (t > len - fade) envelope = (len - t) / fade;

            // Sine wave * envelope
            buffer[i] = envelope * Mathf.Sin(2f * Mathf.PI * freq * t);
        }

        var clip = AudioClip.Create("Beep",
                                    totalSamples,
                                    1,                // channels
                                    sampleRate,
                                    false);           // streaming
        clip.SetData(buffer, 0);
        return clip;
    }
}
