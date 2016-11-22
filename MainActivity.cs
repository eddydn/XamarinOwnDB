using Android.App;
using Android.Widget;
using Android.OS;
using XamarinOwnDB.Helper;
using Android.Database.Sqlite;
using System;
using Android.Views;
using Android.Content;
using Android.Database;
using System.Collections.Generic;

namespace XamarinOwnDB
{
    [Activity(Label = "XamarinOwnDB", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        DbHelper db;
        SQLiteDatabase sqliteDB;
        LinearLayout container;
        Button btnGetData;
        List<Account> lstUser = new List<Account>();
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);

            db = new DbHelper(this);
            sqliteDB = db.WritableDatabase;

            container = FindViewById<LinearLayout>(Resource.Id.container);
            btnGetData = FindViewById<Button>(Resource.Id.btnGetData);
            btnGetData.Click += delegate {
                addData();
            };
            
        }

        private void addData()
        {
            ICursor selectData = sqliteDB.RawQuery("select * from Account",new string[]{ });
            if(selectData.Count > 0)
            {
                selectData.MoveToFirst();
                do
                {
                    Account user = new Account();
                    user.UserName = selectData.GetString(selectData.GetColumnIndex("UserName"));
                    user.Email = selectData.GetString(selectData.GetColumnIndex("Email"));

                    lstUser.Add(user);
                }
                while (selectData.MoveToNext());
                selectData.Close();
            }
            foreach(var item in lstUser)
            {
                LayoutInflater layoutInflater = (LayoutInflater)BaseContext.GetSystemService(Context.LayoutInflaterService);
                View addView = layoutInflater.Inflate(Resource.Layout.row, null);
                TextView txtUser = addView.FindViewById<TextView>(Resource.Id.txtUser);
                TextView txtEmail = addView.FindViewById<TextView>(Resource.Id.txtEmail);

                txtUser.Text = item.UserName;
                txtEmail.Text = item.Email;

                container.AddView(addView);

            }
        }
    }
}

