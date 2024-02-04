using Godot;
using System;

public partial class DashGhost : Sprite2D
{
	Tween tween;

	public override void _Ready()
	{
		tween = CreateTween();
		tween.TweenProperty(this, "modulate:a", 0.0, 0.3);
		tween.TweenCallback(Callable.From(RemoveThisInstance));
	}

	private void RemoveThisInstance()
	{
		GetParent().RemoveChild(this);
	}
}
