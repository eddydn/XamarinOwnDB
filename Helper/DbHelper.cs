using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Database.Sqlite;
using System.IO;

namespace XamarinOwnDB.Helper
{
    public class DbHelper : SQLiteOpenHelper
    {
        private static string DB_PATH = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        private static string DB_NAME = "MyDB.db";
        private static int VERSION = 1;
        private Context context;

        public DbHelper(Context context):base(context,DB_NAME,null,VERSION)
        {
            this.context = context;
        }

        private string GetSQLiteDBPath()
        {
            return Path.Combine(DB_PATH, DB_NAME);
        }

        public override SQLiteDatabase WritableDatabase
        {
            get
            {
                return CreateSQLiteDB();
            }
        }

        private SQLiteDatabase CreateSQLiteDB()
        {
            SQLiteDatabase sqliteDB=null;
            string path = GetSQLiteDBPath();
            Stream streamSQLite = null;
            FileStream streamWriter = null;
            Boolean isSQLiteInit = false;
            try
            {
                if (File.Exists(path))
                    isSQLiteInit = true;
                else
                {
                    streamSQLite = context.Resources.OpenRawResource(Resource.Raw.MyDB);
                    streamWriter = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
                    if(streamSQLite != null && streamWriter != null)
                    {
                        if (CopySQLiteDB(streamSQLite, streamWriter))
                            isSQLiteInit = true;
                    }
                }
                if (isSQLiteInit)
                    sqliteDB = SQLiteDatabase.OpenDatabase(path, null, DatabaseOpenFlags.OpenReadonly);
            }
            catch { }
            return sqliteDB;
        }

        private bool CopySQLiteDB(Stream streamSQLite, FileStream streamWriter)
        {
            bool isSuccess = false;
            int length = 256;
            Byte[] buffer = new Byte[length];
            try
            {
                int bytesRead = streamSQLite.Read(buffer, 0, length);
                while(bytesRead > 0)
                {
                    streamWriter.Write(buffer, 0, bytesRead);
                    bytesRead = streamSQLite.Read(buffer, 0, length);
                }
                isSuccess = true;
            }
            catch { }
            finally
            {
                streamSQLite.Close();
                streamWriter.Close();
            }
            return isSuccess;
        }

        public override void OnCreate(SQLiteDatabase db)
        {
            throw new NotImplementedException();
        }

        public override void OnUpgrade(SQLiteDatabase db, int oldVersion, int newVersion)
        {
            throw new NotImplementedException();
        }
    }
}