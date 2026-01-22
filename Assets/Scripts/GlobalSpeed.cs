using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalSpeed : MonoBehaviour
{
    public static GlobalSpeed Instance;

    [Header("Global Speed Settings")]
    public float initialGlobalSpeed = 5f; // starting speed for each run
    public float globalSpeed;             // current speed that increases over time
    public float acceleration = 1f;       // rate at which speed increases

    void Awake()
    {
        // Implement singleton pattern if needed
        if (Instance == null)
        {
            Instance = this;
            // Optionally, DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Initialize globalSpeed at the start of a run
        ResetGlobalSpeed();
    }

    // Update is called once per frame
    void Update()
    {
        // Increase the global speed over time
        //globalSpeed += acceleration * Time.deltaTime;
    }

    public void ResetGlobalSpeed()
    {
        globalSpeed = initialGlobalSpeed;
    }

}
