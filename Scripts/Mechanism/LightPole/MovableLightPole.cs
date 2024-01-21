using Godot;
using System;
using System.Collections.Generic;

public partial class MovableLightPole : RigidBody2D, ILightPole
{

	[Export]
	public bool IsRechargeable { get; set; }

	public AnimatedSprite2D _AnimatedSprite { get; set; }

	public PointLight2D _PointLight2D {get; set;}

	public Area2D _LightArea2D { get; set; }

	public RayCast2D _RayCast2D { get; set; }

	public Tween RechargeableTween { get; set; }

	private readonly List<Area2D> detectedLightSources = new List<Area2D>(); 
	private readonly List<Area2D> areasCurrentlyInArea2D = new List<Area2D>(); 


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_AnimatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_PointLight2D = GetNode<PointLight2D>("PointLight2D");
		_LightArea2D = GetNode<Area2D>("LightArea2D");
		_RayCast2D = GetNode<RayCast2D>("RayCast2D");

		// Initialize the rechargeableLightPole
		if(IsRechargeable)
			InitializeRechargeableLightPole();

		RechargeableLightPoleHelper.ChangeAnimation(this);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		if(!IsRechargeable) return;

		DetectAndTrackLightSources();

		if(detectedLightSources.Count >= 1)
			FadeInLight();
		else
			FadeOutLight();
	}

	private void InitializeRechargeableLightPole()
	{
		_PointLight2D.Energy = 0.0f;

		if(_LightArea2D.GetNode<CollisionShape2D>("CollisionShape2D").Shape is CircleShape2D circleShape2D)
			circleShape2D.Radius = 0.0f;
	}


	private void FadeOutLight()
	{
		RechargeableTween?.Kill();
		RechargeableTween = CreateTween();

		RechargeableTween.Parallel().TweenProperty(_PointLight2D, "energy", 0.0f, 15.0f);

		if(_LightArea2D.GetNode<CollisionShape2D>("CollisionShape2D").Shape is CircleShape2D circleShape2D)
			RechargeableTween.Parallel().TweenProperty(circleShape2D, "radius", 0.0f, 15.0f);
	}

	private void FadeInLight()
	{
		RechargeableTween?.Kill();
		RechargeableTween = CreateTween();

		RechargeableTween.Parallel().TweenProperty(_PointLight2D, "energy", 0.6f, 4.0f);

		if(_LightArea2D.GetNode<CollisionShape2D>("CollisionShape2D").Shape is CircleShape2D circleShape2D)
			RechargeableTween.Parallel().TweenProperty(circleShape2D, "radius", 100.0f, 4.0f);
	}

	private void DetectAndTrackLightSources()
	{
		foreach(Area2D areaToVerified in areasCurrentlyInArea2D)
		{
			Node2D parentOfVerifiedArea = areaToVerified.GetParent<Node2D>();

			_RayCast2D.TargetPosition = parentOfVerifiedArea.Position - Position;
			_RayCast2D.ForceRaycastUpdate();
			
			GodotObject collider = _RayCast2D.GetCollider();
			if(collider == null) continue;
			
			bool isColliderALightSource = collider is Node2D nodeCollider && nodeCollider.IsInGroup("LightSources");

			if(_RayCast2D.IsColliding() && isColliderALightSource)
			{
				if(!detectedLightSources.Contains(areaToVerified))
					detectedLightSources.Add(areaToVerified);

			} else if(_RayCast2D.IsColliding())
			{
				if(detectedLightSources.Contains(areaToVerified))
					detectedLightSources.Remove(areaToVerified);
			}
		}
	}


	public void _OnRechargeableArea2dAreaEntered(Area2D area)
	{
		if(!IsRechargeable) return;


		if(!areasCurrentlyInArea2D.Contains(area) && area.GetParent() != this)
			areasCurrentlyInArea2D.Add(area);

	}

	public void _OnRechargeableArea2dAreaExited(Area2D area)
	{
		if(!IsRechargeable) return;

		if(areasCurrentlyInArea2D.Contains(area))
			areasCurrentlyInArea2D.Remove(area);
	

		if(detectedLightSources.Contains(area))
			detectedLightSources.Remove(area);	
	}
}