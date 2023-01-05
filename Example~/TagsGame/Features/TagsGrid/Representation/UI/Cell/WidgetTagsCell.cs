using Lukomor.Presentation.Views.Widgets;
using Lukomor.TagsGame.TagsGrid;
using UnityEngine;
using UnityEngine.UI;

namespace Lukomor.TagsGame.UI
{
	public class WidgetTagsCell : Widget<WidgetTagsCellViewModel>
	{
		[SerializeField] private Button _button;
		[SerializeField] private Text _textNumber;

		public override void Subscribe()
		{
			base.Subscribe();
			
			ViewModel.Data.Changed += OnDataChanged;
			
			_button.onClick.AddListener(OnCellClicked);
		}

		public override void Unsubscribe()
		{
			base.Unsubscribe();

			ViewModel.Data.Changed -= OnDataChanged;
			
			_button.onClick.RemoveListener(OnCellClicked);
		}

		private void OnDataChanged(ICell newValue)
		{
			FillNumberFromModel();
		}

		private void FillNumberFromModel()
		{
			var isActive = ViewModel.Data.Value.Number != 0;
			
			_textNumber.gameObject.SetActive(isActive);
			_button.gameObject.SetActive(isActive);
			
			if (isActive)
			{
				_textNumber.text = ViewModel.Data.Value.Number.ToString();
			}
		}
		
		private void OnCellClicked()
		{
			ViewModel.RequestMoveCell();
		}

		private void Reset()
		{
			if (_button == null)
			{
				_button = GetComponentInChildren<Button>();
			}

			if (_textNumber == null)
			{
				_textNumber = GetComponentInChildren<Text>();
			}
		}
	}
}