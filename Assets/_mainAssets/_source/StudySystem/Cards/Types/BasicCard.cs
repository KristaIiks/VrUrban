using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace StudySystem
{
	public class BasicCard : Card
	{
		[SerializeField] private List<Card> Cards => _allCards;
		
		protected override void Continue()
		{
			base.Continue();
			
			// Cards completed => return back
			if (Cards.All((card) => card.IsCompleted))
			{
				m_previousCard?.Invoke();
				return;
			}
			
			// TODO: draw if cards > 1
		}
	}
}
