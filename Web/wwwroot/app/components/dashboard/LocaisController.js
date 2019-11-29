;
(function ()
{
  "use strict";

  angular
    .module("boilerplate")
    .controller("LocaisController", LocaisController);

  LocaisController.$inject = ["$scope", "$state", "LocalStorage", "QueryService", "SweetAlert", "CadastroService"];

  function LocaisController($scope, $state, LocalStorage, QueryService, SweetAlert, CadastroService)
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
    self.estaBuscando = false;
    self.busca = "";
    self.locaisEncontrados = null;
    self.locaisAdicionados = [];


    var obterLocais = function ()
    {
      QueryService
        .query("GET", "dashboards/locais", { idPerfil: self.perfilSelecionado.IdPerfil })
        .then(function (d)
        {
          self.locaisAdicionados = d.data;
        });
    };


    obterLocais();


    self.buscar = function (form)
    {
      if (form.$valid)
      {
        self.estaBuscando = true;

        QueryService
          .query("GET", "dashboards/instagram/locais", { local: self.busca, perfil: self.perfilSelecionado.Usuario })
          .then(function (d)
          {
            self.estaBuscando = false;
            self.locaisEncontrados = d.data;
          });
      }
    };


    self.adicionarLocal = function (local)
    {
      if (self.locaisAdicionados.some(e => e.IdInstagram === local.IdInstagram))
      {
        SweetAlert.info("Você já adicionou este local!", { title: "Ops..." });
        return false;
      }

      local.IdPerfil = self.perfilSelecionado.IdPerfil;

      QueryService
        .query("POST", "dashboards/locais", null, local)
        .then(function ()
        {
          self.busca = "";
          self.locaisEncontrados = null;
          obterLocais();
        });
    };


    self.removerLocal = function (id)
    {
      SweetAlert
        .confirm("Confirma a remoção deste local?", { title: "Tem certeza?", cancelButtonText: 'Cancelar!' })
        .then(function (acao)
        {
          if (acao)
          {
            QueryService
              .query("DELETE", "dashboards/locais", { id }, null)
              .then(obterLocais);
          }
        });
    };


  }
})();
