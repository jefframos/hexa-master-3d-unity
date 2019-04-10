using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandWait : CommandDefault
{
    internal class CommandWaitData
    {
        public float value = 0.5f;
        public string name = "Wait1";
    }
    CommandWaitData data;
    public override void SetData(object obj)
    {
        param = obj;
        data = obj as CommandWaitData;
    }
    public override void Update()
    {

        data.value -= Time.deltaTime;
        if (data.value <= 0)
        {
            Kill();
        }
        base.Update();
    }
    public override void Play()
    {
        base.Play();
    }
}
