using Godot;
using System;
using System.Collections.Generic;

public partial class LightSensor : Activator
{

	private RayCast2D _raycast2D;

	private List<Area2D> detectedLightSources = new List<Area2D>(); 

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_raycast2D = GetNode<RayCast2D>("RayCast2D");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	// public void _OnArea2dBodyEntered(Node2D body)
	// {
	// 	// RayCast2D rayCast = new();

	// 	// rayCast.TargetPosition = body.Position;
	// 	// rayCast.CollisionMask = 1;

	// 	// if(rayCast.GetCollider() == body)
	// 	// {

	// 	// }

	// 	GD.Print("body", body.Name);
	// }

	public void _OnArea2dAreaEntered(Area2D area)
	{
		GD.Print("area", area.Name);

		_raycast2D.TargetPosition = area.GetParent<Node2D>().Position - _raycast2D.GetParent<Node2D>().Position;
		_raycast2D.ForceRaycastUpdate();

		GodotObject collider = _raycast2D.GetCollider();
		Type colliderType = collider.GetType();

		if(_raycast2D.IsColliding() && colliderType != typeof(TileMap))
		{
			GD.Print("RAYCASTING THE BODY", _raycast2D.GetCollider());
			isEnabled = true;

			if(!detectedLightSources.Contains(area))
				detectedLightSources.Add(area);
		}

		GD.Print("Light sensor is enabled ? ", isEnabled);
	}

	public void _OnArea2dAreaExited(Area2D area)
	{
		if(detectedLightSources.Contains(area))
			detectedLightSources.Remove(area);

		if(detectedLightSources.Count == 0)
		{
			isEnabled = false;
		}

		GD.Print("Light sensor is enabled ? ", isEnabled);
	}
}
