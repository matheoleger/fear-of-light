using Godot;
using System;

public partial class Door : Receiver
{
	private AnimatedSprite2D _topAnimatedSprite;
	private AnimatedSprite2D _downAnimatedSprite;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GD.Print("ISENABLED ?", IsEnabled);

		_topAnimatedSprite = GetNode<AnimatedSprite2D>("TopAnimatedSprite2D");
		_downAnimatedSprite = GetNode<AnimatedSprite2D>("DownAnimatedSprite2D");
	
		SetupSignals();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		HandleAnimation();
	}

	private void HandleAnimation()
	{
		_topAnimatedSprite.Play(IsEnabled ? "open" : "close");
		_downAnimatedSprite.Play(IsEnabled ? "open" : "close");
	}
}
