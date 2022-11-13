using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Text.RegularExpressions;
using UnityEngine.Networking;
using DG.Tweening;
using MBR_EXECUTA;
using FISK_Class;
using MBR_Class;
public class menuCriarConta : MonoBehaviour
{
    
    int numIdioma;

    public GameObject pnlCriarConta1; // animação de transição painel 1
    public GameObject pnlControlePais; // referente ao controle parental
    public GameObject CanvasAvatar;
    public GameObject telaLoad;

    public InputField senhaCadastro;//input para a senha do aluno
    public InputField senhaConfirmCadastro;// input para confirmação da senha do aluno
    private bool cadastrarCamposOK = false;
    public Text[] alertasCadastro; // campos de alerta para cada input
    public Color corCadastroInvalido;//cor para aplicar no input quando algum dado estiver incorreto
    public Color corCadastroValido;//cor para aplicar no input quando algun dado estiver correto
    public GameObject CanvasLogin;
    string IdDaFaixaEtaria;

    public URLCodigo scriptTelaPais;

    private void Start()
    {
        print("PRIMEIRO ACESSO: "+FISK.RetornaPrimeiroAcesso());
        numIdioma = PlayerPrefs.GetInt("IDIOMA");
        retornaIDLivro();
        Destroy(GameObject.Find("Canvas - Login(Clone)"));
        if (int.Parse(IdDaFaixaEtaria) < 4)
        {
            scriptTelaPais.abreTelaControlePais();
        }
       
    }

    private IEnumerator FecharPnlCriarConta()
    {
        pnlCriarConta1.transform.DOMoveX(1500, 0.2f);

        yield return new WaitForSeconds(0.6f);

        pnlCriarConta1.SetActive(false);
    }

    public void fechaModalCodigoPais(){
        /*resetaTelaCadastrar();
        scriptTelaPais.finaliza2();
        pnlCriarConta1.SetActive(true);*/
        GameObject RemoveMenuPais = GameObject.Find("Canvas - CriaSenha(Clone)");
        Instantiate(CanvasLogin, new Vector3(0, 0, 0), Quaternion.identity);
        Destroy(RemoveMenuPais);
    }
    
    void resetaTelaCadastrar()
    {
        senhaCadastro.text = "";
        senhaConfirmCadastro.text = "";
        senhaCadastro.gameObject.GetComponent<Image>().color = corCadastroValido;
        senhaConfirmCadastro.gameObject.GetComponent<Image>().color = corCadastroValido;
        alertasCadastro[0].text = "";
    }
    
    public void cadastrar()
    {
        StartCoroutine(validaCadastro());
    }
    public void retornaIDLivro()
    {
        IdDaFaixaEtaria = FISK.RetornaIdFaixaEtariaLivro();
    }
    IEnumerator validaCadastro()
    {
        yield return null;
        cadastrarCamposOK = false;

        ValidaSenha();
        
      
         if (cadastrarCamposOK == true)
        {
            SalvarCadastro(senhaCadastro.text);
            pnlCriarConta1.SetActive(false);              
            // Instantiate(CanvasAvatar, new Vector3(0, 0, 0), Quaternion.identity);
                print("KIDS");
            //AVATAR INFANTIL
            if (int.Parse(IdDaFaixaEtaria) < 4)
            {
                yield return StartCoroutine(ControleAvatarInfantil.GetAssetBundleAvatar((result) =>
                {
                    if (!result)
                    {
                        MBR.CarregaAvisoSemInternet();
                    }
                }));
            }
            else
            {
                //AVATAR ADULTO - ADOLECENTE
                Home.CarregaTelaEditaAvatar();
            }
                Destroy(GameObject.FindWithTag("loading"));
            
        }
        else
        {
            print("ALGUNS CAMPOS NÃO ESTÃO CORRETOS");
        }
 
    }

   

    void ValidaSenha()
    {

        if (senhaCadastro.text.Length == 0 && senhaConfirmCadastro.text.Length == 0)
        {
            senhaCadastro.gameObject.GetComponent<Image>().color = corCadastroInvalido;

            senhaConfirmCadastro.gameObject.GetComponent<Image>().color = corCadastroInvalido;

            if(numIdioma == 0){
                   alertasCadastro[0].text = "Preencha os dois campos com sua senha.";
            }else if(numIdioma == 1){
                alertasCadastro[0].text = "Fill in both fields with your password.";
            }else{
                alertasCadastro[0].text = "Rellene ambos campos con su contraseña.";
            }

            print("O campo senha e campo confirmar senha estão vazios.");
            return;
        }
        
        if (senhaCadastro.text.Length == 0)
        {
            senhaCadastro.gameObject.GetComponent<Image>().color = corCadastroInvalido;
            senhaConfirmCadastro.gameObject.GetComponent<Image>().color = corCadastroInvalido;

            if (numIdioma == 0){
                alertasCadastro[0].text =  "Insira uma senha.";
            }else if(numIdioma == 1){
                alertasCadastro[0].text = "Rewrite your password.";
            }else{
                alertasCadastro[0].text = "Ingrese una contraseña.";
            }
            print("O campo senha está vazio.");
            return;
        }
        if (senhaConfirmCadastro.text.Length == 0)
        {
            senhaConfirmCadastro.gameObject.GetComponent<Image>().color = corCadastroInvalido;
            senhaCadastro.gameObject.GetComponent<Image>().color = corCadastroInvalido;
            if (numIdioma == 0){
                alertasCadastro[0].text =  "Reescreva sua senha.";
            }else if(numIdioma == 1){
                alertasCadastro[0].text = "Rewrite your password.";
            }else{
                alertasCadastro[0].text = "Reescribe tu contraseña.";
            }
            print("Campo Confirma Senha está vazio");
            return;
        }
        if (senhaCadastro.text.Length < 5)
        {
            senhaCadastro.gameObject.GetComponent<Image>().color = corCadastroInvalido;
            senhaConfirmCadastro.gameObject.GetComponent<Image>().color = corCadastroInvalido;

            if (numIdioma == 0)
            {
                alertasCadastro[0].text = "Senha muito curta.";
            }
            else if (numIdioma == 1)
            {
                alertasCadastro[0].text = "Too short password.";
            }
            else
            {
                alertasCadastro[0].text = "Ingrese una contraseña.";
            }
            print("O campo senha é muito pequeno.");
            return;
        }

        if (senhaCadastro.text == senhaConfirmCadastro.text)
        {
            cadastrarCamposOK = true;
            alertasCadastro[0].text = "";
            senhaCadastro.gameObject.GetComponent<Image>().color = corCadastroValido;
            senhaConfirmCadastro.gameObject.GetComponent<Image>().color = corCadastroValido;
        }
        else
        {
            cadastrarCamposOK = false;
           

            if(numIdioma == 0){
                alertasCadastro[0].text =  "O campo Confirmar Senha está diferente do campo Senha.";
            }else if(numIdioma == 1){
                alertasCadastro[0].text = "The Confirm Password field is different from the Password field.";
            }else{
                alertasCadastro[0].text = "El campo Confirmar contraseña es diferente del campo Contraseña.";
            }

            senhaConfirmCadastro.gameObject.GetComponent<Image>().color = corCadastroInvalido;
        }


    }

  



    public void saiuInpuCodigo()
    {
      //  StartCoroutine(verficaCodigoExiste());
    }


    public void fechaCriarConta()
    {
        StartCoroutine(destruirTelaAnim());
    }
    IEnumerator destruirTelaAnim()
    {
        telaLoad.SetActive(true);

        yield return new WaitForSeconds(1f);
        telaLoad.SetActive(false);

        Destroy(GameObject.FindWithTag("loading"));
        if (int.Parse(IdDaFaixaEtaria) < 4)
        {
            scriptTelaPais.abreTelaControlePais();
        }
        else
        {
            Instantiate(CanvasLogin, new Vector3(0, 0, 0), Quaternion.identity);
            Destroy(transform.parent.gameObject);
        }
        
    }
    void SalvarCadastro(string senha)
    {

        PlayerPrefs.SetString("senhaCadastro", senha);
        PlayerPrefs.SetString("tempSenha", senha);
    }
    
   

}



