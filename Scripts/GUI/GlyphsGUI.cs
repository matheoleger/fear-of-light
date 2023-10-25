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
			.ToList();

		_hotbarApparitionCooldown = GetNode<Timer>("HotbarApparitionCooldown");

		//Set the "visibility" to 0 for the start.
		Color modulate = Modulate;
		modulate.A = 0;
		Modulate = modulate;
	}


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

		if(!GameManager.instance.isGameLoaded) return;

		_glyphCursor ??= GameManager.instance.glyphCursor;

		HandleVisibility();
		HandleSelection();
	}

	private void HandleVisibility()
	{
		bool isAimed = Input.IsActionJustPressed("aim_with_glyph");
		if(isAimed) 
		{
			isHotbarVisible = true;
			_hotbarApparitionCooldown.Start();
		}

		ChangeHotbarVisibility(isHotbarVisible);
		
		if(!isHotbarVisible) return;

		Panel currentSelectedSlot = _slots[(int)_glyphCursor.SelectedCursorGlyph]; 
		Panel previousSelectedSlot = _slots[(int)previousSelectedGlyph];

		ChangeSelectorVisibility(previousSelectedSlot, false);
		ChangeSelectorVisibility(currentSelectedSlot, true);
	}

	private void ChangeHotbarVisibility(bool isVisible)
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
		HandleShortcuts();

		bool isSelectNextGlyph = Input.IsActionJustPressed("select_next_glyph");
		bool isSelectPreviousGlyph = Input.IsActionJustPressed("select_previous_glyph");

		int numberOfGlyphs = Enum.GetNames(typeof(GlyphCursor.Glyph)).Length;

		if(!isSelectNextGlyph && !isSelectPreviousGlyph) return;

		if(isSelectNextGlyph)
		{
			int selectedGlyphNumber =  (int)(_glyphCursor.SelectedCursorGlyph + 1) % numberOfGlyphs;
			SelectGlyph((GlyphCursor.Glyph)selectedGlyphNumber);
		} else if (isSelectPreviousGlyph)
		{
			int selectedGlyphNumber =  (int)(_glyphCursor.SelectedCursorGlyph - 1 + numberOfGlyphs) % numberOfGlyphs;
			SelectGlyph((GlyphCursor.Glyph)selectedGlyphNumber);
		}

		SetHotbarVisible();
	}

	private void HandleShortcuts()
	{
		if(Input.IsActionJustPressed("light_glyph_shortcut"))
		{
			SelectGlyph(GlyphCursor.Glyph.LightGlyph);

			SetHotbarVisible();

		} else if(Input.IsActionJustPressed("movement_glyph_shortcut"))
		{
			SelectGlyph(GlyphCursor.Glyph.MovementGlyph);

			SetHotbarVisible();
		}
	}

	private void SelectGlyph(GlyphCursor.Glyph selectedGlyph)
	{
		previousSelectedGlyph = _glyphCursor.SelectedCursorGlyph;

		_glyphCursor.SelectedCursorGlyph = selectedGlyph;
		_glyphCursor.ChangeCursorTexture();
	}

	private void ChangeSelectorVisibility(Panel slot, bool isEnabled)
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

	private void SetHotbarVisible()
	{
		//Show the hotbar for {timer} seconds
		isHotbarVisible = true;
		_hotbarApparitionCooldown.Start();
	}

	public void _OnHotbarApparitionCooldownTimeout()
	{
		isHotbarVisible = false;
	}
}
