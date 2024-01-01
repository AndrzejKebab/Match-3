using UnityEngine;

namespace PatataStudio.MatchGame
{
	[CreateAssetMenu(fileName = "NewGemType", menuName = "Match 3/New Gem Type")]
	public class GemType : ScriptableObject
	{
		public Sprite Sprite;
	}
}