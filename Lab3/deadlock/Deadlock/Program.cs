using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;

namespace Deadlock
{
    class Program
    {
        //String connectionString = "Data Source = DESKTOP - 9IL2B76\\SQLEXPRESS;Initial Catalog = TestPracticeShoeStore; Integrated Security = True";
        String sp1 = "updateWomenTable";
        String sp2 = "updateShoeModelTable";

        static void Main(string[] args)
        {
            Program p = new Program();

            Thread threadA = new Thread(new ThreadStart(p.TransactionAThread))
            {
                Name = "ThreadA"
            };
            Thread threadB = new Thread(new ThreadStart(p.TransactionBThread))
            {
                Name = "ThreadB"
            };


            threadA.Start();
            threadB.Start();
        }

        public void TransactionBThread()
        {
            String threadName = Thread.CurrentThread.Name;
            Console.WriteLine("In thread: {0}", threadName);
            using (SqlConnection con = new SqlConnection("Data Source=DESKTOP-9IL2B76\\SQLEXPRESS;Initial Catalog=TestPracticeShoeStore;Integrated Security=True"))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = con;
                SqlTransaction tranB = con.BeginTransaction("TransactionB");
                int r = 0;
                cmd.Transaction = tranB;
                try
                {
                    cmd.CommandText = sp1;
                    cmd.Parameters.AddWithValue("@name", "TransactionB");
                    Console.WriteLine("Executing procedure {0} in thread {1}", cmd.CommandText, threadName);
                    r = cmd.ExecuteNonQuery();
                    Console.WriteLine("Thread B r = {0}", r);
                    r = 0;
                    Thread.Sleep(1000);
                    cmd.CommandText = sp2;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@season", "TransactionB");
                    Console.WriteLine("Executing procedure {0} in thread {1}", cmd.CommandText, threadName);
                    r = cmd.ExecuteNonQuery();
                    Console.WriteLine("Thread B r = {0}", r);
                    tranB.Commit();
                    con.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    
                    int tries = 2;
                    while (r == 0 && tries > 0)
                    {
                        try
                        {
                            cmd.CommandText = sp1;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@name", "TransactionB");
                            Console.WriteLine("Executing procedure {0} in thread {1}", cmd.CommandText, threadName);
                            r = cmd.ExecuteNonQuery();
                            Console.WriteLine("Thread B r = {0}", r);
                            r = 0;
                            cmd.CommandText = sp2;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@season", "TransactionB");
                            Console.WriteLine("Executing procedure {0} in thread {1}", cmd.CommandText, threadName);
                            r = cmd.ExecuteNonQuery();
                            Console.WriteLine("Thread B r = {0}", r);
                            con.Close();
                        }
                        catch (Exception e2)
                        {
                            Console.WriteLine(e2.Message);
                            tries--;
                        }
                    }
                    
                }
            }
        }

        public void TransactionAThread()
        {
            String threadName = Thread.CurrentThread.Name;
            Console.WriteLine("In thread: {0}", threadName);
            using (SqlConnection con = new SqlConnection("Data Source=DESKTOP-9IL2B76\\SQLEXPRESS;Initial Catalog=TestPracticeShoeStore;Integrated Security=True"))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = con;
                SqlTransaction tranA = con.BeginTransaction("TransactionA");
                cmd.Transaction = tranA;
                int r = 0;
                try
                {
                    cmd.CommandText = sp2;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@season", "TransactionA");
                    Console.WriteLine("Executing procedure {0} in thread {1}", cmd.CommandText, threadName);
                    r = cmd.ExecuteNonQuery();
                    Console.WriteLine("Thread A r = {0}", r);
                    r = 0;
                    Thread.Sleep(1000);
                    cmd.CommandText = sp1;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@name", "TransactionA");
                    Console.WriteLine("Executing procedure {0} in thread {1}", cmd.CommandText, threadName);
                    r = cmd.ExecuteNonQuery();
                    Console.WriteLine("Thread A r = {0}", r);
                    tranA.Commit();
                    con.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    
                    int tries = 2;
                    while (r == 0 && tries > 0)
                    {
                        try
                        {
                            cmd.CommandText = sp2;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@season", "TransactionA");
                            Console.WriteLine("Executing procedure {0} in thread {1}", cmd.CommandText, threadName);
                            r = cmd.ExecuteNonQuery();
                            Console.WriteLine("Thread A r = {0}", r);
                            r = 0;
                            cmd.CommandText = sp1;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@name", "TransactionA");
                            Console.WriteLine("Executing procedure {0} in thread {1}", cmd.CommandText, threadName);
                            r = cmd.ExecuteNonQuery();
                            Console.WriteLine("Thread A r = {0}", r);
                            con.Close();
                        }
                        catch (Exception e2) {
                            Console.WriteLine(e2.Message);
                            tries--;
                        }
                    }
                 
                }
            }
        }
    }
}
