using NHibernate;

namespace Sl.DataAccess.NH
{
    public static class NHSessionExtensions
    {
        /// <summary>
        /// N = Nullable
        /// Returns null if id is null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="NHSession"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static T LoadN<T>(this ISession NHSession, object id)
        {
            if (id == null)
                return default(T);

            return NHSession.Load<T>(id);
        }

        /// <summary>
        /// N = Nullable
        /// Returns null if id is null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="NHSession"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static T GetN<T>(this ISession NHSession, object id)
        {
            if (id == null)
                return default(T);

            return NHSession.Get<T>(id);
        }

        /// <summary>
        /// Locks table until transaction is complete
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="NHSession"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static T LoadForUpdate<T>(this ISession NHSession, object id)
        {
            T obj = NHSession.Load<T>(id);
            NHSession.Lock(obj, LockMode.Upgrade);
            return obj;
        }

        /// <summary>
        /// Locks table until transaction is complete
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="NHSession"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static T GetForUpdate<T>(this ISession NHSession, object id)
        {
            T obj = NHSession.Get<T>(id);
            NHSession.Lock(obj, LockMode.Upgrade);
            return obj;
        }

        /// <summary>
        /// N = Nullable
        /// Returns null if id is null
        /// Locks table until transaction is committed
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="NHSession"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static T LoadNForUpdate<T>(this ISession NHSession, object id)
        {
            if (id == null)
                return default(T);

            return NHSession.LoadForUpdate<T>(id);
        }

        /// <summary>
        /// N = Nullable
        /// Returns null if id is null
        /// Locks table until transaction is committed
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="NHSession"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static T GetNForUpdate<T>(this ISession NHSession, object id)
        {
            if (id == null)
                return default(T);

            return NHSession.GetForUpdate<T>(id);
        }


        /// <summary>
        /// Loads and Deletes object with id
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="NHSession"></param>
        /// <param name="id"></param>
        public static void DeleteID<T>(this ISession NHSession, object id)
        {
            T obj = NHSession.Load<T>(id);            
            NHSession.Delete(obj);
        }
    }
}
