using Godot;
using System;

public partial class Activator : Node2D
{
    private bool _isEnabled = false;
    private bool previousIsEnabled = false;

    public bool IsEnabled
    {
        get
        {
            return _isEnabled;
        }

        protected set
        {
            _isEnabled = value;
            EmitSignal(SignalName.ActivatorHasChangedState, this);
        }
    }

    [Signal]
    public delegate void ActivatorHasChangedStateEventHandler(Activator activator);

}