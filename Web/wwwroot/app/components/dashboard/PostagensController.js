;
(function ()
{
  "use strict";

  angular
    .module("boilerplate")
    .controller("PostagensController", PostagensController);

  PostagensController.$inject = ["$scope", "$state", "LocalStorage", "QueryService", "SweetAlert"];

  function PostagensController($scope, $state, LocalStorage, QueryService, SweetAlert)
  {
    // 'controller as'
    var self = this;

    // Padrão
    self.autenticacao = LocalStorage.get("autenticacao") || {};
    self.conta = self.autenticacao.Conta;
    self.perfil = self.autenticacao.Perfil;
    self.perfilSelecionado = LocalStorage.get("perfilselecionado") || {};

    // Local
    self.posts = null;


    var obterPostsPendentes = function ()
    {
      QueryService
        .query("GET", "dashboards/posts/pendentes", { idPerfil: self.perfilSelecionado.IdPerfil })
        .then(function (d)
        {
          self.pendentes = d.data;
        });
    };


    var obterPosts = function ()
    {
      QueryService
        .query("GET", "dashboards/posts", { idPerfil: self.perfilSelecionado.IdPerfil })
        .then(function (d)
        {
          self.posts = d.data;
        });
    };


    self.remover = function (id)
    {
      SweetAlert
        .confirm("", { title: "Tem certeza?", cancelButtonText: 'Cancelar!' })
        .then(function (acao)
        {
          if (acao)
          {
            QueryService
              .query("DELETE", `dashboards/posts/${id}`)
              .then(function (d)
              {
                obterPosts();
                obterPostsPendentes();
              });
          }
        });
    }


    obterPostsPendentes();
    obterPosts();

  }
})();
