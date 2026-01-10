using Bunit;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using WordSearch.Web.Layout;
using Xunit;

namespace WordSearch.Tests.Ui.Components;

public class NavBarTests : TestContext
{
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

        var guideLink = cut.Find("a[href='guide/']");
        guideLink.Should().NotBeNull();
        guideLink.TextContent.Should().Be("User Guide");
        guideLink.GetAttribute("target").Should().Be("_blank");
    }

    [Fact]
    public void NavBar_ShouldRenderAboutLink()
    {
        var cut = RenderComponent<NavBar>();

        cut.Markup.Should().Contain("About");
        cut.Markup.Should().Contain("/about");
    }

    [Fact]
    public void NavBar_ShouldHaveWordSearchLink_WhenNotOnHomePage()
    {
        var navManager = Services.GetRequiredService<NavigationManager>();
        navManager.NavigateTo("/about");

        var cut = RenderComponent<NavBar>();

        cut.Markup.Should().Contain("Word Search");
    }

    [Fact]
    public void NavBar_ShouldNotHaveWordSearchLink_WhenOnHomePage()
    {
        var cut = RenderComponent<NavBar>();

        // Default navigation is to root "/"
        // Word Search link should not appear on home page
        var links = cut.FindAll("a").Select(a => a.TextContent).ToList();
        // The NavLink to "/" with text "Word Search" should not be present
        cut.Markup.Should().NotContain(">Word Search</a>");
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
    public void NavBar_ShouldRenderLinks_InNavbarLinksSpan()
    {
        var cut = RenderComponent<NavBar>();

        var linksSection = cut.Find(".navbar-links");
        linksSection.InnerHtml.Should().Contain("User Guide");
        linksSection.InnerHtml.Should().Contain("About");
    }

    [Fact]
    public void NavBar_ShouldUpdateCurrentPath_OnNavigation()
    {
        var cut = RenderComponent<NavBar>();
        var navManager = Services.GetRequiredService<NavigationManager>();

        // Initially on home page
        cut.Markup.Should().NotContain(">Word Search</a>");

        // Navigate to about page
        navManager.NavigateTo("/about");
        cut.Render();

        // Now Word Search link should appear
        cut.Markup.Should().Contain("Word Search");
    }
}
