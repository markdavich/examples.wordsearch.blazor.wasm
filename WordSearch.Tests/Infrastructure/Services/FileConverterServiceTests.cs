using FluentAssertions;
using WordSearch.Infrastructure.Services;

namespace WordSearch.Tests.Infrastructure.Services;

public class FileConverterServiceTests
{
    private readonly FileConverterService _converter = new();

    #region ToBase64Async Tests

    [Fact]
    public async Task ToBase64Async_ShouldConvertStreamToBase64()
    {
        var content = "Hello, World!";
        using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(content));

        var result = await _converter.ToBase64Async(stream);

        result.Should().Be(Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(content)));
    }

    [Fact]
    public async Task ToBase64Async_ShouldHandleEmptyStream()
    {
        using var stream = new MemoryStream();

        var result = await _converter.ToBase64Async(stream);

        result.Should().Be(string.Empty);
    }

    [Fact]
    public async Task ToBase64Async_ShouldHandleBinaryData()
    {
        var binaryData = new byte[] { 0x00, 0xFF, 0x7F, 0x80 };
        using var stream = new MemoryStream(binaryData);

        var result = await _converter.ToBase64Async(stream);

        result.Should().Be(Convert.ToBase64String(binaryData));
    }

    #endregion

    #region ReadTextAsync Tests

    [Fact]
    public async Task ReadTextAsync_ShouldReadTextContent()
    {
        var content = "Hello, World!";
        using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(content));

        var result = await _converter.ReadTextAsync(stream);

        result.Should().Be(content);
    }

    [Fact]
    public async Task ReadTextAsync_ShouldHandleMultilineText()
    {
        var content = "Line 1\nLine 2\nLine 3";
        using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(content));

        var result = await _converter.ReadTextAsync(stream);

        result.Should().Be(content);
    }

    [Fact]
    public async Task ReadTextAsync_ShouldHandleEmptyStream()
    {
        using var stream = new MemoryStream();

        var result = await _converter.ReadTextAsync(stream);

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task ReadTextAsync_ShouldHandleUnicode()
    {
        var content = "Hello, 世界! 🌍";
        using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(content));

        var result = await _converter.ReadTextAsync(stream);

        result.Should().Be(content);
    }

    #endregion

    #region IsPlainTextFile Tests

    [Theory]
    [InlineData("text/plain", "file.txt", true)]
    [InlineData("text/csv", "file.csv", true)]
    [InlineData("text/html", "file.html", true)]
    [InlineData("application/json", "file.json", true)]
    [InlineData("application/xml", "file.xml", true)]
    [InlineData("text/xml", "file.xml", true)]
    public void IsPlainTextFile_ShouldReturnTrue_ForTextMimeTypes(string mimeType, string fileName, bool expected)
    {
        var result = _converter.IsPlainTextFile(mimeType, fileName);

        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("application/octet-stream", "file.txt", true)]
    [InlineData("application/octet-stream", "file.csv", true)]
    [InlineData("application/octet-stream", "file.html", true)]
    [InlineData("application/octet-stream", "file.htm", true)]
    [InlineData("application/octet-stream", "file.json", true)]
    [InlineData("application/octet-stream", "file.xml", true)]
    [InlineData("application/octet-stream", "file.md", true)]
    [InlineData("application/octet-stream", "file.markdown", true)]
    public void IsPlainTextFile_ShouldReturnTrue_ForTextFileExtensions(string mimeType, string fileName, bool expected)
    {
        var result = _converter.IsPlainTextFile(mimeType, fileName);

        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("image/png", "file.png", false)]
    [InlineData("image/jpeg", "file.jpg", false)]
    [InlineData("application/pdf", "file.pdf", false)]
    [InlineData("application/octet-stream", "file.pdf", false)]
    [InlineData("application/octet-stream", "file.docx", false)]
    public void IsPlainTextFile_ShouldReturnFalse_ForNonTextFiles(string mimeType, string fileName, bool expected)
    {
        var result = _converter.IsPlainTextFile(mimeType, fileName);

        result.Should().Be(expected);
    }

    #endregion

    #region IsImageFile Tests

    [Theory]
    [InlineData("image/png", true)]
    [InlineData("image/jpeg", true)]
    [InlineData("image/jpg", true)]
    [InlineData("image/webp", true)]
    [InlineData("image/gif", true)]
    [InlineData("image/bmp", true)]
    [InlineData("image/tiff", true)]
    public void IsImageFile_ShouldReturnTrue_ForImageMimeTypes(string mimeType, bool expected)
    {
        var result = _converter.IsImageFile(mimeType);

        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("text/plain", false)]
    [InlineData("application/pdf", false)]
    [InlineData("application/json", false)]
    [InlineData("video/mp4", false)]
    public void IsImageFile_ShouldReturnFalse_ForNonImageMimeTypes(string mimeType, bool expected)
    {
        var result = _converter.IsImageFile(mimeType);

        result.Should().Be(expected);
    }

    #endregion

    #region IsSupportedFile Tests

    [Theory]
    [InlineData("image/png", true)]
    [InlineData("image/jpeg", true)]
    [InlineData("image/jpg", true)]
    [InlineData("image/webp", true)]
    [InlineData("image/gif", true)]
    [InlineData("image/bmp", true)]
    [InlineData("image/tiff", true)]
    public void IsSupportedFile_ShouldReturnTrue_ForImageTypes(string mimeType, bool expected)
    {
        var result = _converter.IsSupportedFile(mimeType);

        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("text/plain", true)]
    [InlineData("text/csv", true)]
    [InlineData("text/html", true)]
    [InlineData("application/json", true)]
    [InlineData("application/pdf", true)]
    [InlineData("application/vnd.openxmlformats-officedocument.wordprocessingml.document", true)]
    [InlineData("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", true)]
    [InlineData("application/msword", true)]
    [InlineData("application/vnd.ms-excel", true)]
    [InlineData("application/xml", true)]
    [InlineData("text/xml", true)]
    public void IsSupportedFile_ShouldReturnTrue_ForDocumentTypes(string mimeType, bool expected)
    {
        var result = _converter.IsSupportedFile(mimeType);

        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("video/mp4", false)]
    [InlineData("audio/mpeg", false)]
    [InlineData("application/zip", false)]
    [InlineData("application/x-executable", false)]
    public void IsSupportedFile_ShouldReturnFalse_ForUnsupportedTypes(string mimeType, bool expected)
    {
        var result = _converter.IsSupportedFile(mimeType);

        result.Should().Be(expected);
    }

    #endregion
}
