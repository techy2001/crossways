using System;
using UnityEngine;

public class GameMath {
	public static float distanceTo(Vector3 pos1, Vector3 pos2) {
		float x = pos1.x - pos2.x;
		float y = pos1.y - pos2.y;
		float z = pos1.z - pos2.z;
		return (float) Math.Sqrt(x * x + y * y + z * z);
	}
}