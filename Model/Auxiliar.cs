using System;
using System.IO;
using System.Text;
using RestSharp;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Data.OleDb;
using System.Data.Odbc;
using System.Net;
using System.Net.Http;
using HtmlAgilityPack;
using Npgsql;
using System.Configuration;
using System.Text.Json;
using System.Data;
using System.Collections.Generic;




    

namespace APICET_BKEND
{


    /*public static class Aux2
    {
        public static string teste = "--";


         public static string quote2(string texto){

            return "\'" + texto + "\'";
            

        }

    }*/


    public static class Auxiliar
    {

    


        public static string quote(string texto){

            return "\'" + texto + "\'";
            

        }

        public static void saveFile(string textFile, string content, Encoding encoding){
            
            
            //saveFile(textFile, contents, Encoding.UTF8);
            if (ENV.MyEnv=="DEV"){
                File.WriteAllText(textFile, content, Encoding.UTF8);
            }


       }
       


        public static string jsonReg(string field, string value, int tabSize){
                    
            string jsonRegister = "\""+field+"\": ";
            jsonRegister = jsonRegister + "\""+value+"\"";
            string tab = "".PadLeft(tabSize, ' ');
            jsonRegister = tab + jsonRegister;
            return jsonRegister;

        }

        public static int qtdeReg(string contents){

            //var qtLines = contents.Split('\n').Length;
            int qtLines = 0;

            var reader = new StringReader(contents);            
            for (string line = reader.ReadLine(); line != null; line = reader.ReadLine())
            {                                
                var key = line.Substring(0, 1);
                if(key != " "){
                 qtLines++;
                }
            }

            return qtLines;

        }






        public static DataTable ExecuteSelectQuery(string sql)
        {
            
                                      

            try
            {
                                
                NpgsqlConnection conn = new NpgsqlConnection(ENV.api_strConn);                                                
                conn.Open();
                NpgsqlCommand command = new NpgsqlCommand(sql, conn);                
                //NpgsqlDataReader dr = execReader(sql);            
                //dr.Read();
                //conn.Open();
                
                if (conn == null)
                {
                    return null;
                }

                DataTable dataTable = new DataTable();
                DataSet dataSet = new DataSet();
                NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(command);
                dataSet.Reset();
                dataAdapter.Fill(dataSet);
                dataTable = dataSet.Tables[0];
                conn.Close();

                return dataTable.Rows.Count <= 0 ? null : dataTable;
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }


        public static string ExecuteNonQuery(string sql){


            string resultado = "0";

            try
            {
                                
                NpgsqlConnection conn = new NpgsqlConnection(ENV.api_strConn);                                                
                conn.Open();                
                                
                if (conn == null)
                {
                    return resultado;
                }

                NpgsqlCommand command = new NpgsqlCommand(sql, conn); 
                command.ExecuteNonQuery();
                conn.Close();
                resultado = "1";
                
            }
            catch (NpgsqlException ex)
            {
                //Console.WriteLine(ex.Message);
                resultado = ex.Message;
                return resultado;
            }


            return resultado;

        }
        

        /*private static NpgsqlDataReader execReader(string sql){



           *NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, conn);
            da.Fill(localDB, "words");
            if (localDB.Tables["words"].Rows.Count == 0)



            //NpgsqlConnection conn = new NpgsqlConnection(api_strConn);                                                
            //conn.Open();            
            //NpgsqlCommand command = new NpgsqlCommand(sql, conn);
            //NpgsqlDataReader dr = command.ExecuteReader();                                                        
            //return dr;
                                    
        }*/


    }


}