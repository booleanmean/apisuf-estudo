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
using static APICET_BKEND.Auxiliar;




    

namespace APICET_BKEND
{





    public class APISUFRAMA_T
    {

        
             

        //private ENVIRONMENT ENV = new ENVIRONMENT();
        

        public APISUFRAMA_T()
        {                       
           //ENV = new ENVIRONMENT();
        }

        public string RESULTAPI = "";
      



      





        
        public static string ProcProdutos0(string IE)
        {
                                                                      
            
            //https://200.198.228.134/servicos/estrangeiro/consultasinternas/Siscomex/GeracaoArquivoTexto/EST_GeracaoArqTxtEmpresaGerador.asp?inscsuf=200106023
            string result = "900"; //Unrecognized error
            string erro   = "";          
            //string fileName = "produtos_"+IE+".txt";
            string textFile = ENV.pathResponse+"produtos_"+IE+".txt";
            string jsonFile = ENV.pathResponse+"produtos_"+IE+".json";
            string txtProdutos = "";
            string jsonProdutos = "";

 
            try{
            try{

                          
                
                if (File.Exists(textFile))
                {                    
                    File.Delete(textFile);
                }
                
                
                //read suframa download
                txtProdutos = txtProdutos + "line 1" + "; " + "\r";
                txtProdutos = txtProdutos + "line 2" + "; " + "\r";                
                saveFile(textFile, txtProdutos, Encoding.UTF8);
                

              
                //Do Json
                jsonProdutos = "{" + "\r";

                string[] lines = File.ReadAllLines(textFile);  
                
                foreach (string line in lines)  {
                    jsonProdutos = jsonProdutos + line + "\r";
                }                                
                jsonProdutos = jsonProdutos + "}";                
                saveFile(jsonFile, jsonProdutos, Encoding.UTF8);
                
                

                
                //result = "200";  
                
                result = File.ReadAllText(jsonFile);
                saveFile(ENV.pathLogs+"APISUFRAMA_PROC.log",  "Processado com sucesso!", Encoding.UTF8);
                


            }
            catch(Exception ex){
                erro = "APISUFRAMA --> error[" + ex.Message+"]";
                //File.WriteAllText("log/APISUFRAMA_erro.log", erro);  
            }    
            }            
            finally{
                
                
                
               
            }

            return result;

               
        }



        private static string getProdutos(string IE){


            //GetRequestTokenAsync requi = new GetRequestTokenAsync("");
        
            string URI = "https://200.198.228.134/servicos/estrangeiro/consultasinternas/Siscomex/GeracaoArquivoTexto/EST_GeracaoArqTxtEmpresaGerador.asp?";
            //https://200.198.228.134/servicos/estrangeiro/consultasinternas/Siscomex/GeracaoArquivoTexto/EST_GeracaoArqTxtEmpresaGerador.asp?inscsuf=200106023

            //RestClient restClient = new RestClient("http://wwws.suframa.gov.br/servicos/")
            RestClient client = new RestClient(URI)
            {
                //Proxy = new WebProxy("wcgproxy-mao:8080", true)
                //{
                //    UseDefaultCredentials = true
                //},
                //Encoding = Encoding.GetEncoding(28591),
                //CookieContainer = new CookieContainer()
            };

            RestRequest req = new RestRequest("inscsuf=200106023", Method.Get);

            Encoding encoding = Encoding.GetEncoding(28591);
            RestResponse consultaResponse = new RestResponse
            {
                //ContentEncoding = "iso-8859-1"
            };
            //client.ExecuteAsync(req); OK

            var response = client.ExecuteAsync(req);




            string result = "900"; 
            return result;

        }



        public static void gravaListaPadrao(string JsonInsumo){


            //JsonProduto = File.ReadAllText(pathResponse+"produtos200106023b.json");            
            //ProdutosIE produtos = JsonSerializer.Deserialize<ProdutosIE>(JsonProduto)!;
            InsumosIE insumos = JsonSerializer.Deserialize<InsumosIE>(JsonInsumo)!;

            //string strConn = stringcon();                        
            //NpgsqlConnection conn = new NpgsqlConnection(strConn);            
            //NpgsqlCommand command;
            string sql;
            DataTable dtb;


            try
            {

                foreach (Insumo ins in  insumos.insumos){
                    
                    
                    sql = "select count(*) total from \"APISUF\".lista_padrao "+                    
                          "where ncm='"+ins.NCM+"' and detalhe='"+ins.detalhe+"'";                    
                    
                    dtb = ExecuteSelectQuery(sql);
                    int total = Convert.ToInt32(dtb.Rows[0]["total"]);                                                     
                    
                    //Insert
                    if(total == 0){
                        sql = "insert into \"APISUF\".lista_padrao values("+
                            quote(ins.NCM)+", "+quote(ins.detalhe)+", "+quote(ins.descricao)+")";
                    };
                    
                    //Update
                    if(total > 0){
                        sql = "update \"APISUF\".lista_padrao set "+
                              "  descricao="+quote(ins.descricao)+", "+
                              "where ncm="+quote(ins.NCM)+ " and detalhe="+quote(ins.detalhe);
                    };
                    
                    string resultado = ExecuteNonQuery(sql);
                                                                                                    

               }                

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);                
            }
            finally
            {
                //conn.Close();
            }



            

           /* foreach(DataRow row in dtb.Rows)
                    {
                        foreach(DataColumn column in thisTable.Columns)
                        {
                            Console.WriteLine(row[column]);
                        }
                    }*/            


        }


        public static string ListaPadraoJson(){

            
            
            string strConn = ENV.api_strConn;
            NpgsqlConnection conn = new NpgsqlConnection(strConn);            
            NpgsqlCommand command;
            string sql;
            string jsonListaPadrao;
            ListaPadrao listap = new ListaPadrao();
            List<Insumo> insumos = new List<Insumo>();
            

            sql = "select count(distinct ncm) total,  count(detalhe) total2 from \"APISUF\".lista_padrao ";                                              
            DataTable dtb = ExecuteSelectQuery(sql);
            listap.qtde_ncm = dtb.Rows[0]["total"].ToString();
            listap.qtde_insumos = dtb.Rows[0]["total2"].ToString();
            
                    

            
            try{
                conn.Open();
                sql = "select * from \"APISUF\".lista_padrao ";
                command = new NpgsqlCommand(sql, conn);                
                NpgsqlDataReader dr = command.ExecuteReader();


                while(dr.Read())
                {
                    
                    
                    string vNCM = dr["ncm"].ToString();
                    string vDETALHE = dr["detalhe"].ToString();
                    string vDESCRICAO = dr["descricao"].ToString();
                    insumos.Add(new Insumo() { NCM=vNCM, detalhe=vDETALHE, descricao=vDESCRICAO });
                                                            
                    
                }

                
            }
            finally
            {
                conn.Close();
            }
            

                                                
            listap.insumos = insumos;
            string jsonStrInsumos="";
            string registro;
            string comma;
            int qtInsumos = 0;
            //jsonListaPadrao = JsonSerializer.Serialize(listap);
            foreach(Insumo insumo in listap.insumos){
                qtInsumos++;
                registro = jsonReg("ncm", insumo.NCM, 0) +", "+ jsonReg("detalhe", insumo.detalhe, 0) +", "+ jsonReg("descricao", insumo.descricao, 0);
                if(qtInsumos > 1){ comma=", \n"; }else{ comma=""; };
                jsonStrInsumos = jsonStrInsumos + comma + "        { " +registro+ " }";
            }


            jsonListaPadrao = 
            "{\n";
            jsonListaPadrao = jsonListaPadrao +
            jsonReg("qtde_ncm", listap.qtde_ncm, 4) + ", \n"+
            jsonReg("qtde_insumos", listap.qtde_insumos, 4) + ", \n"+            
            "    \"insumos\": [ \n" +
                   jsonStrInsumos + "\n" +    
            "    ] \n"+
            "}";
            

            

            
            return jsonListaPadrao;

        }



        public static void gravaInsumo0(string JsonInsumo){


            //JsonProduto = File.ReadAllText(pathResponse+"produtos200106023b.json");            
            //ProdutosIE produtos = JsonSerializer.Deserialize<ProdutosIE>(JsonProduto)!;
            InsumosIE insumos = JsonSerializer.Deserialize<InsumosIE>(JsonInsumo)!;

            string strConn = ENV.api_strConn;
            NpgsqlConnection conn = new NpgsqlConnection(strConn);
            conn.Open();
            NpgsqlCommand command;
            string sql;

            try{

                foreach (Insumo ins in  insumos.insumos){
                    
                    sql = "select count(*) total from \"APISUF\".lista_padrao "+                    
                          "where ncm='"+ins.NCM+"' and detalhe='"+ins.detalhe+"'";
                    command = new NpgsqlCommand(sql, conn);
                    NpgsqlDataReader dr = command.ExecuteReader();

                        
                    dr.Read();
                    int total = Convert.ToInt32(dr["total"]);
                    
                    //Insert
                    if(total == 0){
                        sql = "insert into \"APISUF\".lista_padrao values("+
                            quote(ins.NCM)+", "+quote(ins.detalhe)+", "+quote(ins.descricao)+")";
                    };
                    
                    //Update
                    if(total > 0){
                        sql = "update \"APISUF\".lista_padrao set "+
                              "  descricao="+quote(ins.descricao)+", "+
                              "where ncm="+quote(ins.NCM)+ " and detalhe="+quote(ins.detalhe);
                    };
                    
                    //command = new NpgsqlCommand(sql, conn);
                    //command.ExecuteNonQuery();

                    
                    
                    /*while(dr.Read())
                    {                                            
                        var produto = dr["produto"];
                    }*/



                    //command.ExecuteNonQuery();

               }                

            }
            finally
            {
                conn.Close();
            }



            


        }


        public async Task<string> procListaPadraoTeste(string IE){        

            APISUFRAMA suf = new APISUFRAMA();
            
            string jsonFile = ENV.pathResponse +"lista_padrao"+IE+".json";
            
            //1. baixa produtos: #procProdutos(IE);
            //2. Processa insumos
            //foreach substr(produto,4,4){
            //  baixa insumo: procInsumos(IE, produto)
            //    
            //}


            string strConn = ENV.api_strConn;                        
            NpgsqlConnection conn = new NpgsqlConnection(strConn);            
            NpgsqlCommand command;
            string sql;
            string JsonInsumo;
            //InsumosIE insumos;


            //Processa insumo por produto            
            try{
                conn.Open();
                sql = "select distinct substr(produto,1,4) produto from \"APISUF\".produto ";
                command = new NpgsqlCommand(sql, conn);                
                NpgsqlDataReader dr = command.ExecuteReader();

                while(dr.Read())
                {
                    
                    
                    string produto = dr["produto"].ToString();
                    //****JsonInsumo = await suf.procInsumos(IE, produto);
                    //simula retorno de...await suf.procInsumos(IE, produto):;
                    //JsonInsumo = File.ReadAllText(pathResponse+"insumos200106023_0001b.json");            
                    //*****gravaListaPadrao(JsonInsumo);
                    
          
                }

                

            }
            catch (Exception ex)
            {                
                string erro = "APISUFRAMA --> [procListaPadrao/"+IE+"] error[" + ex.Message+"]";
                //File.WriteAllText("log/APISUFRAMA_erro.log", erro);  
            }
            finally
            {
                conn.Close();
            }




            //Retorna json lista padrão
            string jsonListaPdrao = ListaPadraoJson();                        
            saveFile(jsonFile, jsonListaPdrao, Encoding.UTF8);
            return jsonListaPdrao;



        }


        public string downloadLista(){

            string resultado = "mdb baixado";


            //string stringConn = @"Driver={Microsoft Access Driver (*.mdb)};DBQ=c:\DATA\pli_suframa.mdb";
            string stringConn= "DSN=suframa";

            try
            {
                OdbcConnection connection = new OdbcConnection(stringConn);
                connection.Open();
                //myCommand.Connection.Open();
            }
            catch (OdbcException e)
            {
                     Console.WriteLine("Error: {0}", e.Errors[0].Message);
            }

            return "---";

            //try
            //{

               // OdbcConnection connection = new OdbcConnection(stringConn);
                //connection.Open();
                //using (OdbcConnection connection = new OdbcConnection(connectionString))
                //{
                //command.Connection = connection;
                
                //command.ExecuteNonQuery();
                //OdbcCommand MyCommand = new OdbcCommand("select * from SUFNCM", connection);                
            /*}
            catch(OdbcConnection e)            
            {
                Console.WriteLine("Error: {0}", e.Errors[0].Message);
            }*/


           




            OleDbConnection aConnection = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=c:\DATA\pli_suframa.mdb");

            
            OleDbCommand aCommand = new OleDbCommand("select * from SUFNCM", aConnection);

            try
            {
                aConnection.Open();
                //cria o objeto datareader para fazer a conexao com a tabela
                OleDbDataReader dr = aCommand.ExecuteReader();
                Console.WriteLine("Os valores retornados da tabela são : ");

                //Faz a interação com o banco de dados lendo os dados da tabela
                while(dr.Read())
                {
                    //Console.WriteLine(dr[""]);
                    Console.WriteLine(dr.GetString(0));                    
                }
                //fecha o reader
                dr.Close();
                    //fecha a conexao
                aConnection.Close();
            //pausa
               //Console.Readkey();
            }
            catch(OleDbException e)
            {
                Console.WriteLine("Error: {0}", e.Errors[0].Message);
            }


            return resultado;



        }






        public void conecta(){

            
            int i = 0;
            
            var teste = ENV.api_strConn;

            string strConn = ENV.api_strConn;
            
            NpgsqlConnection conn = new NpgsqlConnection(strConn);
            conn.Open();

            NpgsqlCommand command = new NpgsqlCommand("select * from \"APISUF\".produto", conn);


            try
            {
            NpgsqlDataReader dr = command.ExecuteReader();
            while(dr.Read())
            {
                for (i = 0; i < dr.FieldCount; i++)
                {
                    Console.Write("{0} \t", dr[i]);
                }
                Console.WriteLine();
            }

            }

            finally
            {
            conn.Close();
            }
                        
            
            
            
            
            
            
            //'DRIVER={MySQL ODBC 8.0 Ansi Driver}; SERVER=localhost; port=3307; DATABASE=lpddbice; USER=anebrev; PASSWORD=12345;OPTION=3';
            //string strConn = @"Server = .\sqlexpress;Database = NorthWind; Integrated Security = SSPI;";            
            //string strConn = "DRIVER={MySQL ODBC 8.0 Ansi Driver}; SERVER=localhost; port=3307; DATABASE=lpddbice; USER=anebrev; PASSWORD=12345;OPTION=3";
            //string strConn = "Data Source=132.148.148.197;Initial Catalog=Neuron;Persist Security Info=True;User ID=sa;Password=iGtZ@@X2zLVD3Z";
            //string strConn = "Data Source=ec2-18-210-233-138.compute-1.amazonaws.com;Initial Catalog=d854k1o2v7u9ee;Persist Security Info=True;User ID=iqoysubhhasnui;Password=348747f12b0325a42a06faf7450964c4767c70e79581da04f50a890305aa826f";

            //Abre a conexão
            //SqlConnection conn = new SqlConnection(strConn);
            //cria um DataAdapter selecionando os dados de um tabela do SQL Server
            //SqlDataAdapter da = new SqlDataAdapter("Select * from Customers", conn);
            
            //preenche o DataTable
            //da.Fill(dt);


        }


    
       

    }


    

}




/* foreach(DataRow row in dtb.Rows)
    {
        foreach(DataColumn column in thisTable.Columns)
        {
            Console.WriteLine(row[column]);
        }
    }*/  