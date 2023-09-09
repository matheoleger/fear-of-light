using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security;

public partial class Receiver : Node2D
{
    [Export]
    protected Activator[] activators;

    private bool isEnabled = false;
    public bool IsEnabled { get; protected set; }

    private int numberOfActivatedActivators = 0;
    private int numberOfActivator;

    protected enum LogicGate 
    {
        Not,
        And,
        Or,
        Xor,
        Nand,
        Nor,
        Xnor
    }

    [Export]
    protected LogicGate logicGate = LogicGate.And;

    public override void _Ready()
    {
        foreach (Activator activator in activators)
        {
            activator.ActivatorHasChangedState += OnActivatorHasChangedState;
        }

        numberOfActivator = activators.Length;

        CheckReceiverState();
    }

    protected virtual void OnActivatorHasChangedState(Activator activator)
    {
        if(activator.IsEnabled && numberOfActivatedActivators < numberOfActivator)
        {
            numberOfActivatedActivators++;
        } else if (!activator.IsEnabled && numberOfActivatedActivators > 0)
        {
            numberOfActivatedActivators--;
        }

        CheckReceiverState();
    }

    protected void CheckReceiverState()
    {

        bool state = false;

        switch (logicGate)
        {
            case LogicGate.Not:
                state = numberOfActivatedActivators == 0;
                break;
            case LogicGate.And:
                state = numberOfActivatedActivators == numberOfActivator;
                break;
            case LogicGate.Or:
                state = numberOfActivatedActivators >= 1;
                break;
            case LogicGate.Xor:
                state = numberOfActivatedActivators % 2 == 1; // Odd verification
                break;
            case LogicGate.Nand:
                state = numberOfActivatedActivators < numberOfActivator;
                break;
            case LogicGate.Nor:
                state = numberOfActivatedActivators == 0;
                break;
            case LogicGate.Xnor:
                state = numberOfActivatedActivators % 2 == 0; // Even verification
                break;
        }

        IsEnabled = state;
    }
}