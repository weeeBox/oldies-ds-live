float4x4 World;
float4x4 View;
float4x4 Projection;
float Timer : TIME;
float Amplitude;
float WaveLength;
float Omega;
float Top;
float Phase;
float4 Color : COLOR0;

// TODO: add effect parameters here.

struct VertexShaderInput
{
    float4 Position : POSITION0;

    // TODO: add input channels such as texture
    // coordinates and vertex colors here.
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;

    // TODO: add vertex shader outputs such as colors and texture
    // coordinates here. These values will automatically be interpolated
    // over the triangle, and provided as input to your pixel shader.
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;     
    
    float4 worldPosition = mul(input.Position, World);
    float phase = worldPosition.y == Top ? 0 : Phase;
      
    worldPosition.y += Amplitude * sin(2 * 3.1415 * worldPosition.x / WaveLength - Omega * Timer + phase);               
        
    float4 viewPosition = mul(worldPosition, View);    
    output.Position = mul(viewPosition, Projection);

    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
    return Color;
}

technique Technique1
{
    pass Pass1
    {
        // TODO: set renderstates here.

        VertexShader = compile vs_1_1 VertexShaderFunction();
        PixelShader = compile ps_1_1 PixelShaderFunction();
    }
}
