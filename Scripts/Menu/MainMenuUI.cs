using Godot;
using System;

public partial class MainMenuUI : Control
{

	private SceneTransitionManager _sceneTransitionManager;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_sceneTransitionManager = GetNode<SceneTransitionManager>("/root/SceneTransitionManager");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void _OnNewGameButtonPressed()
	{
		_sceneTransitionManager.ChangeScene("res://Scenes/TestScene.tscn");
	}

	public void _OnOptionsButtonPressed()
	{

	}

	public void _OnQuitButtonPressed()
	{
		GetTree().Quit();
	}
}
