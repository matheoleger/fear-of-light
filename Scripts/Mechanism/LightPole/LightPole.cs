using Godot;
using System;

public partial class LightPole : StaticBody2D, ILightPole
{
	[Export]
	public bool IsRechargeable { get; set; }

	public AnimatedSprite2D _AnimatedSprite { get; set; }

	public PointLight2D _PointLight2D { get; set; }

	public Area2D _LightArea2D { get; set; }

	public Tween RechargeableTween { get; set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_AnimatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_PointLight2D = GetNode<PointLight2D>("PointLight2D");
		_LightArea2D = GetNode<Area2D>("LightArea2D");

		RechargeableTween = GetTree().CreateTween();

		RechargeableLightPoleHelper.ChangeAnimation(this);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
