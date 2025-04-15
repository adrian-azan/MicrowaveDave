shader_type spatial;
render_mode unshaded, cull_disabled;

uniform vec4 neon_color : source_color = vec4(0.0, 1.0, 1.0, 1.0);
uniform float intensity : hint_range(0.0, 10.0) = 2.0;
uniform float glow_threshold : hint_range(0.0, 1.0) = 0.5;

void fragment() {
    vec3 color = neon_color.rgb * intensity;

    // Simulate a slight bloom by clamping highlights
    vec3 glow = max(vec3(0.0), color - vec3(glow_threshold));

    ALBEDO = color;
    EMISSION = glow;
    ALPHA = neon_color.a;
}