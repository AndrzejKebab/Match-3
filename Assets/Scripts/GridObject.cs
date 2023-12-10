using UnityEngine;

namespace PatataStudio
{

	public class GridObject<T>
	{
		private GridSystem2D<GridObject<T>> grid;
		private int x;
		private int y;
		private T gem;

		public GridObject(GridSystem2D<GridObject<T>> grid, int x, int y, T gem)
		{
			this.grid = grid;
			this.x = x;
			this.y = y;
			this.gem = gem;
		}
	}
}