Shader "DepthMask"
{
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry+500"}
        
        Lighting Off

        Pass
        {
            Cull Front
            ColorMask 0
        }
    }
}