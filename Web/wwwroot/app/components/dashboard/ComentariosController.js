;
(function ()
{
  "use strict";

  angular
    .module("boilerplate")
    .controller("ComentariosController", ComentariosController);

  ComentariosController.$inject = ["$scope", "$state", "LocalStorage", "QueryService", "SweetAlert", "$window"];

  function ComentariosController($scope, $state, LocalStorage, QueryService, SweetAlert, $window)
  {
    var self = this;

    // Padrão
    self.autenticacao = LocalStorage.get("autenticacao") || {};
    self.conta = self.autenticacao.Conta;
    self.perfil = self.autenticacao.Perfil;
    self.perfilSelecionado = LocalStorage.get("perfilselecionado") || {};

    // Local
    self.comentariosNovos = [];
    self.comentariosRespondidos = [];


    var obterComentarios = function ()
    {
      QueryService
        .query("GET", "dashboards/instagram/comentarios", { Usuario: self.perfilSelecionado.Usuario })
        .then(function (d)
        {
          d = d.data;

          self.comentariosNovos = [];
          self.comentariosRespondidos = [];

          self.comentariosNovos = _.filter(d, { Comentarios: [{ Respondido: false }] });

          self.comentariosRespondidos = _.filter(d, { Comentarios: [{ Respondido: true }] });
        });
    };


    self.responderComentario = function (idPostagem, idComentario, resposta)
    {
      if (!resposta)
      {
        SweetAlert.error("É necessário informar uma resposta para este comentário!", { title: "Ops..." });
        return false;
      }

      QueryService
        .query("POST", "dashboards/instagram/comentarios", null, { usuario: self.perfilSelecionado.Usuario, idPostagem: idPostagem, idComentario: idComentario, resposta: resposta })
        .then(function ()
        {
          SweetAlert.success("Comentário respondido!", { title: "Tudo certo!" }).then(function ()
          {
            return obterComentarios();
          });
        });
    };


    // Sem uso
    self.curtirComentario = function (idComentario)
    {
      QueryService
        .query("POST", "dashboards/instagram/comentarios/curtidas", null, { usuario: self.perfilSelecionado.Usuario, idComentario: idComentario })
        .then(function ()
        {
          SweetAlert.success("Comentário curtido!", { title: "Tudo certo!" }).then(function ()
          {
            return obterComentarios();
          });
        });
    };


    self.removerComentario = function (idPostagem, idComentario)
    {
      SweetAlert
        .confirm("Confirma a remoção deste comentário?", { title: "Tem certeza?", cancelButtonText: 'Cancelar!' })
        .then(function (acao)
        {
          if (acao)
          {
            QueryService
              .query("DELETE", "dashboards/instagram/comentarios", { usuario: self.perfilSelecionado.Usuario, idPostagem: idPostagem, idComentario: idComentario })
              .then(function ()
              {
                SweetAlert.success("Comentário removido!", { title: "Tudo certo!" }).then(function ()
                {
                  return obterComentarios();
                });
              });
          }
        });
    };


    // Init
    (function ()
    {
      // Carrega todos os comentários
      obterComentarios();
    })();

  }
})();
