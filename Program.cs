using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Net;
using System.Linq;
using System.Configuration;

//using System.IO;
//using System.Threading.Tasks;
//using System.Collections.Generic;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Logging;
//using System.Net.Http;
//using System.Text;
//using System.Collections.Specialized;






namespace APICET_BKEND
{
    public class Program
    {

        
       
        

        //static async Task Main(string[] args)
        public static void Main(string[] args)
        {
     
     
            //APISUFRAMA suf = new APISUFRAMA();            
            //APISUFRAMA.listaPadrao("IE");
            //var lista = APISUFRAMA.ListaPadraoJson();
            //APISUFRAMA.gravaInsumo("");


     
            CreateHostBuilder(args).Build().Run();

        }





        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });




        private static void RemoveSecurityProtocols()
        {
            var securityProtocolsToRemove = ConfigurationManager.AppSettings["SecurityProtocols.Remove"]
                ?.Split(',')
                .Select(o => o.Trim())
                .ToArray();

            if (securityProtocolsToRemove == null || securityProtocolsToRemove.Length == 0)
            {
                return;
            }

            foreach (var securityProtocolString in securityProtocolsToRemove)
            {
                SecurityProtocolType securityProtocolEnum;
                if (Enum.TryParse(securityProtocolString, out securityProtocolEnum))
                {
                    // removes security protocol using binary operation
                    ServicePointManager.SecurityProtocol &= ~securityProtocolEnum;
                }
            }
        }

        private static void AddSecurityProtocols()
        {
            var securityProtocolsAdd = ConfigurationManager.AppSettings["SecurityProtocols.Add"]
                ?.Split(',')
                .Select(o => o.Trim())
                .ToArray();

            if (securityProtocolsAdd == null || securityProtocolsAdd.Length == 0)
            {
                return;
            }

            foreach (var securityProtocolString in securityProtocolsAdd)
            {
                SecurityProtocolType securityProtocolEnum;
                if (Enum.TryParse(securityProtocolString, out securityProtocolEnum))
                {
                    // adds security protocol using binary operation
                    ServicePointManager.SecurityProtocol |= securityProtocolEnum;
                }
            }
        }



        /*static void ReadAllSettings()  
        {  
            try  
            {  
                var appSettings = ConfigurationManager.AppSettings;  

                if (appSettings.Count == 0)  
                {  
                    Console.WriteLine("AppSettings is empty.");  
                }  
                else  
                {  
                    foreach (var key in appSettings.AllKeys)  
                    {  
                        Console.WriteLine("Key: {0} Value: {1}", key, appSettings[key]);  
                    }  
                }  
            }  
            catch (ConfigurationErrorsException)  
            {  
                Console.WriteLine("Error reading app settings");  
            }  
        } */ 

        
    }



}
 