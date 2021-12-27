using FluentNHibernate.Cfg.Db;
using Sl.DataAccess.NH.AutoMap;
using NHibernate;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using TestDomain;

namespace NUnitTests
{
    [SetUpFixture]
    public class NUSetup
    {
        public static ISessionFactory SqliteSessionFactory { get; private set; }


        [OneTimeSetUp]
        public void Setup()
        {

            IPersistenceConfigurer SQLITE_CONFIG = SQLiteConfiguration.Standard.UsingFile("test.sqlite");
            SqliteSessionFactory = NHAutoMapper.CreateSessionFactory(Assembly.GetAssembly(typeof(Person))
                , SQLITE_CONFIG,

                SessionContextType.ThreadStatic, DBSchemaUpdateMode.Drop_And_Recreate_Tables);
        }

        [OneTimeTearDown]
        public void Terardown()
        {
            File.Delete("test.sqlite");
        }

    }
}
