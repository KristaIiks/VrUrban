Shader "Custom/StencilShader"
{
    SubShader
    {
        Tags { "Queue"="Transparent-1" }
        
        Pass
        {
            Cull Front
            Blend Zero One
        }
    }
}
