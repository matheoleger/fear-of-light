using Godot;
using System;
using System.Collections.Generic;

public partial class Receiver : Node2D
{
    [Export]
    protected Activator[] activators;

    protected bool isEnabled = false;

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
}