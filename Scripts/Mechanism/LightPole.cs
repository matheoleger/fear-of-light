using Godot;
using System;

public partial class LightPole : StaticBody2D, IRechargeableLightPole
{
	[Export]
	public bool IsRechargeable { get; set; }

	public AnimatedSprite2D _AnimatedSprite { get; set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_AnimatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

		ChangeAnimation();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void ChangeAnimation()
	{
		if(IsRechargeable)
			_AnimatedSprite.Play("rechargeable");
		else 
			_AnimatedSprite.Play("default");
	}
}
