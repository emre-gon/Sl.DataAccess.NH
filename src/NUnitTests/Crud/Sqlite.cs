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
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void InsertPerson()
        {
            General.InsertPerson();
        }


        [Test]
        public void ComponentTest()
        {
            General.ComponentTest();
        }


        [Test]
        public void FilterTurkishCharacters()
        { 
            //hata alacak. Sqlite � harfinde �uvall�yor.
            //Ayr�ca sort s�ras�nda � � gibi harflerde de �uvall�yor, en sona at�yor
            //Standart Sqlite'nin Unicode sort ve filtre deste�i yokmu�
            //Ara�t�r: Sqlite ICU extension = Bu extension ile Unicode destekler hale geliyormu�
            //Nuget: Sqlite ICU paketi .Net i�in var .NetCore i�in yok
            General.FilterTurkishCharacters();
        }
    }
}