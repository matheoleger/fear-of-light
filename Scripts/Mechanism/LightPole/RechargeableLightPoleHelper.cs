using Godot;
using System;

public partial class RechargeableLightPoleHelper : Node
{
	/// <remarks>
	///     <para>
	///         This method can be called in the <c>_Ready</c> method of LightPole classes to initialize values for RechargeableLightPole (according to <c>IsRechargeable</c>)
	///     </para>
	///     <para>
	///         Define default values of the lightPole (according to <c>IsRechargeable</c>).
	///     </para>
	/// </remarks>
	public static void InitializeRechargeableLightPole(ILightPole lightPole)
	{
		ChangeAnimation(lightPole);

		if (!lightPole.IsRechargeable) return;

		lightPole._PointLight2D.Energy = 0.0f;

		if (lightPole._LightArea2D.GetNode<CollisionShape2D>("CollisionShape2D").Shape is CircleShape2D circleShape2D)
			circleShape2D.Radius = 0.0f;

	}

	private static void ChangeAnimation(ILightPole lightPole)
	{
		if (lightPole.IsRechargeable)
			lightPole._AnimatedSprite.Play("rechargeable");
		else
			lightPole._AnimatedSprite.Play("default");
	}

	public static void FadeOutLight(LightPole lightPole)
	{
		lightPole.RechargeableTween?.Kill();
		lightPole.RechargeableTween = lightPole.CreateTween();

		HandleFadeOutLight(lightPole);
	}

	public static void FadeOutLight(MovableLightPole movableLightPole)
	{
		movableLightPole.RechargeableTween?.Kill();
		movableLightPole.RechargeableTween = movableLightPole.CreateTween();

		HandleFadeOutLight(movableLightPole);
	}

	public static void FadeInLight(LightPole lightPole)
	{
		lightPole.RechargeableTween?.Kill();
		lightPole.RechargeableTween = lightPole.CreateTween();

		HandleFadeOutLight(lightPole);
	}

	public static void FadeInLight(MovableLightPole movableLightPole)
	{
		movableLightPole.RechargeableTween?.Kill();
		movableLightPole.RechargeableTween = movableLightPole.CreateTween();

		HandleFadeInLight(movableLightPole);
	}

	private static void HandleFadeOutLight(ILightPole lightPole)
	{
		lightPole.RechargeableTween.Parallel().TweenProperty(lightPole._PointLight2D, "energy", 0.0f, 15.0f);

		if (lightPole._LightArea2D.GetNode<CollisionShape2D>("CollisionShape2D").Shape is CircleShape2D circleShape2D)
			lightPole.RechargeableTween.Parallel().TweenProperty(circleShape2D, "radius", 0.0f, 15.0f);
	}

	private static void HandleFadeInLight(ILightPole lightPole)
	{
		lightPole.RechargeableTween.Parallel().TweenProperty(lightPole._PointLight2D, "energy", 0.6f, 4.0f);

		if (lightPole._LightArea2D.GetNode<CollisionShape2D>("CollisionShape2D").Shape is CircleShape2D circleShape2D)
			lightPole.RechargeableTween.Parallel().TweenProperty(circleShape2D, "radius", 100.0f, 4.0f);
	}
}