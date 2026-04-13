#version 450 core

// vertex
layout (location = 0) in vec2 v_pos;
layout (location = 1) in vec2 v_tex_coords;
layout (location = 2) in vec4 v_color;

// instance
layout (location = 3) in vec2 i_model_c0;
layout (location = 4) in vec2 i_model_c1;
layout (location = 5) in vec2 i_model_c2;
layout (location = 6) in vec4 i_color;
layout (location = 7) in vec2 i_uv_offset;
layout (location = 8) in vec2 i_uv_scale;
layout (location = 9) in float i_layer;
layout (location = 10) in uint i_tsi;

// uniform
layout (std140, set = 0, binding = 0) uniform UniformBlock {
    mat3 u_view;
    vec4 u_color;
    vec2 u_res;
    vec2 u_mouse;
    float u_time;
};

// output
layout (location = 0) out vec2 f_tex_coords;
layout (location = 1) out vec4 f_color;
layout (location = 2) flat out uint f_tsi;

mat3x3 buildProjectionMatrix(vec2 res) {
    return mat3x3(2.0 / res.x, 0.0, 0.0, 0.0, 2.0 / res.y, 0.0, -1.0, -1.0, 1.0);
}

void main() {
    mat3x3 proj = buildProjectionMatrix(u_res);
    mat3x3 model = mat3x3(vec3(i_model_c0, 0), vec3(i_model_c1, 0), vec3(i_model_c2, 1));
    vec3 pos = proj * u_view * model * vec3(v_pos, 1);
    gl_Position = vec4(pos.xy, 0, 1);

    f_color = v_color * i_color * u_color;
    f_tex_coords = v_tex_coords * i_uv_scale + i_uv_offset;
    f_tsi = i_tsi;
}
