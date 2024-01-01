using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace PatataStudio.MatchGame
{
	public class Match3 : MonoBehaviour
	{
		[SerializeField] private int width = 8;
		[SerializeField] private int height = 8;
		[SerializeField] private float cellSize = 1f;
		[SerializeField] private Vector3 originPosition = Vector3.zero;
		[SerializeField] private bool debug = true;

		[SerializeField] private Gem gemPrefab;
		[SerializeField] private GemType[] gemTypes;
		[SerializeField] private Ease ease = Ease.InQuad;

		private GridSystem2D<GridObject<Gem>> grid;

		private InputReader inputReader;
		private Vector2Int selectedGem = Vector2Int.one * -1;

		private void Awake()
		{
			inputReader = gameObject.GetComponent<InputReader>();
		}

		private void Start()
		{
			InitializeGrid();
			inputReader.Fire += OnSelectGem;
		}

		private void OnDestroy()
		{
			inputReader.Fire -= OnSelectGem;
		}

		private void OnSelectGem()
		{
			var _gridPos = grid.GetXY(Camera.main.ScreenToWorldPoint(inputReader.Selected));

			if (!IsValidPosition(_gridPos) || IsEmptyPosition(_gridPos)) return;

			if (selectedGem == _gridPos)
			{
				DeselectGem();
			}
			else if (selectedGem == Vector2Int.one * -1)
			{
				SelectGem(_gridPos);
			}
			else
			{
				StartCoroutine(RunGameLoop(selectedGem, _gridPos));
			}
		}

		private IEnumerator RunGameLoop(Vector2Int gridPosA, Vector2Int gridPosB)
		{
			yield return StartCoroutine(SwapGems(gridPosA, gridPosB));

			DeselectGem();
		}

		private IEnumerator SwapGems(Vector2Int gridPosA, Vector2Int gridPosB)
		{
			var _gridObjectA = grid.GetValue(gridPosA.x, gridPosA.y);
			var _gridObjectB = grid.GetValue(gridPosB.x, gridPosB.y);

			_gridObjectA.GetValue().transform.DOLocalMove(grid.GetWorldPostionCentre(gridPosB.x, gridPosB.y), 0.5f).SetEase(ease);
			_gridObjectB.GetValue().transform.DOLocalMove(grid.GetWorldPostionCentre(gridPosA.x, gridPosA.y), 0.5f).SetEase(ease);

			grid.SetValue(gridPosA.x, gridPosA.y, _gridObjectB);
			grid.SetValue(gridPosB.x, gridPosB.y, _gridObjectA);

			yield return new WaitForSeconds(0.5f);
		}

		private void InitializeGrid()
		{
			grid = GridSystem2D<GridObject<Gem>>.VerticalGrid(width, height, cellSize, originPosition, debug);

			for (byte x = 0; x < width; x++)
			{
				for (byte y = 0; y < height; y++)
				{
					CreateGem(x, y);
				}
			}
		}

		private void CreateGem(byte x, byte y)
		{
			var _gem = Instantiate(gemPrefab, grid.GetWorldPostionCentre(x, y), Quaternion.identity, transform);
			_gem.SetType(gemTypes[Random.Range(0, gemTypes.Length)]);
			var _gridObject = new GridObject<Gem>(grid, x, y);
			_gridObject.SetValue(_gem);
			grid.SetValue(x, y, _gridObject);
		}

		private void SelectGem(Vector2Int gridPos) => selectedGem = gridPos;

		private void DeselectGem() => selectedGem = new Vector2Int(-1, -1);

		private bool IsEmptyPosition(Vector2Int gridPosition) => grid.GetValue(gridPosition.x, gridPosition.y) == null;

		private bool IsValidPosition(Vector2Int gridPosition)
		{
			return gridPosition.x >= 0 && gridPosition.x < width && gridPosition.y >= 0 && gridPosition.y < height;
		}
	}
}