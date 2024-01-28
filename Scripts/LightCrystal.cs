using Godot;
using System;

public partial class LightCrystal : Node2D
{

	private AnimatedSprite2D _animatedSprite;
	

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        _animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_animatedSprite.Play("apparition");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void DestroySelf()
	{
		_animatedSprite.Play("disparition");
	}

	public void _OnAnimatedSprite2dAnimationFinished()
	{
		GD.Print(_animatedSprite.Animation);

		if(_animatedSprite.Animation == "apparition")
			_animatedSprite.Play("default");

		if(_animatedSprite.Animation == "disparition")
			GetTree().CurrentScene.RemoveChild(this);
	}

}
