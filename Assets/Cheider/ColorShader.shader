Shader "Custom/ColorShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
    }

    SubShader
    {
        Tags{"RenderType" = "Transparent" "Queue" = "Transparent"}

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };
            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 worldPos : TEXTCOORD0;
                float2 uv : TEXCOORD0;
            };
            float4 _Color;
            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.uv = v.uv;
                return o;
            }
            fixed4 frag(v2f i) : SV_TARGET
            {
                //return _Color;
                return float4(i.worldPos.x, i.worldPos.y, i.worldPos.z, 1.0);
            }
            ENDCG
        }
    }
}
