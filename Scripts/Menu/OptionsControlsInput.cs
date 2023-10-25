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

		// GD.Print(InputMap.ActionGetEvents(action)[3].AsText());

		// OS.GetKeycodeString(InputMap.ActionGetEvents(action)[3].keyCode);

		if(InputMap.ActionGetEvents(action)[0] is InputEventKey eventKey)
		{

			// OS.GetKeycodeString(eventKey.GetKeycodeWithModifiers());
			
			// GD.Print();

			// GD.Print("Is eventkey ", eventKey.AsTextPhysicalKeycode());
			GD.Print("Is eventkey ", eventKey.GetPhysicalKeycodeWithModifiers());
			Text = eventKey.AsTextPhysicalKeycode(); //Warning: All the time physical ?
		} else
		{
			GD.Print("Is mouseEvent");
			Text = InputMap.ActionGetEvents(action)[0].AsText();
		}

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
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
		if(canChangeKey) return;

		canChangeKey = true;
		Text = "Press key";
	}
}