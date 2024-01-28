using Godot;
using System;
using System.Collections.Generic;

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

	/// <remarks>
	///     <para>
	///         This <c>PointLight2D</c> must be define in the <c>_Ready</c> method
	///     </para>
	///     <para>
	///         PointLight2D of the LightPole (light source).
	///     </para>
	/// </remarks>
	abstract PointLight2D _PointLight2D { get; set; }

	/// <remarks>
	///     <para>
	///         This <c>Area2D</c> must be define in the <c>_Ready</c> method
	///     </para>
	///     <para>
	///         Area2D to detect a light source (to recharge the lightPole).
	///     </para>
	/// </remarks>
	abstract Area2D _LightArea2D { get; set; }

	/// <remarks>
	///     <para>
	///         This <c>RayCast2D</c> must be define in the <c>_Ready</c> method
	///     </para>
	///     <para>
	///         RayCast2D to detect a light source (and verify if it's through an object/wall) (to recharge the lightPole).
	///     </para>
	/// </remarks>
	abstract RayCast2D _RayCast2D { get; set; }


	/// <remarks>
	///     <para>
	///         This <c>Tween</c> must be define in the <c>_Ready</c> method
	///     </para>
	///     <para>
	///         Tween to fade in and out the light (used in <see cref="RechargeableLightPoleHelper"/>).
	///     </para>
	/// </remarks>
	abstract Tween RechargeableTween {get; set; }
}
