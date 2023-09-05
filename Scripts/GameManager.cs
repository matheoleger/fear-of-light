using Godot;
using System;
using System.Linq;

public partial class GameManager : Node
{

	#region Singleton instance 
	public static GameManager instance;

	#endregion

	public Player player;

	private string[] gameScenes = {
		"TestScene"
	};

	public override void _Ready()
	{
		instance = this;

		player = GetTree().CurrentScene.GetNodeOrNull<Player>("Player");

		// [WARNING] Depends on the GameManager is not shared between scenes.
		if(gameScenes.Contains<string>(GetTree().CurrentScene.Name))
			Input.MouseMode = Input.MouseModeEnum.Hidden;
	}

	public override void _Process(double delta)
	{
		// [TODO] Add verification if the player is in "playmode" and not in "menu" or "pausemode"
	}
}
