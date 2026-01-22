using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    public float speed = 5f;


    private void Start()
    {
        speed = GlobalSpeed.Instance.globalSpeed;
    }

    void Update()
    {
        // Move the GameObject this script is attached to, to the left.
        transform.Translate(Vector2.left * speed * Time.deltaTime, Space.World);
    }
}
