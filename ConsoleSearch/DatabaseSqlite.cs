using System;
using System.Collections.Generic;
using System.Linq;
using Shared;
using Shared.Model;
using Microsoft.Data.Sqlite;

namespace ConsoleSearch;

    public class DatabaseSqlite : IDatabase
    {
        private SqliteConnection _connection;

        private Dictionary<string, int> mWords = null;

        public DatabaseSqlite()
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder();

            connectionStringBuilder.DataSource = Paths.SQLITE_DATABASE;


            _connection = new SqliteConnection(connectionStringBuilder.ConnectionString);

            _connection.Open();


        }

        private void Execute(string sql)
        {
            var cmd = _connection.CreateCommand();
            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();
        }





        // key is the id of the document, the value is number of search words in the document
        public List<(int docId, int hits)> GetDocuments(List<int> wordIds)
        {
            var res = new List<(int docId, int hits)>();

            /* Example sql statement looking for doc id's that
               contain words with id 2 and 3
            
               SELECT docId, COUNT(wordId) as count
                 FROM Occ
                WHERE wordId in (2,3)
             GROUP BY docId
             ORDER BY COUNT(wordId) DESC 
             */

            var sql = "SELECT docId, COUNT(wordId) as count FROM Occ where ";
            sql += "wordId in " + AsString(wordIds) + " GROUP BY docId ";
            sql += "ORDER BY count DESC;";

            var selectCmd = _connection.CreateCommand();
            selectCmd.CommandText = sql;

            using (var reader = selectCmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var docId = reader.GetInt32(0);
                    var count = reader.GetInt32(1);

                    res.Add((docId, count));
                }
            }

            return res;
        }

        private string AsString(List<int> x) => $"({string.Join(',', x)})";



       

        private Dictionary<string, int> GetAllWords()
        {
            Dictionary<string, int> res = new Dictionary<string, int>();

            var selectCmd = _connection.CreateCommand();
            selectCmd.CommandText = "SELECT * FROM word";

            using (var reader = selectCmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var id = reader.GetInt32(0);
                    var w = reader.GetString(1);

                    res.Add(w, id);
                }
            }
            return res;
        }
        
        public BEDocument GetDocDetails(int docId)
        {
            var selectCmd = _connection.CreateCommand();
            selectCmd.CommandText = $"SELECT * FROM document where id = {docId}";

            using (var reader = selectCmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    var id = reader.GetInt32(0);
                    var url = reader.GetString(1);
                    var idxTime = reader.GetDateTime(2);
                    var creationTime = reader.GetDateTime(3);

                    return new BEDocument { Id = id, Url = url, IdxTime = idxTime, CreationTime = creationTime };
                }
            }
            return null;
        }

        /* Return a list of id's for words; all them among wordIds, but not present in the document
         */
        public List<int> GetMissing(int docId, List<int> wordIds)
        {
            var sql = "SELECT wordId FROM Occ where ";
            sql += "wordId in " + AsString(wordIds) + " AND docId = " + docId;
            sql += " ORDER BY wordId;";

            var selectCmd = _connection.CreateCommand();
            selectCmd.CommandText = sql;

            List<int> present = new List<int>();

            using (var reader = selectCmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var wordId = reader.GetInt32(0);
                    present.Add(wordId);
                }
            }
            var result = new List<int>(wordIds);
            foreach (var w in present)
                result.Remove(w);


            return result;
        }

        public List<string> WordsFromIds(List<int> wordIds)
        {
            var sql = "SELECT name FROM Word where ";
            sql += "id in " + AsString(wordIds);

            var selectCmd = _connection.CreateCommand();
            selectCmd.CommandText = sql;

            List<string> result = new List<string>();

            using (var reader = selectCmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var wordId = reader.GetString(0);
                    result.Add(wordId);
                }
            }
            return result;
        }

        public List<int> GetWordIds(string[] query, bool ignoreCases, out List<string> outIgnored)
        {
            if (mWords == null)
                mWords = GetAllWords();
            var res = new List<int>();
            var ignored = new List<string>();
            if (!ignoreCases)
            {
                foreach (var aWord in query)
                {
                    if (mWords.ContainsKey(aWord))
                        res.Add(mWords[aWord]);
                    else
                        ignored.Add(aWord);
                }

                outIgnored = ignored;
                return res;
            }
            // here we know that cases must be ignored
            //step 1: query in lowercase
            query = query.Select((w) => w.ToLower()).ToArray();
            
            // step 2: goes through all words to see if they are in query
            foreach (var aWord in mWords)
            {
                if ( query.Contains(aWord.Key.ToLower()) )
                     res.Add(aWord.Value);
            }
           
            foreach (var aWord in query)
            {
                if (!mWords.ContainsKey(aWord))
                    ignored.Add(aWord);
            }
            outIgnored = ignored;
            return res;
        }

        private bool Match(string s1, string s2, bool ignoreCases)
        {
            return ignoreCases ? s1.Equals(s2, StringComparison.InvariantCultureIgnoreCase) : s1.Equals(s2);
        }
    }

