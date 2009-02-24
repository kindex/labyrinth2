//
[Vertex Shader]

attribute vec3 vertex_position;
attribute vec2 vertex_texcoord;
attribute vec4 vertex_tangent;
attribute vec3 vertex_normal;

varying vec2 fragment_texcoord;
varying vec3 fragment_tangent;
varying vec3 fragment_binormal;
varying vec3 fragment_normal;

void main()
{
    gl_Position = gl_ModelViewProjectionMatrix * vec4(vertex_position, 1.0);
    
    fragment_texcoord = vertex_texcoord;
    fragment_tangent = gl_NormalMatrix * vertex_tangent.xyz;
    fragment_normal = gl_NormalMatrix * vertex_normal;
    fragment_binormal = cross(fragment_tangent, fragment_normal) * vertex_tangent.w;
}

[Fragment Shader]

uniform sampler2D texture;
uniform sampler2D texture_nmap;

varying vec2 fragment_texcoord;
varying vec3 fragment_tangent;
varying vec3 fragment_binormal;
varying vec3 fragment_normal;

void main()
{
    vec3 bump = normalize(texture2D(texture_nmap, fragment_texcoord).xyz * 2.0 - 1.0);
    vec3 normal = normalize(fragment_tangent * bump.x + fragment_binormal * bump.y + fragment_normal * bump.z);

    gl_FragData[0] = texture2D(texture, fragment_texcoord);
    gl_FragData[1] = vec4(normal * 0.5 + 0.5, 0.0);
}
