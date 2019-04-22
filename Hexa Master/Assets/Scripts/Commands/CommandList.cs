using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CommandList : MonoBehaviour
{
    public class CommandEvent : UnityEvent<CommandDefault> { };
    public CommandEvent onFinishCommand = new CommandEvent();

    public class CommandListEvent : UnityEvent { };
    public CommandListEvent onFinishQueue = new CommandListEvent();

    // Start is called before the first frame update
    List<CommandDefault> listOfCommands;
    CommandDefault currentCommand;
    int commandIndex;
    bool isPause;
    bool ignoreCallback = false;
    public string commandName;

    public void ResetQueue()
    {
        listOfCommands = new List<CommandDefault>();
        isPause = false;
        
    }

    internal void Finish()
    {
        commandIndex = 0;

        if (!ignoreCallback)
        {
            onFinishQueue.Invoke();
        }

        listOfCommands = new List<CommandDefault>();
        currentCommand = null;
    }
    internal void Continue()
    {
        isPause = false;
        commandIndex++;
        GotoNextCommand();
    }
    internal void Play(bool _ignoreCallback = false)
    {
        ignoreCallback = _ignoreCallback;
        commandIndex = 0;
        if (listOfCommands.Count <= 0)
        {
            Debug.Log("Theres no commands");
            return;
        }
        GotoNextCommand();

    }

    internal CommandDefault AddCommand(CommandDefault command)
    {
        listOfCommands.Add(command);
        return command;
    }

    void GotoPrevCommand()
    {

    }
    void GotoNextCommand()
    {
        if (commandIndex >= listOfCommands.Count)
        {
            Finish();
            return;
        }
 
        currentCommand = listOfCommands[commandIndex];
        currentCommand.Reset();
        currentCommand.Play();
    }
    void FinishCommand()
    {
        commandIndex++;
        if (currentCommand != null)
        {
           
            currentCommand.Deactive();
            onFinishCommand.Invoke(currentCommand);
        }
    }
    void SkipCommand()
    {

    }
    internal void Restart()
    {
        commandIndex = 0;
    }
    internal void Pause(bool v = true)
    {
        isPause = v;

    }
   
    // Update is called once per frame
    void Update()
    {
        if (isPause || currentCommand == null)
        {
            return;
        }

        if (currentCommand.IsFinished())
        {
            FinishCommand();
            GotoNextCommand();
        }
        else
        {
            currentCommand.Update();
        }
    }

    internal void Destroy()
    {
        ResetQueue();
        commandIndex = 0;
        currentCommand = null;
    }
}
