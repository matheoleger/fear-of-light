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

	private const float movementGlyphForce = 300;

	private bool isCursorEnabled = true;

	public enum Glyph
	{
		LightGlyph,
		MovementGlyph
	}

	private Glyph selectedCursorGlyph = Glyph.MovementGlyph;

	public Glyph SelectedCursorGlyph {
		get { return selectedCursorGlyph; }
		set { selectedCursorGlyph = value; }
	}

	private const float maxDistance = 130f;

	private RigidBody2D selectedMovableObject;
	private RigidBody2D detectedMovableObject;
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
	public override void _PhysicsProcess(double delta)
	{
		if(!GameManager.instance.isGameLoaded) return;
		
		HandleAim();

		if(isCursorEnabled)
			HandleAction();
	}

	public void ChangeCursorTexture()
	{
		switch(selectedCursorGlyph){
			case Glyph.LightGlyph:
				_sprite2D.Texture = lightGlyphCursor;
				_pointLight.Color = new Color("ff4e4f");
				break;
			case Glyph.MovementGlyph:
				_sprite2D.Texture = movementGlyphCursor;
				_pointLight.Color = new Color("6efd99");
				break;
		}
	}

	private void HandleAim()
	{
		if(Input.IsActionPressed("aim_with_glyph"))
		{
			Visible = true;
			Input.MouseMode = Input.MouseModeEnum.ConfinedHidden;

			MoveGlyphCursor();
			ChangeCursorVisualState();
			
		} else {
			Visible = false;
			Input.MouseMode = Input.MouseModeEnum.Hidden;

			Position = GameManager.instance.player.Position; // Reinitialize position to exit from the Area of movableObject.

			if(selectedCursorGlyph == Glyph.MovementGlyph)
				ReinitializedMovementGlyphProperties();
		}
	}

	private void MoveGlyphCursor()
	{
		Vector2 mousePosition = GetGlobalMousePosition();

		Vector2 distance = mousePosition - GameManager.instance.player.Position;

		if(distance.Length() > maxDistance)
			Position = GameManager.instance.player.Position + distance.Normalized() * maxDistance;
		else
			Position = GameManager.instance.player.Position + distance;
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

	private void ReinitializedMovementGlyphProperties()
	{
		if(selectedMovableObject != null)
		{
			ChangeMovableObjectOutline(false);
			selectedMovableObject = null;
		}

		isMovingObject = false;
	}

	private void HandleAction() 
	{
		bool isAiming = Input.IsActionPressed("aim_with_glyph");
		if(!isAiming) return;

		if(selectedCursorGlyph == Glyph.LightGlyph)
		{
			HandleLightGlyph();
		} else if(selectedCursorGlyph == Glyph.MovementGlyph)
		{
			HandleMovementGlyph();
		}
	}

	private void HandleLightGlyph()
	{
		bool isPlacing = Input.IsActionPressed("trigger_glyph");

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
		if(detectedMovableObject == null) return;

		selectedMovableObject ??= detectedMovableObject;

		ChangeMovableObjectOutline(true);
		
		bool isTriggered = Input.IsActionJustPressed("trigger_glyph");
		if(isTriggered)
		{
			isMovingObject = !isMovingObject;
			ChangeMovableObjectPointLight(isMovingObject);
		}

		if(isMovingObject)
		{		
			Vector2 direction = (Position - selectedMovableObject.GlobalPosition).Normalized();

			if(selectedMovableObject.Position.DistanceTo(Position) > 5) // Condition to avoid shaking
				selectedMovableObject.ApplyCentralImpulse(direction * movementGlyphForce);
		}
	}

	private void ChangeMovableObjectOutline(bool isSelected)
	{
		if(selectedMovableObject.GetNode<Sprite2D>("Sprite2D") is Sprite2D movableObjectSprite)
			movableObjectSprite.Material.Set("shader_parameter/line_thickness", isSelected ? 1 : 0);
	}

	private void ChangeMovableObjectPointLight(bool isMoving)
	{
		if(selectedMovableObject.GetNode<PointLight2D>("PointLight2D") is PointLight2D movableObjectPointLight)
			movableObjectPointLight.Enabled = isMoving;
	}

	public void _OnCooldownTimerTimeout()
	{
		isCursorEnabled = true;
	}

	public void _OnArea2dBodyEntered(Node2D body)
	{
		if(isMovingObject || selectedCursorGlyph != Glyph.MovementGlyph) return;

		if(body.IsInGroup("MovableObjects") && body is RigidBody2D rigidBody)
			detectedMovableObject = rigidBody;
	}

	public void _OnArea2dBodyExited(Node2D body)
	{
		if(isMovingObject || body != detectedMovableObject) return;

		ReinitializedMovementGlyphProperties();
		detectedMovableObject = null;
	}
}
