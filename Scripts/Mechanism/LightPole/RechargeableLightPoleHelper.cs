using Godot;
using System;

public partial class RechargeableLightPoleHelper : Node
{
    /// <remarks>
	///     <para>
	///         This method can be called in the <c>_Ready</c> method of LightPole classes to define the animation (according to <c>IsRechargeable</c>)
	///     </para>
	///     <para>
	///         Define the animation of the lightPole.
	///     </para>
	/// </remarks>
    public static void ChangeAnimation(ILightPole lightPole)
	{
		if(lightPole.IsRechargeable)
			lightPole._AnimatedSprite.Play("rechargeable");
		else 
			lightPole._AnimatedSprite.Play("default");
	}

	public static void FadeOutLight(ILightPole lightpole)
	{
		// SceneTree tree = lightpole._PointLight2D.GetParent().GetTree();
		// // SceneTree pointLightTree = lightpole._PointLight2D.GetTree();
		
		// Tween tween = tree.CreateTween();

		// tween.

		lightpole.RechargeableTween ??= lightpole._PointLight2D.GetParent().GetTree().CreateTween();


		lightpole.RechargeableTween.Parallel().TweenProperty(lightpole._PointLight2D, "energy", 0.0f, 15.0f);
		
		// CircleShape2D lightAreaCircleShape = lightpole._LightArea2D.GetNode<CollisionShape2D>("CollisionShape2D").Shape.GetType()

		

		// GD.Print(lightpole._LightArea2D.GetNode<CollisionShape2D>("CollisionShape2D").Shape.GetType());

		if(lightpole._LightArea2D.GetNode<CollisionShape2D>("CollisionShape2D").Shape is CircleShape2D circleShape2D)
			lightpole.RechargeableTween.Parallel().TweenProperty(circleShape2D, "radius", 0.0f, 15.0f);
	}
}