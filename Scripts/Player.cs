using Godot;
using System;

public partial class Player : CharacterBody2D
{
	private AnimatedSprite2D _animatedSprite;
	private Timer _dashCooldownTimer;

	private const float MaxSpeed = 200.0f;
	private const float DashSpeed = 700.0f; 
	private const float DashAcceleration = 2500.0f;

	private const float Friction = 1200.0f;
	private const float Acceleration = 1500.0f;

	private bool canDash = true;
	private bool isDashing = false;

	private Vector2 previousDirection;

    public override void _Ready()
    {
        _animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_dashCooldownTimer = GetNode<Timer>("DashCooldownTimer");
    }

	public override void _PhysicsProcess(double delta)
	{
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");

		HandleAnimation(direction);

		if(direction != Vector2.Zero) previousDirection = direction;

		HandleDash();
		float maxSpeed = isDashing ? DashSpeed : MaxSpeed;
		float acceleration = isDashing ? DashAcceleration : Acceleration;
		MovePlayer(direction, acceleration, maxSpeed, (float)delta);
	}

	private void HandleDash()
	{
		if(Input.IsActionPressed("dash") && canDash)
    	{
			canDash = false;
			isDashing = true;
			_dashCooldownTimer.Start();
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
		canDash = true;
		isDashing = false;
	}

	private void HandleAnimation(Vector2 currentDirection) 
	{
		if(currentDirection == Vector2.Down)
		{
			_animatedSprite.Play("walk-down");
		} else if(currentDirection == Vector2.Up)
		{
			_animatedSprite.Play("walk-top");
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
			_animatedSprite.Play("idle-top");
		} else if(previousDirection == Vector2.Left)
		{
			_animatedSprite.Play("idle-left");
		} else if(previousDirection == Vector2.Right)
		{
			_animatedSprite.Play("idle-right");
		}
	}
}
