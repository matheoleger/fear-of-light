using Godot;
using System;

public partial class OptionsMenu : Control
{
	private AnimationPlayer _animationPlayer;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}


	public void ChangeVisibility(bool visible)
	{
		if(visible)
		{
			_animationPlayer.Play("fade_in");
		} else
		{
			_animationPlayer.Play("fade_out");
		}
	}

	public void _OnBackButtonPressed()
	{
		ChangeVisibility(false);
	}
}
