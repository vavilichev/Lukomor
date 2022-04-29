using Lukomor.Example.Domain.TagsGrid;
using Lukomor.Presentation.Controllers;
using Lukomor.Presentation.Views.Widgets;
using UnityEngine;
using UnityEngine.UI;

namespace Lukomor.Example.Presentation.Cell
{
	public class TagsCellWidget : Widget<TagsCellModel>
	{
		public static class PayloadKeys
		{
			public static string TagCellKey = nameof(TagCellKey);
		}
		
		[SerializeField] private Button _button;
		[SerializeField] private Text _textNumber;

		protected override Controller<TagsCellModel> CreateController()
		{
			return new TagsCellController();
		}

		protected override void Refresh(TagsCellModel model)
		{
			base.Refresh(model);

			SetTagsCellFromPayload(PayloadKeys.TagCellKey);
		}

		protected override void Subscribe(TagsCellModel model)
		{
			base.Subscribe(model);
			
			model.Data.Changed += OnDataChanged;
			
			_button.onClick.AddListener(OnCellClicked);
		}

		protected override void Unsubscribe(TagsCellModel model)
		{
			base.Unsubscribe(model);

			model.Data.Changed -= OnDataChanged;
			
			_button.onClick.RemoveListener(OnCellClicked);
		}

		protected override void OnPayloadAdded(string payloadKey)
		{
			base.OnPayloadAdded(payloadKey);

			SetTagsCellFromPayload(payloadKey);
		}

		private void SetTagsCellFromPayload(string payloadKey)
		{
			if (IsReady)
			{
				var cell = GetPayload<TagsCell>(payloadKey);

				if (cell != null)
				{
					Model.Data.Value = cell;
				}
			}
		}

		private void OnDataChanged(TagsCell newValue)
		{
			FillNumberFromModel();
		}

		private void FillNumberFromModel()
		{
			var isActive = Model.Data.Value.Number != 0;
			
			_textNumber.gameObject.SetActive(isActive);
			_button.gameObject.SetActive(isActive);
			
			if (isActive)
			{
				_textNumber.text = Model.Data.Value.Number.ToString();
			}
		}
		
		private void OnCellClicked()
		{
			Model.MoveCell.Execute(Model.Data.Value);
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