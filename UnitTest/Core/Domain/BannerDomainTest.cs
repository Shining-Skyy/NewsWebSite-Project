using Domain.Banners;

namespace UnitTest.Core.Domain
{
    public class BannerDomainTest
    {
        [Fact]
        public void Banner_Constructor_Should_Set_Properties_Correctly()
        {
            // Arrange
            var banner = new Banner
            {
                Id = 1,
                Name = "Test Banner",
                Image = "test_image.jpg",
                Link = "http://example.com",
                IsActive = true,
                Priority = 2,
                Position = Position.Line_1
            };

            // Act & Assert
            Assert.Equal(1, banner.Id);
            Assert.Equal("Test Banner", banner.Name);
            Assert.Equal("test_image.jpg", banner.Image);
            Assert.Equal("http://example.com", banner.Link);
            Assert.True(banner.IsActive);
            Assert.Equal(2, banner.Priority);
            Assert.Equal(Position.Line_1, banner.Position);
        }

        [Fact]
        public void Position_Enum_Should_Have_Correct_Values()
        {
            // Act & Assert
            Assert.Equal(0, (int)Position.Slider);
            Assert.Equal(1, (int)Position.Line_1);
            Assert.Equal(2, (int)Position.Line_2);
            Assert.Equal(3, (int)Position.Line_3);
            Assert.Equal(4, (int)Position.Line_4);
            Assert.Equal(5, (int)Position.Line_5);
        }
    }
}
