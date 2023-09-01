using Godot;
using System;
using System.Linq;

public partial class GameManager : Node
{

	#region Singleton instance 
	public static GameManager instance;

	#endregion

	private Player player;

	private string[] gameScenes = {
		"TestScene"
	};

	public override void _Ready()
	{
		instance = this;

		// player = GetNode<Player>("/root/TestScene/Player");
		player = GetTree().CurrentScene.GetNode<Player>("Player");

		// [WARNING] Depends on the GameManager is not shared between scenes.
		if(gameScenes.Contains<string>(GetTree().CurrentScene.Name))
			Input.MouseMode = Input.MouseModeEnum.Hidden;
	}

	public override void _Process(double delta)
	{
		GD.Print(player.previousDirection);
		
	}
}
