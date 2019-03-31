using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCopy : MonoBehaviour
{
    public Camera follow;
    // Start is called before the first frame update
    void Start()
    {
        Camera camera = GetComponent<Camera>();
        camera.cameraType = follow.cameraType;
        camera.fieldOfView = follow.fieldOfView;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = follow.transform.position;
        transform.localRotation = follow.transform.localRotation;
    }
}
