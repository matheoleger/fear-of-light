using Godot;
using System;

public partial class CursorManager : Node2D
{

	private Resource cursorImage;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		cursorImage = ResourceLoader.Load("res://Resources/Images/cursor-v1.png");
		
		Input.SetCustomMouseCursor(cursorImage);
		Input.MouseMode = Input.MouseModeEnum.Hidden;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// [TODO] Add verification if the player is in "playmode" and not in "menu" or "pausemode"

		if(Input.IsActionPressed("mouse_right"))
		{
			Input.MouseMode = Input.MouseModeEnum.Visible;
		} else {
			Input.MouseMode = Input.MouseModeEnum.Hidden;
		}

		// [TODO] Add mouse left action
	}
}
