using Godot;
using System;

public partial class Player : CharacterBody2D
{
	private AnimatedSprite2D _animatedSprite;
	private Timer _dashCooldownTimer;
	private Timer _dashGhostTimer;

	private PointLight2D _pointLight;
	private AudioStreamPlayer2D _audioStreamPlayer;

	private PackedScene _dashGhostScene;

	private const float MaxSpeed = 200.0f;
	private const float DashSpeed = 700.0f; 
	private const float DashAcceleration = 2500.0f;

	private const float Friction = 1200.0f;
	private const float Acceleration = 1500.0f;

	private bool isDashing = false;

	private bool isAwake = false;
	private bool isAlreadyAwake = false;

	public Vector2 previousDirection;

    public override void _Ready()
    {
		// Define nodes
        _animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_dashCooldownTimer = GetNode<Timer>("DashCooldownTimer");
		_dashGhostTimer = GetNode<Timer>("DashGhostTimer");
		_pointLight = GetNode<PointLight2D>("PointLight2D");
		_audioStreamPlayer = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D");

		_dashGhostScene = GD.Load<PackedScene>("res://Scenes/Player/DashGhost.tscn");

		// Define default states
		_pointLight.Energy = 0;
    }

	public override void _PhysicsProcess(double delta)
	{
		Vector2 direction = Input.GetVector("move_left", "move_right", "move_up", "move_down");

		HandleAwakening((float)delta);

		HandleAnimation(direction);

		if(direction != Vector2.Zero) previousDirection = direction;

		ApplyForceToRigidBodies();

		HandleDash();
		float maxSpeed = isDashing ? DashSpeed : MaxSpeed;
		float acceleration = isDashing ? DashAcceleration : Acceleration;
		MovePlayer(direction, acceleration, maxSpeed, (float)delta);
	}

	private void HandleAwakening(float delta)
	{
		if(Input.IsAnythingPressed() && !isAwake)
			isAwake = true;

		if(isAwake && _pointLight.Energy == 0.0) 
			_audioStreamPlayer.Play();
			isAlreadyAwake = true;

		if(isAwake && _pointLight.Energy < 0.6)
			_pointLight.Energy += 0.8f*delta;
	}

	private void HandleDash()
	{
		if(Input.IsActionJustPressed("dash"))
    	{
			isDashing = true;

			_dashCooldownTimer.Start();
			_dashGhostTimer.Start();
		}

		if(!isDashing)
			_dashGhostTimer.Stop();

	}

	private void InstanceDashGhost()
	{
			DashGhost dashGhostInstance = _dashGhostScene.Instantiate<DashGhost>();
			dashGhostInstance.Texture = _animatedSprite.SpriteFrames.GetFrameTexture(_animatedSprite.Animation, _animatedSprite.Frame);
			dashGhostInstance.GlobalPosition = GlobalPosition;
			
			GetParent().AddChild(dashGhostInstance);
	}

	//TODO: Show if there is a better solution ?
	private void ApplyForceToRigidBodies()
	{
		for(int slideIndex = 0; slideIndex < GetSlideCollisionCount(); slideIndex++)
		{
			KinematicCollision2D collision = GetSlideCollision(slideIndex);
			if(collision.GetCollider() is RigidBody2D rigidBodyObject && rigidBodyObject.IsInGroup("MovableObjects")) //clean here
				rigidBodyObject.ApplyCentralImpulse(-collision.GetNormal() * 10);
		}
	}

	private void MovePlayer(Vector2 direction, float acceleration, float maxSpeed, float delta)
	{
		Vector2 velocity = Velocity;

		//Smooth movement
		if(direction == Vector2.Zero) 
		{
			if(velocity.Length() > (Friction * (float)delta))
			{
				velocity -= velocity.Normalized() * Friction * (float)delta;
			} else 
			{
				velocity = Vector2.Zero;
			}
		} else 
		{
			velocity += direction * acceleration * (float)delta;
			velocity = velocity.LimitLength(maxSpeed);
		}
		
		Velocity = velocity;
		MoveAndSlide();
	}

	public void _OnDashCooldownTimeout()
	{
		isDashing = false;
	}

	public void _OnDashGhostTimerTimeout()
	{
		InstanceDashGhost();
	}

	private void HandleAnimation(Vector2 currentDirection) 
	{
		if(currentDirection == Vector2.Down)
		{
			_animatedSprite.Play("walk-down");
		} else if(currentDirection == Vector2.Up)
		{
			_animatedSprite.Play("walk-up");
		} else if(currentDirection == Vector2.Left)
		{
			_animatedSprite.Play("walk-left");
		} else if(currentDirection == Vector2.Right)
		{
			_animatedSprite.Play("walk-right");

		} 

		if(currentDirection != Vector2.Zero) return;
		
		if(previousDirection == Vector2.Down)
		{
			_animatedSprite.Play("idle-down");
		} else if(previousDirection == Vector2.Up)
		{
			_animatedSprite.Play("idle-up");
		} else if(previousDirection == Vector2.Left)
		{
			_animatedSprite.Play("idle-left");
		} else if(previousDirection == Vector2.Right)
		{
			_animatedSprite.Play("idle-right");
		}
	}
}
