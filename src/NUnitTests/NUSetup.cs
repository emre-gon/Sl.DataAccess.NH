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
using Sl.DataAccess.NH;
using Sl.DataAccess.NH.Audit;

namespace NUnitTests
{
    [SetUpFixture]
    public class NUSetup
    {
        [OneTimeSetUp]
        public void Setup()
        {

            IPersistenceConfigurer SQLITE_CONFIG = SQLiteConfiguration.Standard.UsingFile("test.sqlite");
            SlSession.ConfigureSessionFactory(Assembly.GetAssembly(typeof(Person))
                , SQLITE_CONFIG,
                SessionContextType.ThreadStatic, 
                null,
                DBSchemaUpdateMode.Drop_And_Recreate_Tables);
        }

        [OneTimeTearDown]
        public void Terardown()
        {
            File.Delete("test.sqlite");
        }

    }
}
