//
[Vertex Shader]

attribute vec3 vertex_position;

void main()
{
    gl_Position = gl_ModelViewProjectionMatrix * vec4(vertex_position, 1.0);
}

[Fragment Shader]

void main()
{
}
