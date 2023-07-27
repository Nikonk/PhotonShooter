Shader "Custom/Healthbar"
{
    Properties
    {
        _Health ("Health", Range(0, 1)) = 1.0
        _LowHealthColor ("Low Health Color", Color) = (1, 0, 0, 1)
        _HighHealthColor ("High Health Color", Color) = (0, 1, 0, 1)
        _LowHealthThreshold ("Low Health Threshold", Range(0, 1)) = 0.2
        _HighHealthThreshold ("High Health Threshold", Range(0, 1)) = 0.8
        [NoScaleOffset] _MainTex ("Texture", 2D) = "white" {}
        [MaterialToggle] _PulseIfLow ("Pulse If Low", Float) = 1.0
        _Ratio ("Ratio", Float) = 8.0
        _BorderColor("Border Color", Color) = (0, 0, 0, 1)
        _BorderThickness ("Border Thickness", Range(0, 0.5)) = 0.2
    }
    SubShader
    {
        Tags {
            "RenderType" = "Opaque"
        }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct meshdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct interpolators
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            float _Health;
            float4 _LowHealthColor;
            float4 _HighHealthColor;
            float _LowHealthThreshold;
            float _HighHealthThreshold;
            sampler2D _MainTex;
            float _PulseIfLow;
            float _Ratio;
            float4 _BorderColor;
            float _BorderThickness;

            interpolators vert (const meshdata v)
            {
                interpolators o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float InverseLerp(const float a, const float b, const float v)
            {
                return ( v - a ) / ( b - a );
            }

            float4 frag (interpolators i) : SV_Target
            {
                float2 coords = i.uv;
                coords.x *= _Ratio;

                float2 pointOnLineSegment = float2( clamp( coords.x, 0.5, _Ratio - 0.5 ), 0.5 );
                float sdf = distance( pointOnLineSegment, coords ) * 2 - 1;
                clip( -sdf );

                float borderSdf = sdf + _BorderThickness;
                float pd = fwidth(borderSdf); // screen space partial derivative of borderSdf
                float borderMask = 1 - saturate( borderSdf / pd );

                float fillMask = i.uv.x < _Health;

                float3 texCol = tex2D( _MainTex, float2( _Health, i.uv.y ) );
                float tFillColor = saturate( InverseLerp ( _LowHealthThreshold, _HighHealthThreshold, _Health ) );
                float3 col = lerp( _LowHealthColor, _HighHealthColor, tFillColor );
                col *= texCol;

                if ( ( _PulseIfLow == 1 ) && ( _Health <= 0.2 ) ) {
                    float flash = cos( _Time.y * 4 ) * 0.3;
                    col = saturate( col * ( 1 + flash ) );
                }

                return float4( lerp( _BorderColor, col * fillMask, borderMask ), 1 );
            }
            ENDCG
        }
    }
}
