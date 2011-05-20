float4x4 World;
float4x4 View;
float4x4 Projection;

float TextureSize;
float Radius;

struct VertexShaderInput
{
    float4 Position : POSITION0;
	float4 Cord : TEXCOORD0;
	float4 Color : COLOR0;
    // TODO: add input channels such as texture
    // coordinates and vertex colors here.
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
	float4 Cord : TEXCOORD0;
	float4 Color : COLOR0;
    // TODO: add vertex shader outputs such as colors and texture
    // coordinates here. These values will automatically be interpolated
    // over the triangle, and provided as input to your pixel shader.
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;

    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);
	output.Cord = input.Cord;
	output.Color = input.Color;

    // TODO: add your vertex shader code here.

    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{   	
	float R = Radius / TextureSize;
	float R2 = 4*R*R;

	float4 color = input.Color;

	float2 tex0 = input.Cord.xy;
	float2 UV = (tex0-0.5)*2;  
	float dotUV = dot(UV,UV);
	float dist2 = R2 - dotUV;
	clip(dist2);

	float dist = R2 - sqrt(dotUV);
	float alpha = saturate(0.5 * dist*TextureSize);
	color *= alpha;
	return color;	
}

technique Technique1
{
    pass Pass1
    {
        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
