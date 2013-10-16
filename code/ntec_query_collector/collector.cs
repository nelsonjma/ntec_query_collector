using System.Collections.Generic;
using System.Linq;
using DbConfig;

namespace ntec_query_collector
{
    public class Collector
    {
        private readonly int     _scheduleInterval;
        private readonly string  _dbFilePath;
        private readonly string  _xmlFolderPath;

        /// <param name="scheduleInterval">query execution interval in minutes</param>
        /// <param name="dbFilePath">ntec site database file path</param>
        /// <param name="xmlFolderPath">default xml folder</param>
        public Collector(int scheduleInterval, string dbFilePath, string xmlFolderPath)
        {
            _scheduleInterval = scheduleInterval;
            _dbFilePath = dbFilePath;
            _xmlFolderPath = xmlFolderPath;
        }

        /* PUBLIC */
        public List<Dictionary<string, string>> GetQueries()
        {
            List<Dictionary<string, string>> queries = new List<Dictionary<string, string>>();

            // leaves if imput values are not all correct
            if (_scheduleInterval < 0 || _dbFilePath == string.Empty || _xmlFolderPath == string.Empty) return queries;

            db_config_page pages = new db_config_page();
            // open the connection to the sqlite database
            pages.Open(_dbFilePath);

            foreach (Page page in pages.AllPages)
            {
                // get folder path
                string path = !page.XMLFolderPath.StartsWith("default") & page.XMLFolderPath != string.Empty 
                                                                                                        ? page.XMLFolderPath.Trim()
                                                                                                        : _xmlFolderPath.Trim();

                List<Frame> frames = (from f in pages.GetAllFrames(page.ID)
                                             where f.ScheduleInterval == _scheduleInterval && f.IsActive == 1
                                             select f).ToList();

                foreach (Frame frame in frames)
                {
                    // add the options to the option smasher :) to get the individual strings
                    OptionItems oi = new OptionItems(frame.Options);

                    string frameTitle = frame.Title;
                    string pageTitle = page.Title;
                    string sql = oi.GetSingle("sql");           // first sql found
                    string conn = oi.GetSingle("conn");         // first conn found
                    string xmlFile = oi.GetSingle("xml_file");  // forst xml file found

                    // if any of the above strins its empty, its because the user did someting very bad
                    if (sql != string.Empty && conn != string.Empty && xmlFile != string.Empty)
                    {
                        // if the xml file contains ":\" its because its a c:\bla bla or d:\bla bla or someting like that.
                        queries.Add(BuildQueryObject(
                                                        sql,
                                                        conn,
                                                        path.EndsWith(@"\") ? path + xmlFile : path + @"\" + xmlFile,
                                                        pageTitle,
                                                        frameTitle
                                                    ));
                    }
                    
                }
            }

            return queries;
        }

        /* PRIVATE */
        private static Dictionary<string, string> BuildQueryObject(string query, string conn, string xmlFile, string pageTitle, string frameTitle)
        {
            return new Dictionary<string, string>
                                                {
                                                    {"sql", query}, 
                                                    {"conn", conn}, 
                                                    {"xml_file", xmlFile},
                                                    {"frame_title", frameTitle},
                                                    {"page_title", pageTitle},
                                                };
        }
    }

    
}
