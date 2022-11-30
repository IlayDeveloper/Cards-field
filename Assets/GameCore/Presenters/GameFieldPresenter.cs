using System.Collections.Generic;
using GameCore.Models;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore.Presenters
{
    [RequireComponent(typeof(GameFieldModel))]
    public class GameFieldPresenter : MonoBehaviour
    {
        [SerializeField] private GameObject _cardPrefab;
        [SerializeField] private Transform _gameField;
        [SerializeField] private Button _loadButton;
        [SerializeField] private Dropdown _dropdown;

        private List<Image> _spawnedCards;
        private GameFieldModel _model;

        private void Awake()
        {
            _model = GetComponent<GameFieldModel>();
        }

        private void OnEnable()
        {
            _model.OnDataUpdated += UpdateView;
            _loadButton.onClick.AddListener(_model.Refresh);
        }

        private void OnDisable()
        {
            _model.OnDataUpdated -= UpdateView;
            _loadButton.onClick.RemoveListener(_model.Refresh);
        }

        private void UpdateView()
        {
            if (_spawnedCards == null)
            {
                InitCards();
            }

            for (var i = 0; i < _model.Cards.Length; i++)
            {
                var card = _model.Cards[i];
                _spawnedCards[i].sprite =
                    Sprite.Create(card, new Rect(0, 0, card.width, card.height), new Vector2(0.5f, 0.5f));
            }
        }

        private void InitCards()
        {
            _spawnedCards = new List<Image>();
            for (var index = 0; index < _model.Cards.Length; index++)
            {
                var cardView = Instantiate(_cardPrefab, _gameField);
                _spawnedCards.Add(cardView.GetComponent<Image>());
            }
        }
    }
}