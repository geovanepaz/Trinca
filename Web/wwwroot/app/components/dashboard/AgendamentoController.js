;
(function ()
{
  "use strict";

  angular
    .module("boilerplate")
    .controller("AgendamentoController", AgendamentoController);

  AgendamentoController.$inject = ["$scope", "$state", "LocalStorage", "QueryService", "SweetAlert", "$stateParams"];

  function AgendamentoController($scope, $state, LocalStorage, QueryService, SweetAlert, $stateParams)
  {
    // 'controller as'
    var self = this;

    // Padrão
    self.autenticacao = LocalStorage.get("autenticacao") || {};
    self.conta = self.autenticacao.Conta;
    self.perfil = self.autenticacao.Perfil;
    self.perfilSelecionado = LocalStorage.get("perfilselecionado") || {};


    // Local
    self.postagem = {
      Id: 0,
      IdPerfil: self.perfilSelecionado.IdPerfil,
      Imagem: "",
      PostarNoStories: true,
      AgendarPrimeiroComentario: true,
      Descricao: "",
      PrimeiroComentario: "",
      Data: null,
      Hora: null,
      DataMinima: moment().format('DD/MM/YYYY'),
      DataMaxima: moment().add('1', 'year').format('DD/MM/YYYY')
    };


    self.pendentes = 0;


    self.atualizacao = false;


    var obterPostPorId = function ()
    {
      QueryService
        .query("GET", "dashboards/posts/" + self.postagem.Id)
        .then(function (d)
        {
          if (d.data)
          {
            self.atualizacao = true;

            d = d.data;

            self.postagem.AgendarPrimeiroComentario = d.AgendarPrimeiroComentario;
            self.postagem.Descricao = d.Descricao;
            self.postagem.Imagem = d.Imagem;
            self.postagem.PostarNoStories = d.PostarNoStories;
            self.postagem.PrimeiroComentario = d.PrimeiroComentario;
            self.postagem.Data = moment(d.Agendamento);
            self.postagem.Hora = moment(d.Agendamento);
            self.postagem.Agendamento = d.Agendamento;
            self.postagem.Status = d.Status;
          }
        });
    };


    if ($stateParams.id)
    {
      self.postagem.Id = $stateParams.id;

      obterPostPorId();
    }


    var obterPostsPendentes = function ()
    {
      QueryService
        .query("GET", "dashboards/posts/pendentes", { idPerfil: self.perfilSelecionado.IdPerfil })
        .then(function (d)
        {
          self.pendentes = d.data;
        });
    };


    obterPostsPendentes();


    self.novo = function ()
    {
      $state.transitionTo($state.current, {}, {
        reload: true, inherit: false, notify: true
      });
    };


    self.agendar = function (form)
    {
      if (form.$valid)
      {
        var title = self.atualizacao ? 'alteração' : 'agendamento';

        SweetAlert
          .confirm("", { title: `Confirma ${title}?`, cancelButtonText: 'Cancelar!' })
          .then(function (acao)
          {
            if (acao)
            {
              var zone = "America/Sao_Paulo";

              var novaHora = moment.tz(self.postagem.Hora, zone).format();
              var novaData = moment.tz(self.postagem.Data, zone).format();

              self.postagem.Data = novaData;
              self.postagem.Hora = novaHora;

              var verbo = self.atualizacao ? 'PUT' : 'POST';

              QueryService
                .query(verbo, "dashboards/posts", null, self.postagem)
                .then(function ()
                {
                  SweetAlert.success("", { title: "Tudo certo!" }).then(function ()
                  {
                    $state.reload();
                  });
                });
            }
          });
      }
    };



  }
})();
