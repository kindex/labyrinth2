//
[Vertex Shader]

attribute vec3 vertex_position;

//uniform float direction;
//varying float alpha;

void main()
{
    gl_Position = gl_ModelViewProjectionMatrix * vec4(vertex_position, 1.0);
    
    //vec4 pos = gl_ModelViewMatrix * vec4(vertex_position, 1.0);
    //pos.z *= direction;
    
	//float len = length(pos.xyz);
	//pos.xyz /= len;

    //alpha = pos.z;
	
	//pos.xy /= 1.0 + pos.z;
	//pos.z = (len - zNear) / (zFar - zNear) - 0.0005;
	//pos.w = 1.0;

	//gl_Position = pos;
}

[Fragment Shader]

//varying float alpha;

void main()
{
    
    //if (alpha < 0.0)
    //{
    //    discard;
    //}
}
