using FluentNHibernate.Cfg.Db;
using Sl.DataAccess;
using Sl.Extensions;
using NHibernate;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using TestDomain;

namespace NUnitTests.Crud
{
    public class General
    {
        public static void InsertPerson(ISessionFactory sessionFactory)
        {
            string ad = StringExtensions.RandomString(6);
            int personIDAfterSave;
            using (var session = sessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    Person p = new Person();
                    p.NameSurname = ad;
                    p.Validation = new AppValidation();
                    session.Save(p);
                    transaction.Commit();
                    personIDAfterSave = p.PersonID;
                }
            }


            using (var session = sessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    var p = session.Get<Person>(personIDAfterSave);

                    Assert.IsNotNull(p);
                    Assert.AreEqual(p.NameSurname, ad);
                    Assert.AreEqual(p.PersonID, personIDAfterSave);
                }
            }
        }


        public static void ComponentTest(ISessionFactory sessionFactory)
        {
            string ad = StringExtensions.RandomString(6);
            int personIDAfterSave;
            using (var session = sessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    Person p = new Person();
                    p.Validation = new AppValidation();
                    p.Validation.IsValidated = true;
                    p.Validation.ValidatedBy = "TestBy";
                    p.NameSurname = ad;
                    session.Save(p);
                    transaction.Commit();
                    personIDAfterSave = p.PersonID;
                }
            }


            using (var session = sessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    var p = session.Get<Person>(personIDAfterSave);

                    Assert.IsNotNull(p);
                    Assert.IsTrue(p.Validation.IsValidated);
                    Assert.AreEqual(p.Validation.ValidatedBy, "TestBy");
                }
            }
        }



        public static void FilterTurkishCharacters(ISessionFactory sessionFactory)
        {
            #region crate people with turkish characters
            List<int> personIDs = new List<int>();
            using (var session = sessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    Person p0 = new Person();
                    p0.NameSurname = "İİİ";
                    session.Save(p0);

                    Person p1 = new Person();
                    p1.NameSurname = "III";
                    session.Save(p1);

                    Person p2 = new Person();
                    p2.NameSurname = "ııı";
                    session.Save(p2);

                    Person p3 = new Person();
                    p3.NameSurname = "iii";
                    session.Save(p3);

                    Person p4 = new Person();
                    p4.NameSurname = "şşŞŞşş";
                    session.Save(p4);


                    Person p5 = new Person();
                    p5.NameSurname = "ĞĞğğğĞĞ";
                    session.Save(p5);

                    transaction.Commit();

                    personIDs.Add(p0.PersonID);
                    personIDs.Add(p1.PersonID);
                    personIDs.Add(p2.PersonID);
                    personIDs.Add(p3.PersonID);
                    personIDs.Add(p4.PersonID);
                    personIDs.Add(p5.PersonID);
                }
            }
            #endregion

            using (var session = sessionFactory.OpenSession())
            {
                var ıGecenler = session.Query<Person>()
                    .Where(f => f.NameSurname.Contains("ı"))
                    .OrderBy(f => f.PersonID)
                    .Select(f => f.PersonID).ToList();
                //büyük küçükler dahil 2 kişi olmalı p1 ve p2
                Assert.AreEqual(2, ıGecenler.Count());
                Assert.AreEqual(personIDs[1], ıGecenler.First());
                Assert.AreEqual(personIDs[2], ıGecenler.Last());


               var ıGecenler2 = session.Query<Person>()
                    .Where(f => f.NameSurname.EndsWith("I"))
                    .OrderBy(f => f.PersonID)
                    .Select(f => f.PersonID).ToList();
                //yukarıdaki query ile aynı sonucu vermeli p1 ve p2
                Assert.AreEqual(2, ıGecenler2.Count());
                Assert.AreEqual(personIDs[1], ıGecenler2.First());
                Assert.AreEqual(personIDs[2], ıGecenler2.Last());




                var iGecenler = session.Query<Person>()
                    .Where(f => f.NameSurname.StartsWith("i"))
                    .OrderBy(f => f.PersonID)
                    .Select(f => f.PersonID).ToList();
                //büyük küçükler dahil 2 kişi olmalı p0 ve p3
                Assert.AreEqual(2, iGecenler.Count());
                Assert.AreEqual(personIDs[0], iGecenler.First());
                Assert.AreEqual(personIDs[3], iGecenler.Last());


                iGecenler = session.Query<Person>()
                    .Where(f => f.NameSurname.EndsWith("İ"))
                    .OrderBy(f => f.PersonID)
                    .Select(f => f.PersonID).ToList();
                //yukarıdaki query ile aynı sonucu vermeli p0 ve p3
                Assert.AreEqual(2, iGecenler.Count());
                Assert.AreEqual(personIDs[0], iGecenler.First());
                Assert.AreEqual(personIDs[3], iGecenler.Last());




                var yumuşakGveŞ = session.Query<Person>()
                    .Where(f => f.NameSurname.EndsWith("ş")
                            || f.NameSurname.Contains("Ğ"))
                    .OrderBy(f => f.NameSurname)
                    .Select(f => f.PersonID).ToList();
                //p4 ve p5 dönmeli
                Assert.AreEqual(2, iGecenler.Count());
                Assert.AreEqual(personIDs[5], yumuşakGveŞ.First()); //ğ
                Assert.AreEqual(personIDs[4], yumuşakGveŞ.Last()); //ş
            }




            using (var session = sessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    var ps = session.Query<Person>().Where(f => personIDs.Contains(f.PersonID));
                    foreach (var p in ps)
                    {
                        session.Delete(p);
                    }
                    transaction.Commit();
                }
            }
        }
    }
}
