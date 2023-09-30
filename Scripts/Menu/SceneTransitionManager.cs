using Godot;
using System;

public partial class SceneTransitionManager : CanvasLayer
{

	private AnimationPlayer _animationPlayer;

    [Signal]
    public delegate void OnStartTransitionEventHandler();

	private string targetedScene;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		_animationPlayer.Play("fade_out");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void ChangeScene(string target)
	{
		_animationPlayer.Play("fade_in");
		EmitSignal(SignalName.OnStartTransition);
		targetedScene = target;
	}

	public void _OnAnimationPlayerAnimationFinished(StringName animName)
	{
		if(animName != "fade_in") return;

		GetTree().ChangeSceneToFile(targetedScene);
		_animationPlayer.Play("fade_out");
	}
}
