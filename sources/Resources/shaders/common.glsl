//

// All vectors are passed in world space
vec3 calcDirLight(in vec3 N, // normalized normal vector
                  in vec3 L, // normalized light direction
                  in vec3 E, // direction to eye
                  in vec3 light_color,
                  in vec3 light_params)
{
    float diffuse = max(0.0, dot(N, L));
    
    vec3 R = reflect(L, N);
    float specular = max(0.0, dot(R, E));

    float intensity = light_params.x * diffuse + light_params.y * pow(specular, light_params.z);

    return intensity * light_color;
}

// All vectors are passed in eye-space, so eye position = vec3(0,0,0)
vec3 calcPointLight(in vec3 N, // normalized normal vector
                    in vec3 L, // light position
                    in vec3 F, // fragment position
                    in float shadow,
                    in vec3 fragment_color,
                    in vec3 light_color,
                    in vec4 light_params,
                    in float light_radius2)
{
    vec3 lightVec = L - F;

    vec3 lightDir = normalize(lightVec);
    float diffuse = max(0.0, dot(N, lightDir));

    vec3 R = reflect(-lightDir, N);
    vec3 viewDir = normalize(-F);
    float specular = max(0.0, dot(R, viewDir));

    float att = max(0.0, 1.0 - dot(lightVec, lightVec) / light_radius2);

    return att * light_color * (light_params.x * fragment_color +
                                shadow * light_params.y * diffuse +
                                shadow * light_params.z * pow(specular, light_params.w));
}

// All vectors are passed in eye-space, so eye position = vec3(0,0,0)
vec3 calcSpotLight(in vec3 N, // normalized normal vector
                   in vec3 Lp, // light position
                   in vec3 Ld, // light direction
                   in vec3 F, // fragment position
                   in float shadow,
                   in vec3 fragment_color,
                   in vec3 light_color,
                   in vec4 light_params,
                   in vec4 light_params2) // radius^2, angleParam1, angleParam2, decayExponent
{
    vec3 lightVec = Lp - F;
    
    vec3 lightDir = normalize(lightVec);
    float diffuse = max(0.0, dot(N, lightDir));

    vec3 R = reflect(-lightDir, N);
    vec3 viewDir = normalize(-F);
    float specular = max(0.0, dot(R, viewDir));

    float spotEffect = dot(Ld, -lightDir);
    
    float att = max(0.0, 1.0 - dot(lightVec, lightVec) / light_params2.x)
              * pow( spotEffect * light_params2.y + light_params2.z, light_params2.w);

    return att * light_color * (light_params.x * fragment_color +
                                shadow * light_params.y * diffuse +
                                shadow * light_params.z * pow(specular, light_params.w));
}

[Vertex Shader]

[Fragment Shader]

vec3 getNormal(in sampler2D normal_texture, in vec2 texcoord)
{
    return normalize(texture2D(normal_texture, texcoord).xyz * 2.0 - 1.0);
}
