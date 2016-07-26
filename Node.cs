using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Node {

	public bool walkable;
	//public bool reserved;
	public Vector3 worldPos;
	public int gridX;
	public int gridY;

	public int gCost;
	public int hCost;
	public Node parent;

	public Node(bool _walkable,/* bool _reserved,*/ Vector3 _worldPos, int _gridX, int _gridY) {
		walkable = _walkable;
		//reserved = _reserved;
		//reserved = false;
		worldPos = _worldPos;
		gridX = _gridX;
		gridY = _gridY;
	}

	public int fCost {
		get {
			return gCost + hCost;
		}
	}
}