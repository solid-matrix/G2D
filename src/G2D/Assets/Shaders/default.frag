#version 450 core
#extension GL_EXT_nonuniform_qualifier: enable

// input
layout (location = 0) in vec2 f_tex_coords;
layout (location = 1) in vec4 f_color;
layout (location = 2) flat in uint f_tsi[15];

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
layout (set = 1, binding = 0) uniform texture2D u_textures[];
layout (set = 2, binding = 0) uniform sampler u_samplers[8];


void main() {
    uint tid = f_tsi[0] >> 16;
    uint sid = f_tsi[0] & 0xFFFFU;
    vec4 c = texture(sampler2D(u_textures[tid], u_samplers[sid]), f_tex_coords);
    o_color = vec4(c.rgb * c.a, c.a) * f_color;
}