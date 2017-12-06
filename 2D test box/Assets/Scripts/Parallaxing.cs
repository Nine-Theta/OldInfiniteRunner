using UnityEngine;
using System.Collections;

public class Parallaxing : MonoBehaviour
{
    public int tiling = 10;
    public Transform[] backgrounds;     //array (list) of all back- and forgrounds to be parallaxed
    public float[] parallaxScales;  //the proportion of the camera's movement to move the backgrounds by
    public float smoothing = 1f;    //how smooth the parallax is going to be, Must be above 0 otherwize the parallax will not work

    private Transform cam;  //reference to the camera's transform
    private Vector3 previousCamPos;     //the position of the camera in the previous frame

    void Awake()
    {
        cam = Camera.main.transform;
    }

    void Start()
    {
        previousCamPos = cam.position;

        for (int i = 0; i < backgrounds.Length; i++)
        {
            for (int j = 0; j < tiling; j++)
            {
                InstantiateExtras(i, j);
            }
        }
    }

    void Update()
    {

        for (int i = 0; i < backgrounds.Length; i++)
        {
            float parallax = (previousCamPos.x - cam.position.x) * parallaxScales[i];

            float backgroundTargetPosX = backgrounds[i].position.x + parallax;

            Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, backgrounds[i].position.y, backgrounds[i].position.z);

            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);

        }

        previousCamPos = cam.position;

    }

    private void InstantiateExtras(int index, int offset = 1)
    {
        offset++;
        GameObject objectCopy = Instantiate(backgrounds[index].gameObject);
        objectCopy.transform.localPosition = new Vector3(backgrounds[index].position.x + (backgrounds[index].GetComponent<SpriteRenderer>().bounds.size.x * offset), backgrounds[index].position.y, backgrounds[index].position.z);
        objectCopy.transform.SetParent(backgrounds[index]);
        objectCopy.transform.localScale = new Vector3(1, 1, 1);
    }
}

#region disclaimer
/*
**Parts of this code
Code written by joedanhol, find this on GitHub at https://github.com/joedanhol/Parallax2D :D Last test done 27/04/15
Feel free to edit this at your own will, this code was made to be compleatly hackable, and feel free to message me at
joedanhol@gmail.com for any help required.

*/
#endregion