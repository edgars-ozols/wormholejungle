﻿using UnityEngine;
using System.Collections;

public static class Helpers {

	public static Vector3 ToVector3(this Vector2 v)
	{
		return new Vector3 (v.x, v.y);
	}
}
