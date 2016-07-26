using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System;

public class Pathfinding : MonoBehaviour {

	public Grid grid;
	public GameObject gridObject;
	Node startNode;
	Node targetNode;
	//Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
	//PathRequest currentPathRequest;

	static Pathfinding instance;
	Pathfinding pathfinding;

	bool isProcessingPath;

	void Awake(){		
		
		gridObject = GameObject.FindGameObjectWithTag("Grid");
		grid = gridObject.GetComponent<Grid>();
		instance = this;
		pathfinding = GetComponent<Pathfinding>();
	}

	public void StartFindPath(Vector3 startPos, Vector3 targetPos){
		FindPath(startPos, targetPos);
	}

	public void FindPath(Vector3 startPos, Vector3 targetPos) { 

		//Stopwatch sw = new Stopwatch();
		//sw.Start();

		Vector3[] waypoints = new Vector3[0];
		bool pathSuccess = false;

		startNode = grid.NodeAtWorldPos(startPos);
		targetNode = grid.NodeAtWorldPos(targetPos);

		if(startNode.walkable && targetNode.walkable){

			List<Node> openSet = new List<Node>();
			List<Node> closedSet = new List<Node>();
			openSet.Add(startNode);

			while(openSet.Count > 0) {
				Node currentNode = openSet[0];//each iteration searches through entire open set - not efficient - change to heap to improve efficiency
				for (int i = 1; i < openSet.Count; i ++) {
					if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost) {
						currentNode = openSet[i];
					}
				}

				openSet.Remove(currentNode);
				closedSet.Add(currentNode);

				if(currentNode == targetNode){					
					//sw.Stop();
					//print ("Path Found" + sw.ElapsedMilliseconds + "ms");
					pathSuccess = true;
					RetracePath(startNode,targetNode);
					break;
				}

				foreach(Node adjacent in grid.GetAdjacent(currentNode)){
					if (!adjacent.walkable || closedSet.Contains(adjacent)) {
						continue;
					}

					int newMovementCostToAdjacent = currentNode.gCost + GetDistance(currentNode, adjacent);
					if(newMovementCostToAdjacent < adjacent.gCost || !openSet.Contains(adjacent)) {
						adjacent.gCost = newMovementCostToAdjacent;
						adjacent.hCost = GetDistance(adjacent, targetNode);
						adjacent.parent = currentNode;

						if (!openSet.Contains(adjacent))
							openSet.Add(adjacent);
					}
				}
			}
		}

		//yield return null;
		if (pathSuccess){
			waypoints = RetracePath(startNode, targetNode);
		}
		gameObject.GetComponent<ZombieMovementAStar>().OnPathFound(waypoints, pathSuccess);
	}

	Vector3[] RetracePath(Node startNode, Node endNode) {
		List<Node> path = new List<Node>();
		List<Node> resPath = new List<Node>();
		Node currentNode = endNode;
		//currentNode.reserved = true; BREAKS EVERYTHING
		//print( currentNode.gridX + " + " + currentNode.gridY + " = " + "Reserved");
		while (currentNode != startNode) {
			path.Add(currentNode);
			resPath.Add(currentNode);

			currentNode = currentNode.parent;
		}
		//ReservationTable.Reservation(resPath);
		Vector3[] waypoints = SimplifyPath(path);
		Array.Reverse(waypoints);

		return waypoints;

		//grid.path = path; visualisation only

	}

	Vector3[] SimplifyPath(List<Node> path){
		List<Vector3> waypoints = new List<Vector3>();
		Vector2 oldDirection = Vector2.zero;

		for (int i = 1; i < path.Count; i++){
//			if (path[i]!= null){//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//			newPath[i] = path[i];
//			newPath[i].reserved = true;
//			}
			//path[i].reserved = true;
			Vector2 newDirection = new Vector2(path[i-1].gridX - path[i].gridX, path[i-1].gridY - path[i].gridY);
			if (newDirection != oldDirection){
				waypoints.Add(path[i].worldPos);
			}
			oldDirection = newDirection;
		}
		return waypoints.ToArray();
	}

	int GetDistance(Node nodeA, Node nodeB) {
		int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
		int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

		if (dstX > dstY)
			return 14*dstY + 10* (dstX-dstY);
		return 14*dstX + 10 * (dstY-dstX);
	}
}