## Shader `outline2D_outer.shader` (`InnerOutline`)

### Source 

[GDQuest/godot-shaders](https://github.com/GDQuest/godot-shaders/blob/master/godot/Shaders/outline2D_inner.shader)

### Tips

> if you're using Godot 4, and chose to use the Outer Outline shader, then change the line: 
"uniform vec4 line_color : hint_color = vec4(1.0);" to: 
"uniform vec4 line_color : source_color = vec4(1.0);".
It seems Godot 4 doesnt support "hint_color" any longer, it will give an error if not changed to "source_color".

----

## Shader `outline2D_outer.shader` (not use for now)

### Source 

[GDQuest/godot-shaders](https://github.com/GDQuest/godot-shaders/blob/master/godot/Shaders/outline2D_outer.shader)

### Tips

> if you're using Godot 4, and chose to use the Outer Outline shader, then change the line: 
"uniform vec4 line_color : hint_color = vec4(1.0);" to: 
"uniform vec4 line_color : source_color = vec4(1.0);".
It seems Godot 4 doesnt support "hint_color" any longer, it will give an error if not changed to "source_color".

