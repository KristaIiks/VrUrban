using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace StudySystem
{
	public sealed class ConditionalCard : QuestCard
	{
		[Space(25), Header("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~"), Space(25)]
		[SerializeField] private List<Card> CorrectCards = new List<Card>();
		[SerializeField] private List<Card> WrongCards = new List<Card>();
		[SerializeField] private ConditionalType ConditionalType;
		
		
		[Space(25), Header("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~"), Space(25)]
		[SerializeField] private UnityEvent OnCorrectEvent;
		[SerializeField] private UnityEvent OnWrongEvent;
		
		protected override List<Card> _allCards { get; set; }
		private bool _conditional;

		private void OnValidate()
		{			
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
						_conditional = Quests.All((quest) => quest.Result.IsCorrect);
						break;
					case ConditionalType.AllWrong:
						_conditional = Quests.All((quest) => !quest.Result.IsCorrect);
						break;
					case ConditionalType.AnyCorrect:
						_conditional = Quests.Any((quest) => quest.Result.IsCorrect);
						break;
					case ConditionalType.AnyWrong:
						_conditional = Quests.Any((quest) => !quest.Result.IsCorrect);
						break;
				}
				
				if (_conditional)
					OnCorrectEvent?.Invoke();
				else
					OnWrongEvent?.Invoke();
				
				// TODO: reward
				Continue();
			}
		}

		public override void SkipAll()
		{
			Quests.ForEach((quest) => quest.Skip());
			OnCorrectEvent?.Invoke();
			
			CorrectCards.ForEach((card) => card.SkipAll());
		}
		public override void Skip(Card to)
		{
			base.Skip(to);
			
			if(CorrectCards.Contains(to))
			{
				switch (ConditionalType)
				{
					case ConditionalType.AllCorrect:
					case ConditionalType.AnyCorrect:
						Quests.ForEach((quest) => quest.Skip());
						break;
					case ConditionalType.AllWrong:
					case ConditionalType.AnyWrong:
						Quests.ForEach((quest) => quest.Skip(false));
						break;
				}
				OnCorrectEvent?.Invoke();
			}
			else
			{
				switch (ConditionalType)
				{
					case ConditionalType.AllCorrect:
					case ConditionalType.AnyCorrect:
						Quests.ForEach((quest) => quest.Skip(false));
						break;
					case ConditionalType.AllWrong:
					case ConditionalType.AnyWrong:
						Quests.ForEach((quest) => quest.Skip());
						break;
				}
				OnWrongEvent?.Invoke();
			}
		}

		protected override void Continue()
		{
			base.Continue();
			
			IsCompleted = _conditional ? CorrectCards.All((card) => card.IsCompleted) : WrongCards.All((card) => card.IsCompleted);
			if (IsCompleted) { m_previousCard.Invoke(); return; }
			
			CardsWindow.Instance.DisplayCards(
				() => Continue(), 
				updateBranch, 
				_conditional ? CorrectCards.ToArray() : WrongCards.ToArray()
			);
		}
	}
}
