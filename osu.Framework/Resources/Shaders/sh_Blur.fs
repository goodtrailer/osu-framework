#include "sh_Utils.h"

#define INV_SQRT_2PI 0.39894
#define MAX_KERNEL_RADIUS 200

varying mediump vec2 v_TexCoord;

uniform lowp sampler2D m_Sampler;

uniform highp vec2 g_BlurDirection;
uniform int g_GaussianRadiiCount;
uniform mediump float g_GaussianRadii[MAX_KERNEL_RADIUS / 2];
uniform mediump float g_GaussianFactors[MAX_KERNEL_RADIUS / 2];
uniform mediump float g_GaussianInverseIntegral;

void main(void)
{
	mediump vec4 sum = vec4(0.0);

	for (int i = 0; i < g_GaussianRadiiCount; i++)
	{
		mediump vec2 radius = g_GaussianRadii[i] * g_BlurDirection;
		sum += texture2D(m_Sampler, v_TexCoord + radius) * g_GaussianFactors[i];
		sum += texture2D(m_Sampler, v_TexCoord - radius) * g_GaussianFactors[i];
	}

	gl_FragColor = toSRGB(sum * g_GaussianInverseIntegral);
}
