;
(function ()
{
  "use strict";

  angular
    .module("boilerplate")
    .controller("ConfiguracoesController", ConfiguracoesController);

  ConfiguracoesController.$inject = ["$scope", "$state", "LocalStorage", "QueryService", "SweetAlert"];

  function ConfiguracoesController($scope, $state, LocalStorage, QueryService, SweetAlert)
  {
    // 'controller as'
    var self = this;

    // Padrão
    self.autenticacao = LocalStorage.get("autenticacao") || {};
    self.conta = self.autenticacao.Conta;
    self.perfil = self.autenticacao.Perfil;
    self.perfilSelecionado = LocalStorage.get("perfilselecionado") || {};

    // Local
    self.configuracoes = {
      Id: "",

      IdPerfil: self.perfilSelecionado.IdPerfil,

      SeguirParametro: 10,
      StatusSeguir: 0, // Enum 0 = Trabalhando | 1 = Parado
      SeguirAtivo: true,
      LogSeguir: null,
      AlteracaoSeguir: null,
      AlteracaoSeguirFormatado: null,
      AlteracaoSeguirVoltar: null,

      PararSeguirParametro: 10,
      StatusPararSeguir: 0,
      PararSeguirAtivo: true,
      LogPararSeguir: null,
      AlteracaoPararSeguir: null,
      AlteracaoPararSeguirFormatado: null,
      AlteracaoPararSeguirVoltar: null,

      PostarParametro: 10,
      StatusPostar: 0,
      PostarAtivo: true,
      LogPostar: null,
      AlteracaoPostar: null,
      AlteracaoPostarFormatado: null,
      AlteracaoPostarVoltar: null,

      Alerta: 'O Instagram identificou um comportamento estranho desta ação em sua conta. Sugerimos que você reduza o valor acima e volte a ativar para simularmos um comportamento mais "humano" através dos nossos robôs.'
    };

    self.configuracaoExistente = true;

    var obterConfiguracoes = function ()
    {
      QueryService
        .query("GET", "dashboards/configuracoes", { idPerfil: self.perfilSelecionado.IdPerfil })
        .then(function (d)
        {
          if (d.data)
          {
            var dados = d.data;

            if (dados.AlteracaoSeguir)
            {
              dados.AlteracaoSeguirFormatado = moment(dados.AlteracaoSeguir).format('DD/MM/YYYY HH:mm');
              dados.AlteracaoSeguirVoltar = moment(dados.AlteracaoSeguir).add(24, 'hours').format('DD/MM/YYYY HH:mm');
            }

            if (dados.AlteracaoPararSeguir)
            {
              dados.AlteracaoPararSeguirFormatado = moment(dados.AlteracaoPararSeguir).format('DD/MM/YYYY HH:mm');
              dados.AlteracaoPararSeguirVoltar = moment(dados.AlteracaoPararSeguir).add(24, 'hours').format('DD/MM/YYYY HH:mm');
            }

            if (dados.AlteracaoPostar)
            {
              dados.AlteracaoPostarFormatado = moment(dados.AlteracaoPostar).format('DD/MM/YYYY HH:mm');
              dados.AlteracaoPostarVoltar = moment(dados.AlteracaoPostar).add(24, 'hours').format('DD/MM/YYYY HH:mm');
            }

            if (dados.StatusSeguir === 0)
              dados.SeguirAtivo = true;
            else
              dados.SeguirAtivo = false;

            if (dados.StatusPostar === 0)
              dados.PostarAtivo = true;
            else
              dados.PostarAtivo = false;

            if (dados.StatusPararSeguir === 0)
              dados.PararSeguirAtivo = true;
            else
              dados.PararSeguirAtivo = false;

            self.configuracoes = angular.extend(self.configuracoes, dados);

            self.configuracaoExistente = true;
          }
          else
          {
            self.configuracaoExistente = false;
          }
        });
    };

    obterConfiguracoes();

    self.salvarConfiguracao = function (form)
    {
      if (form.$valid)
      {
        var metodo = !!self.configuracoes.Id ? "PUT" : "POST";

        // Desativado
        if (!self.configuracoes.SeguirAtivo)
        {
          self.configuracoes.StatusSeguir = 1;
          self.configuracoes.AlteracaoSeguir = self.configuracoes.AlteracaoSeguir;
        }
        else
        {
          self.configuracoes.StatusSeguir = 0;
          self.configuracoes.AlteracaoSeguir = null;
          self.configuracoes.LogSeguir = null;
        }

        // Desativado
        if (!self.configuracoes.PostarAtivo)
        {
          self.configuracoes.StatusPostar = 1;
          self.configuracoes.AlteracaoPostar = self.configuracoes.AlteracaoPostar;
        }
        else
        {
          self.configuracoes.StatusPostar = 0;
          self.configuracoes.AlteracaoPostar = null;
          self.configuracoes.LogPostar = null;
        }

        // Desativado
        if (!self.configuracoes.PararSeguirAtivo)
        {
          self.configuracoes.StatusPararSeguir = 1;
          self.configuracoes.AlteracaoPararSeguir = self.configuracoes.AlteracaoPararSeguir;
        }
        else
        {
          self.configuracoes.StatusPararSeguir = 0;
          self.configuracoes.AlteracaoPararSeguir = null;
          self.configuracoes.LogPararSeguir = null;
        }

        QueryService
          .query(metodo, "dashboards/configuracoes", null, self.configuracoes)
          .then(function ()
          {
            obterConfiguracoes();

            SweetAlert.success("As configurações foram salvas!", { title: "Sucesso" });
          });
      };
    };
  }

})();
