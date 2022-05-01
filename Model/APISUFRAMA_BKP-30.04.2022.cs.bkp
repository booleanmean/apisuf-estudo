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





    public class APISUFRAMA
    {
                        

        public APISUFRAMA()
        {
            
        }

        public string RESULTAPI = "";
        



        
        //public async Task<string> procProdutos(string IE){
        public async Task<ProdutosIE> procProdutos(string IE){
            

          
            string textFile = ENV.pathResponse+"produtos"+IE+".txt";
            string jsonFile = ENV.pathResponse +"produtos"+IE+".json";
            string URI="https://200.198.228.134/servicos/estrangeiro/consultasinternas/Siscomex/GeracaoArquivoTexto/EST_GeracaoArqTxtEmpresaGerador.asp?inscsuf="+IE;            
            string jsonText = "";
            string erro="";
            ProdutosIE produtos;

            
            if(ENV.MyEnv=="PRD1"){
                jsonText = doJsonProdutoDB(IE); 
                produtos = JsonSerializer.Deserialize<ProdutosIE>(jsonText)!;
                return produtos;
            }


            try
            {
            
            
                using (var httpClientHandler = new HttpClientHandler())
                {
                    
                    httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                    using (var client = new HttpClient(httpClientHandler))
                    {
                        // Make your request...                                         
                        var response =  await client.GetAsync(URI);                    
                        var buffer = await response.Content.ReadAsByteArrayAsync();
                        var byteArray = buffer.ToArray();
                        var contents = Encoding.GetEncoding("ISO-8859-1").GetString(byteArray, 0, byteArray.Length);                                                                              
                        saveFile(textFile, contents, Encoding.UTF8);
                                                                                    
                        //Convert json
                        jsonText = doJsonProduto(contents, IE);                    
                        saveFile(jsonFile, jsonText, Encoding.UTF8);
                        gravaProduto(jsonText);

                        produtos = JsonSerializer.Deserialize<ProdutosIE>(jsonText)!;
                        return produtos;                        
                                                                                                                            
                    }
                }

            }
            catch (HttpRequestException ex)//(Exception ex)
            {                
                //jsonText = "APISUFRAMA --> [procListaPadrao/"+IE+"] error[" + ex.Message+"]";
                erro = ex.InnerException.ToString();
                //jsonText = "APISUFRAMA --> [procListaPadrao/"+IE+"] error[" +erro+" ]";
                //File.WriteAllText("log/APISUFRAMA_erro.log", erro);  
                jsonText = doJsonProdutoDB(IE); 
            }
            finally{
                
            }
            //return jsonText;            
            //return response.ErrorException.InnerException.ToString();
            produtos = JsonSerializer.Deserialize<ProdutosIE>(jsonText)!;
            return produtos;

        }

        public async Task<InsumosIE> procInsumos(string IE, string produto){

            string textFile = ENV.pathResponse+"insumos"+IE+"_"+produto+".txt";
            string jsonFile = ENV.pathResponse+"insumos"+IE+"_"+produto+".json";
            string URI="https://200.198.228.134/Servicos/Estrangeiro/Consultasinternas/Siscomex/ProdutosInsumosVinculados/EST_ProdutoLista_ItensPublicos_xls.asp?";
            string URL_REQUEST = URI + "cd_inscsuf="+IE+"&cd_produto="+produto;
            string jsonText;
            string erro;
            InsumosIE insumos;

            if(ENV.MyEnv=="PRD"){
                jsonText = doJsonInsumosDB(IE, produto);                 
                insumos = JsonSerializer.Deserialize<InsumosIE>(jsonText)!;
                return insumos;
            }

            try
            {

                using (var httpClientHandler = new HttpClientHandler())
                {
                    httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                    using (var client = new HttpClient(httpClientHandler))
                    {
                                                                
                        var response =  await client.GetAsync(URL_REQUEST);
                        
                        var buffer = await response.Content.ReadAsByteArrayAsync();
                        var byteArray = buffer.ToArray();
                        var contents = Encoding.GetEncoding("ISO-8859-1").GetString(byteArray, 0, byteArray.Length);                                                                                                  
                        saveFile(textFile, contents, Encoding.UTF8);
                                                                                    
                        //Convert json
                        jsonText = doJsonInsumos(contents, IE, produto);                    
                        saveFile(jsonFile, jsonText, Encoding.UTF8);
                        gravaInsumo(jsonText);
                        
                    }
                }


             }
            catch (HttpRequestException ex) //(Exception ex)
            {                
                erro = ex.InnerException.ToString();
                jsonText = "APISUFRAMA --> [procInsumos/"+IE+"/"+produto+"] error[" +erro+" ]";
                saveFile("log/APISUFRAMA_erro.log", jsonText, Encoding.UTF8);                                
                //jsonText = doJsonInsumosDB(IE, produto); 
            }
            finally{
                
            }
            
            //return jsonText;
            return insumos;

        
        }

        public async Task<ListaPadrao> procListaPadrao(string IE){        

            string jsonFile = ENV.pathResponse +"lista_padrao"+IE+".json";
            string sql;            
            DataTable dtb;


            
            ////////////////////////////////////////////////////////////////////
            //1. BAIXA PRODUTOS: ///////////////////////////////////////////////
            ////////////////////////////////////////////////////////////////////
            ProdutosIE produtos = await procProdutos(IE); 
            //string jsonProduto = await procProdutos(IE);
            


            
            ////////////////////////////////////////////////////////////////////
            //2. PROCESSA INSUMOS (dos produtos da IE): ////////////////////////
            ////////////////////////////////////////////////////////////////////                             
            try
            {                
                sql = "select distinct substr(produto,1,4) produto from \"APISUF\".produto where IE="+quote(IE);
                dtb = ExecuteSelectQuery(sql);

                if (dtb is not null){
                    for (int i = 0; i <  dtb.Rows.Count; i++){
                        /*simula retorno de procInsumos(IE, produto): */ //JsonInsumo=File.ReadAllText(pathResponse+"insumos200106023_0001b.json");                      
                        InsumosIE insumos = await procInsumos(IE, dtb.Rows[i]["produto"].ToString()); 
                        //gravaListaPadrao(JsonInsumo); *nesta etapa grava apenas os insumos por IE/produto
                    }
                }                                

            }
            catch (Exception ex)
            {                
                string erro = "APISUFRAMA --> [procListaPadrao-procInsumos/"+IE+"] error[" + ex.Message+"]";
                saveFile("log/APISUFRAMA_erro.log", erro, Encoding.UTF8);                
            }
           



            ////////////////////////////////////////////////////////////////////
            //3. GRAVA LISTA PADRÃO (base insumumos daIE): ////////////////////////
            ////////////////////////////////////////////////////////////////////                             
            try{                
                sql = "delete from \"APISUF\".lista_padrao"; //where IE="+quote(IE);                
                ExecuteNonQuery(sql);

                sql = "insert into \"APISUF\".lista_padrao (ncm, detalhe, descricao) "+
                      "select distinct ncm, detalhe, descricao from \"APISUF\".insumo where IE="+quote(IE);
                ExecuteNonQuery(sql);
                
            }
            catch (Exception ex)
            {                
                string erro = "APISUFRAMA --> [procListaPadrao-grava lista/"+IE+"] error[" + ex.Message+"]";
                saveFile("log/APISUFRAMA_erro.log", erro, Encoding.UTF8);                
            }
           




            ////////////////////////////////////////////////////////////////////
            //4. RETORNA LISTA PADRÃO (JSON): //////////////////////////////////
            ////////////////////////////////////////////////////////////////////             
            string jsonListaPdrao = ListaPadraoJson();                        
            saveFile(jsonFile, jsonListaPdrao, Encoding.UTF8);
            
            ListaPadrao listap = JsonSerializer.Deserialize<ListaPadrao>(jsonListaPdrao);
            
            return listap;



        }


              


        private string doJsonProduto(string contents, string IE){

            /*[
                 {
                    "IE": "999999999",
                    "produtos": 
                        [
                        { }
                        ]
                 }
              ] */
            
            //var qtProdutos = qtdeReg(contents);
            string jsonStr = "";
            string jsonStrItens = "";
            string registro = "";
            string comma = "";
            int qtLine = 0;
            int qtProdutos = 0;
            bool temProduto = false;
            
            
            Produto0 prod = new Produto0();

            var reader = new StringReader(contents);
            


            for (string line = reader.ReadLine(); line != null; line = reader.ReadLine())
            {

                qtLine++;
                if(qtLine == 1){                  
                  prod.IE = line.Substring(0, 9);
                  if (prod.IE != IE){
                      temProduto = false;
                      break;
                  }else{
                      temProduto = true;
                  }                                    
                }

                qtProdutos++;
                prod.produto = line.Substring(9, 11);
                var tam = line.Trim().Length;
                prod.descricao = line.Substring(30, line.Trim().Length-30);
                
                registro = jsonReg("produto", prod.produto, 0) + ", " + jsonReg("descricao", prod.descricao, 0);

                if(qtProdutos > 1){ comma=", \n"; }else{ comma=""; };
                jsonStrItens = jsonStrItens + comma + "        { " + registro + " }" ;         

            }

            if (temProduto){
            
                jsonStr =                 
                "{\n";
                jsonStr = jsonStr +
                jsonReg("IE", IE, 4) + ", \n"+
                jsonReg("qtde_produtos", qtProdutos.ToString(), 4) + ", \n"+           
                "    \"produtos\": [ \n" +
                    jsonStrItens + "\n" +    
                "    ] \n"+
                "}";
                

            } else {
              jsonStr = "Retorno para a Inscrição Estadual \""+IE.Trim()+"\": \n " +contents;
            }


            /*for (string line = reader.ReadLine(); line != null; line = reader.ReadLine())
            {
                qtLine++;
                if(qtLine == 1){
                  prod.IE = line.Substring(0, 9);
                  jsonStr = jsonStr +
                  jsonReg("IE", prod.IE, 4) + ", \n"+
                  jsonReg("qtde_produtos", qtProdutos.ToString(), 4) + ", \n"+
                  "    \"produtos\": [";
                }
                
                prod.produto = line.Substring(9, 11);
                var tam = line.Trim().Length;
                prod.descricao = line.Substring(30, line.Trim().Length-30);
                
                if(qtLine > 1){ comma=", "; }else{ comma=""; };

                jsonStr = jsonStr +comma+ "\n" + 
                "      { \n" +  
                        jsonReg("produto", prod.produto, 8) + ", " + jsonReg("descricao", prod.descricao, 0) +"\n"+
                "      }" ; 
            }

             jsonStr = jsonStr+ "\n" + 
             "    ] \n"+
             "  } \n"+
             "]";
             */



            
            return jsonStr;

            
        }

        private string doJsonProdutoDB(string IE){


            string jsonStr = "";
            string jsonStrItens = "";
            string registro = "";
            string comma = "";            
            int qtProdutos = 0;
            DataTable dtb;
            string produto;
            string descricao;
            
            
            string sql = 
            "select * from \"APISUF\".produto where IE= "+quote(IE);                    
            dtb = ExecuteSelectQuery(sql);

            if (dtb is not null){

                for (int i = 0; i <  dtb.Rows.Count; i++){

                    qtProdutos++;
                    produto = dtb.Rows[i]["produto"].ToString();
                    descricao = dtb.Rows[i]["descricao"].ToString();
                    registro = jsonReg("produto", produto, 0) + ", " + jsonReg("descricao", descricao, 0);

                    if(qtProdutos > 1){ comma=", \n"; }else{ comma=""; };
                    jsonStrItens = jsonStrItens + comma + "        { " + registro + " }" ; 

                }
            }
             
             
             
             
            
             //if (qtProdutos > 0){
            
                jsonStr =                 
                "{\n";
                jsonStr = jsonStr +
                jsonReg("IE", IE, 4) + ", \n"+
                jsonReg("qtde_produtos", qtProdutos.ToString(), 4) + ", \n"+           
                "    \"produtos\": [ \n" +
                    jsonStrItens + "\n" +    
                "    ] \n"+
                "}";
                

            //} else {
            //  jsonStr = "Nenhum produto encontrado para a Inscrição Estadual \""+IE.Trim()+"\".";
           //}
            
                
            


            
            
            return jsonStr;

        }

        private string doJsonProdutoDB0(string IE){


            string jsonStr = "";
            string jsonStrItens = "";
            string registro = "";
            string comma = "";            
            int qtProdutos = 0;
            DataTable dtb;
            string produto;
            string descricao;
            
            
            string sql = 
            "select * from \"APISUF\".produto where IE= "+quote(IE);                    
            dtb = ExecuteSelectQuery(sql);

            if (dtb is not null){

                for (int i = 0; i <  dtb.Rows.Count; i++){

                    qtProdutos++;
                    produto = dtb.Rows[i]["produto"].ToString();
                    descricao = dtb.Rows[i]["descricao"].ToString();
                    registro = jsonReg("produto", produto, 0) + ", " + jsonReg("descricao", descricao, 0);

                    if(qtProdutos > 1){ comma=", \n"; }else{ comma=""; };
                    jsonStrItens = jsonStrItens + comma + "        { " + registro + " }" ; 

                }
            }
             
             
             
             
            
             if (qtProdutos > 0){
            
                jsonStr =                 
                "{\n";
                jsonStr = jsonStr +
                jsonReg("IE", IE, 4) + ", \n"+
                jsonReg("qtde_produtos", qtProdutos.ToString(), 4) + ", \n"+           
                "    \"produtos\": [ \n" +
                    jsonStrItens + "\n" +    
                "    ] \n"+
                "}";
                

            } else {
              jsonStr = "Nenhum produto encontrado para a Inscrição Estadual \""+IE.Trim()+"\".";
            }
            
                
            


            
            
            return jsonStr;

        }

        

        private string doJsonInsumos(string contents, string IE, string produto){




            HtmlDocument doc = new HtmlDocument();            
            doc.LoadHtml(contents);
            //string tabela = "";
            //string cellID = "";
            string tableID = "";            
            string field = "";
            string tdValue = "";
            string registro ="";
            bool rodape = false;
            int tab = 0;
            int i = 0;
            int j = 0;
            int qtLine = 0;
            string comma = "";
            string jsonStr = "";
            string jsonStrItens = "";
            
            foreach (HtmlNode table in doc.DocumentNode.SelectNodes("//table")) {                
                
                tableID = table.Id;               
                foreach (HtmlNode row in table.SelectNodes("tr")) {                    
                    i++;
                    j=0;
                    tdValue = "";
                    field = "";
                    
                    //Escreve linha do registro
                    if (i >=6){

                        qtLine++;
                        registro = "";
                        foreach (HtmlNode cell in row.SelectNodes("th|td")) { 
                            
                            j++;                                                                                    
                            switch(j){
                                case 1: field = "NCM"; break;
                                case 2: field = "detalhe"; break;
                                case 3: field = "descricao"; break;                                
                            }                                                        
                            //cellID = "["+i.ToString()+", "+j.ToString()+"] = ";
                            //tabela = tabela + cellID + cell.InnerText;                                                        
                            
                            //pega valor
                            tdValue = cell.InnerText.Trim();
                            tdValue = tdValue.Replace('"', '^');
                            //tdValue = tdValue.Replace("\"", "#");
                            if ( field=="detalhe" ) { tdValue.PadLeft(4,'0'); };
                                                                                                                                                                                                    
                            if(j > 1){ comma=", "; tab=0; }else{ comma=""; tab=8; };
                            registro = registro +comma+ jsonReg(field, tdValue, tab);
                            
                            if( tdValue=="Empresa:" ) { rodape=true;};
                                                                                                    
                        }

                        if (rodape){break;};
                        //adiciona linha do registro ao Json:                                              
                        if(qtLine > 1){ comma=", \n"; }else{ comma=""; };
                        jsonStrItens = jsonStrItens + comma + "        { " +registro+ " }";                        
                        
                    }
                }
            }
           
                                  
            
            jsonStr = 
            "{\n";
            jsonStr = jsonStr +
            jsonReg("IE", IE, 4) + ", \n"+
            jsonReg("produto", produto, 4) + ", \n"+
            jsonReg("qtde_insumos", (qtLine-1).ToString(), 4) + ", \n"+
            "    \"insumos\": [ \n" +
                   jsonStrItens + "\n" +    
            "    ] \n"+
            "}";


           return jsonStr;



          





            /*   testes OK 
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(@"<html><body><p><table id=""foo""><tr><th>hello</th></tr><tr><td>world</td></tr></table></body></html>");
            foreach (HtmlNode table in doc.DocumentNode.SelectNodes("//table")) {
                Console.WriteLine("Found: " + table.Id);
                foreach (HtmlNode row in table.SelectNodes("tr")) {
                    Console.WriteLine("row");
                    foreach (HtmlNode cell in row.SelectNodes("th|td")) {
                        Console.WriteLine("cell: " + cell.InnerText);
                    }
                }
            }

            var contents2 = File.ReadAllText(pathResponse+"tablesample.txt");           
            doc = new HtmlDocument();
            doc.LoadHtml(contents2);
            foreach (HtmlNode table in doc.DocumentNode.SelectNodes("//table")) {
                Console.WriteLine("Found: " + table.Id);
                foreach (HtmlNode row in table.SelectNodes("tr")) {
                    Console.WriteLine("row");
                    foreach (HtmlNode cell in row.SelectNodes("th|td")) {
                        Console.WriteLine("cell: " + cell.InnerText);
                    }
                }
            }
            */



        }

        private string doJsonInsumosDB(string IE, string produto){


            string jsonStr = "";
            string jsonStrItens = "";
            string registro = "";
            string comma = "";            
            int qtInsumos = 0;
            DataTable dtb;            
            string ncm;
            string detalhe;
            string descricao;
            
            
            string sql = 
            "select * from \"APISUF\".insumo where IE="+quote(IE)+" and produto="+quote(produto);
            dtb = ExecuteSelectQuery(sql);

            if (dtb is not null){

                for (int i = 0; i <  dtb.Rows.Count; i++){

                    qtInsumos++;
                    ncm = dtb.Rows[i]["ncm"].ToString();
                    detalhe = dtb.Rows[i]["detalhe"].ToString();
                    descricao = dtb.Rows[i]["descricao"].ToString();
                    registro = jsonReg("ncm",ncm,0) +", "+ jsonReg("detalhe",detalhe,0) +", "+ jsonReg("descricao",descricao,0);

                    if(qtInsumos > 1){ comma=", \n"; }else{ comma=""; };
                    jsonStrItens = jsonStrItens + comma + "        { " + registro + " }" ; 

                }
            }
             
                                       
            
             if (qtInsumos > 0){
            
                jsonStr =                 
                "{\n";
                jsonStr = jsonStr +
                jsonReg("IE", IE, 4) + ", \n"+
                jsonReg("produto", produto, 4) + ", \n"+
                jsonReg("qtde_insumos", qtInsumos.ToString(), 4) + ", \n"+           
                "    \"insumos\": [ \n" +
                    jsonStrItens + "\n" +    
                "    ] \n"+
                "}";
                

            } else {
              jsonStr = "Nenhum insumo encontrado para a Inscrição Estadual/produto \""+IE.Trim()+"\""+ "/\""+produto+"\"";
            }
                                                                
            return jsonStr;

        }





        public static void gravaProduto(string JsonProduto){


            //JsonProduto = File.ReadAllText(pathResponse+"produtos200106023b.json"); **teste
            ProdutosIE produtos = JsonSerializer.Deserialize<ProdutosIE>(JsonProduto)!;


            string sql;
            string resultado = "";
            

            //limpa produtos da IE
            sql = "delete from \"APISUF\".produto where IE="+quote(produtos.IE);
            resultado = ExecuteNonQuery(sql);


            try{

                foreach (Produto prod in produtos.produtos){

                    sql = "insert into \"APISUF\".produto values("+
                          "'"+produtos.IE+"', '"+prod.produto+"', '"+prod.descricao+"')";
                    resultado = ExecuteNonQuery(sql);                    

               }                

            }
            finally
            {
                
            }

        
        }

        public static void gravaInsumo(string JsonInsumo){


            //JsonInsumo = File.ReadAllText(pathResponse+"insumos200106023_0001b.json"); //**teste
            InsumosIE insumosIE = JsonSerializer.Deserialize<InsumosIE>(JsonInsumo)!;


            string sql;
            string resultado = "";
            string erro = "";
            

            //limpa insumos da IE/produto
            sql = "delete from \"APISUF\".insumo where IE="+quote(insumosIE.IE)+" and produto"+quote(insumosIE.produto);
            resultado = ExecuteNonQuery(sql);


            try{

                foreach (Insumo ins in insumosIE.insumos){

                    sql = 
                    "insert into \"APISUF\".insumo (ie, produto, ncm, detalhe, descricao) values("+
                     quote(insumosIE.IE)+", "+quote(insumosIE.produto)+", "+quote(ins.NCM)+", "+quote(ins.detalhe)+", "+quote(ins.descricao)+")";
                    resultado = ExecuteNonQuery(sql);
                    
               }                

            }
            
            catch (Exception ex)
            {
                
                erro = "APISUFRAMA --> [gravaInsumo()] error[" +ex.Message+" ]";
                File.WriteAllText("log/APISUFRAMA_erro.log", erro); 
                            
            }
            
            finally
            {
                
            }



        
        }


        public static string ListaPadraoJson(){

            
            
            string strConn =  ENV.api_strConn;
            NpgsqlConnection conn = new NpgsqlConnection(strConn);            
            NpgsqlCommand command;
            string sql;
            string jsonListaPadrao;
            ListaPadrao listap = new ListaPadrao();
            List<Insumo> insumos = new List<Insumo>();
            

            sql = "select count(distinct ncm) total,  count(detalhe) total2 from \"APISUF\".lista_padrao ";                                              
            DataTable dtb = ExecuteSelectQuery(sql);
            if (dtb is not null){
                listap.qtde_ncm = dtb.Rows[0]["total"].ToString();
                listap.qtde_insumos = dtb.Rows[0]["total2"].ToString();
            }
            
                    

            
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



       





        


      

    }


    

}
