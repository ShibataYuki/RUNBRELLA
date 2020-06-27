
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
            _Thick("Line Thickness", Int) = 8
            //// 以下の設定がないとworningが出るため記述
            _StencilComp("Stencil Comparison", Float) = 8.000000
            _Stencil("Stencil_ID", Float) = 0
            _StencilOp("Stencil Operation", Float) = 0
            _StencilWriteMask("Stencil Write Mask", Float) = 255.000000
            _StencilReadMask("Stencil Read Mask", Float) = 255.000000
            _ColorMask("Color Mask", Float) = 15.000000
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
float4 frag(v2f IN) : COLOR
            {            
                // テクスチャの色
                fixed4 textureColor = tex2D(_MainTex, IN.texcoord);
                // ポリゴンの色
                fixed4 rendererColor = IN.color;
                // マテリアルの色
                fixed4 materialColor = _Color;
                // チェック距離の制限
                half limitRange = _Thick * _Thick;
                // このピクセルを縁取りとして塗っていいかのフラグ：1ならtrue 0ならfalse
                fixed isPaintable = 0;
                // このピクセルが縁取りとして塗っていいピクセルか調べる
                for (int x = -_Thick; x <= _Thick; ++x) // そのピクセルから左に指定の長さ分右に指定の長さ分
                {
                    for (int y = -_Thick; y <= _Thick; ++y) // そのピクセルから下に指定の長さ分上に指定の長さ分
                    {
                        // 1ピクセル当たりの幅
                        float onePixcelWidth = x / _Width;
                        // 1ピクセル当たりの高さ
                        float onePixcelHight = y / _Height;
                        // チェックするピクセルに対応した位置のテクスチャのα値を調べる
                        fixed texAlpha = tex2D(_MainTex, IN.texcoord + float2(onePixcelWidth, onePixcelHight)).a;
                        // 今のピクセルからチェックするピクセルまでの距離
                        half checkRange = x * x + y * y;
                        // チェックした範囲にテクスチャが透明でない部分がある
                        // (このピクセルが縁取りする絵から離れすぎていない)かつ
                        if (texAlpha > 0.4) 
                        {
                            // 距離の制限をクリアしている
                            if(checkRange  <= limitRange)
                            {
                                // 縁取りしていいフラグON
                                isPaintable = 1;  
                            }                                        
                        }                  
    
                    }// 2重ループの内側end
                }// 2重ループの外側end

                // このピクセルが透明かつ塗っていい範囲内のピクセルなら
                if (textureColor.a <= 0.4 && isPaintable == 1)
                {                      
                    // 縁取りする色を反映
                                  // rgb -> マテリアルの色　α値-> マテリアルの色*スプライトの色
                    return fixed4(materialColor.rgb, materialColor.a* rendererColor.a);
                }
                // そうでなければテクスチャの色を反映
                else
                {
                           // テクスチャの色　* Image や Sprite の色
                    return textureColor* rendererColor;
                }           
        }
        ENDCG
     }
    }
    Fallback "Sprites/Default"
}
