using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Coin : MonoBehaviour
{
    // How many points this coin is worth
    public int coinValue;

    // Reference to this coin's sprite renderer
    private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void SetCoinColor(Color color)
    {
        if (sr != null)
        {
            sr.color = color;
        }
    }
}