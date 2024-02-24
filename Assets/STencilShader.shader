Shader "Custom/STencilShader"
{
    SubShader
    {
        Tags { "Queue"="Transparent+1" }
        
        Pass
        {
            Cull Front
            Blend Zero One
        }
    }
}
