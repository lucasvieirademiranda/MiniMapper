using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Cfg;
using NHibernate;
using NHibernate.Cfg;

namespace MiniMapper.Infrastructure.Data.Access
{
    public class NHibernateHelper
    {
        public static IStatelessSession GetStatelessSession()
        {
            var statelessSession = Fluently.Configure()
                                           .BuildSessionFactory()
                                           .OpenStatelessSession();
            return statelessSession;
        }
    }
}
