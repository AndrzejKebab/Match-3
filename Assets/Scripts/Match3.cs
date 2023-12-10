﻿using UnityEngine;

namespace PatataStudio
{
	public class Match3 : MonoBehaviour
	{
		[SerializeField] private int width = 8;
		[SerializeField] private int height = 8;
		[SerializeField] private float cellSize = 1f;
		[SerializeField] private Vector3 originPosition = Vector3.zero;
		[SerializeField] private bool debug = true;

		private GridSystem2D<GridObject<Gem>> grid;

		private void Start()
		{
			grid = GridSystem2D<GridObject<Gem>>.VerticalGrid(width, height, cellSize, originPosition, debug);
		}
	}
}