using Domain.Categorys;
using Domain.Posts;

namespace UnitTest.Core.Domain
{
    public class PostDomainTest
    {
        [Fact]
        public void Post_Should_Have_Valid_Properties()
        {
            // Arrange
            var post = new Post
            {
                Id = 1,
                Titel = "Sample Title",
                PostDescription = "This is a sample description.",
                Content = "This is the content of the post.",
                TimeRequired = 5,
                UserId = "user123",
                VisitCount = 10,
                CategoryTypeId = 2,
                CategoryType = new CategoryType(),
                PostImages = new List<PostImage>(),
                PostFavourites = new List<PostFavorite>()
            };

            // Act & Assert
            Assert.Equal(1, post.Id);
            Assert.Equal("Sample Title", post.Titel);
            Assert.Equal("This is a sample description.", post.PostDescription);
            Assert.Equal("This is the content of the post.", post.Content);
            Assert.Equal(5, post.TimeRequired);
            Assert.Equal("user123", post.UserId);
            Assert.Equal(10, post.VisitCount);
            Assert.Equal(2, post.CategoryTypeId);
            Assert.NotNull(post.CategoryType);
            Assert.NotNull(post.PostImages);
            Assert.NotNull(post.PostFavourites);
        }

        [Fact]
        public void PostImages_Should_Be_Initialized()
        {
            // Arrange
            var post = new Post();

            // Act
            post.PostImages = new List<PostImage>();

            // Assert
            Assert.NotNull(post.PostImages);
            Assert.Empty(post.PostImages);
        }

        [Fact]
        public void PostFavourites_Should_Be_Initialized()
        {
            // Arrange
            var post = new Post();

            // Act
            post.PostFavourites = new List<PostFavorite>();

            // Assert
            Assert.NotNull(post.PostFavourites);
            Assert.Empty(post.PostFavourites);
        }
    }
}
