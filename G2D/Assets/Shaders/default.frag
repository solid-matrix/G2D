#version 450 core

// input
layout (location = 0) in vec2 f_tex_coords;
layout (location = 1) in vec4 f_color;
layout (location = 2) flat in uint f_is_texture;
layout (location = 3) flat in uint f_ti[14];

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
layout (set = 1, binding = 0) uniform texture2D u_textures[65536];
layout (set = 2, binding = 0) uniform sampler u_samplers[8];

/* CUSTOM-BEGIN */
vec4 effect(vec4 color, texture2D tex, vec2 tex_coords, vec2 screen_coords) {
    return vec4(0.0);
}
/* CUSTOM-END*/

void main() {
    if (f_is_texture == 1u){
        vec4 c = texture(sampler2D(u_textures[0], u_samplers[0]), f_tex_coords);
        o_color = vec4(c.rgb*c.a, c.a) * f_color;
    } else {
        o_color = f_color;
    }
}