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

uniform sampler2D texture;
varying vec2 fragment_texcoord;

void main()
{
    gl_FragColor = texture2D(texture, fragment_texcoord);
}
