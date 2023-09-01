using Godot;
using System;
using System.Text.Json.Serialization;

public partial class GlyphCursor : Node2D
{

	private Resource lightGlyphCursor;

	private Vector2 previousMousePosition;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		lightGlyphCursor = ResourceLoader.Load("res://Resources/Images/light-glyph-v1.png");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// [TODO] Add verification if the player is in "playmode" and not in "menu" or "pausemode"

		// if(Input.IsActionPressed("mouse_right"))
		// {
		// 	Input.MouseMode = Input.MouseModeEnum.Visible;
		// } else {
		// 	Input.MouseMode = Input.MouseModeEnum.Hidden;
		// }

		// if(Input.GetMouse)

		// GetViewport().WarpMouse(Vector2.Zero);

		// [TODO] Add mouse left action
	}

    // public override void _Input(InputEvent @event)
    // {
	// 	// if(@event is InputEventMouseButton eventMouseButton && eventMouseButton.IsActionPressed("mouse_right"))
	// 	// {
	// 	// 	Input.MouseMode = Input.MouseModeEnum.Visible;

	// 	// } else {
	// 	// 	Input.MouseMode = Input.MouseModeEnum.Hidden;
	// 	// }

	// 	if(Input.IsActionPressed("mouse_right"))
	// 	{
	// 		Input.MouseMode = Input.MouseModeEnum.Visible;

	// 		if(@event is InputEventMouseMotion eventMouseMotion)
	// 		{
	// 			// GD.Print(eventMouseMotion.Position);
				
	// 			Vector2 test = eventMouseMotion.Position - Vector2.Zero;

	// 			GD.Print(test.Length());

	// 			if(test.Length() >= 300)
	// 			{
					
	// 				// eventMouseMotion.Position = Vector2.Zero;
	// 				// GD.Print("EVENTMOUSEPOSITION", eventMouseMotion.Position);
	// 				// GetViewport().WarpMouse(previousMousePosition); // work but not well.
	// 				Input.SetDefaultCursorShape(Input.CursorShape.CanDrop);
	// 			} else 
	// 			{
	// 				previousMousePosition = eventMouseMotion.Position;
	// 				Input.SetDefaultCursorShape(Input.CursorShape.Arrow);
	// 			}

	// 			GD.Print("test", test);
	// 		}
	// 	} else 
	// 	{
	// 		Input.MouseMode = Input.MouseModeEnum.Hidden;
	// 	}
    // }
}
