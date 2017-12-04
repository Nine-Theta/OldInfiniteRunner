using UnityEngine;
using System.Collections;

public class ParallaxScroller : MonoBehaviour
{
    public float scrollSpeed;
    public float tileSizeZ;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float newPosition = Mathf.Repeat(Time.time * scrollSpeed, tileSizeZ);
        //Debug.Log(Time.time);
        transform.position = startPosition + Vector3.right * newPosition;
    }
}
