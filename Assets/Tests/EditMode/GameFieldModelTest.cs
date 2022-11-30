using System.Collections.Generic;
using System.Linq;
using FluentAssert;
using GameCore.Models;
using GameCore.Services;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace Tests.EditMode
{
    public class GameFieldModelTest
    {
        private GameFieldModel _gameFieldModel;
        private ILoadImage _imageLoader;
        
        [SetUp]
        public void Init()
        {
            var gameObject = new GameObject();
            _gameFieldModel = gameObject.AddComponent<GameFieldModel>();
            _imageLoader = Substitute.For<ILoadImage>();
        }
        
        [Test]
        public void WhenGameFieldInitialized_ThenAllCardsImageLoaded()
        {
            // prepare
            int totalCards = 6;
            _imageLoader.LoadRandomTexture(Arg.Any<int>(), Arg.Any<int>())
                .Returns(new Texture2D(200, 200));
            _gameFieldModel.ConstructFromCode(totalCards, _imageLoader);

            // act
            _gameFieldModel.Start();

            // assert
            _gameFieldModel.Cards.Where(c => c != null).Count().ShouldBeEqualTo(totalCards);
        }

        [Test]
        public void WhenGameFieldInitializedRefreshed_ThenAllCardsImageUpdated()
        {
            // prepare
            int totalCards = 1;
            _imageLoader.LoadRandomTexture(Arg.Any<int>(), Arg.Any<int>())
                .Returns(new Texture2D(0, 0), new Texture2D(0, 0));
            _gameFieldModel.ConstructFromCode(totalCards, _imageLoader);
            _gameFieldModel.Start();
            var cardsID = _gameFieldModel.Cards.Select(c => c.GetInstanceID()).ToList();

            // act
            _gameFieldModel.Refresh();

            // assert
            var refreshedIds = _gameFieldModel.Cards.Select(c => c.GetInstanceID()).ToList();
            List<bool> notRefreshed = new List<bool>();
            for (int i = 0; i < cardsID.Count; i++)
            {
                notRefreshed.Add(cardsID[i] == refreshedIds[i]);
            }

            notRefreshed.Where(result => result).Count().ShouldBeEqualTo(0);
        }
    }
}