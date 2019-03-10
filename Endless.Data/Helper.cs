using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Endless.Data
{
    public static class Helper
    {
        static Helper()
        {
        }

        
        public static TResult UsingConnection<TResult>(string connectionString, Func<SqlConnection, TResult> function)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    return function(connection);

                }
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }

            return default(TResult);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="action"></param>
        public static void UsingConnection(string connectionString, Action<SqlConnection> action)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    action(connection);

                }
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
        }

        public static TResult UsingTransaction<TResult>(string connectionString, Func<SqlTransaction, TResult> function)
        {
            return UsingConnection<TResult>(connectionString,(connection)=> { 
                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                        return function(transaction);
                    }
                
            });
            
            
        }


        public static void UsingTransaction<TResult>(string connectionString, Action<SqlTransaction> action)
        {
            UsingConnection(connectionString, (connection) => {
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                     action(transaction);
                }

            });

        }

        public static void UsingReader<TResult>(string connectionString, Action<SqlTransaction> action,Func<SqlConnection,SqlCommand> commandFactoryMethod, Func<SqlDataReader,TResult> rowResultFactoryMethod)
        {
            UsingConnection(connectionString, (connection) => {
                using (SqlCommand command = commandFactoryMethod(connection))
                {
                    return ExecuteReader(rowResultFactoryMethod, command);
                }

            });

        }

        public static List<TResult> ExecuteReader<TResult>(Func<SqlDataReader, TResult> rowResultFactoryMethod, SqlCommand command)
        {
            var reader = command.ExecuteReader();

            List<TResult> results = new List<TResult>();
            while (reader.Read())
            {
                var result = rowResultFactoryMethod(reader);
                results.Add(result);
            }

            return results;
        }
    }
}
