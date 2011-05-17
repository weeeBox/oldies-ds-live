float4 colorAdd = {0, 0, 0, 0};
float4 colorMul = {1, 1, 1, 1};

sampler TextureSampler : register(s0);


float4 main(float4 color : COLOR0, float2 texCoord : TEXCOORD0) : COLOR0
{
    // Look up the texture color.
    float4 tex = tex2D(TextureSampler, texCoord);    
	
	tex.a = max(0, min(tex.a * colorMul.a + colorAdd.a, 1.0));
	tex.r = max(0, min(tex.r * colorMul.r + colorAdd.r, 1.0)) * tex.a;
	tex.g = max(0, min(tex.g * colorMul.g + colorAdd.g, 1.0)) * tex.a;
	tex.b = max(0, min(tex.b * colorMul.b + colorAdd.b, 1.0)) * tex.a;	
	
    return tex;
}


technique Colorize
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 main();
    }
}