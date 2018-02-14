package md5f3d8730488abcdde2517c249627c813f;


public class CustomUrlSchemeInterceptorActivity
	extends android.app.Activity
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"n_onNewIntent:(Landroid/content/Intent;)V:GetOnNewIntent_Landroid_content_Intent_Handler\n" +
			"";
		mono.android.Runtime.register ("AuthPortablePOC.Droid.CustomUrlSchemeInterceptorActivity, AuthPortablePOC.Android, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", CustomUrlSchemeInterceptorActivity.class, __md_methods);
	}


	public CustomUrlSchemeInterceptorActivity ()
	{
		super ();
		if (getClass () == CustomUrlSchemeInterceptorActivity.class)
			mono.android.TypeManager.Activate ("AuthPortablePOC.Droid.CustomUrlSchemeInterceptorActivity, AuthPortablePOC.Android, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);


	public void onNewIntent (android.content.Intent p0)
	{
		n_onNewIntent (p0);
	}

	private native void n_onNewIntent (android.content.Intent p0);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
