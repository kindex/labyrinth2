//
[Vertex Shader]

attribute vec3 vertex_position;

void main()
{
    gl_Position = gl_ModelViewProjectionMatrix * vec4(vertex_position, 1.0);
}

[Fragment Shader]

uniform vec3 color;

void main()
{
    gl_FragColor.rgb = color;
}
