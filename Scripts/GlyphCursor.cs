using Godot;
using System;

public partial class GlyphCursor : Node2D
{

	private Texture2D lightGlyphCursor;
	private Texture2D movementGlyphCursor;
	private PackedScene lightCrystalScene;
	private Sprite2D _sprite2D;
	private Timer _cooldownTimer;
	private PointLight2D _pointLight;

	private bool isCursorEnabled = true;

	private enum CursorGlyph
	{
		LightGlyph,
		MovementGlyph
	}

	private CursorGlyph selectedCursorGlyph = CursorGlyph.MovementGlyph;

	private Vector2 previousMousePosition;
	private const float maxDistance = 130f;

	private RigidBody2D selectedMovableObject;
	private bool isMovingObject = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		lightGlyphCursor = ResourceLoader.Load<Texture2D>("res://Resources/Images/light-glyph-v1.png");
		movementGlyphCursor = ResourceLoader.Load<Texture2D>("res://Resources/Images/movement-glyph-v1.png");
		lightCrystalScene = GD.Load<PackedScene>("res://Scenes/LightCrystal.tscn");
		_sprite2D = GetNode<Sprite2D>("Sprite2D");
		_cooldownTimer = GetNode<Timer>("CooldownTimer");
		_pointLight = GetNode<PointLight2D>("PointLight2D");

		// Define default glyph cursor
		ChangeCursorTexture();

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

			if(selectedCursorGlyph == CursorGlyph.MovementGlyph)
				ReinitializedMovementGlyphProperties();
		}
	}

	private void ChangeCursorVisualState()
	{
		const float disabledCursorModulateOpacity = 0.5f;
		const float cursorLightDefaultEnergy = 0.3f;

		Color modulate = Modulate;

		if(!isCursorEnabled) 
		{
			_pointLight.Energy = 0.0f;

			modulate.A = disabledCursorModulateOpacity;
			Modulate = modulate;
		}
		else if (_pointLight.Energy < cursorLightDefaultEnergy)
		{
			_pointLight.Energy += 0.01f;

			modulate.A = 1f;
			Modulate = modulate;
		}
	}

	private void ChangeCursorTexture()
	{
		switch(selectedCursorGlyph){
			case CursorGlyph.LightGlyph:
				_sprite2D.Texture = lightGlyphCursor;
				break;
			case CursorGlyph.MovementGlyph:
				_sprite2D.Texture = movementGlyphCursor;
				break;
		}
	}

	private void ReinitializedMovementGlyphProperties()
	{
		if(selectedMovableObject != null)
			selectedMovableObject = null;

		isMovingObject = false;
	}

	private void HandleAction() 
	{
		bool isAiming = Input.IsActionPressed("aim_with_glyph");
		if(!isAiming) return;

		if(selectedCursorGlyph == CursorGlyph.LightGlyph)
		{
			HandleLightGlyph();
		} else if(selectedCursorGlyph == CursorGlyph.MovementGlyph)
		{
			HandleMovementGlyph();
		}
	}

	private void HandleLightGlyph()
	{
		bool isPlacing = Input.IsActionPressed("place_glyph");

		if(!isPlacing) return;

		Node previousLightCrystal = GetTree().CurrentScene.GetNodeOrNull("LightCrystal");
		LightCrystal lightCrystalInstance = lightCrystalScene.Instantiate<LightCrystal>();
	
		lightCrystalInstance.Position = Position;

		if(previousLightCrystal != null)
			GetTree().CurrentScene.RemoveChild(previousLightCrystal);

		GetTree().CurrentScene.AddChild(lightCrystalInstance);


		isCursorEnabled = false;
		_cooldownTimer.Start();
	}

	private void HandleMovementGlyph()
	{
		if(selectedMovableObject == null) return;

		bool isSelecting = Input.IsActionPressed("place_glyph");

		if(isSelecting)
		{
			isMovingObject = !isMovingObject;
		}

		if(isMovingObject)
		{		
			Vector2 direction = (Position - selectedMovableObject.GlobalPosition).Normalized();

			if(selectedMovableObject.Position.DistanceTo(Position) > 5)
				selectedMovableObject.ApplyCentralImpulse(direction * 300); // Clean code (add const ?)
		}
		
	}

	public void _OnCooldownTimerTimeout()
	{
		isCursorEnabled = true;
	}

	public void _OnArea2dBodyEntered(Node2D body)
	{
		if(isMovingObject || selectedCursorGlyph != CursorGlyph.MovementGlyph) return;

		if(body.IsInGroup("MovableObjects") && body is RigidBody2D rigidBody)
		{
			selectedMovableObject = rigidBody;
			GD.Print("Bonsoir je viens de changer selectedMovableObject", selectedMovableObject);
		}
	}

	public void _OnArea2dBodyExited(Node2D body)
	{
		GD.Print("ON SORT OU QUOI ?");
		// if(isMovingObject || !body.IsInGroup("MovableObjects")) return;
		if(isMovingObject || body != selectedMovableObject) return;

		selectedMovableObject = null;
	}
}
