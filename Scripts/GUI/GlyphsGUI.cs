using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class GlyphsGUI : Control
{

	GlyphCursor _glyphCursor;
	GridContainer _glyphsHotbar;
	List<Panel> _slots;

	Timer _hotbarApparitionCooldown;

	GlyphCursor.Glyph previousSelectedGlyph;

	bool isHotbarVisible = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_glyphCursor = GameManager.instance.glyphCursor;
		_glyphsHotbar = GetNode<GridContainer>("GlyphsHotbar");
		_slots = _glyphsHotbar.GetChildren()
			.Where(child => child is Panel)
			.Select(child => child)
			.Cast<Panel>()
			.ToList(); //TODO: refacto ?

		_hotbarApparitionCooldown = GetNode<Timer>("HotbarApparitionCooldown");
	}


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

		bool isAimed = Input.IsActionJustPressed("aim_with_glyph");

		if(isAimed) 
		{
			isHotbarVisible = true;
			_hotbarApparitionCooldown.Start();
		}

		// TODO: Visible when we aim or when we use the mousewheel/keyboard numbers. Else, it's hidden.
		ChangeVisualState(isHotbarVisible);

		HandleSelection();

		//TODO WARNING: Refacto

		Panel currentSelectedSlot = _slots[(int)_glyphCursor.SelectedCursorGlyph]; 
		Panel previousSelectedSlot = _slots[(int)previousSelectedGlyph];

		ChangeSelectorVisualState(previousSelectedSlot, false);
		ChangeSelectorVisualState(currentSelectedSlot, true);

	}

	private void ChangeVisualState(bool isVisible)
	{
		const float disabledCursorModulateOpacity = 0f;

		Color modulate = Modulate;

		if(!isVisible && modulate.A > disabledCursorModulateOpacity)
		{
			modulate.A -= 0.1f;
			Modulate = modulate;
		} else if (isVisible && modulate.A < 1f) {
			modulate.A += 0.1f;
			Modulate = modulate;
		}
	}

	private void HandleSelection()
	{
		bool isSelectNextGlyph = Input.IsActionJustPressed("select_next_glyph");
		bool isSelectPreviousGlyph = Input.IsActionJustPressed("select_previous_glyph");

		int numberOfGlyphs = Enum.GetNames(typeof(GlyphCursor.Glyph)).Length;

		if(!isSelectNextGlyph && !isSelectPreviousGlyph) return;

		if(isSelectNextGlyph)
		{
			int selectedGlyphNumber =  (int)(_glyphCursor.SelectedCursorGlyph + 1) % numberOfGlyphs;
			SelectGlyph((GlyphCursor.Glyph)selectedGlyphNumber);
			// ChangeVisualState(isSelectNextGlyph);
		} else if (isSelectPreviousGlyph)
		{
			// ChangeVisualState(isSelectPreviousGlyph);
			int selectedGlyphNumber =  (int)(_glyphCursor.SelectedCursorGlyph - 1 + numberOfGlyphs) % numberOfGlyphs;
			SelectGlyph((GlyphCursor.Glyph)selectedGlyphNumber);
		}

		isHotbarVisible = true;
		_hotbarApparitionCooldown.Start();
	}

	private void SelectGlyph(GlyphCursor.Glyph selectedGlyph)
	{
		previousSelectedGlyph = _glyphCursor.SelectedCursorGlyph;

		_glyphCursor.SelectedCursorGlyph = selectedGlyph;
		_glyphCursor.ChangeCursorTexture();

		GD.Print(_glyphCursor.SelectedCursorGlyph);

	}

	private void ChangeSelectorVisualState(Panel slot, bool isEnabled)
	{
		const float disabledCursorModulateOpacity = 0f;

		Color modulate = slot.GetNode<TextureRect>("SelectIndicatorTexture").Modulate;

		if(!isEnabled && modulate.A > disabledCursorModulateOpacity)
		{
			modulate.A -= 0.1f;
			slot.GetNode<TextureRect>("SelectIndicatorTexture").Modulate = modulate;
		} else if (isEnabled && modulate.A < 1f) {
			modulate.A += 0.1f;
			slot.GetNode<TextureRect>("SelectIndicatorTexture").Modulate = modulate;
		}
	}

	public void _OnHotbarApparitionCooldownTimeout()
	{
		isHotbarVisible = false;
	}
}
