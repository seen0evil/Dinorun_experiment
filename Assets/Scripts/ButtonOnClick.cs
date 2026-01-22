using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class ButtonOnClick : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject gameobject;
    [SerializeField] string sceneName;
    public TMP_InputField inputFieldA;
    public TMP_InputField inputFieldB;
    public TMP_InputField inputFieldC;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Click()
    {
        gameobject.GetComponent<AudioSource>().Play();
        if (SceneManager.GetActiveScene().name!= "GameTransition") {
            string retrivedA = inputFieldA.text;
            //string retrivedB = inputFieldB.text;
            //string retrivedC = inputFieldC.text;

            //float lowerBound = float.Parse(retrivedA);
            //float upperBound = float.Parse(retrivedB);
            //float jitter = float.Parse(retrivedC);
            float lowerBound = 120f;
            float upperBound = 160f;
            float jitter = 450f;

            GameData.id = retrivedA;
            GameData.lowerBound = lowerBound;
            GameData.upperBound = upperBound;
            GameData.jitter = jitter;
            if (gameobject.transform.name == "Button_nsd")
            {
                GameData.isDelay = false;
            }
            else
            {
                GameData.isDelay = true;
            }
            string packed = $"{GameData.isDelay.ToString()},{lowerBound},{upperBound},Jitter: {jitter}";
            Tinylytics.AnalyticsManager.LogCustomMetric($"{retrivedA} SETUP", packed);
        }
        SceneManager.LoadScene(sceneName);
    }
}
