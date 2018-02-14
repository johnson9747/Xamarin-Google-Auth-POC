using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Auth;

namespace AuthPortablePOC
{
    public class AuthChecks
    {
		public static async Task<bool> CheckIfTokenExpiredAsync()
		{
			try
			{
				var account = AccountStore.Create().FindAccountsForService(Constants.AppName).FirstOrDefault();
				if (account == null)
					return true;
				var request = new OAuth2Request("GET", new Uri(Constants.tokenInfoUrl), null, account);
				var response = await request.GetResponseAsync();

				var text = response.GetResponseText();

				var json = JObject.Parse(text);
				double expiry = (double)json["expires_in"];
				if (expiry > 10)
					return false;
				else
					return true;
			}
			catch
			{
			}
			return true;
		}
	}
}
