using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DG.Tweening;
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
            RenderDropdown();
        }

        private void RenderDropdown()
        {
            var options = new List<Dropdown.OptionData>();
            options.Add(new Dropdown.OptionData(LoadMode.AllAtOnce.ToString()));
            options.Add(new Dropdown.OptionData(LoadMode.OneByOne.ToString()));
            options.Add(new Dropdown.OptionData(LoadMode.SeparatedOnLoadCompleted.ToString()));
            _dropdown.options = options;
        }

        private void OnEnable()
        {
            _model.OnDataUpdated += UpdateView;
            _loadButton.onClick.AddListener(RefreshCards);
            _dropdown.onValueChanged.AddListener(ChangeLoadingMode);
        }

        private void OnDisable()
        {
            _model.OnDataUpdated -= UpdateView;
            _loadButton.onClick.RemoveListener(RefreshCards);
            _dropdown.onValueChanged.RemoveListener(ChangeLoadingMode);
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
                var spawnedCard = _spawnedCards[i];
                if (spawnedCard.sprite.texture == card)
                    continue;

                spawnedCard.sprite =
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

        private async void RefreshCards()
        {
            await Task.WhenAll(RotateCards(0));
            _model.Refresh();
            await Task.WhenAll(ShakeCards(1.5f));
            await Task.WhenAll(RotateCards(180));
        }

        private List<Task> ShakeCards(float duration)
        {
            return _spawnedCards.Select(c => c.transform.DOShakeRotation(duration)
                .AsyncWaitForCompletion()).ToList();
        }
        
        private List<Task> RotateCards(float degree)
        {
            return _spawnedCards.Select(c => c.transform.DORotate(new Vector3(0, degree, 0), 1)
                .AsyncWaitForCompletion()).ToList();
        }
        

        private void ChangeLoadingMode(int arg)
        {
            _model.LoadMode = (LoadMode)arg;
        }
    }

    public static class Extensions
    {
        public static async System.Threading.Tasks.Task AsyncWaitForCompletion(this Tween t)
        {
            if (!t.active)
            {
                //if (Debugger.logPriority > 0) Debugger.LogInvalidTween(t);
                return;
            }

            while (t.active && !t.IsComplete()) await System.Threading.Tasks.Task.Yield();
        }
    }
}