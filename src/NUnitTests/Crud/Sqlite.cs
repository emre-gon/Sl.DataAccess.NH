using FluentNHibernate.Cfg.Db;
using Sl.DataAccess.NH.AutoMap;
using NHibernate;
using NUnit.Framework;
using System.IO;
using System.Reflection;
using System.Xml;

namespace NUnitTests.Crud
{
    public class SqliteTests
    {
        ISessionFactory SessionFactory;
        [SetUp]
        public void Setup()
        {
            SessionFactory = NUSetup.SqliteSessionFactory;
        }

        [Test]
        public void InsertPerson()
        {
            General.InsertPerson(SessionFactory);
        }


        [Test]
        public void ComponentTest()
        {
            General.ComponentTest(SessionFactory);
        }


        [Test]
        public void FilterTurkishCharacters()
        { 
            //hata alacak. Sqlite ý harfinde çuvallýyor.
            //Ayrýca sort sýrasýnda þ ð gibi harflerde de çuvallýyor, en sona atýyor
            //Standart Sqlite'nin Unicode sort ve filtre desteði yokmuþ
            //Araþtýr: Sqlite ICU extension = Bu extension ile Unicode destekler hale geliyormuþ
            //Nuget: Sqlite ICU paketi .Net için var .NetCore için yok
            General.FilterTurkishCharacters(SessionFactory);
        }
    }
}