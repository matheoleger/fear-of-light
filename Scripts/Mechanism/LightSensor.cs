using Godot;
using System;
using System.Collections.Generic;

public partial class LightSensor : Activator
{

	private RayCast2D _raycast2D;
	private AnimatedSprite2D _animatedSprite;

	private List<Area2D> detectedLightSources = new List<Area2D>(); 
	private List<Area2D> areasCurrentlyInArea2D = new List<Area2D>(); 

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_raycast2D = GetNode<RayCast2D>("RayCast2D");
		_animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}

	public override void _PhysicsProcess(double delta)
	{
		DetectAndTrackLightSources();

		if(detectedLightSources.Count == 0 && IsEnabled)
			IsEnabled = false;
		else if(detectedLightSources.Count > 0 && !IsEnabled)
			IsEnabled = true;

		HandleAnimation();
	}

	private void DetectAndTrackLightSources()
	{
		foreach(Area2D areaToVerified in areasCurrentlyInArea2D)
		{
			Node2D parentOfVerifiedArea = areaToVerified.GetParent<Node2D>();

			_raycast2D.TargetPosition = parentOfVerifiedArea.Position - Position;
			_raycast2D.ForceRaycastUpdate();
			
			GodotObject collider = _raycast2D.GetCollider();
			if(collider == null) continue;
			bool isColliderALightSource = collider is Node2D nodeCollider && nodeCollider.IsInGroup("LightSources");

			if(_raycast2D.IsColliding() && isColliderALightSource)
			{
				if(!detectedLightSources.Contains(areaToVerified))
					detectedLightSources.Add(areaToVerified);

			} else if(_raycast2D.IsColliding())
			{
				if(detectedLightSources.Contains(areaToVerified))
					detectedLightSources.Remove(areaToVerified);
			}
		}
	}

	public void _OnArea2dAreaEntered(Area2D area)
	{
		if(!areasCurrentlyInArea2D.Contains(area))
			areasCurrentlyInArea2D.Add(area);
	}

	public void _OnArea2dAreaExited(Area2D area)
	{

		if(areasCurrentlyInArea2D.Contains(area))
			areasCurrentlyInArea2D.Remove(area);

		if(detectedLightSources.Contains(area))
			detectedLightSources.Remove(area);		
	}

	private void HandleAnimation()
	{
		_animatedSprite.Play(IsEnabled ? "enabled" : "disabled");
	}
}
