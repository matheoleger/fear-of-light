using Godot;
using System;

public partial class LightCrystal : Node2D
{

	private AnimatedSprite2D _animatedSprite;

	private bool isDestroying = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_animatedSprite.Play("apparition");
	}

	public override void _Process(double delta)
	{
		HandleDestroying();
	}

	public void Destroy()
	{
		isDestroying = true;
	}

	private void HandleDestroying()
	{
		if (!isDestroying) return;

		bool isRightFrame = _animatedSprite.Frame == 13 
		|| _animatedSprite.Frame == 12 
		|| _animatedSprite.Frame == 0 
		|| _animatedSprite.Frame == 1;

		if (isRightFrame)
			_animatedSprite.Play("disappearance");
	}

	public void _OnAnimatedSprite2dAnimationFinished()
	{
		if (_animatedSprite.Animation == "apparition")
			_animatedSprite.Play("default");

		if (_animatedSprite.Animation == "disappearance")
			GetTree().CurrentScene.RemoveChild(this);
	}

}
