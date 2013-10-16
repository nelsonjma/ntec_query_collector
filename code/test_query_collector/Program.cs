using System.Collections.Generic;
using System.Data;

using ntec_query_collector;

namespace test_query_collector
{
    class Program
    {
        static void Main(string[] args)
        {
            Collector querieCollector = new Collector(30, @"C:\site\ntec\App_Data\configs.db", @"C:\site\ntec\xml");

            List<Dictionary<string, string>> queries = querieCollector.GetQueries();

            foreach (Dictionary<string, string> query in queries)
            {
                RunQuery(
                        query["conn"],
                        query["sql"],
                        query["xml_file"]
                    );
            }

            int x;

        }

        private static void RunQuery(string conn, string sql, string xmlFile)
        {
            DbConnection.RunQuery query = new DbConnection.RunQuery();
            DataTable dt = query.GetData(conn, sql);

            dt.WriteXml(xmlFile);
        }

    }
}
