using Godot;
using System;
using System.Diagnostics;
using System.Linq;

public partial class GameManager : Node
{

	#region Singleton instance 
	public static GameManager instance;

	#endregion

	public Player player;

	public GlyphCursor glyphCursor;

	private string[] gameScenes = {
		"TestScene"
	};

	public bool isGameLoaded = false;
	public bool isGamePaused = false;

	public override void _Ready()
	{
		instance = this;

		player = GetTree().CurrentScene.GetNodeOrNull<Player>("Player");
		glyphCursor = GetTree().CurrentScene.GetNodeOrNull<GlyphCursor>("GlyphCursor");

		ProcessMode = ProcessModeEnum.Always;

		Input.MouseMode = Input.MouseModeEnum.Visible;
	}

	public override void _Process(double delta)
	{
		// [TODO] Add verification if the player is in "playmode" and not in "menu" or "pausemode"

		if(gameScenes.Contains<string>(GetTree().CurrentScene.Name)) 
		{
			player ??= GetTree().CurrentScene.GetNodeOrNull<Player>("Player");
			glyphCursor ??= GetTree().CurrentScene.GetNodeOrNull<GlyphCursor>("GlyphCursor");

			if(!isGameLoaded) isGameLoaded = true;

			if(isGamePaused)
			{
				Input.MouseMode = Input.MouseModeEnum.Visible;
			}
		
			HandleGamePause();
		} else {
			isGamePaused = false;
			Input.MouseMode = Input.MouseModeEnum.Visible;
		}
	}

	private void HandleGamePause()
	{
		GetTree().Paused = isGamePaused;
	}
}
