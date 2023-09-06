using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour {

    private PathFinding pathFinding;

    private void Start() {
        pathFinding = new PathFinding(41, 21);
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Vector3 mouseWorldPosition = Auxiliares.GetMouseWorldPosition();
            Debug.Log("MousePos " + mouseWorldPosition);
            pathFinding.GetGridMap().GetXY(mouseWorldPosition, out int x, out int y);
            List<PathNode> path = pathFinding.FindPath(0, 0, x , y);
            if (path != null) {
                for (int i = 0; i < path.Count - 1; i++) {
                    Debug.DrawLine(new Vector3(path[i].x, path[i].y) * 1f + Vector3.one * .5f, new Vector3(path[i + 1].x, path[i + 1].y) * 1f + Vector3.one * .5f, Color.green, 3f);
                }
            }
        }
        if (Input.GetMouseButtonDown(1)) {
            Vector3 mouseWorldPosition = Auxiliares.GetMouseWorldPosition();
            Debug.Log("La posicion " + mouseWorldPosition + " Se a seteado como intransitable");
            pathFinding.GetGridMap().GetXY(mouseWorldPosition, out int x, out int y);
            pathFinding.GetNode(x, y).SetIsWalkable(!pathFinding.GetNode(x, y).isWalkable);
        }
    }
}
