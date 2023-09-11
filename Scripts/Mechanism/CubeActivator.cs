using Godot;
using System;

public partial class CubeActivator : Activator
{

	private AnimatedSprite2D _animatedSprite;

	private bool cubeIsDetected = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		HandleAnimation();

		ChangeState();
	}

	private void HandleAnimation()
	{
		_animatedSprite.Play(IsEnabled ? "enabled" : "disabled");
	}

	private void ChangeState()
	{
		//We do that because we can"t use AreaEntered/Exited to change the state of collision shape of the door. (https://ask.godotengine.org/38401/does-cant-change-this-state-while-flushing-queries-error-mean)
		if(cubeIsDetected && !IsEnabled)
			IsEnabled = true;
		else if(!cubeIsDetected && IsEnabled)
			IsEnabled = false;
	}

	public void _OnArea2dBodyEntered(Node2D body)
	{
		if(!body.IsInGroup("Cube")) return;

		cubeIsDetected = true;
	}

	public void _OnArea2dBodyExited(Node2D body)
	{
		if(!body.IsInGroup("Cube")) return;

		cubeIsDetected = false;
	}
}
