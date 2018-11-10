using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using PDJaya.Models;

namespace PDJaya.Kiosk.Helpers
{
    public class PDJayaDBSqlite : ISQLDataAccess
    {
        public string ConStr { set; get; }
        static SQLite.Net.SQLiteConnection Conn;
        public PDJayaDBSqlite()
        {
            if (Conn == null)
            {
                ConStr = "kioskdb.sqlite";

                Conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.Generic.SQLitePlatformGeneric(), ConStr);
                //create db and tables
                Conn.CreateTable<Device>();
                Conn.CreateTable<UserProfile>();
                Conn.CreateTable<Payment>();
                Conn.CreateTable<Bill>();
                Conn.CreateTable<Market>();
                Conn.CreateTable<Tenant>();
                Conn.CreateTable<TenantCard>();
                Conn.CreateTable<AppLog>();
                Conn.CreateTable<SyncLog>();
            }
        }
        public bool DeleteAllData<T>(string TableName) where T : class
        {
            var query = Conn.Table<T>();
            var res = Conn.Execute($"DELETE FROM {TableName}");
            return true;
        }

        public bool DeleteData<T>(string TableName, long id) where T : class
        {
            var query = Conn.Table<T>();
            var res = Conn.Execute($"DELETE FROM {TableName} where Id = {id}");
            return true;
        }

        public bool DeleteDataBulk<T>(string TableName, IEnumerable<T> Ids) where T : class
        {
            int count = 0;
            string idstr = "";
            foreach (var idstring in Ids)
            {
                if (count > 0) idstr += ",";
                idstr += idstring;
                count++;
            }
            var res = Conn.Execute($"DELETE FROM {TableName} where Id in ({idstr})");
            return true;
        }

        public List<T> GetAllData<T>() where T : class
        {
            return Conn.Table<T>().ToList();
        }

        public List<T> GetAllData<T>(int Limit) where T : class
        {
            return Conn.Table<T>().Take(Limit).ToList();
        }

        public T GetDataById<T>(long Id) where T : class
        {
            if (typeof(T) == typeof(Device))
            {

                var data = from x in Conn.Table<T>()
                           where (x as Device).Id == Id
                           select x;
                return data.SingleOrDefault();
            }
            else if (typeof(T) == typeof(UserProfile))
            {

                var data = from x in Conn.Table<T>()
                           where (x as UserProfile).Id == Id
                           select x;
                return data.SingleOrDefault();
            }
            else if (typeof(T) == typeof(Payment))
            {

                var data = from x in Conn.Table<T>()
                           where (x as Payment).Id == Id
                           select x;
                return data.SingleOrDefault();
            }
            else if (typeof(T) == typeof(Bill))
            {

                var data = from x in Conn.Table<T>()
                           where (x as Bill).Id == Id
                           select x;
                return data.SingleOrDefault();
            }
            else if (typeof(T) == typeof(Market))
            {

                var data = from x in Conn.Table<T>()
                           where (x as Market).Id == Id
                           select x;
                return data.SingleOrDefault();
            }
            else if (typeof(T) == typeof(Tenant))
            {

                var data = from x in Conn.Table<T>()
                           where (x as Tenant).Id == Id
                           select x;
                return data.SingleOrDefault();
            }
            else if (typeof(T) == typeof(TenantCard))
            {

                var data = from x in Conn.Table<T>()
                           where (x as TenantCard).Id == Id
                           select x;
                return data.SingleOrDefault();
            }
            else if (typeof(T) == typeof(AppLog))
            {

                var data = from x in Conn.Table<T>()
                           where (x as AppLog).Id == Id
                           select x;
                return data.SingleOrDefault();
            }
            else if (typeof(T) == typeof(SyncLog))
            {

                var data = from x in Conn.Table<T>()
                           where (x as SyncLog).Id == Id
                           select x;
                return data.SingleOrDefault();
            }
            return default(T);
        }

        public List<T> GetDataByIds<T>(params long[] Ids) where T : class
        {
            int count = 0;
            string idstr = "";
            foreach (var idstring in Ids)
            {
                if (count > 0) idstr += ",";
                idstr += idstring;
                count++;
            }
            if (typeof(T) == typeof(Device))
            {
                var data = Conn.Table<T>().Where(k => Ids.Contains((k as Device).Id)).ToList();
                return data;
            }
            else if (typeof(T) == typeof(UserProfile))
            {
                var data = Conn.Table<T>().Where(k => Ids.Contains((k as UserProfile).Id)).ToList();
                return data;
            }
            else if (typeof(T) == typeof(Payment))
            {
                var data = Conn.Table<T>().Where(k => Ids.Contains((k as Payment).Id)).ToList();
                return data;

            }
            else if (typeof(T) == typeof(Bill))
            {
                var data = Conn.Table<T>().Where(k => Ids.Contains((k as Bill).Id)).ToList();
                return data;
            }
            else if (typeof(T) == typeof(Market))
            {
                var data = Conn.Table<T>().Where(k => Ids.Contains((k as Market).Id)).ToList();
                return data;

            }
            else if (typeof(T) == typeof(Tenant))
            {
                var data = Conn.Table<T>().Where(k => Ids.Contains((k as Tenant).Id)).ToList();
                return data;

            }
            else if (typeof(T) == typeof(TenantCard))
            {
                var data = Conn.Table<T>().Where(k => Ids.Contains((k as TenantCard).Id)).ToList();
                return data;

            }
            else if (typeof(T) == typeof(SyncLog))
            {
                var data = Conn.Table<T>().Where(k => Ids.Contains((k as SyncLog).Id)).ToList();
                return data;

            }
            else if (typeof(T) == typeof(AppLog))
            {
                var data = Conn.Table<T>().Where(k => Ids.Contains((k as AppLog).Id)).ToList();
                return data;

            }
            return null;

        }

        public List<T> GetDataByStartId<T>(int Limit, long StartId) where T : class
        {
            if (typeof(T) == typeof(Device))
            {

                var data = from x in Conn.Table<T>()
                           where (x as Device).Id >= StartId
                           select x;
                return data.Take(Limit).ToList();
            }
            else if (typeof(T) == typeof(UserProfile))
            {

                var data = from x in Conn.Table<T>()
                           where (x as UserProfile).Id >= StartId
                           select x;
                return data.Take(Limit).ToList();
            }
            else if (typeof(T) == typeof(Payment))
            {

                var data = from x in Conn.Table<T>()
                           where (x as Payment).Id >= StartId
                           select x;
                return data.Take(Limit).ToList();
            }
            else if (typeof(T) == typeof(Bill))
            {

                var data = from x in Conn.Table<T>()
                           where (x as Bill).Id >= StartId
                           select x;
                return data.Take(Limit).ToList();
            }
            else if (typeof(T) == typeof(Market))
            {

                var data = from x in Conn.Table<T>()
                           where (x as Market).Id >= StartId
                           select x;
                return data.Take(Limit).ToList();
            }
            else if (typeof(T) == typeof(Tenant))
            {

                var data = from x in Conn.Table<T>()
                           where (x as Tenant).Id >= StartId
                           select x;
                return data.Take(Limit).ToList();
            }
            else if (typeof(T) == typeof(TenantCard))
            {

                var data = from x in Conn.Table<T>()
                           where (x as TenantCard).Id >= StartId
                           select x;
                return data.Take(Limit).ToList();
            }
            else if (typeof(T) == typeof(AppLog))
            {

                var data = from x in Conn.Table<T>()
                           where (x as AppLog).Id >= StartId
                           select x;
                return data.Take(Limit).ToList();
            }
            else if (typeof(T) == typeof(SyncLog))
            {

                var data = from x in Conn.Table<T>()
                           where (x as SyncLog).Id >= StartId
                           select x;
                return data.Take(Limit).ToList();
            }
            return null;
        }

        public long GetSequence<T>()
        {
            throw new NotImplementedException();
        }

        public bool InsertBulkData<T>(IEnumerable<T> data) where T : class
        {
            foreach (var item in data)
            {
                var x = Conn.Insert(item);
            }

            return true;
        }

        public bool InsertData<T>(T data) where T : class
        {
            var x = Conn.Insert(data);

            return true;
        }

        public bool UpdateFlag<T>(T data) where T : class
        {
            if (data is Bill)
            {
                var datas = from x in Conn.Table<T>()
                           where (x as Bill).BillID == (data as Bill).BillID
                            select x;
                foreach(var item in datas)
                {
                    var cl = item as Bill;
                    cl.IsPaid = true;
                    Conn.Update(cl);
                }
            }
            return true;
        }
    }
}
