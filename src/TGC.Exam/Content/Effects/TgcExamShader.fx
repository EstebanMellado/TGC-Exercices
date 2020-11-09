#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

static const int kernelRadius = 5;
static const int kernelSize = 25;
static const float kernel[kernelSize] =
{
    0.003765, 0.015019, 0.023792, 0.015019, 0.003765,
    0.015019, 0.059912, 0.094907, 0.059912, 0.015019,
    0.023792, 0.094907, 0.150342, 0.094907, 0.023792,
    0.015019, 0.059912, 0.094907, 0.059912, 0.015019,
    0.003765, 0.015019, 0.023792, 0.015019, 0.003765,
};

float4x4 World;
float4x4 View;
float4x4 Projection;
float4x4 WorldViewProjection;
float4x4 InverseTransposeWorld;

float3 CameraPosition;

float3 LightOnePosition;
float3 LightTwoPosition;
float3 LightOneColor;
float3 LightTwoColor;

struct VertexShaderInput
{
	float4 Position : POSITION0;
	float4 Color : COLOR0;
    float2 TextureCoordinate : TEXCOORD0;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
    float4 Color : COLOR0;
    float2 TextureCoordinate : TEXCOORD1;
};

texture ModelTexture;
sampler2D textureSampler = sampler_state
{
    Texture = (ModelTexture);
    MagFilter = Linear;
    MinFilter = Linear;
    AddressU = Clamp;
    AddressV = Clamp;
};

float Time = 0;

VertexShaderOutput MainVS(in VertexShaderInput input)
{
	VertexShaderOutput output = (VertexShaderOutput)0;

	// Project position
    output.Position = mul(input.Position, WorldViewProjection);

	// Propagate texture coordinates
    output.TextureCoordinate = input.TextureCoordinate;

	// Propagate color by vertex
    output.Color = input.Color;

    return output;
}

float4 MainPS(VertexShaderOutput input) : COLOR
{
    return tex2D(textureSampler, input.TextureCoordinate);
}





struct PostProcessingVertexShaderInput
{
    float4 Position : POSITION0;
    float2 TextureCoordinate : TEXCOORD0;
};

struct PostProcessingVertexShaderOutput
{
    float4 Position : SV_POSITION;
    float2 TextureCoordinate : TEXCOORD1;
};



PostProcessingVertexShaderOutput PostProcessVS(in PostProcessingVertexShaderInput input)
{
    PostProcessingVertexShaderOutput output = (PostProcessingVertexShaderOutput) 0;

	// Propagate position
    output.Position = input.Position;

	// Propagate texture coordinates
    output.TextureCoordinate = input.TextureCoordinate;

    return output;
}



float4 PostProcessPS(PostProcessingVertexShaderOutput input) : COLOR
{
    return tex2D(textureSampler, input.TextureCoordinate);
}






technique BasicShader
{
	pass P0
	{
		VertexShader = compile VS_SHADERMODEL MainVS();
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};


technique PostProcessing
{
    pass P0
    {
        VertexShader = compile VS_SHADERMODEL PostProcessVS();
        PixelShader = compile PS_SHADERMODEL PostProcessPS();
    }
}








