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
        
        [Test]
        public void WhenGameFieldInitializedRefreshed_ThenAllCardsImageUpdated()
        {
            // prepare
            var gameObject = new GameObject();
            GameFieldModel gameFieldModel = gameObject.AddComponent<GameFieldModel>();
            int totalCards = 6;
            var imageLoader = Substitute.For<ILoadImage>();
            imageLoader.LoadRandomTexture(Arg.Any<int>(), Arg.Any<int>())
                .Returns(new Texture2D(200, 200));
            gameFieldModel.ConstructFromCode(totalCards, imageLoader);
            gameFieldModel.Start();

            var cardsID = gameFieldModel.Cards.Select(c => c.GetInstanceID()).ToList();

            // act
            gameFieldModel.Refresh();

            var refreshedIds = gameFieldModel.Cards.Select(c => c.GetInstanceID()).ToList();
            // assert
            List<bool> notRefreshed = new List<bool>();
            
            for (int i = 0; i < cardsID.Count(); i++)
            {
                notRefreshed.Add(cardsID[i] == refreshedIds[i]);    
            }

            notRefreshed.Count(result => result).ShouldBeEqualTo(0);
        }
    }
}