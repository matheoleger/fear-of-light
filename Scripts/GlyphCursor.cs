using Godot;
using System;

public partial class GlyphCursor : Node2D
{

	private Texture2D lightGlyphCursor;
	private PackedScene lightCrystalScene;
	private Sprite2D _sprite2D;
	private Timer _cooldownTimer;
	private PointLight2D _pointLight;

	private bool isCursorEnabled = true;

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
		lightCrystalScene = GD.Load<PackedScene>("res://Scenes/LightCrystal.tscn");
		_sprite2D = GetNode<Sprite2D>("Sprite2D");
		_cooldownTimer = GetNode<Timer>("CooldownTimer");
		_pointLight = GetNode<PointLight2D>("PointLight2D");

		// Define default glyph cursor
		_sprite2D.Texture = lightGlyphCursor;

		Visible = false;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		HandleAim();

		if(isCursorEnabled)
			HandleAction();
	}

	private void HandleAim()
	{
		if(Input.IsActionPressed("aim_with_glyph"))
		{
			Visible = true;
			Input.MouseMode = Input.MouseModeEnum.ConfinedHidden;
			Vector2 mousePosition = GetGlobalMousePosition();

			Vector2 distance = mousePosition - GameManager.instance.player.Position;

			if(distance.Length() > maxDistance)
				Position = GameManager.instance.player.Position + distance.Normalized() * maxDistance;
			else
				Position = GameManager.instance.player.Position + distance;

			ChangeCursorVisualState();
			
		} else {
			Visible = false;
			Input.MouseMode = Input.MouseModeEnum.Hidden;
		}
	}

	private void ChangeCursorVisualState()
	{
		const float diabledCursorModulateOpacity = 0.5f;
		const float cursorLightDefaultEnergy = 0.3f;

		Color modulate = Modulate;

		if(!isCursorEnabled) 
		{
			_pointLight.Energy = 0.0f;

			modulate.A = diabledCursorModulateOpacity;
			Modulate = modulate;
		}
		else if (_pointLight.Energy < cursorLightDefaultEnergy)
		{
			_pointLight.Energy += 0.01f;

			modulate.A = 1f;
			Modulate = modulate;
		}
	}

	private void HandleAction() 
	{
		bool isAiming = Input.IsActionPressed("aim_with_glyph");
		bool isPlacing = Input.IsActionPressed("place_glyph");

		if(!isPlacing || !isAiming) return;

		if(selectedActions == CursorActions.SummonLightCrystal)
		{
			Node previousLightCrystal = GetTree().CurrentScene.GetNodeOrNull("LightCrystal");
			LightCrystal lightCrystalInstance = lightCrystalScene.Instantiate<LightCrystal>();
		
			lightCrystalInstance.Position = Position;

			if(previousLightCrystal != null)
				GetTree().CurrentScene.RemoveChild(previousLightCrystal);

			GetTree().CurrentScene.AddChild(lightCrystalInstance);
		}

		isCursorEnabled = false;
		_cooldownTimer.Start();
	}

	public void _OnCooldownTimerTimeout()
	{
		isCursorEnabled = true;
	}
}