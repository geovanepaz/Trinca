;
(function ()
{
  "use strict";

  angular
    .module("boilerplate")
    .controller("HashtagController", HashtagController);

  HashtagController.$inject = ["$scope", "$state", "LocalStorage", "QueryService", "SweetAlert", "CadastroService"];

  function HashtagController($scope, $state, LocalStorage, QueryService, SweetAlert, CadastroService)
  {
    // 'controller as'
    var self = this;

    // Padrão
    self.autenticacao = LocalStorage.get("autenticacao") || {};
    self.conta = self.autenticacao.Conta;
    self.perfil = self.autenticacao.Perfil;
    self.perfilSelecionado = LocalStorage.get("perfilselecionado") || {};
    self.ramosAtividades = {};

    // Local
    self.hashtag = "";
    self.SeguirQuemCurtiu = false;
    self.hashtagsAdicionadas = [];


    var obterHashes = function ()
    {
      QueryService
        .query("GET", "dashboards/hashtags", { idPerfil: self.perfilSelecionado.IdPerfil })
        .then(function (d)
        {
          self.hashtagsAdicionadas = d.data;
        });
    };


    obterHashes();


    self.adicionar = function (form, eBlack)
    {
      if (form.$valid)
      {
        if (self.hashtagsAdicionadas.some(e => e.Hashtag == self.hashtag))
        {
          SweetAlert.info("Você já adicionou esta hashtag!", { title: "Ops..." });
          return false;
        }

        var obj =
        {
          Hashtag: eBlack ? self.blackhashtag : self.hashtag,
          IdPerfil: self.perfilSelecionado.IdPerfil,
          BlackHashtag: eBlack,
          SeguirQuemCurtiu: self.SeguirQuemCurtiu ? 1 : 0
        };

        QueryService
          .query("POST", "dashboards/hashtags", null, obj)
          .then(function ()
          {
            if (eBlack)
            {
              self.blackhashtag = "";
              self.SeguirQuemCurtiu = false;
            }
            else
            {
              self.hashtag = "";
              self.SeguirQuemCurtiu = false;
            }

            obterHashes();
          });
      }
    };


    self.remover = function (id)
    {
      SweetAlert
        .confirm("Confirma a remoção desta hashtag?", { title: "Tem certeza?", cancelButtonText: 'Cancelar!' })
        .then(function (acao)
        {
          if (acao)
          {
            QueryService
              .query("DELETE", "dashboards/hashtags", { id }, null)
              .then(obterHashes);
          }
        });
    };


  }
})();
