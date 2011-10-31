﻿using NUnit.Framework;
using SpecsFor.Demo.Web.Controllers;
using SpecsFor.Demo.Web.Models;
using SpecsFor.Web;
using Should;
using MvcContrib.TestHelper;

namespace SpecsFor.Demo.Web.UITests
{
	public class LoginSpecs
	{
		public class when_logging_in_with_an_invalid_username_and_password : SpecsFor<MvcWebApp>
		{
			protected override void Given()
			{
				SUT.NavigateTo<AccountController>(c => c.LogOn());
			}

			protected override void When()
			{
				SUT.FindFormFor<LogOnModel>()
					//TODO: Eventually expose a fluent API like:
					//.ForField(m => m.UserName).TypeText("bad@user.com")
					.SetFieldValue(m => m.UserName, "bad@user.com")
					.SetFieldValue(m => m.Password, "BadPass")
					.Submit();

				SUT.WaitForComplete();
			}

			[Test]
			public void then_it_should_redisplay_the_page()
			{
				SUT.Route.ShouldMapTo<AccountController>(c => c.LogOn());
			}

			[Test]
			public void then_it_should_contain_a_validation_error()
			{
				SUT.ValidationSummary.Text.ShouldContain("The user name or password provided is incorrect.");
			}
		}

		public class when_logging_in_with_valid_credentials : SpecsFor<MvcWebApp>
		{
			protected override void Given()
			{
				SUT.NavigateTo<AccountController>(c => c.LogOn());
			}

			protected override void When()
			{
				SUT.FindFormFor<LogOnModel>()
					.SetFieldValue(m => m.UserName, "mhoneycutt")
					.SetFieldValue(m => m.Password, "Testing")
					.Submit();

				SUT.WaitForComplete();
			}

			[Test]
			public void then_it_redirects_to_the_home_page()
			{
				SUT.Route.ShouldMapTo<HomeController>(c => c.Index());
			}
		}
	}
}