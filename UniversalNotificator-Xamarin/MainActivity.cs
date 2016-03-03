using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Database;
using Model;
using System.Linq;

namespace UniversalNotificator_Xamarin
{
    [Activity(Label = "UniversalNotificator_Xamarin", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : ListActivity
    {
        int count = 1;

        private NotificationsDBAdapter dbHelper;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.MyButton);

            button.Click += delegate { button.Text = string.Format("{0} clicks!", count++); };

            this.dbHelper = new NotificationsDBAdapter(this);
            this.dbHelper.Open();
            this.FillData();
        }

        private void FillData()
        {
            var repo = new RemoteRepository();
            var result = repo.GetAllEntries();
            var items = result.Select(x => x.title).ToArray();
            ListAdapter = new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleListItem1, items);
        }
    }
}

