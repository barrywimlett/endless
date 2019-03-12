using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Transactions;
using Endless.Diagnostics;

namespace Endless.Data.ADO
{
    
    public static class AdoHelper
    {
        static AdoHelper()
        {
        }

        public static SqlCommand StoredProcedureCommand(SqlConnection connection,string storedProcedureName,int timeOutSeconds=5)
        {
            return new SqlCommand(storedProcedureName, connection) {CommandTimeout = timeOutSeconds};
        }

        public static TResult UsingConnection<TResult>(string connectionString, Func<SqlConnection, TResult> function)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    try
                    {
                        return function(connection);
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                //TODO:replace using Endless.Exceptions
                Trace.TraceError(ex.Message);
            }

            return default(TResult);
        }

        /// <summary>Exception
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

                    try
                    {
                        action(connection);
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                //TODO:replace using Endless.Exceptions
                Trace.TraceError(ex.Message);
            }

        }

        public static TResult UsingTransaction<TResult>(string connectionString, Func<SqlTransaction, TResult> function)
        {
            return UsingConnection<TResult>(connectionString, (connection) =>
            {
                using (SqlTransaction transaction = connection.BeginTransaction())
                {

                    try
                    {
                        var result=function(transaction);
                        transaction.Commit();
                        return result;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new TransactionAbortedException("TransactionRolledBack", ex);
                    }

                }

            });


        }


        public static void UsingTransaction<TResult>(string connectionString, Action<SqlTransaction> action)
        {
            UsingConnection(connectionString, (connection) =>
            {
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    action(transaction);
                    try
                    {
                        action(transaction);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new TransactionAbortedException("TransactionRolledBack", ex);
                    }

                }

            });

        }

        public static List<TResult> ReadRowsUsingDataReader<TResult>(SqlTransaction transaction,
            Func<SqlConnection, SqlCommand> commandFactoryMethod, Func<SqlDataReader, TResult> rowResultFactoryMethod)
        {
            //TODO: exception handling?
            using (SqlCommand command = commandFactoryMethod(transaction.Connection))
            {
                Contract.Assume(command != null);
                Contract.Assume(command.Transaction == null);

                command.Transaction = transaction;

                SqlDataReader reader = null;
                try
                {
                    reader = command.ExecuteReader();
                }
                catch (Exception ex)
                {
                    Trace.TraceError($"Command Text:{command.CommandText} threw Exception {ex.Message}");
                    throw;
                }

                return ReadRows(rowResultFactoryMethod, reader);
            }

        }

        public static List<TResult> ReadRowsUsingDataReader<TResult>(string connectionString,
            Func<SqlConnection, SqlCommand> commandFactoryMethod, Func<SqlDataReader, TResult> rowResultFactoryMethod)
        {

            return UsingReader<List<TResult>>(connectionString, commandFactoryMethod,
                (reader) => ReadRows<TResult>(rowResultFactoryMethod,reader));
            
        }

        public static TResult UsingReader<TResult>(string connectionString,
            Func<SqlConnection, SqlCommand> commandFactoryMethod, Func<SqlDataReader, TResult> readerResult)
        {
            return UsingConnection(connectionString, (connection) =>
            {
                using (SqlCommand command = commandFactoryMethod(connection))
                {
                    SqlDataReader reader=null;
                    try
                    {
                        reader = command.ExecuteReader();
                    }
                    catch (Exception ex)
                    {
                        Trace.TraceError($"Command Text:{command.CommandText} threw Exception {ex.Message}");
                        throw;
                    }

                    return readerResult(reader);
                }

            });

        }

        
        public static List<TResult> ReadRows<TResult>(Func<SqlDataReader, TResult> rowResultFactoryMethod,SqlDataReader reader)
        {
            Contract.Assume(reader != null);
            Contract.Assume(rowResultFactoryMethod != null);

            int rows = 0;
            try
            {
                return TimedExecution.Do<List<TResult>>("ReadingRows", () =>
                {
                    List<TResult> results = new List<TResult>();
                    while (reader.Read())
                    {
                        var result = rowResultFactoryMethod(reader);
                        results.Add(result);
                        rows++;
                    }

                    return results;
                });
            }
            catch (Exception ex)
            {
                //TODO: custom exception
                throw new AdoHelperException($"Exception thrown reading after reading {rows} rows:", ex);
            }

        }
    }
}

