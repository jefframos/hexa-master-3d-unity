using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateGlowColor : MonoBehaviour {

    public Color firstColor = Color.blue;
    public Color secondColor = Color.red;

    [Range(0,1)]
    public float delta;

    public float frequence = 2;

    private Material glowMat;
    
    public Color glowColor;

	void Start () {
        var rd = this.GetComponent<MeshRenderer>();
        glowMat = rd.material;
    }
	
	// Update is called once per frame
	void Update () {

        
        delta = Mathf.PingPong(Time.time * frequence, 1);

        glowColor = Color.Lerp(firstColor, secondColor, delta);
        glowMat.SetColor("_GlowColor", glowColor);
	}
}
