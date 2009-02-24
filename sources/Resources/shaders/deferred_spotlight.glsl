//
[Vertex Shader]

attribute vec3 vertex_position;

void main()
{
    gl_Position = gl_ModelViewProjectionMatrix * vec4(vertex_position, 1.0);
}

[Fragment Shader]

uniform sampler2D color_texture;
uniform sampler2D normal_texture;
uniform sampler2D depth_texture;
uniform sampler2DShadow shadow_texture;

uniform vec2 buffer_range;

uniform vec3 light_position;
uniform vec3 light_direction;
uniform vec3 light_color;
uniform vec4 light_params;
uniform vec4 light_params2;

uniform mat4 ndc_to_view;
uniform mat4 shadow_matrix;

void main()
{
    vec2 texcoord = gl_FragCoord.xy * buffer_range;
    
    vec3 Lp = light_position;
    vec3 Ld = light_direction;
    vec3 N = getNormal(normal_texture, texcoord);

    vec4 wPos = ndc_to_view * vec4(texcoord, texture2D(depth_texture, texcoord).x, 1.0);
    vec3 F = wPos.xyz / wPos.w;
        
    vec3 fragment_color = texture2D(color_texture, texcoord).rgb;
    
    vec4 shadow_coord = shadow_matrix * vec4(F, 1.0);
    float shadow = shadow2DProj(shadow_texture, shadow_coord).r;
    
    gl_FragColor.rgb = calcSpotLight(N, Lp, Ld, F, shadow, fragment_color, light_color, light_params, light_params2);
}
