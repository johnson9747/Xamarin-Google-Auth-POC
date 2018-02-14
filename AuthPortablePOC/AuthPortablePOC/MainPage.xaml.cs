using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Auth;
using Xamarin.Forms;

namespace AuthPortablePOC
{
	public partial class MainPage : ContentPage
	{
		Xamarin.Auth.OAuth2Authenticator authenticator = null;
		public MainPage()
		{			
			InitializeComponent();
			BypassLoginAsync();
			btnShowProfile.Clicked += ShowProfileButtonClicked;
			btnClear.Clicked += ClearProfileDetails;
			btnRefreshToken.Clicked += GetRefreshToken;
			btnLogout.Clicked += LogoutAsync;
		}

		private async void LogoutAsync(object sender, EventArgs e)
		{
			var account = AccountStore.Create().FindAccountsForService(Constants.AppName).FirstOrDefault();
			if (account == null)
			{
				await Navigation.PushModalAsync(new Login());
				return;
			}
			var httpClient = new HttpClient();
			var pairs = new List<KeyValuePair<string, string>>
			{
				new KeyValuePair<string, string>("token",account.Properties["access_token"].ToString())
			};
			var content = new FormUrlEncodedContent(pairs);
			var response = httpClient.PostAsync(Constants.revokeTokenUrl, content).Result;
			if (response.IsSuccessStatusCode)
			{
				await DisplayAlert("Info", "Your access revoked", "OK");
				AccountStore.Create().Delete(account, Constants.AppName);
				await Navigation.PushModalAsync(new Login());
			}
		}

		private async void BypassLoginAsync()
		{
			if (await AuthChecks.CheckIfTokenExpiredAsync())
			{
				await Navigation.PushModalAsync(new Login());
			}
		}
		async void GetRefreshToken(object sender, EventArgs e)
		{
			await GetRefreshToken();
		}

		private async Task GetRefreshToken()
		{
			try
			{
				var account = AccountStore.Create().FindAccountsForService(Constants.AppName).FirstOrDefault();
				if (account == null)
				{
					await Navigation.PushModalAsync(new Login());
					return;
				}
				var refreshToken = account.Properties["refresh_token"];

				if (string.IsNullOrWhiteSpace(refreshToken))
					return;
				var queryValues = new Dictionary<string, string>
			{
				{"refresh_token", refreshToken},
				{"client_id", Constants.AndroidClientId},
				{"grant_type", "refresh_token"},
				{"client_secret", null},
			};
				authenticator = new OAuth2Authenticator(
										Constants.AndroidClientId,
										null,
										Constants.Scope,
										new Uri(Constants.AuthorizeUrl),
										new Uri(Constants.AndroidRedirectUrl),
										new Uri(Constants.AccessTokenUrl),
										null,
										true);

				var result = await authenticator.RequestAccessTokenAsync(queryValues);

				if (result.ContainsKey("access_token"))
					account.Properties["access_token"] = result["access_token"];

				if (result.ContainsKey("refresh_token"))
					account.Properties["refresh_token"] = result["refresh_token"];

				//save account to account store
				AccountStore.Create().Save(account, Constants.AppName);

				await DisplayAlert("Info", "Refresh succeeded", "Ok");
			}
			catch (Exception ex)
			{
				await DisplayAlert("Error", "Refresh failed " + ex.Message, "Ok");
			}
		}

		private void ClearProfileDetails(object sender, EventArgs e)
		{
			lblName.Text = "";
			lblEmail.Text = "";
			imgPRofile.Source = null;
		}

		async void ShowProfileButtonClicked(object sender, EventArgs e)
		{
			if (await AuthChecks.CheckIfTokenExpiredAsync())
			{
				await GetRefreshToken();
			}
			await GetProfileDetails();
		}
		//private async Task<bool> CheckIfTokenExpiredAsync()
		//{
		//	try
		//	{
		//		var account = AccountStore.Create().FindAccountsForService(Constants.AppName).FirstOrDefault();
		//		var request = new OAuth2Request("GET", new Uri(Constants.tokenInfoUrl), null, account);
		//		var response = await request.GetResponseAsync();

		//		var text = response.GetResponseText();

		//		var json = JObject.Parse(text);
		//		double expiry = (double)json["expires_in"];
		//		if (expiry > 10)
		//			return false;
		//		else
		//			return true;
		//	}
		//	catch
		//	{
		//	}
		//	return true;
		//}

		private async Task GetProfileDetails()
		{
			try
			{
				var account = AccountStore.Create().FindAccountsForService(Constants.AppName).FirstOrDefault();
				if (account == null)
					return;
				var request = new OAuth2Request("GET", new Uri(Constants.UserInfoUrl), null, account);
				var response = await request.GetResponseAsync();

				var text = response.GetResponseText();

				var json = JObject.Parse(text);
				lblName.Text = "Name: " + (string)json["name"];
				lblEmail.Text = "Email: " + (string)json["email"];
				var imageRequest = new OAuth2Request("GET", new Uri((string)json["picture"]), null, account);
				var stream = await (await imageRequest.GetResponseAsync()).GetResponseStreamAsync();
				imgPRofile.Source = ImageSource.FromStream(() => stream);
			}
			catch
			{
				await DisplayAlert("Error", "An errror occured while fetching user details", "OK");
			}
		}
	}
}
