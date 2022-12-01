using System;
using System.Threading.Tasks;
using GameCore.Services;
using UnityEngine;

namespace GameCore.Models
{
    public enum LoadMode
    {
        AllAtOnce = 0,
        OneByOne = 1,
        SeparatedOnLoadCompleted = 2,
    }

    public class GameFieldModel : MonoBehaviour
    {
        public event Action OnDataUpdated = delegate { };
        public Texture2D[] Cards { get; private set; }
        public LoadMode LoadMode { get; set; }

        [SerializeField] private int _totalCards;
        [SerializeField] private int _cardWidth;
        [SerializeField] private int _cardHeight;

        private ILoadImage _imageLoader;
        private bool _isInitialized;

        private void Awake()
        {
            if (_isInitialized)
            {
                RiseInitErrorMessage();
                return;
            }

            _imageLoader = GetComponent<ILoadImage>();
            _isInitialized = true;
        }

        public void ConstructFromCode(int totalCards, ILoadImage imageLoader)
        {
            if (_isInitialized)
            {
                RiseInitErrorMessage();
                return;
            }

            _totalCards = totalCards;
            _imageLoader = imageLoader;
            _isInitialized = true;
        }

        public void Start()
        {
            Cards = new Texture2D[_totalCards];
            LoadAllAtOnce();
        }

        public void Refresh()
        {
            switch (LoadMode)
            {
                case LoadMode.AllAtOnce:
                    LoadAllAtOnce();
                    break;
                case LoadMode.OneByOne:
                    LoadOneByOne();
                    break;
                case LoadMode.SeparatedOnLoadCompleted:
                    LoadSeparated();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(LoadMode), LoadMode, null);
            }
        }

        private async Task LoadAllAtOnce()
        {
            for (int i = 0; i < _totalCards; i++)
            {
                Task<Texture2D> request = _imageLoader.LoadRandomTexture(_cardWidth, _cardHeight);
                await request;
                Cards[i] = request.Result;
            }

            OnDataUpdated.Invoke();
        }

        private async Task LoadOneByOne()
        {
            for (int i = 0; i < _totalCards; i++)
            {
                Task<Texture2D> request = _imageLoader.LoadRandomTexture(_cardWidth, _cardHeight);
                await request;
                Cards[i] = request.Result;
                OnDataUpdated.Invoke();
            }
        }

        private void LoadSeparated()
        {
            for (int i = 0; i < _totalCards; i++)
            {
                var index = i;
                _imageLoader.LoadRandomTexture(_cardWidth, _cardHeight, texture =>
                {
                    Cards.SetValue(texture, index);
                    OnDataUpdated.Invoke();
                });
            }
        }

        private static void RiseInitErrorMessage() =>
            Debug.LogError($"Try to init {nameof(GameFieldModel)}, but it's already initialized!");
    }
}