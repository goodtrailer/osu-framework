#include "sh_Utils.h"

varying lowp vec4 v_Colour;

uniform lowp float g_ColourArray[3];

void main(void)
{
    lowp vec4 colourArray = vec4(g_ColourArray[0], g_ColourArray[1], g_ColourArray[2], 1.0);
    gl_FragColor = toSRGB(v_Colour * colourArray);
}
