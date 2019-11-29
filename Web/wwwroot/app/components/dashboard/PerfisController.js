;
(function ()
{
  "use strict";

  angular
    .module("boilerplate")
    .controller("PerfisController", PerfisController);

  PerfisController.$inject = ["$scope", "$state", "LocalStorage", "QueryService", "SweetAlert", "CadastroService"];

  function PerfisController($scope, $state, LocalStorage, QueryService, SweetAlert, CadastroService)
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
    self.perfilEncontrado = null;
    self.perfisConcorrentes = {};


    var obterConcorrentes = function ()
    {
      QueryService
        .query("GET", "dashboards/concorrentes", { idPerfil: self.perfilSelecionado.IdPerfil })
        .then(function (d)
        {
          self.perfisConcorrentes = d.data;
        });
    };


    obterConcorrentes();


    self.buscar = function (form)
    {
      if (form.$valid)
      {
        self.estaBuscando = true;

        QueryService
          .query("GET", "dashboards/instagram/usuarios", { usuario: self.busca, perfil: self.perfilSelecionado.Usuario })
          .then(function (d)
          {
            self.estaBuscando = false;
            self.perfilEncontrado = d.data;
          });
      }
    };


    self.adicionarPerfil = function ()
    {
      if (self.perfilSelecionado.Usuario == self.perfilEncontrado.Usuario)
      {
        SweetAlert.info("Não é possível adicionar o próprio perfil!", { title: "Ops..." });
        return false;
      }

      if (self.perfisConcorrentes.some(e => e.Usuario === self.perfilEncontrado.Usuario))
      {
        SweetAlert.info("Você já adicionou este perfil!", { title: "Ops..." });
        return false;
      }

      const obj =
      {
        IdInstagram: self.perfilEncontrado.Id,
        IdPerfil: self.perfilSelecionado.IdPerfil,
        Usuario: self.perfilEncontrado.Usuario,
        Nome: self.perfilEncontrado.Nome,
        SeguirQuemCurtiu: self.perfilEncontrado.SeguirQuemCurtiu ? 1 : 0
      };

      QueryService
        .query("POST", "dashboards/concorrentes", null, obj)
        .then(function ()
        {
          self.busca = "";
          self.perfilEncontrado = null;
          obterConcorrentes();
        });
    };


    self.removerPerfil = function (id)
    {
      SweetAlert
        .confirm("Confirma a remoção deste perfil?", { title: "Tem certeza?", cancelButtonText: 'Cancelar!' })
        .then(function (acao)
        {
          if (acao)
          {
            QueryService
              .query("DELETE", "dashboards/concorrentes", { id }, null)
              .then(obterConcorrentes);
          }
        });
    };


  }
})();
