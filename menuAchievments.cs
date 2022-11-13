using FISK_Class;
using MBR_Class;
using MBR_EXECUTA;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class menuAchievments : MonoBehaviour
{
    public Button btSalvarCadastro;//guarda o botão com a ação de salavar o cadastro do aluno
    public GameObject BtnSimOn;
    public GameObject BtnNaoOn;
    public GameObject loading;
    public GameObject box;
    public GameObject boxCadastroSucesso;
    public GameObject boxCadastroFalhou;
    private bool podeCompartilhar = true;
    public GameObject menuCriaSenha;
    string IdDaFaixaEtaria;
    public void clickCompartilhar(bool ehCompartilhar)
    {
        if (ehCompartilhar)
        {
            BtnSimOn.GetComponent<Image>().color = new Color(255, 255, 255, 1);
            BtnNaoOn.GetComponent<Image>().color = new Color(255, 255, 255, 0.5f);
            PlayerPrefs.SetString("podeCompartilhar", "S");
        }
        else
        {
            BtnSimOn.GetComponent<Image>().color = new Color(255, 255, 255, 0.5f);
            BtnNaoOn.GetComponent<Image>().color = new Color(255, 255, 255, 1f);
            PlayerPrefs.SetString("podeCompartilhar", "N");
        }
        print("ehCompartilhar " + PlayerPrefs.GetString("podeCompartilhar"));
    }
    //função que define a escolha do nivel de dificuldade do Cyber


    //função que é chamada para concluir o cadastro do aluno
    public void finalizaCadastro()
    {
        print("FINALIZANDO CADASTRO");
        StartCoroutine(cmdEnviaDadosPrimeiroAcesso());
        
    }
    public IEnumerator cmdEnviaDadosPrimeiroAcesso()
    {
         
        loading.SetActive(true);
        yield return FISK.GravaRelatorioSemAtvJogada();
        string senha = PlayerPrefs.GetString("senhaCadastro");
        string compartilhar = PlayerPrefs.GetString("COMPARTILHAR");
        string primeiroAcesso = "N";
        Executa.Cmd(FISK.cmdFinalizaPrimeiroAcesso.Replace(":id", FISK.RetornaIDUsuario()).Replace(":usuarioAdm", "0").Replace(":senha", senha).Replace(":primeiro_acesso", primeiroAcesso).Replace(":compartilhar", compartilhar).Replace(":ativo", "S").Replace(":unidade", FISK.RetornaIdUnidade()), (resp) =>
        {
            if (resp.tipo == TipoResposta.erro)
            {
                print(resp.erro);
                print("ERRO AO ENVIAR INFORMAÇÕES");
                if(boxCadastroFalhou) boxCadastroFalhou.SetActive(true);
            }
            else
            {
                print("INFORMAÇÕES ALTERADAS COM SUCESSO");
                if(boxCadastroSucesso) boxCadastroSucesso.SetActive(true);
               PlayerPrefs.SetString("PRIMEIRO_ACESSO" , "N");
                //TelaLoading.SetActive(false);
            }

        });

       
        loading.SetActive(false);
    }
    public void carregaHome()
    {
        FISK.GravaLoginSenha(PlayerPrefs.GetString("tempLogin"), PlayerPrefs.GetString("tempSenha"));
        PlayerPrefs.DeleteKey("tempSenha");
        PlayerPrefs.DeleteKey("tempLogin");
        PlayerPrefs.DeleteKey("senhaCadastro");
        MBR.CarregaCenaAsync("Home");
    }

    IEnumerator GravaRelatorioSemAtv(){
        yield return FISK.GravaRelatorioSemAtvJogada();
    }
    public void voltar()
    {
        IdDaFaixaEtaria = FISK.RetornaIdFaixaEtariaLivro();
        if (int.Parse(IdDaFaixaEtaria) < 4)
        {
            StartCoroutine(voltaJogo());
        }
        else
        {
            //avatarEder
        }
    }
    IEnumerator voltaJogo()
    {
        loading.SetActive(true);
        AssetBundle.UnloadAllAssetBundles(true);
        Destroy(GameObject.Find("AvatarInfantil(Clone)"));
        yield return new WaitForSeconds(1f);
        /* yield return StartCoroutine(ControleAvatarInfantil.GetAssetBundleAvatar((result) =>
          {
              if (!result)
              {
                  MBR_Class.MBR.CarregaAvisoSemInternet();
              }
          }));*/
        Destroy(GameObject.Find("Canvas - CriaSenha(Clone)"));
        Instantiate(menuCriaSenha, new Vector3(0, 0, 0), Quaternion.identity);
        loading.SetActive(false);
      
        Destroy(gameObject);
    }
        /*StartCoroutine(voltaJogo());
       
    }
    IEnumerator voltaJogo()
    {
        loading.SetActive(true);
       // Destroy(ControleAvatarInfantil.go);
       /* yield return StartCoroutine(ControleAvatarInfantil.GetAssetBundleAvatar((result) =>
        {
            if (!result)
            {
                MBR_Class.MBR.CarregaAvisoSemInternet();
            }
        }));*/
      /*  loading.SetActive(false);
        Destroy(gameObject);
        //SceneManager.LoadScene("Home");

    }*/
}
