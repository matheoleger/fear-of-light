using Godot;
using System;

public partial class Player : CharacterBody2D
{
	public const float Speed = 300.0f;

	private AnimatedSprite2D _animatedSprite;

	private Vector2 previousDirection;

    public override void _Ready()
    {
        _animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
    }

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");

		HandleAnimation(direction);

		velocity.X = direction.X * Speed;
		velocity.Y = direction.Y * Speed;

		previousDirection = direction;

		Velocity = velocity;
		MoveAndSlide();
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
		
		if(lastDirection == Vector2.Down)
		{
			_animatedSprite.Play("idle-down");
		} else if(lastDirection == Vector2.Up)
		{
			_animatedSprite.Play("idle-top");
		} else if(lastDirection == Vector2.Left)
		{
			_animatedSprite.Play("idle-left");
		} else if(lastDirection == Vector2.Right)
		{
			_animatedSprite.Play("idle-right");
		}
	}
}
