using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Math
{
    /// <summary>
    /// Gets rotation from four vectors 
    /// </summary>
    /// <param name="a">Front left vector.</param>
    /// <param name="b">Front right vector.</param>
    /// <param name="c">Back left vector.</param>
    /// <param name="c">Back right vector.</param>
    public static Vector3 GetRot(Vector3 one, Vector3 two, Vector3 three, Vector3 four)
    {
        //get z axis rotation of 4 vectors
        float distanceBetweenFeet1 = Vector3.Distance(one, two);
        float distanceBetweenFeet2 = Vector3.Distance(three, four);
        float differenceInHeight1 = one.y - two.y;
        float differenceInHeight2 = three.y - four.y;

        float legAngle1 = Mathf.Atan2(differenceInHeight1, distanceBetweenFeet1) * 180 / Mathf.PI * -1;
        float legAngle2 = Mathf.Atan2(differenceInHeight2, distanceBetweenFeet2) * 180 / Mathf.PI * -1;

        float averagedLegAngle1 = (legAngle1 + legAngle2) / 2;

        //get x axis rotation of 4 vectors
        float distanceBetweenFeet3 = Vector3.Distance(one, three);
        float distanceBetweenFeet4 = Vector3.Distance(two, four);
        float differenceInHeight3 = one.y - three.y;
        float differenceInHeight4 = two.y - four.y;

        float legAngle3 = (Mathf.Atan2(differenceInHeight3, distanceBetweenFeet3) * 180 / Mathf.PI);
        float legAngle4 = (Mathf.Atan2(differenceInHeight4, distanceBetweenFeet4) * 180 / Mathf.PI);

        float averagedLegAngle2 = (legAngle3 + legAngle4) / 2;
        return new Vector3(averagedLegAngle2, 0, averagedLegAngle1);
    }

    public static float GetVerticalAngle(Vector3 one, Vector3 two)
    {
        float distanceBetweenPoints = Vector3.Distance(one, two);
        float differenceInHeight = one.y - two.y;
        float angle = (Mathf.Atan2(differenceInHeight, distanceBetweenPoints) * 180 / Mathf.PI)  * 4 + 180;
        return angle;
    }

    public static float GetHorizontalAngle(Vector3 one, Vector3 two)
    {
        float distanceBetweenPoints = Vector3.Distance(one, two);
        float differenceInHeight = one.x - two.x;
        float angle = (Mathf.Atan2(differenceInHeight, distanceBetweenPoints) * 180 / Mathf.PI) * 4 + 180;
        return angle;
    }
}
