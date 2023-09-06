using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMap<TGridMapObject> {

    public event EventHandler<OnGridMapObjectChangedEventArgs> OnGridMapObjectChanged;
    public class OnGridMapObjectChangedEventArgs : EventArgs {
        public int x;
        public int y;
    }

    private int width;
    private int height;
    private float cellSize;
    private Vector3 originPosition;
    private TGridMapObject[,] gridArray;

    public GridMap (int width, int height, float cellSize, Vector3 originPosition, Func<GridMap<TGridMapObject>, int,int, TGridMapObject> createGridMapObject) {

        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;

        gridArray = new TGridMapObject[width, height];

        for (int x = 0; x < gridArray.GetLength(0); x++) {
            for (int y = 0; y < gridArray.GetLength(1); y++) {
                gridArray[x, y] = createGridMapObject(this, x, y);
            }
        }

        bool showDebug = true;
        if (showDebug == true) {
            TextMesh[,] debugTextArray = new TextMesh[width, height];

            for (int x = 0; x < gridArray.GetLength(0); x++) {
                for (int y = 0; y < gridArray.GetLength(1); y++) {
                    debugTextArray[x, y] = Auxiliares.CreateWorldText(gridArray[x, y]?.ToString(), null, GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * .5f, 5, Color.white, TextAnchor.MiddleCenter);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
                }
            }
            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);
        }
    }

    //Convierte la posicion del Grid a la del mundo
    private Vector3 GetWorldPosition(int x, int y) {
        return new Vector3(x, y) * cellSize + originPosition;
    }

    //Convierte la posicion en el mundo a la posicion en el Grid
    public void GetXY(Vector3 worldPosition, out int x, out int y) {
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
    }

    //Setea el valor de un Grid
    public void SetGridMapObject(int x, int y, TGridMapObject value) {
        if(x >= 0 && y >= 0 && x < width && y < height) {
            gridArray[x, y] = value;
            if(OnGridMapObjectChanged != null) OnGridMapObjectChanged(this, new OnGridMapObjectChangedEventArgs {x = x, y = y});
        }
    }

    public void TriggerGridMapObjectChanged(int x, int y) {
        if (OnGridMapObjectChanged != null) OnGridMapObjectChanged(this, new OnGridMapObjectChangedEventArgs { x = x, y = y });
    }

    public void SetGridMapObject(Vector3 worldPosition, TGridMapObject value) {
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetGridMapObject(x, y, value);
    }

    //Obtiene el valor de un grid
    public TGridMapObject GetGridMapObject(int x, int y) {
        if (x >= 0 && y >= 0 && x < width && y < height) {
            return gridArray[x, y];
        } else {
            return default(TGridMapObject);
        }
    }

    public TGridMapObject GetGridMapObject(Vector3 worldPosition) {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetGridMapObject(x, y);
    }

    public int GetWidth() {
        return width;
    }

    public int GetHeight() {
        return height;
    }

}
