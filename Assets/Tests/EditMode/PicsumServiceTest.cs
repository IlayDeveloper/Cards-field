using System.Threading.Tasks;
using FluentAssert;
using NSubstitute;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityHttpWrapper;

namespace Tests.EditMode
{
    public class PicsumServiceTest
    {
        [Test]
        public async Task WhenLoadRandomTextureAndHttpWorksFine_ThenReturnNotNullAndNotEmptyTexture()
        {
            // prepare
            var gameObject = new GameObject();
            var service = gameObject.AddComponent<PicsumService>();
            var httpClient = Substitute.For<IHttpClient>();
            var mockImage = MockImage;
            var data = mockImage.EncodeToJPG();
            httpClient.SendRequest(Arg.Any<string>()).Returns(data);
            service.Resolve(httpClient);

            // act
            Task<Texture2D> request = service.LoadRandomTexture(200, 200);
            await request;

            // assert
            request.Result.ShouldNotBeNull();
            request.Result.width.ShouldBeEqualTo(mockImage.width);
            request.Result.height.ShouldBeEqualTo(mockImage.height);
        }

        private static Texture2D MockImage => AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Tests/SampleData/1.jpg");
    }
}