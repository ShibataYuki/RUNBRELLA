
Shader "Custom/PlayerOutLine"
{
        // インスペクタービューで設定できる値
           Properties
        {
            // 色
            _Color("Color", Color) = (1,1,1,0.5)
            // テクスチャ
            _MainTex("Texture", 2D) = "white" { }
            // スプライトの幅
            _Width("Sprite Width", Float) = 216.0
            // スプライトの高さ
            _Height("Sprite Height", Float) = 216.0
            // 線の幅
            _Thick("Line Thickness", Int) = 2
        }

        SubShader
        {
        Tags {"Queue"="Transparent" "IgnoreProjector"="true" "RenderType"="Transparent"}
        ZWrite Off Blend SrcAlpha OneMinusSrcAlpha Cull Off

        Pass
        {

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            # include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                half2 texcoord  : TEXCOORD0;
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
            };
            
            // 変数の宣言
            fixed4 _Color;
            sampler2D _MainTex;
            float _Width;
            float _Height;
            int _Thick;

            // 頂点シェーダ
            v2f vert(appdata IN)
            {                
                v2f OUT;
                // 2次元の座標に変換
                OUT.vertex = UnityObjectToClipPos(IN.vertex);
                // uv座標
                OUT.texcoord = IN.texcoord;
                // 頂点カラー
                OUT.color = IN.color;
                return OUT;
            }

            // フラグメントシェーダ
            float4 frag(v2f i) : COLOR
            {            
                // このピクセルの色にテクスチャの色をセット
                float4 thisPixelCol = tex2D(_MainTex, i.texcoord);
                float limitRange = _Thick * _Thick; 
                // このピクセルの周囲の透明度の最大値
                float alphaMax = 0.0f;
                // 周りのピクセルの透明度を指定された長さぶん確認する
                for (int x = -_Thick; x <= _Thick; ++x)
                {
                    for (int y = -_Thick; y <= _Thick; ++y)
                    {
                        // 1ピクセル当たりの幅
                        float onePixcelWidth = x / _Width;
                        // 1ピクセル当たりの高さ
                        float onePixcelHight = y / _Height;
                        
                        float alpha = tex2D(_MainTex, i.texcoord + float2(onePixcelWidth, onePixcelHight)).a;
                        
                        // 透明で制限範囲内のピクセルなら透明度を１にする
                        if (alpha > 0.4 && x* x + y* y <= limitRange) 
                        {
                            alphaMax = 1;              
                        }
                    }
                }
                // このピクセルが透明なら、指定された色を周囲の透明度の最大値で塗る。
                if (thisPixelCol.a <= 0.4)
                {
                    return float4(_Color.xyz, alphaMax);
                }
                // 透明でなければテクスチャの色を反映
                else
                {
                    return thisPixelCol;
                }
           
        }
        ENDCG
     }
    }
    Fallback "Sprites/Default"
}
