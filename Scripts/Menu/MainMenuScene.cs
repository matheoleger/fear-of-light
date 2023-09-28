using Godot;
using System;

public partial class MainMenuScene : Control
{

	private AudioStreamPlayer _backgroundMusicPlayer;

	private PackedScene _splashScreenMenuScene;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_backgroundMusicPlayer = GetNode<AudioStreamPlayer>("BackgroundMusicPlayer");
		_splashScreenMenuScene = GD.Load<PackedScene>("res://Scenes/Menu/SplashScreenMenu.tscn");

		Node splashScreen = _splashScreenMenuScene.Instantiate();

		GetTree().CurrentScene.AddChild(splashScreen);
		splashScreen.GetNode<AnimationPlayer>("AnimationPlayer").Play("splash_screen_animation"); //TODO : Refacto ?
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}
}
