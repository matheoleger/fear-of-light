using Godot;
using System;

public partial class PauseMenu : Control
{

	private SceneTransitionManager _sceneTransitionManager;
	private PanelContainer _quitSubMenu;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_sceneTransitionManager = GetNode<SceneTransitionManager>("/root/SceneTransitionManager");
		_quitSubMenu = GetNode<PanelContainer>("QuitSubMenu");

		Visible = false;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

		bool isTriggeredPauseAction = Input.IsActionJustPressed("pause_menu");

		if(isTriggeredPauseAction)
			GameManager.instance.isGamePaused = !GameManager.instance.isGamePaused;

		if(GameManager.instance.isGamePaused)
			Visible = true;
		else {
			Visible = false;
			_quitSubMenu.Visible = false;
		}
	}

	public void _OnResumeButtonPressed()
	{
		GameManager.instance.isGamePaused = false;
	}

	public void _OnOptionsButtonPressed()
	{

	}

	public void _OnQuitButtonPressed()
	{
		_quitSubMenu.Visible = true;
	}

	public void _OnCloseQuitSubMenuPressed()
	{
		_quitSubMenu.Visible = false;
	}

	public void _OnToMenuButtonPressed()
	{
		_sceneTransitionManager.ChangeScene("res://Scenes/Menu/MainMenuScene.tscn");
	}

	public void _OnToDesktopButtonPressed()
	{
		GetTree().Quit();
	}
}
