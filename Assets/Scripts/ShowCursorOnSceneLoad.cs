using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowCursorOnSceneLoad : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;   // or CursorLockMode.Confined if you want it kept inside the game window
        Cursor.visible = true;                   // make it visible again
    }
}
