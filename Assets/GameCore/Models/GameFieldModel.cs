using System;
using System.Threading.Tasks;
using GameCore.Services;
using UnityEngine;

namespace GameCore.Models
{
    public class GameFieldModel : MonoBehaviour
    {
        public event Action OnDataUpdated = delegate {};
        public Texture2D[] Cards { get; private set; }

        [SerializeField] private int _totalCards;

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

        public async void Start()
        {
            Cards = new Texture2D[_totalCards];

            await LoadNewCards();
        }

        public async void Refresh()
        {
            await LoadNewCards();
        }

        private async Task LoadNewCards()
        {
            for (int i = 0; i < _totalCards; i++)
            {
                Task<Texture2D> request = _imageLoader.LoadRandomTexture(200, 200);
                await request;
                Cards[i] = request.Result;
            }
            
            OnDataUpdated.Invoke();
        }

        private static void RiseInitErrorMessage() =>
            Debug.LogError($"Try to init {nameof(GameFieldModel)}, but it's already initialized!");
    }
}