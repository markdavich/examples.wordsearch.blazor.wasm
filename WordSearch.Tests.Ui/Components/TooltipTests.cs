using Bunit;
using FluentAssertions;
using WordSearch.Web.Components;
using Xunit;

namespace WordSearch.Tests.Ui.Components;

public class TooltipTests : TestContext
{
    [Fact]
    public void Tooltip_ShouldRenderChildContent()
    {
        var cut = RenderComponent<Tooltip>(parameters => parameters
            .Add(p => p.Title, "Test Title")
            .Add(p => p.Description, "Test Description")
            .AddChildContent("<button>Click me</button>"));

        cut.Markup.Should().Contain("<button>Click me</button>");
    }

    [Fact]
    public void Tooltip_ShouldNotShowPopup_Initially()
    {
        var cut = RenderComponent<Tooltip>(parameters => parameters
            .Add(p => p.Title, "Test Title")
            .Add(p => p.Description, "Test Description")
            .AddChildContent("<button>Click me</button>"));

        cut.Find(".tooltip-wrapper").Should().NotBeNull();
        cut.Markup.Should().NotContain("tooltip-popup");
    }

    [Fact]
    public async Task Tooltip_ShouldShowPopup_OnMouseEnter()
    {
        var cut = RenderComponent<Tooltip>(parameters => parameters
            .Add(p => p.Title, "Test Title")
            .Add(p => p.Description, "Test Description")
            .AddChildContent("<button>Click me</button>"));

        await cut.Find(".tooltip-wrapper").TriggerEventAsync("onmouseenter", new Microsoft.AspNetCore.Components.Web.MouseEventArgs());

        cut.Markup.Should().Contain("tooltip-popup");
        cut.Markup.Should().Contain("Test Title");
        cut.Markup.Should().Contain("Test Description");
    }

    [Fact]
    public async Task Tooltip_ShouldHidePopup_OnMouseLeave()
    {
        var cut = RenderComponent<Tooltip>(parameters => parameters
            .Add(p => p.Title, "Test Title")
            .Add(p => p.Description, "Test Description")
            .AddChildContent("<button>Click me</button>"));

        var wrapper = cut.Find(".tooltip-wrapper");
        await wrapper.TriggerEventAsync("onmouseenter", new Microsoft.AspNetCore.Components.Web.MouseEventArgs());
        cut.Markup.Should().Contain("tooltip-popup");

        await wrapper.TriggerEventAsync("onmouseleave", new Microsoft.AspNetCore.Components.Web.MouseEventArgs());

        // Wait for the hide timer (300ms) plus a small buffer
        cut.WaitForState(() => !cut.Markup.Contains("tooltip-popup"), TimeSpan.FromMilliseconds(500));

        cut.Markup.Should().NotContain("tooltip-popup");
    }

    [Fact]
    public async Task Tooltip_ShouldShowGuideLink_WhenProvided()
    {
        var cut = RenderComponent<Tooltip>(parameters => parameters
            .Add(p => p.Title, "Test Title")
            .Add(p => p.Description, "Test Description")
            .Add(p => p.GuideLink, "guide/test-section.html")
            .AddChildContent("<button>Click me</button>"));

        await cut.Find(".tooltip-wrapper").TriggerEventAsync("onmouseenter", new Microsoft.AspNetCore.Components.Web.MouseEventArgs());

        cut.Markup.Should().Contain("tooltip-link");
        cut.Markup.Should().Contain("guide/test-section.html");
        cut.Markup.Should().Contain("Learn more in User Guide");
    }

    [Fact]
    public async Task Tooltip_ShouldNotShowGuideLink_WhenNotProvided()
    {
        var cut = RenderComponent<Tooltip>(parameters => parameters
            .Add(p => p.Title, "Test Title")
            .Add(p => p.Description, "Test Description")
            .AddChildContent("<button>Click me</button>"));

        await cut.Find(".tooltip-wrapper").TriggerEventAsync("onmouseenter", new Microsoft.AspNetCore.Components.Web.MouseEventArgs());

        cut.Markup.Should().NotContain("tooltip-link");
    }

    [Theory]
    [InlineData("top", "tooltip-top")]
    [InlineData("bottom", "tooltip-bottom")]
    [InlineData("left", "tooltip-left")]
    [InlineData("right", "tooltip-right")]
    public async Task Tooltip_ShouldApplyCorrectPositionClass(string position, string expectedClass)
    {
        var cut = RenderComponent<Tooltip>(parameters => parameters
            .Add(p => p.Title, "Test Title")
            .Add(p => p.Description, "Test Description")
            .Add(p => p.Position, position)
            .AddChildContent("<button>Click me</button>"));

        await cut.Find(".tooltip-wrapper").TriggerEventAsync("onmouseenter", new Microsoft.AspNetCore.Components.Web.MouseEventArgs());

        cut.Markup.Should().Contain(expectedClass);
    }

    [Fact]
    public async Task Tooltip_ShouldDefaultToTopPosition()
    {
        var cut = RenderComponent<Tooltip>(parameters => parameters
            .Add(p => p.Title, "Test Title")
            .Add(p => p.Description, "Test Description")
            .AddChildContent("<button>Click me</button>"));

        await cut.Find(".tooltip-wrapper").TriggerEventAsync("onmouseenter", new Microsoft.AspNetCore.Components.Web.MouseEventArgs());

        cut.Markup.Should().Contain("tooltip-top");
    }

    [Fact]
    public async Task Tooltip_ShouldRenderTitle_InStrongTag()
    {
        var cut = RenderComponent<Tooltip>(parameters => parameters
            .Add(p => p.Title, "My Title")
            .Add(p => p.Description, "My Description")
            .AddChildContent("<span>Content</span>"));

        await cut.Find(".tooltip-wrapper").TriggerEventAsync("onmouseenter", new Microsoft.AspNetCore.Components.Web.MouseEventArgs());

        var titleElement = cut.Find(".tooltip-title");
        titleElement.TextContent.Should().Be("My Title");
        titleElement.TagName.Should().Be("STRONG");
    }

    [Fact]
    public async Task Tooltip_ShouldRenderDescription_InParagraph()
    {
        var cut = RenderComponent<Tooltip>(parameters => parameters
            .Add(p => p.Title, "My Title")
            .Add(p => p.Description, "My Description")
            .AddChildContent("<span>Content</span>"));

        await cut.Find(".tooltip-wrapper").TriggerEventAsync("onmouseenter", new Microsoft.AspNetCore.Components.Web.MouseEventArgs());

        var descElement = cut.Find(".tooltip-text");
        descElement.TextContent.Should().Be("My Description");
        descElement.TagName.Should().Be("P");
    }
}
