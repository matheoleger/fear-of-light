using Godot;
using System;

public partial class Door : Receiver
{
	private AnimatedSprite2D _topAnimatedSprite;
	private AnimatedSprite2D _downAnimatedSprite;

	private CollisionShape2D _collisionShape;

	private bool isBodyDetected = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();

		GD.Print("ISENABLED ?", IsEnabled);

		_topAnimatedSprite = GetNode<AnimatedSprite2D>("TopAnimatedSprite2D");
		_downAnimatedSprite = GetNode<AnimatedSprite2D>("DownAnimatedSprite2D");	
		_collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");

		HandleAnimation();
		HandleDoorState();
	}

    public override void _Process(double delta)
    {
		HandleDoorVisualState((float) delta);
    }
    

	protected override void OnActivatorHasChangedState(Activator activator)
	{
		base.OnActivatorHasChangedState(activator);

		HandleAnimation();
		HandleDoorState();
	}

	private void HandleAnimation()
	{
		_topAnimatedSprite.Play(IsEnabled ? "open" : "close");
		_downAnimatedSprite.Play(IsEnabled ? "open" : "close");
	}

	private void HandleDoorState()
	{
		_collisionShape.Disabled = IsEnabled;
	}

	private void HandleDoorVisualState(float delta)
	{
		const float apparitionSpeed = 1.2f; 
		
		Color modulate = Modulate;
		bool isVisible = !isBodyDetected || !IsEnabled;

		if(!isVisible && Modulate.A > 0f) 
			modulate.A -= apparitionSpeed * delta;
		else if (isVisible && Modulate.A < 1f)
			modulate.A += apparitionSpeed * delta;

		Modulate = modulate;
	}

	public void _OnArea2dBodyEntered(Node2D body)
	{
		if(body == this) return;

		isBodyDetected = true;
	}

	public void _OnArea2dBodyExited(Node2D body)
	{
		isBodyDetected = false;
	}
}
