#version 450 core

// vertex
layout (location = 0) in vec2 v_pos;
layout (location = 1) in vec2 v_tex_coords;
layout (location = 2) in vec4 v_color;

// instance
layout (location = 5) in mat3 i_model;
layout (location = 8) in vec4 i_color;
layout (location = 9) in vec2 i_uv_scale;
layout (location = 10) in vec2 i_uv_offset;
layout (location = 11) in uint i_tsi[15];

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
layout (location = 2) flat out uint f_tsi[15];

mat3 buildProjectionMatrix(vec2 res) {
    return mat3(
    2.0 / res.x, 0.0, 0.0,
    0.0, 2.0 / res.y, 0.0,
    -1.0, -1.0, 1.0);
}

void main() {
    //mat3 model = buildModelMatrix(i_translation, i_rotation, i_scale, i_origin, i_shear);
    mat3 proj = buildProjectionMatrix(u_res);

    //    vec3 t1 = model * vec3(v_pos, 1);
    //    vec3 t2 = u_view * vec3(t1.xy, 1);
    //    vec3 t3 = proj * vec3(t2.xy, 1);

    vec3 pos = proj * u_view * i_model * vec3(v_pos, 1);

    gl_Position = vec4(pos.xy, 0, 1);

    f_color = v_color * i_color * u_color;
    f_tex_coords = v_tex_coords * i_uv_scale + i_uv_offset;
    f_tsi = i_tsi;
}
