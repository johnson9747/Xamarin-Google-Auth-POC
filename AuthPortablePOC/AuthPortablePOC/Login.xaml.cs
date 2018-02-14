using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Auth;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AuthPortablePOC
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Login : ContentPage
	{
		public Login()
		{
			InitializeComponent();
			NavigationPage.SetHasBackButton(this, false);
			AuthentiacteUser();
		}		

		Xamarin.Auth.OAuth2Authenticator authenticator = null;				
		private void AuthentiacteUser()
		{
			authenticator = new OAuth2Authenticator(
				Constants.AndroidClientId,
				null,
				Constants.Scope,
				new Uri(Constants.AuthorizeUrl),
				new Uri(Constants.AndroidRedirectUrl),
				new Uri(Constants.AccessTokenUrl),
				null,
				true);
			authenticator.Completed += OnAuthCompletedAsync;
			authenticator.Error += Auth_Error;
			authenticator.BrowsingCompleted += Auth_BrowsingCompleted;
			// after initialization (creation and event subscribing) exposing local object 
			AuthenticationState.Authenticator = authenticator;

			// Presenters Implementation
			Xamarin.Auth.Presenters.OAuthLoginPresenter presenter = null;
			presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
			presenter.Login(authenticator);
			//////below is another implementation
			//Xamarin.Auth.XamarinForms.AuthenticatorPage ap;
			//ap = new Xamarin.Auth.XamarinForms.AuthenticatorPage()
			//{
			//	Authenticator = authenticator,
			//};
			//NavigationPage np = new NavigationPage(ap);
			//await Navigation.PushAsync(np);
		}

		private void Auth_BrowsingCompleted(object sender, EventArgs e)
		{					
			return;
		}

		private void Auth_Error(object sender, AuthenticatorErrorEventArgs e)
		{
			DisplayAlert("Error", "An Error Occured", "Done");
			return;
		}

		private async void OnAuthCompletedAsync(object sender, AuthenticatorCompletedEventArgs e)
		{
			if (e.IsAuthenticated)
			{
				try
				{
					//save account to account store
					AccountStore.Create().Save(e.Account, Constants.AppName);
					//------------------------------------------------------------------
					Account account = e.Account;
					string token = default(string);
					if (null != account)
					{
						string token_name = default(string);
						token_name = "access_token";
						token = account.Properties[token_name].ToString();
					}
					StringBuilder sb = new StringBuilder();
					sb.Append("IsAuthenticated  = ").Append(e.IsAuthenticated)
													.Append(System.Environment.NewLine);
					sb.Append("token            = ").Append(token)
													.Append(System.Environment.NewLine);

					await DisplayAlert("Info", sb.ToString(), "Done");
					await Navigation.PopModalAsync();
					await Navigation.PushAsync(new MainPage());

				}				
				catch (Exception ex)
				{
					await DisplayAlert("Error", ex.Message, "Done");
				}
			}
			else
			{
				await DisplayAlert("Error", "Authentication failed", "Done");
			}
		}		
	}
}