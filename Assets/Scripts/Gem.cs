using UnityEngine;

namespace PatataStudio.MatchGame
{
	[RequireComponent(typeof(SpriteRenderer))]
	public class Gem : MonoBehaviour
	{ 
		public GemType GemType;

		public void SetType(GemType type)
		{
			GemType = type;
			GetComponent<SpriteRenderer>().sprite = type.Sprite;
		}

		public GemType GetType() => GemType;
	}
}