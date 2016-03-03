namespace UniversalNotificator_Xamarin
{
    using Android.Content;
    using Android.Database;
    using Android.Database.Sqlite;
    using Android.Util;
    using Model;
    class NotificationsDBAdapter
    {
        public const string KeyTitle = "title";
        public const string KeyBody = "body";
        public const string KeyRowId = "_id";

        private const string Tag = "NotificationsDBAdapter";

        private const string DatabaseCreate =
            "create table notifications (_id integer primary key autoincrement, "
            + "title text not null, body text not null);";

        private const string DBName = "data";
        private const string DatabaseTable = "notifications";
        private const int DatabaseVersion = 2;

        private DatabaseHelper dbHelper;
        private SQLiteDatabase db;

        private readonly Context ctx;

        public NotificationsDBAdapter(Context ctx)
        {
            this.ctx = ctx;
        }

        public NotificationsDBAdapter Open()
        {
            this.dbHelper = new DatabaseHelper(this.ctx);
            this.db = this.dbHelper.WritableDatabase;
            return this;
        }

        public void Close()
        {
            this.dbHelper.Close();
        }

        public long CreateNotification(string title, string body)
        {
            var initialValues = new ContentValues();
            initialValues.Put(KeyTitle, title);
            initialValues.Put(KeyBody, body);

            return this.db.Insert(DatabaseTable, null, initialValues);
        }

        public bool DeleteNote(long rowId)
        {
            return this.db.Delete(DatabaseTable, KeyRowId + "=" + rowId, null) > 0;
        }

        public ICursor FetchAllNotes()
        {
            var repo = new RemoteRepository();
            var result = repo.GetAllEntries();

            string[] columns = new string[] { "_id", "title", "body" };

            MatrixCursor matrixCursor = new MatrixCursor(columns);

            foreach (var r in result)
            {
                var set = new ArraySet();
                set.Add(r.id);
                set.Add(r.title.ToString());
                set.Add(r.body.ToString());
                matrixCursor.AddRow(set);
            }
            /*var set = new ArraySet();
            set.Add("1");
            set.Add("ZXC");
            set.Add("QWE");

            matrixCursor.AddRow(set);
            matrixCursor.AddRow(set);*/

            return matrixCursor;
            //return this.db.Query(DatabaseTable, new[] { KeyRowId, KeyTitle, KeyBody }, null, null, null, null, null);
        }

        public ICursor FetchNote(long rowId)
        {
            ICursor cursor = this.db.Query(
                true,
                DatabaseTable,
                new[] { KeyRowId, KeyTitle, KeyBody },
                KeyRowId + "=" + rowId,
                null,
                null,
                null,
                null,
                null);

            if (cursor != null)
            {
                cursor.MoveToFirst();
            }
            return cursor;
        }

        public bool UpdateNote(long rowId, string title, string body)
        {
            var args = new ContentValues();
            args.Put(KeyTitle, title);
            args.Put(KeyBody, body);

            return this.db.Update(DatabaseTable, args, KeyRowId + "=" + rowId, null) > 0;
        }

        private class DatabaseHelper : SQLiteOpenHelper
        {
            internal DatabaseHelper(Context context)
                : base(context, DBName, null, DatabaseVersion)
            {
            }

            public override void OnCreate(SQLiteDatabase db)
            {
                db.ExecSQL(DatabaseCreate);
            }

            public override void OnUpgrade(SQLiteDatabase db, int oldVersion, int newVersion)
            {
                Log.Wtf(Tag, "Upgrading database from version " + oldVersion + " to " + newVersion + ", which will destroy all old data");
                db.ExecSQL("DROP TABLE IF EXISTS notifications");
                this.OnCreate(db);
            }
        }
    }
}