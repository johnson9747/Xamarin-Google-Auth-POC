using System;
using System.Collections.Generic;
using System.Text;

namespace AuthPortablePOC
{
	public static class Constants
	{
		// OAuth
		public static string AppName = "TestAuthPortable";
		// For Google login, configure at https://console.developers.google.com/		
		public static string AndroidClientId = "<yourclient_id from google>";

		// These values do not need changing
		public static string Scope = "profile https://www.googleapis.com/auth/userinfo.email";		
		public static string AuthorizeUrl = "https://accounts.google.com/o/oauth2/auth";
		public static string AccessTokenUrl = "https://www.googleapis.com/oauth2/v4/token";
		public static string UserInfoUrl = "https://www.googleapis.com/oauth2/v2/userinfo";
		public static string revokeTokenUrl = "https://accounts.google.com/o/oauth2/revoke";
		public static string tokenInfoUrl = "https://www.googleapis.com/oauth2/v1/tokeninfo";
		// Set these to reversed iOS/Android client ids, with :/oauth2redirect appended		
		public static string AndroidRedirectUrl = "<your redirecturl>:/oauth2redirect";
	}
}
