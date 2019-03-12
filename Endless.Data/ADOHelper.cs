using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Text;

namespace Endless.Data
{
    public static class AdoHelper
    {
        static AdoHelper()
        {
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
            return UsingConnection<TResult>(connectionString,(connection)=> {
                using (SqlTransaction transaction = connection.BeginTransaction())
                {

                    try
                    {
                        return function(transaction);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("TransactionRolledBack", ex);
                    }

                }

            });
            
            
        }


        public static void UsingTransaction<TResult>(string connectionString, Action<SqlTransaction> action)
        {
            UsingConnection(connectionString, (connection) => {
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
                         throw new Exception("TransactionRolledBack", ex);
                     }

                }

            });

        }

        public static List<TResult> UsingReaderByRow<TResult>(string connectionString, Func<SqlConnection,SqlCommand> commandFactoryMethod, Func<SqlDataReader,TResult> rowResultFactoryMethod)
        {
            return UsingConnection(connectionString, (connection) => {
                using (SqlCommand command = commandFactoryMethod(connection))
                {
                    return ExecuteReader(rowResultFactoryMethod, command);
                }

            });

        }

        public static TResult UsingReader<TResult>(string connectionString, Func<SqlConnection, SqlCommand> commandFactoryMethod, Func<SqlDataReader, TResult> readerResult)
        {
            return UsingConnection(connectionString, (connection) => {
                using (SqlCommand command = commandFactoryMethod(connection))
                {
                    SqlDataReader reader = command.ExecuteReader();
                    return readerResult(reader);
                }

            });

        }

        public static List<TResult> UsingReader<TResult>(SqlTransaction transaction,  Func<SqlConnection, SqlCommand> commandFactoryMethod, Func<SqlDataReader, TResult> rowResultFactoryMethod)
        {
            //TODO: exception handling?
            using (SqlCommand command = commandFactoryMethod(transaction.Connection))
            {
                command.Transaction = transaction;
                return ExecuteReader(rowResultFactoryMethod, command);
            }

        }

        public static List<TResult> ExecuteReader<TResult>(Func<SqlDataReader, TResult> rowResultFactoryMethod, SqlCommand command)
        {
            Contract.Assume(command != null);
            Contract.Assume(rowResultFactoryMethod != null);

            var reader = command.ExecuteReader();

            int rows = 0;
            try
            {
                List<TResult> results = new List<TResult>();
                while (reader.Read())
                {
                    var result = rowResultFactoryMethod(reader);
                    results.Add(result);
                    rows++;
                }
                return results;
            }
            catch(Exception ex)
            {
                //TODO: custom exception
                throw new Exception($"Exception thrown reading after reading {rows} rows",ex);
            }

        }
    }
    
public static class TimedExecution
    {
        public static void Do(string description, Action action,TimeSpan? warnAfter=null,TimeSpan? errorAfter=null)
        {
            DateTime start = DateTime.UtcNow;
            Trace.TraceInformation($"Started {description}");
            try
            {
                action();
            }
            catch (Exception ex)
            {
                Trace.TraceError($"Exception thrown during timed section");
                Trace.TraceError(ex.Message);
            }
            finally
            {
                DateTime end = DateTime.UtcNow;
                ReportTiming(description, warnAfter, errorAfter, end, start);
            }
            
        }

        public static TResult Do<TResult>(string description, Func<TResult> function, TimeSpan? warnAfter = null, TimeSpan? errorAfter = null)
        {
            DateTime start = DateTime.UtcNow;
            Trace.TraceInformation($"Started {description}");
            try
            {
                return function();
            }
            catch (Exception ex)
            {
                Trace.TraceError($"Exception thrown during timed section");
                Trace.TraceError(ex.Message);
                throw;
            }
            finally
            {
                DateTime end = DateTime.UtcNow;
                ReportTiming(description, warnAfter, errorAfter, end, start);
            }

        }

        static private void ReportTiming(string description, TimeSpan? warnAfter, TimeSpan? errorAfter, DateTime end,
            DateTime start)
        {
            var duration = end - start;
            if (errorAfter.HasValue && duration > -errorAfter)
            {
                Trace.TraceError(($"Finished {description} took {duration}"));
            }
            else if (warnAfter.HasValue && duration > -warnAfter)
            {
                Trace.TraceWarning($"Finished {description} took {duration}");
            }
            else
            {
                Trace.TraceInformation($"Finished {description} took {duration}");
            }
        }
}
