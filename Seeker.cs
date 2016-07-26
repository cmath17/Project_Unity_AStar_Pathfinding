//using UnityEngine;
//using System.Collections;
//
//public class Seeker : MonoBehaviour {
//
//	public GameObject player;
//	public Transform target;
//	public Vector3 targetPos = new Vector3();
//	public Vector3 newTargetPos = new Vector3();
//	public bool isSeeking = false;
//	float speed = 5f;
//	Vector3[] path;
//	int targetIndex;
//
//
//	// Use this for initialization
//	void Awake () {
//		player = GameObject.FindGameObjectWithTag("Player");
//		target = player.transform;
//		targetPos = target.transform.position;
//		//PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
//		//Every X secs look for path
//		//InvokeRepeating("Seek",0.5f, 0.5f);
//
//	}
//
//
//
//	void Update(){
//		newTargetPos = target.transform.position;
//		if(targetPos != newTargetPos && isSeeking == false){
//			InvokeRepeating("Seek", 1f, 1f);
//			isSeeking = true;
//		}else{
//			CancelInvoke("Seek");
//			isSeeking = false;
//		}
//
//
//	}
//	
//	// Update is called once per frame
//	void Seek () {
//		Pathfinding.RequestPath(transform.position, target.position, OnPathFound);
//	}
//
//	public void OnPathFound( Vector3[] newPath, bool pathSuccessful){
//		if (pathSuccessful){
//			path = newPath;
//			StopCoroutine("FollowPath");
//			StartCoroutine("FollowPath");
//		}
//	}
//
//	IEnumerator FollowPath(){
//		Vector3 currentWaypoint = path[0];
//
//		while (true){
//			if (transform.position == currentWaypoint){
//				targetIndex ++;
//				if (targetIndex >= path.Length){
//					yield break; //exits coroutine
//				}
//				//PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
//				currentWaypoint = path[targetIndex];
//			}
//			transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
//			yield return null;
//		}
//	}
//
//	public void OnDrawGizmos(){
//		if (path != null){
//			for (int i = targetIndex; i < path.Length; i++){
//				Gizmos.color = Color.black;
//				Gizmos.DrawCube(path[i], Vector3.one);
//
//				if (i == targetIndex){
//					Gizmos.DrawLine(transform.position, path[i]);
//				}else{
//					Gizmos.DrawLine(path[i-1],path[i]);
//				}
//			}
//		}
//	}
//
//}
