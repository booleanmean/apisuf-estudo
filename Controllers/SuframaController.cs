
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Net.Mime;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


namespace APICET_BKEND.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //public class API_SuframaController : ControllerBase
    public class SuframaController : ControllerBase
    {


        /*[HttpGet]
        public IEnumerable<string> Get()
        {

            return new string[] { "Value 1", "Value 2", "Value 3" };

        }*/

        /*[HttpGet("{produtos}")]
        public string Get(string IE)
        {

            string jsonStr = ".....";
            return jsonStr;

        } */       


        /// <summary>
        /// Pega produtos suframa da Inscrição Suframa (IS)
        /// </summary>
        /// <param name="IS">Inscricao Estadual</param>
        /// <returns></returns>
        [HttpGet("produtos/{IS}")]
        [Produces(MediaTypeNames.Application.Json)]        
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]                
        public async Task<ActionResult<ProdutosIE>> Get(string IS) ////public async Task<string> Get(string IE)
        {

            
            if(IE.Trim().Length != 9){                 
                return BadRequest();                 
            }
            
                        
            
            APISUFRAMA suf = new APISUFRAMA();
            //string jsonStr = "";
            //var resultado = await suf.procProdutos(IE); //200106023
            //jsonStr = resultado.ToString();
            //return jsonStr;


            ProdutosIE prd = await suf.procProdutos(IE); //200106023
            
            
           
            
            //ProdutosIE prd = new ProdutosIE();


            if (prd.qtde_produtos == "0"){
                //return NoContent();
                return NotFound( new {IE = IE} );
            }

            return Ok(prd);
            
            
            
            
            
           /* 
            testes
            
            string jsonStr = "Inscrição Estadual inválida";
            
            //if(IE == "12345"){
            //    jsonStr = "{ Produtos da IE 12345 }";
           //}
            APISUFRAMA suf = new APISUFRAMA();
            //suf.GetRequestTokenAsync("---");
            
            jsonStr = suf.RESULTAPI;
            //jsonStr = APISUFRAMA.ProcProdutos(IE);
            //APISUFRAMA.APISUFRAMA.            

            return jsonStr;
            */


        }

        


        
        
        /// <summary>
        /// Pega insumos por IE/produto
        /// </summary>
        /// <param name="IE">Inscricao Estadual</param>
        /// <param name="produto">Produto (nnnn)</param>
        /// <returns></returns> 
        [HttpGet("insumos/{IE}/{produto}")]
        [Produces(MediaTypeNames.Application.Json)]        
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]      
        public async Task<ActionResult<InsumosIE>> Get(string IE, string produto)
        {



            if(IE.Trim().Length != 9){ return BadRequest(); }
            if(produto.Trim().Length != 4){ return BadRequest(); }



            
            
            APISUFRAMA suf = new APISUFRAMA();                        
            InsumosIE insumos = await suf.procInsumos(IE, produto); //"200106023", "0001"



            if (insumos.qtde_insumos == "0"){                
                return NotFound( new {IE = IE} );
            }




            return Ok(insumos);

            //OLD with string:
            //string jsonStr = "";
            //var resultado = await suf.procInsumos(IE, produto); //"200106023", "0001"
            //jsonStr = resultado.ToString();
            //return jsonStr;


         




        }



        


        /// <summary>
        /// Pega lista padrão de insumos da IE
        /// </summary>
        /// <param name="IE">Inscricao Estadual</param>        
        /// <returns></returns> 
        [HttpGet("lista_padrao/{IE}")]
        //public string Get(string IE)
        public async Task<ActionResult<ListaPadrao>> Get(string IE, int tipo)
        {

            
            
            
            if(IE.Trim().Length != 9){ return BadRequest(); }
        
                        
            APISUFRAMA suf = new APISUFRAMA();                        
            ListaPadrao listap = await suf.procListaPadrao(IE);   


            if (listap.qtde_insumos == "0"){                
                return NotFound( new {IE = IE} );
            }            
                        
            
            return listap;

            
            
            
            
            //OLD
            //string jsonStr = "";
            //var resultado = await suf.procListaPadrao(IE);                                                
            //jsonStr = resultado.ToString();
            //return jsonStr;
            
                    
        }









        [HttpPost("testepost")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        public ActionResult<Produto> Post( 
            [FromBody] Produto produto
        )
        {



            //ProdutoID prd = new ProdutoID();
            //return prd;
            return Ok();

        }




    }
}
