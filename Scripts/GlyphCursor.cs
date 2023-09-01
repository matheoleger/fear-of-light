using Godot;
using System;
using System.Text.Json.Serialization;

public partial class GlyphCursor : Node2D
{

	private Texture2D lightGlyphCursor;
	private Sprite2D _sprite2D;

	private Vector2 previousMousePosition;
	private const float maxDistance = 130f;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		lightGlyphCursor = ResourceLoader.Load<Texture2D>("res://Resources/Images/light-glyph-v1.png");
		_sprite2D = GetNode<Sprite2D>("Sprite2D");

		// Define default glyph cursor
		GD.Print(lightGlyphCursor);
		_sprite2D.Texture = lightGlyphCursor;

		Visible = false;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

		if(Input.IsActionPressed("mouse_right"))
		{
			Visible = true;
			Input.MouseMode = Input.MouseModeEnum.Confined;
			Vector2 mousePosition = GetGlobalMousePosition();

			Vector2 distance = mousePosition - GameManager.instance.player.Position;

			if(distance.Length() > maxDistance)
			{
				Position = GameManager.instance.player.Position + distance.Normalized() * maxDistance;
			} else 
			{
				Position = GameManager.instance.player.Position + distance;
			}
		} else {
			Visible = false;
			Input.MouseMode = Input.MouseModeEnum.Hidden;
		}
		
		// [TODO] Add mouse left action
	}
}
