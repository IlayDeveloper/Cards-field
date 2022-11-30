using System.Threading.Tasks;
using GameCore.Services;
using UnityEngine;

namespace GameCore.Models
{
    public class GameFieldModel : MonoBehaviour
    {
        public Texture2D[] Cards { get; private set; }
        
        [SerializeField] private int _totalCards;

        private ILoadImage _imageLoader;

        private void Awake()
        {
            _imageLoader = GetComponent<ILoadImage>();
        }

        public void ConstructFromCode(int totalCards, ILoadImage imageLoader)
        {
            _totalCards = totalCards;
            _imageLoader = imageLoader;
        }

        public async void Start()
        {
            Cards = new Texture2D[_totalCards];

            for (int i = 0; i < _totalCards; i++)
            {
                Task<Texture2D> request = _imageLoader.LoadRandomTexture(200, 200);
                await request;
                Cards[i] = request.Result;
            }
        }
    }
}