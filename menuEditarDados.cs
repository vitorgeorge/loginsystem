using FISK_Class;
using MBR_EXECUTA;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using MBR_Class;
public class menuEditarDados : MonoBehaviour
{
    int numIdioma;
    public GameObject boxCadastrado;
    private bool[] cadastrarCamposOK = new bool[1];
    public InputField senhaCadastro;//input para a senha do aluno
    public InputField senhaConfirmCadastro;// input para confirmação da senha do aluno
    public InputField eMailConfirmCadastro;//input para confirmação de email do aluno
    public InputField eMailCadastro;//input para o e-mail do aluno
    public Text[] alertasCadastro; // campos de alerta para cada input
    public Color corCadastroInvalido;//cor para aplicar no input quando algum dado estiver incorreto
    public Color corCadastroValido;//cor para aplicar no input quando algun dado estiver correto
    bool senhaAparece;
    bool senhaApareceConfirma;
    //função que define a escolha do nivel de dificuldade do Cyber
    public Sprite olhoFechado;
    public Sprite olhoAberto;
    public GameObject senhaImage;
    public GameObject confirmaSenhaImage;
    public GameObject transicaoTween;
    //função que é chamada para concluir o cadastro do aluno
    public void finalizaCadastro()
    {
        cadastrarCamposOK = new bool[] { false, false };
        
        ValidaEmail();
        ValidaSenha();
        if (cadastrarCamposOK[0] == true && cadastrarCamposOK[1] == true)
        {
            cmdEditarDados();
        }
      /*  else
        {

        }*/
    }
    private void Start()
    {
        transicaoTween.SetActive(true);
        numIdioma = PlayerPrefs.GetInt("IDIOMA");
        eMailCadastro.text = FISK.RetornaEmailUsuario();
        eMailConfirmCadastro.text = FISK.RetornaEmailUsuario();
    }
    public void cmdEditarDados()
    {
        MBR.CarregaTelaLoading();
        string senha = senhaCadastro.text;
        string email = eMailCadastro.text;
        print("SENHA É: "+senha);
        print("EMAIL É:" + email);
        Executa.Cmd(FISK.cmdAtualizarDados.Replace(":id", FISK.RetornaIDUsuario()).Replace(":usuarioAdm", "0").Replace(":senha", senha).Replace(":email", email).Replace(":ativo", "S").Replace(":unidade", FISK.RetornaIdUnidade()), (resp) =>
        {
            if (resp.tipo == TipoResposta.erro)
            {
                print(resp.erro);
                print("ERRO AO ENVIAR INFORMAÇÕES");
                MBR.RemoveTelaLoading();
            }
            else
            {
                print("INFORMAÇÕES ALTERADAS COM SUCESSO");
                boxCadastrado.SetActive(true);
                FISK.GravaEmailUsuario(eMailCadastro.text);
                //TelaLoading.SetActive(false);
                MBR.RemoveTelaLoading();
            }

        });
        
       //MBR.RemoveTelaLoading();
    }
    public void mostraSenha()
    {        
        if (senhaAparece == false)
        {
            senhaCadastro.contentType = InputField.ContentType.Standard;
            senhaCadastro.Select();
            senhaCadastro.ActivateInputField();
            senhaAparece = true;
            senhaImage.GetComponent<Image>().sprite = olhoFechado;
        }
        else
        {
            senhaCadastro.contentType = InputField.ContentType.Password;
            senhaCadastro.Select();
            senhaCadastro.ActivateInputField();
            senhaAparece = false;
            senhaImage.GetComponent<Image>().sprite = olhoAberto;
        }
    }
    public void mostraSenhaConfirma()
    {
        if (senhaApareceConfirma == false)
        {
            senhaConfirmCadastro.contentType = InputField.ContentType.Standard;
            senhaConfirmCadastro.Select();
            senhaConfirmCadastro.ActivateInputField();
            senhaApareceConfirma = true;
            confirmaSenhaImage.GetComponent<Image>().sprite = olhoFechado;
        }
        else
        {
            senhaConfirmCadastro.contentType = InputField.ContentType.Password;
            senhaConfirmCadastro.Select();
            senhaConfirmCadastro.ActivateInputField();
            senhaApareceConfirma = false;
            confirmaSenhaImage.GetComponent<Image>().sprite = olhoAberto;
        }
    }
    void ValidaSenha()
    {

        if (senhaCadastro.text.Length == 0 && senhaConfirmCadastro.text.Length == 0)
        {
            senhaCadastro.gameObject.GetComponent<Image>().color = corCadastroInvalido;

            senhaConfirmCadastro.gameObject.GetComponent<Image>().color = corCadastroInvalido;

            if (numIdioma == 0)
            {
                alertasCadastro[0].text = "Preencha os dois campos com sua senha.";
            }
            else if (numIdioma == 1)
            {
                alertasCadastro[0].text = "Fill in both fields with your password.";
            }
            else
            {
                alertasCadastro[0].text = "Rellene ambos campos con su contraseña.";
            }

            print("O campo senha e campo confirmar senha estão vazios.");
            return;
        }

        if (senhaCadastro.text.Length == 0)
        {
            senhaCadastro.gameObject.GetComponent<Image>().color = corCadastroInvalido;
            senhaConfirmCadastro.gameObject.GetComponent<Image>().color = corCadastroInvalido;

            if (numIdioma == 0)
            {
                alertasCadastro[0].text = "Insira uma senha.";
            }
            else if (numIdioma == 1)
            {
                alertasCadastro[0].text = "Rewrite your password.";
            }
            else
            {
                alertasCadastro[0].text = "Ingrese una contraseña.";
            }
            print("O campo senha está vazio.");
            return;
        }
        if (senhaConfirmCadastro.text.Length == 0)
        {
            senhaConfirmCadastro.gameObject.GetComponent<Image>().color = corCadastroInvalido;
            senhaCadastro.gameObject.GetComponent<Image>().color = corCadastroInvalido;
            if (numIdioma == 0)
            {
                alertasCadastro[0].text = "Reescreva sua senha.";
            }
            else if (numIdioma == 1)
            {
                alertasCadastro[0].text = "Rewrite your password.";
            }
            else
            {
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
            cadastrarCamposOK[0] = true;
            alertasCadastro[0].text = "";
            senhaCadastro.gameObject.GetComponent<Image>().color = corCadastroValido;
            senhaConfirmCadastro.gameObject.GetComponent<Image>().color = corCadastroValido;
        }
        else
        {
            cadastrarCamposOK[0] = false;


            if (numIdioma == 0)
            {
                alertasCadastro[0].text = "O campo Confirmar Senha está diferente do campo Senha.";
            }
            else if (numIdioma == 1)
            {
                alertasCadastro[0].text = "The Confirm Password field is different from the Password field.";
            }
            else
            {
                alertasCadastro[0].text = "El campo Confirmar contraseña es diferente del campo Contraseña.";
            }

            senhaConfirmCadastro.gameObject.GetComponent<Image>().color = corCadastroInvalido;
        }


    }

    void ValidaEmail()
    {
        if (eMailCadastro.text.Length == 0 && eMailConfirmCadastro.text.Length == 0)
        {
            eMailCadastro.gameObject.GetComponent<Image>().color = corCadastroInvalido;
            eMailConfirmCadastro.gameObject.GetComponent<Image>().color = corCadastroInvalido;
            if (numIdioma == 0)
            {
                alertasCadastro[1].text = "Preencha os dois campos com o seu e-mail";
            }
            else if (numIdioma == 1)
            {
                alertasCadastro[1].text = "Fill in both fields with your e-mail.";
            }
            else
            {
                alertasCadastro[1].text = "Ingrese su correo electrónico.";
            }
            return;
        }
        if (eMailCadastro.text.Length == 0)
        {
            eMailCadastro.gameObject.GetComponent<Image>().color = corCadastroInvalido;
            eMailConfirmCadastro.gameObject.GetComponent<Image>().color = corCadastroInvalido;
            if (numIdioma == 0)
            {
                alertasCadastro[1].text = "Insira seu E-mail.";
            }
            else if (numIdioma == 1)
            {
                alertasCadastro[1].text = "Enter your E-mail.";
            }
            else
            {
                alertasCadastro[1].text = "Ingrese su correo electrónico.";
            }
            return;
        }
        
        bool emailValido;
        const string MatchEmailPattern =
        @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
        + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
          + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
        + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";

        emailValido = Regex.IsMatch(eMailCadastro.text, MatchEmailPattern);

        print(Regex.IsMatch(eMailCadastro.text, MatchEmailPattern));

        if (emailValido && eMailCadastro.text == eMailConfirmCadastro.text)
        {
            print("E-MAIL é VÁLIDO");
            cadastrarCamposOK[1] = true;
            alertasCadastro[1].text = "";
            eMailCadastro.gameObject.GetComponent<Image>().color = corCadastroValido;
            eMailConfirmCadastro.gameObject.GetComponent<Image>().color = corCadastroValido;
        }
        else if(eMailCadastro.text != eMailConfirmCadastro.text)
        {
            print("E-MAIL NÃO É VÁLIDO");
            eMailCadastro.gameObject.GetComponent<Image>().color = corCadastroInvalido;
            eMailConfirmCadastro.gameObject.GetComponent<Image>().color = corCadastroInvalido;
            if (numIdioma == 0)
            {
                alertasCadastro[1].text = "O campo confirmar e-mail está diferente do campo e-mail";
            }
            else if (numIdioma == 1)
            {
                alertasCadastro[1].text = "The Confirm e-mail field is different from the e-mail field.";
            }
            else
            {
                alertasCadastro[1].text = "Por favor ingrese un correo electrónico válido.";
            }
            cadastrarCamposOK[1] = false;
        }
        else
        {
            print("E-MAIL NÃO É VÁLIDO");
            eMailCadastro.gameObject.GetComponent<Image>().color = corCadastroInvalido;
            eMailConfirmCadastro.gameObject.GetComponent<Image>().color = corCadastroInvalido;
            if (numIdioma == 0)
            {
                alertasCadastro[1].text = "Insira um E-mail válido.";
            }
            else if (numIdioma == 1)
            {
                alertasCadastro[1].text = "Please enter a valid email.";
            }
            else
            {
                alertasCadastro[1].text = "Por favor ingrese un correo electrónico válido.";
            }
            cadastrarCamposOK[1] = false;
        }


    }

    public void retornaHome()
    {
        StartCoroutine(loadRetornar());
    }
    IEnumerator loadRetornar()
    {
       
        MBR.CarregaTelaLoading();
        yield return new WaitForSeconds(2f);
        MBR.RemoveTelaLoading();
        Destroy(gameObject);
    }
}
