using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endClick : MonoBehaviour
{
    [SerializeField] GameObject gameobject;
    [SerializeField] private bool isOpened = false;
    [SerializeField] private string url = "https://docs.google.com/forms/d/1M_czlGXZF6FT0BkuRigLFlo41KTA2nUVm-SukXrQKdY/edit";
    // Start is called before the first frame update


    public void Open() {
        bool noticeDelay;
        if (!isOpened) {
            if (gameobject.transform.name == "yesButton")

            {
                noticeDelay = true;
            }
            else
            {
                noticeDelay = false;
            }
            isOpened = true;
            Tinylytics.AnalyticsManager.LogCustomMetric($"{GameData.id} SETUP", $"{GameData.isDelay.ToString()},{noticeDelay}");
            Application.OpenURL(url);
            Application.Quit();
        }

    }

}
