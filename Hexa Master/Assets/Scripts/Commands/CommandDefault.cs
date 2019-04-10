using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandDefault// : MonoBehaviour
{
    protected bool killed = false;
    protected GameObject target;
    protected object param;
    internal delegate void DelegateCallback();
    internal DelegateCallback callback;
    public virtual void Active()
    {
        killed = false;
    }
    internal virtual void AddCallback(DelegateCallback cb)
    {
        if (cb == null)
        {
            throw new ArgumentNullException(nameof(cb));
        }
        callback = cb;
    }
    public virtual void Deactive()
    {
        
    }

    public virtual bool IsFinished()
    {
        return killed;
    }

    public virtual void Kill()
    {
        killed = true;
        if(callback != null)
        {
            callback();
        }
    }

    public virtual void Play()
    {
        killed = false;
    }

    public virtual void Reset()
    {
        killed = false;
    }

    public virtual void SetData(object obj)
    {
        //param = obj;
    }

    public virtual void SetTarget(GameObject gameObject)
    {
        target = gameObject;
    }

    public virtual void Update()
    {
        if (killed)
        {
            return;
        }
    }
}
