using Godot;
using System;

public partial class MainMenuScene : Control
{

	private AnimationPlayer _animationPlayer;
	private SceneTransitionManager _sceneTransitionManager;
	private PackedScene _splashScreenMenuScene;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_sceneTransitionManager = GetNode<SceneTransitionManager>("/root/SceneTransitionManager");
		_splashScreenMenuScene = GD.Load<PackedScene>("res://Scenes/Menu/SplashScreenMenu.tscn");

		_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");

		_sceneTransitionManager.OnStartTransition += HandleSceneTransition;

		if(!GameManager.instance.isGameLoaded)
			InstantiateSplashScreen();
	}

	private void InstantiateSplashScreen()
	{
		Node splashScreen = _splashScreenMenuScene.Instantiate();

		GetTree().CurrentScene.AddChild(splashScreen);
		splashScreen.GetNodeOrNull<AnimationPlayer>("AnimationPlayer").Play("splash_screen_animation"); //TODO : Refacto ?
	}

	public void HandleSceneTransition()
	{
		// TODO: Refacto the signal system because I need to remove the signal when it's unloaded/destroyed.
		_animationPlayer?.Play("music_fade_out");
		_animationPlayer = null;
	}
}
