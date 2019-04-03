using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GlowFX
{
    public class AnimateTexture : MonoBehaviour
    {
        private MeshRenderer _renderer;

        private void Start()
        {
            this._renderer = GetComponent<MeshRenderer>();
        }

        void Update()
        {
            _renderer.material.mainTextureOffset = new Vector2(Time.time * 0.01f, Mathf.Sin(Time.time * 0.05f));
        }
    }
}
