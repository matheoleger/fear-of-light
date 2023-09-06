using Godot;
using System;
using System.Collections.Generic;

public partial class Receiver : Node2D
{
    [Export]
    protected Activator[] activators;

    private bool isEnabled = false;
    public bool IsEnabled { get; protected set; }

    protected enum LogicGates 
    {
        Not,
        And,
        Or,
        Xor,
        Nand,
        Nor
    }

    [Export]
    protected LogicGates logicGates;

    protected void SetupSignals()
    {
        GD.Print("EVERYTHING IS OK");
        foreach (Activator activator in activators)
        {
            activator.ActivatorHasChangedState += OnActivatorHasChangedState;
            // Connect(activator.ActivatorHasChangedState);
        }
    }

    protected void OnActivatorHasChangedState(Activator activator)
    {
        GD.Print("I RECEIVED THE SIGNAL FROM ACTIVATOR");
        GD.Print(activator.IsEnabled);
    }
}