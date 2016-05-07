using PasqualeSite.Data.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PasqualeSite.Services
{
    public abstract class DisposableService : IDisposable
    {
        protected MyDbContext db;

        bool _disposed;

        public DisposableService()
        {
            db = new MyDbContext();
            db.Configuration.LazyLoadingEnabled = false;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~DisposableService()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing && db != null)
            {
                db.Dispose();
            }


            db = null;

            _disposed = true;
        }
    }
}