//
[Vertex Shader]

attribute vec3 vertex_position;

void main()
{
    gl_Position = gl_ModelViewProjectionMatrix * vec4(vertex_position, 1.0);
}

[Fragment Shader]

#extension GL_EXT_gpu_shader4: require

uniform sampler2D color_texture;
uniform sampler2D normal_texture;
uniform sampler2D depth_texture;

//uniform samplerCubeShadow shadow_texture;
uniform samplerCube shadow_texture;

//uniform sampler2D shadowFront_texture;
//uniform sampler2D shadowBack_texture;

//uniform sampler2DShadow shadowFront_texture;
//uniform sampler2DShadow shadowBack_texture;

uniform vec2 buffer_range;

uniform vec3 light_position;
uniform vec3 light_positionW;
uniform vec3 light_color;
uniform vec4 light_params;
uniform float light_radius2;

uniform mat4 ndc_to_view;
uniform mat4 shadow_matrix;
uniform float depth_param1;
uniform float depth_param2;

void main()
{
    vec2 texcoord = gl_FragCoord.xy * buffer_range;
    
    vec3 L = light_position;
    vec3 N = getNormal(normal_texture, texcoord);

    vec4 wPos = ndc_to_view * vec4(texcoord, texture2D(depth_texture, texcoord).x, 1.0);
    vec3 F = wPos.xyz / wPos.w;
    
    vec3 fragment_color = texture2D(color_texture, texcoord).rgb;

    vec4 pos = shadow_matrix * vec4(F, 1.0);
    
    /*
    float len = length(p.xyz);
    p.xyz /= len;

    float depth;
    if (p.z >= 0.0)
    {
        vec2 p0 = p.xy / (1.0 + p.z);
        depth = texture2D(shadowFront_texture, p0 * 0.5 + 0.5).r;
    }
    else
    { 
        vec2 p1 = p.xy / (1.0 - p.z);
        depth = texture2D(shadowBack_texture, p1 * 0.5 + 0.5).r;
    }

	float mydepth = ((len - zNear) / (zFar - zNear)) * 0.5 + 0.5;
	*/
	
	vec3 dir = light_positionW - pos.xyz;
	float z = max(max(abs(dir.x), abs(dir.y)), abs(dir.z));
	float mydepth = (1.0 / z) * depth_param1 + depth_param2;
    
	//float shadow = shadowCube(shadow_texture, vec4(dir, mydepth)).r;
	float depth = textureCube(shadow_texture, dir).r;
    float shadow = depth <= mydepth ? 0.0 : 1.0;
    
    gl_FragColor.rgb = calcPointLight(N, L, F, shadow, fragment_color, light_color, light_params, light_radius2);
}
