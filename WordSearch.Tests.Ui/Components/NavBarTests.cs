using Bunit;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using RichardSzalay.MockHttp;
using WordSearch.Application.Interfaces;
using WordSearch.Web.Layout;
using WordSearch.Web.Services;
using Xunit;

namespace WordSearch.Tests.Ui.Components;

public class NavBarTests : TestContext
{
    public NavBarTests()
    {
        // Register required services
        Services.AddSingleton<PuzzleService>();

        // Register mock HttpClient for ExampleSelector
        var mockHttp = new MockHttpMessageHandler();
        mockHttp.Fallback.Respond("text/plain", "test content");
        var httpClient = mockHttp.ToHttpClient();
        httpClient.BaseAddress = new Uri("http://localhost/");
        Services.AddSingleton(httpClient);

        // Register mock services for AIUpload
        var mockFileConverter = new Mock<IFileConverter>();
        mockFileConverter.Setup(x => x.IsSupportedFile(It.IsAny<string>())).Returns(true);
        Services.AddSingleton(mockFileConverter.Object);

        var mockPuzzleParser = new Mock<IPuzzleParserApi>();
        Services.AddSingleton(mockPuzzleParser.Object);
    }

    [Fact]
    public void NavBar_ShouldRenderTitle()
    {
        var cut = RenderComponent<NavBar>();

        cut.Markup.Should().Contain("Alphabet Soup");
    }

    [Fact]
    public void NavBar_ShouldRenderNavElement()
    {
        var cut = RenderComponent<NavBar>();

        cut.Find("nav").Should().NotBeNull();
        cut.Find(".navbar").Should().NotBeNull();
    }

    [Fact]
    public void NavBar_ShouldRenderUserGuideLink()
    {
        var cut = RenderComponent<NavBar>();

        var guideLink = cut.Find("a[href='guide/index.html']");
        guideLink.Should().NotBeNull();
        guideLink.TextContent.Should().Be("User Guide");
        guideLink.GetAttribute("target").Should().Be("_blank");
    }

    [Fact]
    public void NavBar_ShouldRenderDevelopersLink()
    {
        var cut = RenderComponent<NavBar>();

        var devLink = cut.Find("a[href='dev-guide/index.html']");
        devLink.Should().NotBeNull();
        devLink.TextContent.Should().Be("Developers");
        devLink.GetAttribute("target").Should().Be("_blank");
    }

    [Fact]
    public void NavBar_ShouldRenderAboutLink()
    {
        var cut = RenderComponent<NavBar>();

        cut.Markup.Should().Contain("About");
        cut.Markup.Should().Contain("/about");
    }

    [Fact]
    public void NavBar_ShouldHaveNavbarLinksSection()
    {
        var cut = RenderComponent<NavBar>();

        cut.Find(".navbar-links").Should().NotBeNull();
    }

    [Fact]
    public void NavBar_ShouldHaveNavbarTitleSection()
    {
        var cut = RenderComponent<NavBar>();

        var title = cut.Find(".navbar-title");
        title.Should().NotBeNull();
        title.TextContent.Should().Be("Alphabet Soup");
    }

    [Fact]
    public void NavBar_ShouldRenderLinks_InNavbarLinksSection()
    {
        var cut = RenderComponent<NavBar>();

        var linksSection = cut.Find(".navbar-links");
        linksSection.InnerHtml.Should().Contain("User Guide");
        linksSection.InnerHtml.Should().Contain("Developers");
        linksSection.InnerHtml.Should().Contain("About");
    }

    [Fact]
    public void NavBar_ShouldRenderUploadDropdown()
    {
        var cut = RenderComponent<NavBar>();

        cut.Find(".upload-dropdown").Should().NotBeNull();
    }

    [Fact]
    public void NavBar_ShouldRenderExampleSelector()
    {
        var cut = RenderComponent<NavBar>();

        cut.Find(".example-select").Should().NotBeNull();
    }

    [Fact]
    public void NavBar_ShouldRenderLogoLink()
    {
        var cut = RenderComponent<NavBar>();

        var logoLink = cut.Find(".navbar-logo-link");
        logoLink.Should().NotBeNull();
        logoLink.GetAttribute("href").Should().Be("/");
    }

    [Fact]
    public void NavBar_ShouldRenderSvgLogo()
    {
        var cut = RenderComponent<NavBar>();

        var logo = cut.Find(".navbar-logo");
        logo.Should().NotBeNull();
    }

    [Fact]
    public void NavBar_ShouldHaveBrandSection()
    {
        var cut = RenderComponent<NavBar>();

        cut.Find(".navbar-brand").Should().NotBeNull();
    }
}
