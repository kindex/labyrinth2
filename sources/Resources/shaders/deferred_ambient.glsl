//
[Vertex Shader]

attribute vec2 vertex_position;

varying vec2 fragment_texcoord;

void main()
{
    gl_Position = vec4(vertex_position, 0.0, 1.0);
    fragment_texcoord = (vec2(1.0, 1.0) + vertex_position) * 0.5;
}

[Fragment Shader]

varying vec2 fragment_texcoord;

uniform sampler2D color_texture;

uniform float ambient_light;

void main()
{
    vec3 fragment_color = texture2D(color_texture, fragment_texcoord).rgb;
    
    gl_FragColor.rgb = fragment_color * ambient_light;
}
