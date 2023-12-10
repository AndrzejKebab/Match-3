using System;
using TMPro;
using UnityEngine;

namespace PatataStudio
{
	public class GridSystem2D<T>
	{
		private readonly int width;
		private readonly int height;
		private readonly float cellSize;
		private readonly Vector3 origin;
		private readonly T[,] gridArray;
		private readonly CoordinateConventer coordinateConventer;

		public event Action<int, int, T> OnValueChangeEvent;

		public static GridSystem2D<T> VerticalGrid(int width, int height, float cellSize, Vector3 origin, bool debug = false)
		{
			return new GridSystem2D<T>(width, height, cellSize, origin, new VerticalConverter(), debug);
		}

		public GridSystem2D(int width, int height, float cellSize, Vector3 origin, CoordinateConventer coordinateConventer, bool debug)
		{
			this.width = width;
			this.height = height;
			this.cellSize = cellSize;
			this.origin = origin;
			this.coordinateConventer = coordinateConventer ?? new VerticalConverter();
			
			gridArray = new T[width, height];

			if (debug)
			{
				DrawDebugLines();
			}
		}

		public void SetValue(Vector3 worldPosition, T value)
		{
			Vector2Int pos = coordinateConventer.WorldToGrid(worldPosition, cellSize, origin);
			SetValue(pos.x, pos.y, value);
		}

		public void SetValue(int x, int y, T value)
		{
			if(IsValid(x, y))
			{
				gridArray[x, y] = value;
				OnValueChangeEvent?.Invoke(x, y, value);
			}
		}

		public T GetValue(int x, int y)
		{
			return IsValid(x, y) ? gridArray[x, y] : default;
		}

		private bool IsValid(int x, int y) => x >= 0 && y >= 0 && x < width && y < height;

		public Vector2Int GetXY(Vector3 worldPosition) => coordinateConventer.WorldToGrid(worldPosition, cellSize, origin);

		public Vector3 GetWorldPostionCentre(int x, int y) => coordinateConventer.GridToWorldCenter(x, y, cellSize, origin);

		private Vector3 GetWorldPosition(int x, int y) => coordinateConventer.GridToWorld(x, y, cellSize, origin);

		private void DrawDebugLines()
		{
			const float _duration = 100f;

			var parent = new GameObject("Debugging");

			for (byte x = 0; x < width; x++)
			{
				for(byte y = 0; y < height; y++)
				{
					CreteWorldText(parent, x + " , " + y, GetWorldPostionCentre(x, y), coordinateConventer.Forward);
					Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, _duration);
					Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, _duration);
				}
			}

			Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, _duration);
			Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, _duration);
		}

		private TextMeshPro CreteWorldText(GameObject parten, string text, Vector3 position, Vector3 direction,
				int fontSize = 2, Color color = default, TextAlignmentOptions textAnchor = TextAlignmentOptions.Center, int sortingOrder = 0)
		{
			GameObject _gameObject = new GameObject("DebugText" + text, typeof(TextMeshPro));
			_gameObject.transform.SetParent(parten.transform);
			_gameObject.transform.position = position;
			_gameObject.transform.forward = direction;

			TextMeshPro _textMeshPro = _gameObject.GetComponent<TextMeshPro>();
			_textMeshPro.text = text;
			_textMeshPro.rectTransform.sizeDelta = new Vector2(cellSize, cellSize);
			_textMeshPro.fontSize = fontSize;
			_textMeshPro.color = color == default ? Color.white : color;
			_textMeshPro.alignment = textAnchor;
			_textMeshPro.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;

			return _textMeshPro;
		}

		public abstract class CoordinateConventer
		{
			public abstract Vector3 GridToWorld(int x, int y, float cellSize, Vector3 origin);
			public abstract Vector3 GridToWorldCenter(int x, int y, float cellSize, Vector3 origin);
			public abstract Vector2Int WorldToGrid(Vector3 worldPosition, float cellSize, Vector3 origin);
			public abstract Vector3 Forward { get; }
		}

		public class VerticalConverter : CoordinateConventer
		{
			public override Vector3 Forward => Vector3.forward;

			public override Vector3 GridToWorld(int x, int y, float cellSize, Vector3 origin)
			{
				return new Vector3(x, y, 0) * cellSize + origin;
			}

			public override Vector3 GridToWorldCenter(int x, int y, float cellSize, Vector3 origin)
			{
				return new Vector3(x * cellSize + cellSize * 0.5f, y * cellSize + cellSize * 0.5f, 0) + origin;
			}

			public override Vector2Int WorldToGrid(Vector3 worldPosition, float cellSize, Vector3 origin)
			{
				Vector3 _gridPostion = (worldPosition - origin) / cellSize;
				int _x = Mathf.FloorToInt(_gridPostion.x);
				int _y = Mathf.FloorToInt(_gridPostion.y);

				return new Vector2Int(_x, _y);
			}
		}
	}
}