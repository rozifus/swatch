using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameSpaceController : MonoBehaviour {

	float width = 800;
	float height = 800;
	float tileWidth;
	float tileHeight;
	int rows = 4;
	int cols = 4;
	
	enum Direction { Up, Down, Left, Right};
	
	public GameObject tilePrefab;
	
	GameObject[,] grid;
	
	int[] selected = null;
	
	public void Initialize() {
		
		Rect rect = transform.GetComponent<RectTransform>().rect;
		width = rect.width;
		height = rect.height;
		
		tileWidth = width / cols;
		tileHeight = height / rows;
		
		grid = new GameObject[rows,cols];
		
		Vector3 position;
		
		Color[] baseColors = new Color[] { AppColors.Blue, AppColors.Red };
		
		Dictionary<int,int> randomization = new Dictionary<int,int>();
		
		for (int k = 0; k < rows * cols; k++) {
			while(true) {
				int r = Random.Range (0, rows*cols);
				if (randomization.ContainsKey(r)) {
					continue;
				} else {
					randomization.Add (r,k);
					break;
				}	
			}
		}
		
		for (int i = 0; i < rows; i++) {
			for(int j = 0; j < cols; j++) {
				int baseColorKey = randomization[i*cols+j] / ((cols*rows) / baseColors.Length);
				Debug.Log (baseColorKey);
				position = PositionForGrid(i,j);
				GameObject tile = (GameObject)Instantiate(tilePrefab, position, Quaternion.identity);
				tile.transform.SetParent (transform,false);
				tile.GetComponent<RectTransform>().sizeDelta = new Vector2(tileWidth, tileHeight);
				Color color = baseColors[baseColorKey];
				tile.GetComponent<Image>().color = baseColors[baseColorKey];
				grid[i,j] = tile;
			}
		}
			
	}
	
	Vector3 PositionForGrid(int i, int j) {
		return new Vector3(j*tileWidth+(tileWidth/2), i*tileHeight+(tileHeight/2));
	}
	
	void ShiftRight() {
		for (int i = 0; i < rows; i++) {
			for(int j = cols - 2; j >= 0; j--) {
				GameObject first = grid[i,j];
				GameObject second = grid[i,j+1];
				if (first == null) {
					continue;
				}
				if (second == null) {
					grid[i,j+1] = grid[i,j];
					grid[i,j] = null;
					grid[i,j+1].GetComponent<RectTransform>().position = PositionForGrid(i,j+1);
				} else if (second.GetComponent<TileController>().Absorb(first)) {
					Destroy(first);
					grid[i,j] = null;
				}
				
			}
		}
	}
	
	void ShiftLeft() {
		for (int i = 0; i < rows; i++) {
			for(int j = 1; j < cols; j++) {
				GameObject first = grid[i,j];
				GameObject second = grid[i,j-1];
				if (grid[i,j] == null) {
					continue;
				}
				if (grid[i, j-1] == null) {
					grid[i, j-1] = grid[i,j];
					grid[i,j] = null;
					grid[i,j-1].GetComponent<RectTransform>().position = PositionForGrid(i,j-1);
				} else if (second.GetComponent<TileController>().Absorb(first)) {
					Destroy(first);
					grid[i,j] = null;
				}
			}
		}
	}
	
	void ShiftDown() {
		for(int j = 0; j < cols; j++) {
			for (int i = 1; i < rows; i++) {
				GameObject first = grid[i,j];
				GameObject second = grid[i-1,j];
				if (grid[i,j] == null) {
					continue;
				}
				if (grid[i-1, j] == null) {
					grid[i-1, j] = grid[i,j];
					grid[i,j] = null;
					grid[i-1,j].GetComponent<RectTransform>().position = PositionForGrid(i-1,j);
				} else if (second.GetComponent<TileController>().Absorb(first)) {
					Destroy(first);
					grid[i,j] = null;
				}
			}
		}
	}
	
	void ShiftUp() {
		for(int j = 0; j < cols; j++) {
			for (int i = rows - 2; i >= 0; i--) {
				GameObject first = grid[i,j];
				GameObject second = grid[i+1,j];
				if (first == null) {
					continue;
				}
				if (second == null) {
					grid[i+1, j] = grid[i,j];
					grid[i,j] = null;
					grid[i+1,j].GetComponent<RectTransform>().position = PositionForGrid(i+1,j);
				} else if (second.GetComponent<TileController>().Absorb(first)) {
					Destroy(first);
					grid[i,j] = null;
				}
			}
		}
	}
	
	void Start() {
		Initialize();
	}
	
	void ProcessInput() {
		if (Input.GetKeyDown (KeyCode.W) || Input.GetKeyDown (KeyCode.UpArrow)) {
			ShiftUp();
		}
	
		if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) {
			ShiftDown();
		}
		
		if (Input.GetKeyDown (KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) {
			ShiftLeft();
		}
		
		if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown (KeyCode.RightArrow)) {
			ShiftRight();
		}
	
	}
	
	// Update is called once per frame
	void Update () {
		ProcessInput();
	}
	
	void FixedUpdate() {
		
	}
	
}
