using Godot;
using System;

public partial class OptionsControlsInput : Button
{
	[Export]
	private string action;

	private bool canChangeKey = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if(action == null) return;

		Text = InputMap.ActionGetEvents(action)[0].AsText();
	}

	public override void _Input(InputEvent @event)
	{
		if(@event is InputEventKey keyEvent && keyEvent.Pressed && canChangeKey)
		{
			GD.Print(@event.AsText());

			InputMap.ActionEraseEvents(action);
			InputMap.ActionAddEvent(action, keyEvent);

			Text = keyEvent.AsText();

			canChangeKey = false;
		}

		if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed && canChangeKey)
		{
			GD.Print(@event.AsText());

			InputMap.ActionEraseEvents(action);
			InputMap.ActionAddEvent(action, mouseEvent);

			Text = mouseEvent.AsText();

			canChangeKey = false;
		}
	}

	public void _OnPressed()
	{
		if(canChangeKey || Input.IsKeyPressed(Key.Space)) return;

		canChangeKey = true;
		Text = "Press key";
	}
}
