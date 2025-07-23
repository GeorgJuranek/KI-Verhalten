using UnityEngine;

public class ToCameraTurner : MonoBehaviour
{
    Camera cam;

    void Start()
    {
        cam = Camera.main;


        transform.LookAt(cam.transform);
        transform.rotation = (cam.transform.rotation);

    }

}
