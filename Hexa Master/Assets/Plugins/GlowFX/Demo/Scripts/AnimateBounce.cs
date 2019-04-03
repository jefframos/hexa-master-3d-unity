using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateBounce : MonoBehaviour {

    private Vector3 startPos;

	void Start () {
        startPos = transform.localPosition;

    }
	
	// Update is called once per frame
	void Update () {
        float delta = Mathf.Abs(Mathf.Sin(Time.time * 2));

        this.transform.localPosition = new Vector3(startPos.x, startPos.y + delta * 1, startPos.z);

        if (delta<0.2f)
        {
            delta /= 0.2f;
        }
        else
        {
            delta = 1;
        }


        this.transform.localScale = new Vector3(1, Mathf.Lerp(0.5f, 1, delta), 1);
	}
}
