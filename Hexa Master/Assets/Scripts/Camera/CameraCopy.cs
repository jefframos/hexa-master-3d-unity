﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCopy : MonoBehaviour
{
    public Transform follow;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = follow.position;
        transform.localRotation = follow.localRotation;
    }
}
