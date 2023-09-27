using Godot;
using System;

public partial class MainMenuScene : Node2D
{

	private AudioStreamPlayer2D _backgroundMusicPlayer;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_backgroundMusicPlayer = GetNode<AudioStreamPlayer2D>("BackgroundMusicPlayer");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}
}
