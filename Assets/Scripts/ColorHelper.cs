using UnityEngine;

namespace ColorMixer {

    public static class ColorHelper
    {
        public static Color ColorLerp(Transform[] points) {
            Color colorLerp = default(Color);
            for (int i = 0; i < points.Length; i++)
            {
                if (i == 0) {
                    colorLerp = Color.Lerp(
                        points[1].gameObject.GetComponent<Renderer>().material.color, 
                        points[2].gameObject.GetComponent<Renderer>().material.color, 
                        0.5f);
                }
                if (i >= 3) {
                    colorLerp = Color.Lerp(
                        colorLerp,
                        points[i].gameObject.GetComponent<Renderer>().material.color,
                        0.5f);
                }
            }
            return colorLerp;
        }

        public static int CompareColors(Color color1, Color color2)
        {
            float pR = Match(color1.r, color2.r);
            float pG = Match(color1.g, color2.g);
            float pB = Match(color1.b, color2.b);

            float average = (pR + pG + pB)/3;

            return (int)average;
        }

        public static float Match(float int1, float int2) {
            float match = 0f;
            if (int1 > int2) {
                match = int2/int1*100;
            } else {
                match = int1/int2*100;
            }
            return match;
        }
    }
}
