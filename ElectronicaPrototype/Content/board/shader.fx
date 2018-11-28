float4x4 World;
float4x4 View;
float4x4 Projection;

texture ModelTexture;

sampler2D textureSampler = sampler_state {
	Texture = (ModelTexture);
	MagFilter = Linear;
	MinFilter = Linear;
	AddressU = Wrap;
	AddressV = Wrap;
};

texture BrightnessMap;

sampler2D brightnessSampler = sampler_state {
	Texture = (BrightnessMap);
	MagFilter = Linear;
	MinFilter = Linear;
	AddressU = Clamp;
	AddressV = Clamp;
};

struct VertexShaderInput
{
	float4 Position : POSITION0;
	float2 TextureCoordinate : TEXCOORD0;
	float2 Brightness : TEXCOORD1;
};

struct VertexShaderOutput
{
	float4 Position : POSITION0;
	float2 TextureCoordinate : TEXCOORD0;
	float2 Brightness : TEXCOORD1;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
	VertexShaderOutput output;

	float4 worldPosition = mul(input.Position, World);
	float4 viewPosition = mul(worldPosition, View);
	output.Position = mul(viewPosition, Projection);

	output.TextureCoordinate = input.TextureCoordinate;
	output.Brightness = input.Brightness;

	return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	float4 textureColor = tex2D(textureSampler, input.TextureCoordinate);
	textureColor.a = 1;

	float4 brightness = tex2D(brightnessSampler, input.Brightness);

	return textureColor * brightness;
}

technique Basic
{
	pass Pass1
	{
		VertexShader = compile vs_2_0 VertexShaderFunction();
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}