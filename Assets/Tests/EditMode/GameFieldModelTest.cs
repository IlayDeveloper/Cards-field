using UnityEngine;

namespace Tests.EditMode
{
    public class GameFieldModelTest
    {
        public void WhenGameFieldInitialized_ThenAllCardsImageLoaded()
        {
            // prepare
            var gameObject = new GameObject();
            GameFieldModel gameFieldModel = gameObject.AddComponent<GameFieldModel>();

            // act
            gameFieldModel.Start();
            
            // assert
            gameFieldModel.Cards.All( c => c.ShouldNotBeNull());
        }
    }
}