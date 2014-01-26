using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MathHelper
{
	// taken from http://wiki.unity3d.com/index.php/3d_Math_functions
	//Two non-parallel lines which may or may not touch each other have a point on each line which are closest
	//to each other. This function finds those two points. If the lines are not parallel, the function 
	//outputs true, otherwise false.
	public static bool ClosestPointsOnTwoLines(out Vector3 closestPointLine1, out Vector3 closestPointLine2, Vector3 linePoint1, Vector3 lineVec1, Vector3 linePoint2, Vector3 lineVec2){
		
		closestPointLine1 = Vector3.zero;
		closestPointLine2 = Vector3.zero;
		
		float a = Vector3.Dot(lineVec1, lineVec1);
		float b = Vector3.Dot(lineVec1, lineVec2);
		float e = Vector3.Dot(lineVec2, lineVec2);
		
		float d = a * e - b * b;
		
		//lines are not parallel
		if(d != 0.0f)
		{	
			Vector3 r = linePoint1 - linePoint2;
			float c = Vector3.Dot(lineVec1, r);
			float f = Vector3.Dot(lineVec2, r);
			
			float s = (b * f - c * e) / d;
			float t = (a * f - c * b) / d;
			
			closestPointLine1 = linePoint1 + lineVec1 * s;
			closestPointLine2 = linePoint2 + lineVec2 * t;
			
			return true;
		}
		else
		{
			return false;
		}
	}
	
	//Calculate the intersection point of two lines. Returns true if lines intersect, otherwise false.
	//Note that in 3d, two lines do not intersect most of the time. So if the two lines are not in the 
	//same plane, use ClosestPointsOnTwoLines() instead.
	public static bool LineLineIntersection(out Vector3 intersection, Vector3 linePoint1, Vector3 lineVec1, Vector3 linePoint2, Vector3 lineVec2){
		
		intersection = Vector3.zero;
		
		Vector3 lineVec3 = linePoint2 - linePoint1;
		Vector3 crossVec1and2 = Vector3.Cross(lineVec1, lineVec2);
		Vector3 crossVec3and2 = Vector3.Cross(lineVec3, lineVec2);
		
		float planarFactor = Vector3.Dot(lineVec3, crossVec1and2);
		
		//Lines are not coplanar. Take into account rounding errors.
		if((planarFactor >= 0.00001f) || (planarFactor <= -0.00001f)){
			
			return false;
		}
		
		//Note: sqrMagnitude does x*x+y*y+z*z on the input vector.
		float s = Vector3.Dot(crossVec3and2, crossVec1and2) / crossVec1and2.sqrMagnitude;
		
		if((s >= 0.0f) && (s <= 1.0f)){
			
			intersection = linePoint1 + (lineVec1 * s);
			return true;
		}
		
		else{
			return false;       
		}
	}
}
