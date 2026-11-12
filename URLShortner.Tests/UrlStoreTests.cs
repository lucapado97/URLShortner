namespace URLShortner.Tests;

public class UrlStoreTests
{
    [Fact]
    public void AddUrl_DuplicateUrl()
    {
        // Arrange
        var store = new UrlStore();
        var url = "https://www.example.com";

        // Act
        var key1 = store.AddUrl(url);
        var key2 = store.AddUrl(url);

        // Assert
        Assert.Equal(key1, key2);
    }

    [Fact]
    public void AddUrl_DifferentUrls()
    {
        // Arrange
        var store = new UrlStore();

        // Act
        var key1 = store.AddUrl("https://www.google.com");
        var key2 = store.AddUrl("https://www.github.com");

        // Assert
        Assert.NotEqual(key1, key2);
    }


    [Fact]
    public void GetUrl_InvalidKey()
    {
        // Arrange
        var store = new UrlStore();
        var invalidKey = "999999";

        // Act
        var result = store.GetUrl(invalidKey);

        // Assert
        Assert.Null(result);
    }


    [Fact]
    public void GetUrl_ReturnsCorrectUrls()
    {
        // Arrange
        var store = new UrlStore();
        store.InitializeSampleUrls();

        // Act & Assert
        Assert.Equal("https://www.google.com", store.GetUrl("000001"));
        Assert.Equal("https://www.github.com", store.GetUrl("000002"));
        Assert.Equal("https://www.stackoverflow.com", store.GetUrl("000003"));
    }
}
