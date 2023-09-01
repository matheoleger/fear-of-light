using Godot;
using System;

public partial class LightCrystal : Node2D
{

	private AnimatedSprite2D _animatedSprite;
	

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        _animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_animatedSprite.Play("default");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
