﻿using UnityEngine;
using System.Collections;

public class AsteroidPart2 : GlobalAsteriod {


	void Start () {
		rigidbody2D.velocity = speed * (new Vector2 (0.7f, 0.7f));
	}
}
