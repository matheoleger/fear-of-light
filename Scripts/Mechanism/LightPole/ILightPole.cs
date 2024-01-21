using Godot;
using System;

public interface ILightPole
{
	/// <remarks>
	///     <para>
	///         This boolean must be an exported value with <c>[Export]</c>
	///     </para>
	///     <para>
	///         Define the state of the lightPole. If true, the lightPole loses brightness
	///     </para>
	/// </remarks>
	public bool IsRechargeable { get; set; }

	/// <remarks>
	///     <para>
	///         This <c>AnimatedSprite2D</c> must be define in the <c>_Ready</c> method
	///     </para>
	///     <para>
	///         AnimatedSprite to control the animation of the lightPole.
	///     </para>
	/// </remarks>
	abstract AnimatedSprite2D _AnimatedSprite { get; set; }


	abstract PointLight2D _PointLight2D { get; set; }

	abstract Area2D _LightArea2D { get; set; }

	abstract Tween RechargeableTween {get; set; }

}
