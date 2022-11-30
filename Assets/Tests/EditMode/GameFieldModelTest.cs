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
        [Test]
        public void WhenGameFieldInitialized_ThenAllCardsImageLoaded()
        {
            // prepare
            var gameObject = new GameObject();
            GameFieldModel gameFieldModel = gameObject.AddComponent<GameFieldModel>();
            int totalCards = 6;
            var imageLoader = Substitute.For<ILoadImage>();
            imageLoader.LoadRandomTexture(Arg.Any<int>(), Arg.Any<int>())
                .Returns(new Texture2D(200, 200));
            gameFieldModel.ConstructFromCode(totalCards, imageLoader);

            // act
            gameFieldModel.Start();

            // assert
            gameFieldModel.Cards.Where(c => c != null).Count().ShouldBeEqualTo(totalCards);
        }
    }
}