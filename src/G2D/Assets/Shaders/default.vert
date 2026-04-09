#version 450 core

// vertex
layout (location = 0) in vec2 v_pos;
layout (location = 1) in vec2 v_tex_coords;
layout (location = 2) in vec4 v_color;

// instance
layout (location = 3) in vec2 i_translation;
layout (location = 4) in float i_rotation;
layout (location = 5) in vec2 i_scale;
layout (location = 6) in vec2 i_origin;
layout (location = 7) in vec2 i_shear;
layout (location = 8) in vec4 i_color;
layout (location = 9) in vec2 i_uv_scale;
layout (location = 10) in vec2 i_uv_offset;
layout (location = 11) in uint i_is_texture;
layout (location = 12) in uint i_tsi[14];

// uniform
layout (set = 0, binding = 0) uniform UniformBlock {
    mat4 u_view;
    vec4 u_color;
    vec2 u_res;
    vec2 u_mouse;
    float u_time;
};

// output
layout (location = 0) out vec2 f_tex_coords;
layout (location = 1) out vec4 f_color;
layout (location = 2) flat out uint f_is_texture;
layout (location = 3) flat out uint f_tsi[14];

mat3 buildModelMatrix(vec2 translation, float rotation, vec2 scale, vec2 origin, vec2 shear) {
    mat3 pivotMat = mat3(1.0, 0.0, 0.0, 0.0, 1.0, 0.0, -origin.x, -origin.y, 1.0);
    mat3 scaleMat = mat3(scale.x, 0.0, 0.0, 0.0, scale.y, 0.0, 0.0, 0.0, 1.0);
    mat3 shearMat = mat3(1.0, shear.y, 0.0, shear.x, 1.0, 0.0, 0.0, 0.0, 1.0);
    mat3 rotMat = mat3(cos(rotation), sin(rotation), 0.0, -sin(rotation), cos(rotation), 0.0, 0.0, 0.0, 1.0);
    mat3 transMat = mat3(1.0, 0.0, 0.0, 0.0, 1.0, 0.0, translation.x, translation.y, 1.0);
    return transMat * rotMat * shearMat * scaleMat * pivotMat;
}

mat4 buildProjectionMatrix(vec2 res) {
    return mat4(
    2.0 / res.x, 0.0, 0.0, 0.0,
    0.0, -2.0 / res.y, 0.0, 0.0,
    0.0, 0.0, 1.0, 0.0,
    -1.0, 1.0, 0.0, 1.0);
}

/* CUSTOM-BEGIN */
vec4 position(vec2 model_pos, mat4 view, mat4 proj) {
    return proj * view * vec4(model_pos, 0.0, 1.0);
}
/* CUSTOM-END*/

void main() {
    mat3 model = buildModelMatrix(i_translation, i_rotation, i_scale, i_origin, i_shear);
    vec3 m_pos = model * vec3(v_pos, 1.0f);
    mat4 proj = buildProjectionMatrix(u_res);
    gl_Position = position(m_pos.xy, u_view, proj);
    f_color = v_color * i_color * u_color;
    f_tex_coords = v_tex_coords;// TODO, adjust by i_un_offset and  i_uv_offset
    f_is_texture = i_is_texture;
    f_tsi = i_tsi;
}
