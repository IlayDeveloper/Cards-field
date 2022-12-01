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
        private Queue<Image[]> _cardsToUpdate;
        private bool _animsInProgress;

        private void Awake()
        {
            _model = GetComponent<GameFieldModel>();
            _cardsToUpdate = new Queue<Image[]>();
            InitDropdown();
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
                return;
            }

            var refreshedCards = new List<Image>();
            for (var i = 0; i < _model.Cards.Length; i++)
            {
                var card = _model.Cards[i];
                var spawnedCard = _spawnedCards[i];
                if (spawnedCard.sprite.texture == card)
                    continue;

                refreshedCards.Add(spawnedCard);
                spawnedCard.sprite =
                    ConvertToSprite(card);
            }

            _cardsToUpdate.Enqueue(refreshedCards.ToArray());
            if (_animsInProgress == false)
                AnimateCards();
        }

        private async Task AnimateCards()
        {
            if (_cardsToUpdate.Count == 0)
            {
                _animsInProgress = false;
                SwitchUI(true);
                return;
            }

            _animsInProgress = true;
            var cards = _cardsToUpdate.Dequeue();

            var transforms = cards.Select(c => c.transform).ToList();
            await Task.WhenAll(Shake(transforms, 1));
            await Task.WhenAll(RotateAboutY(transforms, 180));

            AnimateCards();
        }

        private async void RefreshCards()
        {
            SwitchUI(false);
            await Task.WhenAll(RotateAboutY(_spawnedCards.Select(c => c.transform).ToList(), 0));
            _model.Refresh();
        }

        private List<Task> Shake(IEnumerable<Transform> transforms, float duration)
        {
            return transforms.Select(t => t.DOShakeRotation(duration)
                .AsyncWaitForCompletion()).ToList();
        }

        private List<Task> RotateAboutY(IEnumerable<Transform> transforms, float degree)
        {
            return transforms.Select(t => t.DORotate(new Vector3(0, degree, 0), 1)
                .AsyncWaitForCompletion()).ToList();
        }

        private void ChangeLoadingMode(int arg)
        {
            _model.LoadMode = (LoadMode)arg;
        }

        private void SwitchUI(bool state)
        {
            _loadButton.interactable = state;
            _dropdown.interactable = state;
        }
        
        private void InitCards()
        {
            _spawnedCards = new List<Image>();
            for (var index = 0; index < _model.Cards.Length; index++)
            {
                var cardView = Instantiate(_cardPrefab, _gameField);
                var image = cardView.GetComponent<Image>();
                image.sprite = ConvertToSprite(_model.Cards[index]);
                _spawnedCards.Add(image);
            }
        }
        
        private void InitDropdown()
        {
            var options = new List<Dropdown.OptionData>();
            options.Add(new Dropdown.OptionData(LoadMode.AllAtOnce.ToString()));
            options.Add(new Dropdown.OptionData(LoadMode.OneByOne.ToString()));
            options.Add(new Dropdown.OptionData(LoadMode.OnLoadCompleted.ToString()));
            _dropdown.options = options;
        }
        
        private static Sprite ConvertToSprite(Texture2D card) =>
            Sprite.Create(card, new Rect(0, 0, card.width, card.height), 
                new Vector2(0.5f, 0.5f));
    }
}