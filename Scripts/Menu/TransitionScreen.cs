using Godot;
using System;

public partial class TransitionScreen : CanvasLayer
{

	private AnimationPlayer _animationPlayer;

    [Signal]
    public delegate void TransitionedEventHandler();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		Transition();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}

	private void Transition()
	{
		_animationPlayer.Play("fade_in");
	}

	public void _OnAnimationPlayerAnimationFinished(StringName animName)
	{
		if(animName == "fade_in")
		{
			EmitSignal(SignalName.Transitioned, this);
			_animationPlayer.Play("fade_out");
		} else if(animName == "fade_out")
		{
			
		}
	}
}
