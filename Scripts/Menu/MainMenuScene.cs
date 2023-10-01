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

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}

	private void InstantiateSplashScreen()
	{
		Node splashScreen = _splashScreenMenuScene.Instantiate();

		GetTree().CurrentScene.AddChild(splashScreen);
		splashScreen.GetNodeOrNull<AnimationPlayer>("AnimationPlayer").Play("splash_screen_animation"); //TODO : Refacto ?
	}

	public void HandleSceneTransition()
	{
		_animationPlayer.Play("music_fade_out");
	}
}
