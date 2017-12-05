using UnityEngine;
using System.Collections;

public class ParallaxScroller : MonoBehaviour
{
    public float scrollSpeed;
    public float tileSizeZ;
    public Transform cameraWisp;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
        tileSizeZ = Mathf.Abs(tileSizeZ);
    }

    void Update()
    {
        float newPosition = Mathf.Repeat((cameraWisp.position.x - startPosition.x) * scrollSpeed, tileSizeZ);
        //Debug.Log(Time.time);
        transform.position = startPosition + Vector3.right * newPosition;
    }
}
