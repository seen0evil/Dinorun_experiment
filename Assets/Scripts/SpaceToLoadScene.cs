using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpaceToLoadScene : MonoBehaviour
{
    public string sceneToLoad = "C1_Transition_2";
    [SerializeField] GameObject gameobject;
    public AudioSource sfxSource;
    public AudioClip clickClip;
    bool isLoading;                   // prevent double-fires
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if (!isLoading && Input.GetKeyDown(KeyCode.Space)) {
            StartCoroutine(PlayAndLoad());
        };
    }

    IEnumerator PlayAndLoad()
    {
        isLoading = true;

        // 1. Play the sound
        if (sfxSource != null)
        {
            if (clickClip == null)              // clip already on the source
                sfxSource.Play();
            else
                sfxSource.PlayOneShot(clickClip);
        }

        // 2. Wait for it to finish (fall back to 0 s if source/clip missing)
        float wait = sfxSource != null
                     ? (clickClip ? clickClip.length : sfxSource.clip.length)
                     : 0f;
        yield return new WaitForSeconds(wait);

        // 3. Load the next scene (async keeps audio from stuttering on WebGL)
        if (int.TryParse(sceneToLoad, out int index))
            SceneManager.LoadSceneAsync(index);
        else
            SceneManager.LoadSceneAsync(sceneToLoad);
    }

}
