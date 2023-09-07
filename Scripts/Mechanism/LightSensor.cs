using Godot;
using System;
using System.Collections.Generic;

public partial class LightSensor : Activator
{

	private RayCast2D _raycast2D;
	private AnimatedSprite2D _animatedSprite;

	private List<Area2D> detectedLightSources = new List<Area2D>(); 
	private List<Area2D> lightSourcesInArea = new List<Area2D>(); 

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_raycast2D = GetNode<RayCast2D>("RayCast2D");
		_animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}

	public override void _PhysicsProcess(double delta)
	{
		foreach(Area2D lightSource in lightSourcesInArea)
		{
			_raycast2D.TargetPosition = lightSource.GetParent<Node2D>().Position - _raycast2D.GetParent<Node2D>().Position;
			_raycast2D.ForceRaycastUpdate();
			
			GodotObject collider = _raycast2D.GetCollider();
			if(collider == null) continue;
			Type colliderType = collider.GetType();

			if(_raycast2D.IsColliding() && colliderType != typeof(TileMap))
			{
				if(!IsEnabled) IsEnabled = true;

				if(!detectedLightSources.Contains(lightSource))
					detectedLightSources.Add(lightSource);

			} else if(_raycast2D.IsColliding())
			{
				if(detectedLightSources.Contains(lightSource))
					detectedLightSources.Remove(lightSource);
			}
		}

		if(detectedLightSources.Count == 0 && IsEnabled)
			IsEnabled = false;

		HandleAnimation();
	}

	public void _OnArea2dAreaEntered(Area2D area)
	{
		if(!lightSourcesInArea.Contains(area))
			lightSourcesInArea.Add(area);
	}

	public void _OnArea2dAreaExited(Area2D area)
	{

		if(lightSourcesInArea.Contains(area))
			lightSourcesInArea.Remove(area);

		if(detectedLightSources.Contains(area))
			detectedLightSources.Remove(area);		
	}

	private void HandleAnimation()
	{
		_animatedSprite.Play(IsEnabled ? "enabled" : "disabled");
	}
}
