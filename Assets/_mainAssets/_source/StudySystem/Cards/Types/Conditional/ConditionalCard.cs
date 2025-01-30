using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace StudySystem
{
	public sealed class ConditionalCard : Card
	{
		[SerializeField] private List<Card> CorrectCards;
		[SerializeField] private List<Card> WrongCards;
		[SerializeField] private ConditionalType ConditionalType;
		
		[SerializeField] private UnityEvent OnCorrectEvent;
		[SerializeField] private UnityEvent OnWrongEvent;
		
		private bool _conditional;

		protected override void OnValidate()
		{
			base.OnValidate();
			
			List<Card> cards = new List<Card>();
			cards.AddRange(CorrectCards);
			cards.AddRange(WrongCards);
			cards = cards.Distinct().ToList();
			
			_allCards = cards;
		}

		protected override void QuestCompleted(QuestResult result)
		{
			if (Quests.All((quest) => quest.IsCompleted))
			{
				switch (ConditionalType)
				{
					case ConditionalType.AllCorrect:
						_conditional = Quests.All((quest) => quest.GetResult().IsCorrect);
						break;
					case ConditionalType.AllWrong:
						_conditional = Quests.All((quest) => !quest.GetResult().IsCorrect);
						break;
					case ConditionalType.AnyCorrect:
						_conditional = Quests.Any((quest) => quest.GetResult().IsCorrect);
						break;
					case ConditionalType.AnyWrong:
						_conditional = Quests.Any((quest) => !quest.GetResult().IsCorrect);
						break;
				}
				
				// TODO: reward
				Continue();
			}
		}

		public override void Skip(Card to)
		{
			base.Skip(to);
			
			if(CorrectCards.Contains(to))
				OnCorrectEvent?.Invoke();
			else
				OnWrongEvent?.Invoke();
		}

		protected override void Continue()
		{
			base.Continue();
			// TODO: show cards if conditional
			if (_conditional)
			{
				// Draw correct cards
				return;
			}
			// Draw wrong cards
		}
	}
}
