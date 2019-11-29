;
(function ()
{
  "use strict";

  angular
    .module("boilerplate")
    .controller("ChatbotController", ChatbotController);

  ChatbotController.$inject = ["$scope", "$state", "LocalStorage", "QueryService", "SweetAlert", "$window"];

  function ChatbotController($scope, $state, LocalStorage, QueryService, SweetAlert, $window)
  {
    var self = this;

    // Padrão
    self.autenticacao = LocalStorage.get("autenticacao") || {};
    self.conta = self.autenticacao.Conta;
    self.perfil = self.autenticacao.Perfil;
    self.perfilSelecionado = LocalStorage.get("perfilselecionado") || {};

    var UraAlternativa = function ()
    {
      return {
        Codigo: "",
        Acao: 0,
        Opcao: "",
        TipoResposta: "Texto",
        Resposta: "",
        PalavrasChave: [],
        SubMenu: null
      };
    };

    var UraInterna = function ()
    {
      return {
        TextoInicial: "",
        Alternativas: [new UraAlternativa()]
      }
    };

    var Ura = function ()
    {
      return {
        Usuario: self.perfilSelecionado.Usuario,
        TextoInicial: "",
        TextoEncerrarAtendimento: "Muito bom falar com você. Entre em contato sempre que quiser!",
        TextoNaoEntendi: "Desculpa, mas não entendi a sua pergunta!\\r\\n\\nPosso lhe ajudar com mais alguma coisa?\\r\\n\\n",
        Alternativas: [new UraAlternativa()]
      }
    };


    self.chatbot = {};


    self.alternativa = function (tipoResposta, alternativa)
    {
      alternativa.TipoResposta = tipoResposta;

      if (tipoResposta === 'Submenu')
      {
        alternativa.SubMenu = new UraInterna();
      }
      else
      {
        alternativa.SubMenu = null;
      }
    };


    self.adicionarAlternativa = function (colecao)
    {
      colecao.push(new UraAlternativa());
    };


    self.removerAlternativa = function (colecao, indice)
    {
      SweetAlert
        .confirm("", { title: 'Confirma exclusão deste item?', cancelButtonText: 'Cancelar!' })
        .then(function (acao)
        {
          if (acao)
          {
            colecao.splice(indice, 1);
          }
        });
    }


    var tratarRespostaEntrega = function (colecao)
    {
      angular.forEach(colecao, function (valor, chave)
      {
        if (valor.TipoResposta === "Texto")
        {
          valor.Resposta = valor.RespostaTexto;
          return;
        }

        if (valor.TipoResposta === "Imagem")
        {
          valor.Resposta = valor.RespostaImagem;
          return;
        }

        if (valor.TipoResposta === "Link")
        {
          valor.Resposta = valor.RespostaLink;
          return;
        }

        if (valor.TipoResposta === "Submenu")
        {
          tratarRespostaEntrega(valor.SubMenu.Alternativas);
          return;
        }
      });
    };


    var tratarRespostaRetorno = function (colecao)
    {
      angular.forEach(colecao, function (valor, chave)
      {
        if (valor.TipoResposta === "Texto")
        {
          valor.RespostaTexto = valor.Resposta;
          return;
        }

        if (valor.TipoResposta === "Imagem")
        {
          valor.RespostaImagem = valor.Resposta;
          return;
        }

        if (valor.TipoResposta === "Link")
        {
          valor.RespostaLink = valor.Resposta;
          return;
        }

        if (valor.TipoResposta === "Submenu")
        {
          tratarRespostaRetorno(valor.SubMenu.Alternativas);
          return;
        }
      });
    };


    self.salvar = function (form)
    {
      if (!form.$valid)
      {
        SweetAlert.error("Revise seu Chatbot ou Configurações, pois existem campos que não foram preenchidos.", { title: "Ops..." });
        return false;
      }

      // Tratamento de resposta
      tratarRespostaEntrega(self.chatbot.Alternativas);

      QueryService
        .query("POST", "dashboards/chatbots", null, self.chatbot)
        .then(function (d)
        {
          SweetAlert.success("A partir de agora o Chatbot começará as interações com seus contatos.", { title: "Tudo certo!" });
        });
    };


    self.obterChatbot = function ()
    {
      QueryService
        .query("GET", "dashboards/chatbots", { usuario: self.perfilSelecionado.Usuario })
        .then(function (d)
        {
          if (d.data)
          {
            var dados = d.data;

            tratarRespostaRetorno(dados.Alternativas);

            self.chatbot = dados;
          }
          else
          {
            self.chatbot = new Ura();
          }
        });
    }();

  };

})();
