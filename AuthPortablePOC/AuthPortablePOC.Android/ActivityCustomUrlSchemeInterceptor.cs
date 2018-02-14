﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace AuthPortablePOC.Droid
{
	[Activity(Label = "CustomUrlSchemeInterceptorActivity", NoHistory = true, LaunchMode = LaunchMode.SingleTop)]
	[IntentFilter(
	new[] { Intent.ActionView },
	Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
	DataSchemes = new[] { "com.xamarin.traditional.standard.samples.oauth.providers.android",
						"<your redirect url here>",
	"xamarin-auth"},
	DataPath = "/oauth2redirect")]
	public class CustomUrlSchemeInterceptorActivity : Activity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			VerifyIntent(this.Intent);
			global::Android.Net.Uri uri_android = Intent.Data;
			// Convert Android.Net.Uri to C#/netxf/BCL System.Uri - common API
			Uri uri_netfx = new Uri(uri_android.ToString());
			// Load redirectUrl page
			AuthenticationState.Authenticator.OnPageLoading(uri_netfx);
			this.Finish();
			return;
		}
		protected override void OnNewIntent(Intent intent)
		{
			VerifyIntent(intent);

			global::Android.Net.Uri uri_android = intent.Data;

#if DEBUG
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			sb.AppendLine("ActivityCustomUrlSchemeInterceptor.OnNewIntent(Intent intent)");
			sb.Append("     uri_android = ").AppendLine(uri_android.ToString());
			System.Diagnostics.Debug.WriteLine(sb.ToString());
#endif

			return;
		}
		protected void VerifyIntent(Intent intent)
		{
			if (intent.DataString != null && Intent.ActionView == intent.Action)
			{
				// deep link or custom schema
				// identify which schema

#if DEBUG
				System.Text.StringBuilder sb = new System.Text.StringBuilder();
				sb.AppendLine("ActivityCustomUrlSchemeInterceptor.VerifyIntent(Intent intent)");
				System.Diagnostics.Debug.WriteLine(sb.ToString());
#endif

			}

			return;
		}
	}
}