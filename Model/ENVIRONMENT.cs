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





    public class ENVIRONMENT
    {

        public const string MyEnv = "PRD1";
        const string pathResponse = @"C:\Users\BINGO\OneDrive\_DEV\PROJETOS\PROCESSAMENTO\APISUF\";
        const string pathLogs = @"C:\Users\BINGO\OneDrive\_DEV\PROJETOS\PROCESSAMENTO\APISUF\Log\";
        
        //public static string api_strConn = stringcon();
        public const string api_strConn = "stringcon()";
        

        public ENVIRONMENT()
        {
            //api_strConn = stringcon();
        }



        private static string stringcon(){

            
            
            //"Server=127.0.0.1;Port=5432;User Id=joe;Password=secret;Database=joedata;";
            string connStrin = "Server="+ConfigurationManager.AppSettings["db.host"]+";"+
                               "Port="+ConfigurationManager.AppSettings["db.port"]+";"+
                               "User Id="+ConfigurationManager.AppSettings["db.user"]+";"+
                               "Password="+ConfigurationManager.AppSettings["db.pass"]+";"+
                               "Database="+ConfigurationManager.AppSettings["db.database"]+";";

            return connStrin;

            
            /*var connectionstring = ConfigurationManager.AppSettings["db.host"]
                ?.Split(',')
                .Select(o => o.Trim())
                .ToArray();*/

            /*if (connectionstring == null || connectionstring.Length == 0)
            {
                return "";
            }*/

            /*foreach (var securityProtocolString in securityProtocolsToRemove)
            {
                SecurityProtocolType securityProtocolEnum;
                if (Enum.TryParse(securityProtocolString, out securityProtocolEnum))
                {
                    // removes security protocol using binary operation
                    ServicePointManager.SecurityProtocol &= ~securityProtocolEnum;
                }
            }*/
            
                    
           
            //string strConn = "Data Source=ec2-18-210-233-138.compute-1.amazonaws.com;Initial Catalog=d854k1o2v7u9ee;Persist Security Info=True;User ID=iqoysubhhasnui;Password=348747f12b0325a42a06faf7450964c4767c70e79581da04f50a890305aa826f";

            
        }








        

    }

}