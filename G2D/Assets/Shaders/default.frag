#version 320 es
precision highp float;

// input
layout (location = 0) in vec2 f_tex_coords;
layout (location = 1) in vec4 f_color;

// output
layout (location = 0) out vec4 o_color;

// uniform
layout (set = 0, binding = 0) uniform UniformBlock {
    mat4 u_view;
    vec4 u_color;
    vec2 u_res;
    vec2 u_mouse;
    float u_time;
};

/* CUSTOM-BEGIN */
vec4 effect(vec4 color, texture2D tex, vec2 tex_coords, vec2 screen_coords) {
    return vec4(0.0);
}
/* CUSTOM-END*/

void main() {
    o_color = f_color;
}