﻿using System;
using System.Linq;
using System.Threading.Tasks;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Bunit;
using FluentAssertions;
using MudBlazor.Docs.Examples;
using MudBlazor.UnitTests.TestComponents;
using NUnit.Framework;
using static Bunit.ComponentParameterFactory;

namespace MudBlazor.UnitTests.Components
{
    [TestFixture]
    public class ButtonsTests : BunitTest
    {
        /// <summary>
        /// MudButton without specifying HtmlTag, renders a button
        /// </summary>
        [Test]
        public void MudButtonShouldRenderAButtonByDefault()
        {
            var comp = Context.RenderComponent<MudButton>();
            //no HtmlTag nor Link properties are set, so HtmlTag is button by default
            comp.Instance
                .HtmlTag
                .Should()
                .Be("button");
            //it is a button, and has by default stopPropagation on onclick
            comp.Markup
                .Replace(" ", string.Empty)
                .Should()
                .StartWith("<button")
                .And
                .Contain("stopPropagation");
        }

        /// <summary>
        /// MudButton renders an anchor element when Link is set
        /// </summary>
        [Test]
        public void MudButtonShouldRenderAnAnchorIfLinkIsSetAndIsNotDisabled()
        {
            var link = Parameter(nameof(MudButton.Href), "https://www.google.com");
            var target = Parameter(nameof(MudButton.Target), "_blank");
            var disabled = Parameter(nameof(MudButton.Disabled), true);
            var comp = Context.RenderComponent<MudButton>(link, target);
            //Link property is set, so it has to render an anchor element
            comp.Instance
                .HtmlTag
                .Should()
                .Be("a");
            //Target property is set, so it must have the rel attribute set to noopener
            comp.Markup
                .Should()
                .Contain("rel=\"noopener\"");
            //it is an anchor and not contains stopPropagation 
            comp.Markup
                .Replace(" ", string.Empty)
                .Should()
                .StartWith("<a")
                .And
                .NotContain("__internal_stopPropagation_onclick");

            comp = Context.RenderComponent<MudButton>(link, target, disabled);
            comp.Instance.HtmlTag.Should().Be("button");

        }

        /// <summary>
        /// MudButton whithout specifying HtmlTag, renders a button
        /// </summary>
        [Test]
        public void MudIconButtonShouldRenderAButtonByDefault()
        {
            var comp = Context.RenderComponent<MudIconButton>();
            //no HtmlTag nor Link properties are set, so HtmlTag is button by default
            comp.Instance
                .HtmlTag
                .Should()
                .Be("button");
            //it is a button
            comp.Markup
                .Replace(" ", string.Empty)
                .Should()
                .StartWith("<button");
        }

        /// <summary>
        /// MudButton renders an anchor element when Link is set
        /// </summary>
        [Test]
        public void MudIconButtonShouldRenderAnAnchorIfLinkIsSet()
        {
            using var ctx = new Bunit.TestContext();
            var link = Parameter(nameof(MudIconButton.Href), "https://www.google.com");
            var target = Parameter(nameof(MudIconButton.Target), "_blank");
            var comp = ctx.RenderComponent<MudIconButton>(link, target);
            //Link property is set, so it has to render an anchor element
            comp.Instance
                .HtmlTag
                .Should()
                .Be("a");
            //Target property is set, so it must have the rel attribute set to noopener
            comp.Markup
                .Should()
                .Contain("rel=\"noopener\"");
            //it is an anchor
            comp.Markup
                .Replace(" ", string.Empty)
                .Should()
                .StartWith("<a");
        }

        /// <summary>
        /// MudButton whithout specifying HtmlTag, renders a button
        /// </summary>
        [Test]
        public void MudFabShouldRenderAButtonByDefault()
        {
            var comp = Context.RenderComponent<MudFab>();
            //no HtmlTag nor Link properties are set, so HtmlTag is button by default
            comp.Instance
                .HtmlTag
                .Should()
                .Be("button");
            //it is a button
            comp.Markup
                .Replace(" ", string.Empty)
                .Should()
                .StartWith("<button");
        }

        /// <summary>
        /// MudButton renders an anchor element when Link is set
        /// </summary>
        [Test]
        public void MudFabShouldRenderAnAnchorIfLinkIsSet()
        {
            var link = Parameter(nameof(MudFab.Href), "https://www.google.com");
            var target = Parameter(nameof(MudFab.Target), "_blank");
            var comp = Context.RenderComponent<MudFab>(link, target);
            //Link property is set, so it has to render an anchor element
            comp.Instance
                .HtmlTag
                .Should()
                .Be("a");
            //Target property is set, so it must have the rel attribute set to noopener
            comp.Markup
                .Should()
                .Contain("rel=\"noopener\"");
            //it is an anchor
            comp.Markup
                .Replace(" ", string.Empty)
                .Should()
                .StartWith("<a");
        }

        /// <summary>
        /// MudFab should only render an icon if one is specified.
        /// </summary>
        [Test]
        public void MudFabShouldNotRenderIconIfNoneSpecified()
        {
            var comp = Context.RenderComponent<MudFab>();
            comp.Markup
                .Should()
                .NotContainAny("mud-icon-root");
        }

        /// <summary>
        /// MudIconButton should have a title tag/attribute if specified
        /// </summary>
        [Test]
        public void ShouldRenderTitle()
        {
            var title = "Title and tooltip";
            var icon = Parameter(nameof(MudIconButton.Icon), Icons.Material.Filled.Add);
            var titleParam = Parameter(nameof(MudIconButton.Title), title);
            var comp = Context.RenderComponent<MudIconButton>(icon, titleParam);
            comp.Find($"button[title=\"{title}\"]");

            icon = Parameter(nameof(MudIconButton.Icon), "customicon");
            comp.SetParametersAndRender(icon, titleParam);
            comp.Find($"button[title=\"{title}\"]");
        }

        [Test]
        public async Task MudToggleIconTest()
        {
            var comp = Context.RenderComponent<MudToggleIconButton>();
#pragma warning disable BL0005
            await comp.InvokeAsync(() => comp.Instance.Disabled = true);
            await comp.InvokeAsync(() => comp.Instance.SetToggledAsync(true));
            comp.WaitForAssertion(() => comp.Instance.Toggled.Should().BeFalse());
        }

        [Test]
        public void MudButtonSizesTest()
        {
            var comp = Context.RenderComponent<ButtonSizeIconSizeTest>();

            var buttons = comp.Nodes.Where(n => n.NodeName.Equals("BUTTON")).ToArray();
            buttons.Length.Should().Be(6);

            // Buttons 1-3: Explicit button sizes
            ((IHtmlButtonElement)buttons[0]).ClassList.Contains("mud-button-filled-size-small").Should().BeTrue();  // Size="Size.Small"
            ((IHtmlButtonElement)buttons[1]).ClassList.Contains("mud-button-filled-size-medium").Should().BeTrue(); // Size="Size.Medium"
            ((IHtmlButtonElement)buttons[2]).ClassList.Contains("mud-button-filled-size-large").Should().BeTrue();  // Size="Size.Large"
        }

        [Test]
        public void MudButtonIconSizesTest()
        {
            var comp = Context.RenderComponent<ButtonSizeIconSizeTest>();

            var buttons = comp.Nodes.Where(n => n.NodeName.Equals("BUTTON")).ToArray();

            // Button 4: Small button- with large icon size: Size="Size.Small", IconSize="Size.Large"
            ((IHtmlButtonElement)buttons[3]).ClassList.Contains("mud-button-filled-size-small").Should().BeTrue();
            var button4Span = ((IHtmlButtonElement)buttons[3]).Children[0].Children[0];
            button4Span.ClassName.Contains("mud-button-icon-size-large").Should().BeTrue();
            var button4Svg = button4Span.Children[0];
            button4Svg.ClassName.Contains("mud-icon-size-large").Should().BeTrue();

            // Button 5: Defaults: Medium button- and icon size.
            ((IHtmlButtonElement)buttons[4]).ClassList.Contains("mud-button-filled-size-medium").Should().BeTrue();
            var button5Span = ((IHtmlButtonElement)buttons[4]).Children[0].Children[0];
            button5Span.ClassName.Contains("mud-button-icon-size-medium").Should().BeTrue();
            var button5Svg = button5Span.Children[0];
            button5Svg.ClassName.Contains("mud-icon-size-medium").Should().BeTrue();

            // Button 6: Large button- with small icon size: Size="Size.Large", IconSize="Size.Small"
            ((IHtmlButtonElement)buttons[5]).ClassList.Contains("mud-button-filled-size-large").Should().BeTrue();
            var button6Span = ((IHtmlButtonElement)buttons[5]).Children[0].Children[0];
            button6Span.ClassName.Contains("mud-button-icon-size-small").Should().BeTrue();
            var button6Svg = button6Span.Children[0];
            button6Svg.ClassName.Contains("mud-icon-size-small").Should().BeTrue();
        }

        /// <summary>
        /// Ensures buttons inherit their disabled state
        /// </summary>
        [Test]
        public void ButtonsNestedDisabledTest()
        {
            var comp = Context.RenderComponent<ButtonsNestedDisabledTest>();

            comp.FindComponent<MudButton>().Find("button").HasAttribute("disabled").Should().BeFalse();
            comp.FindComponent<MudFab>().Find("button").HasAttribute("disabled").Should().BeFalse();
            comp.FindComponent<MudIconButton>().Find("button").HasAttribute("disabled").Should().BeFalse();

            comp.SetParametersAndRender(parameters => parameters.Add(x => x.Disabled, true)); //buttons should be disabled when the cascading value is disabled

            comp.FindComponent<MudButton>().Find("button").HasAttribute("disabled").Should().BeTrue();
            comp.FindComponent<MudFab>().Find("button").HasAttribute("disabled").Should().BeTrue();
            comp.FindComponent<MudIconButton>().Find("button").HasAttribute("disabled").Should().BeTrue();
        }
    }
}
