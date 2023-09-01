using Godot;
using System;

public partial class GlyphCursor : Node2D
{

	private Texture2D lightGlyphCursor;
	private PackedScene lightCrystal;
	private Sprite2D _sprite2D;

	private enum CursorActions
	{
		SummonLightCrystal
	}

	private CursorActions selectedActions = CursorActions.SummonLightCrystal;

	private Vector2 previousMousePosition;
	private const float maxDistance = 130f;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		lightGlyphCursor = ResourceLoader.Load<Texture2D>("res://Resources/Images/light-glyph-v1.png");
		lightCrystal = GD.Load<PackedScene>("res://Scenes/LightCrystal.tscn");
		_sprite2D = GetNode<Sprite2D>("Sprite2D");

		// Define default glyph cursor
		_sprite2D.Texture = lightGlyphCursor;

		Visible = false;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		HandleAim();
		HandleAction();
	}

	private void HandleAim()
	{
		if(Input.IsActionPressed("mouse_right"))
		{
			Visible = true;
			Input.MouseMode = Input.MouseModeEnum.ConfinedHidden;
			Vector2 mousePosition = GetGlobalMousePosition();

			Vector2 distance = mousePosition - GameManager.instance.player.Position;

			if(distance.Length() > maxDistance)
				Position = GameManager.instance.player.Position + distance.Normalized() * maxDistance;
			else
				Position = GameManager.instance.player.Position + distance;
		} else {
			Visible = false;
			Input.MouseMode = Input.MouseModeEnum.Hidden;
		}
	}

	private void HandleAction() 
	{
		if(selectedActions == CursorActions.SummonLightCrystal && Input.IsActionPressed("mouse_left") && Input.IsActionPressed("mouse_right"))
		{
			Node lightCrystalInstance = lightCrystal.Instantiate();
			
			//[TODO] Improve this part
			lightCrystalInstance.Set("position", Position);
			
			//[TODO] Add verification to delete LightCrystal
			GetTree().CurrentScene.RemoveChild(GetTree().CurrentScene.GetNode("LightCrystal"));

			GetTree().CurrentScene.AddChild(lightCrystalInstance);
		}
	}
}
