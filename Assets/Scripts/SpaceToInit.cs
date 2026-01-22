using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SpaceToInit : MonoBehaviour
{
    public string sceneToLoad = "C1_Start";
    public bool isSoundDelay = true;
    public float lowerBound = 80f;
    public float upperBound = 160f;
    public float jitter = 450f;
    public TMP_InputField inputFieldA;
    [SerializeField] GameObject gameobject;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)&&InputHasText()) {
            Screen.fullScreen = true;   // now allowed ¡V user just interacted
            gameobject.GetComponent<AudioSource>().Play();
            string retrivedA = inputFieldA.text;
            //pass down global variables
            GameData.id = retrivedA;
            GameData.lowerBound = lowerBound;
            GameData.upperBound = upperBound;
            GameData.jitter = jitter;
            GameData.isDelay = isSoundDelay;
            //send data to Tinylytics
            GameData.packed =  $"{GameData.isDelay.ToString()},{lowerBound},{upperBound},Jitter: {jitter}";
            //load next scene
            SceneManager.LoadScene(sceneToLoad);
        };
    }

    bool InputHasText() {
        if (inputFieldA == null) return false;
        return !string.IsNullOrWhiteSpace(inputFieldA.text);
    }
}
