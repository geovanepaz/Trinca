;
(function ()
{
  "use strict";

  angular
    .module("boilerplate")
    .controller("MinhaContaController", MinhaContaController);

  MinhaContaController.$inject = ["$scope", "$compile", "$state", "LocalStorage", "QueryService", "SweetAlert", "CadastroService", "$window"];

  function MinhaContaController($scope, $compile, $state, LocalStorage, QueryService, SweetAlert, CadastroService, $window)
  {
    // 'controller as'
    var self = this;

    // Padrão
    self.autenticacao = LocalStorage.get("autenticacao") || {};
    self.conta = self.autenticacao.Conta;
    self.perfil = self.autenticacao.Perfil;
    self.perfilSelecionado = LocalStorage.get("perfilselecionado") || {};
    self.ramosAtividades = {};

    // Planos que poderão ser contratados
    self.planos = {};
    self.planoSelecionado = {};
    self.etapaAssinatura = 1;

    // Dados comuns
    self.meses = [
      { mes: 1, nome: "Janeiro" },
      { mes: 2, nome: "Fevereiro" },
      { mes: 3, nome: "Março" },
      { mes: 4, nome: "Abril" },
      { mes: 5, nome: "Maio" },
      { mes: 6, nome: "Junho" },
      { mes: 7, nome: "Julho" },
      { mes: 8, nome: "Agosto" },
      { mes: 9, nome: "Setembro" },
      { mes: 10, nome: "Outubro" },
      { mes: 11, nome: "Novembro" },
      { mes: 12, nome: "Dezembro" }];

    self.anos = [];

    // Webhook
    self.pagamento = {
      IdPlano: "",
      IdCliente: "",
      NumeroCartao: "",
      NomeCartao: "",
      CpfCartao: "",
      MesExpiracao: "",
      AnoExpiracao: "",
      CodigoSeguranca: "",
      Parcelamento: ""
    };

    // Planos já contratados
    self.contratacoes = [];

    // Vínculo contratações com perfil
    self.usuarioInstagram = "";
    self.senhaInstagram = "";
    self.IdPerfil = "";
    self.etapaContratacao = 1;
    self.contratacaoSelecionada = {};
    self.contratacaoDisponivel = false;

    // Challenge
    self.challenge = {
      tipo: "",
      mensagem: "",
      codigo: ""
    };

    self.showHidePassword = function (passwordField, showhideICon)
    {
      var field = angular.element(document.querySelector('#' + passwordField));
      var icon = angular.element(document.querySelector('#' + showhideICon));

      if (field.attr("type") === "text")
      {
        icon.addClass("fa-eye-slash");
        icon.removeClass("fa-eye");
        field.attr('type', 'password');
        field.focus();
      }
      else if (field.attr("type") === "password")
      {
        field.attr('type', 'text');
        icon.removeClass("fa-eye-slash");
        icon.addClass("fa-eye");
        field.focus();
      }
    };

    var renovarLogin = function ()
    {
      return QueryService
        .query("POST", "dashboards/logins/renovacao", null, { IdCliente: self.conta.Id, Token: self.autenticacao.Token })
        .then(function (d)
        {
          LocalStorage.update('autenticacao', d.data);

          $window.location.reload();
        });
    };

    self.usuarioPorPerfil = function (idPerfil)
    {
      if (!angular.equals(self.perfil, []))
      {
        var perfil = self.perfil.find(o => o.IdPerfil === idPerfil);

        if (perfil && idPerfil)
        {
          return `( @${perfil.Usuario} )`;
        }
        else
        {
          return "";
        }
      }
    };

    self.atualizarContratacao = function ()
    {
      self.contratacaoSelecionada.IdPerfil = self.IdPerfil;

      return QueryService
        .query("PUT", "dashboards/contratacoes", null, self.contratacaoSelecionada)
        .then(function ()
        {
          angular.element('#modal-adicionar-perfil').modal('hide');

          return renovarLogin();
        });
    };

    self.cancelar = function (idContratacao)
    {
      SweetAlert
        .confirm("", { title: 'Confirma cancelamento desta assinatura?', cancelButtonText: 'Cancelar!' })
        .then(function (acao)
        {
          if (acao)
          {
            return QueryService
              .query("DELETE", "dashboards/contratacoes", { IdContratacao: idContratacao })
              .then(function (d)
              {
                angular.element('#modal-cancelar-assinatura').modal('hide');

                if (d.data == self.perfilSelecionado.IdPerfil)
                {
                  LocalStorage.remove('perfilselecionado');
                }

                return renovarLogin();
              });
          }
        });
    };

    // Etapa 3
    self.validarCodigo = function (form)
    {
      if (form.$valid)
      {
        var rota;

        if (self.challenge.tipo == "PhoneNumber" || self.challenge.tipo == "Email")
        {
          rota = "perfis/instagram/autorizacao";
        }

        if (self.challenge.tipo == "TwoFactorRequired")
        {
          rota = "perfis/instagram/doisfatores";
        }

        QueryService
          .query("POST", rota, null, { Codigo: self.challenge.codigo, Usuario: self.usuarioInstagram, Senha: self.senhaInstagram })
          .then(function (d)
          {
            d = d.data;

            if (d.Sucesso)
            {
              QueryService.query("POST", "dashboards/perfis", null, { Usuario: self.usuarioInstagram, Senha: self.senhaInstagram })
                .then(function (d)
                {
                  self.IdPerfil = d;
                  self.etapaContratacao = 2;
                });
            }
            else
            {
              // Gera um link dinâmico
              if (d.ReenviarCodigo === true)
              {
                var mensagem = `<br><br><a href="${window.location.href}" ng-click="ctrl.novoPerfil('',true)">Quer solicitar um novo código?</a>`;

                var htmlDinamico = $compile(mensagem)($scope);

                SweetAlert.error(d.Mensagem, { title: "Ops...", html: true });

                angular.element(document.querySelector(".sweet-alert p")).append(htmlDinamico);
              }
              else
              {
                SweetAlert.error(d.Mensagem, { title: "Ops..." });
              }

              return false;
            }
          });
      }
    };

    self.novoPerfil = function (form, fastPass)
    {
      fastPass = fastPass || false;

      if (fastPass)
      {
        swal.close();
        self.etapaContratacao = 1;
        self.challenge.codigo = "";
      }

      if (form.$valid || fastPass)
      {
        if (self.perfil.some(e => e.Usuario == self.usuarioInstagram))
        {
          SweetAlert.info("Você já possui este usuário registrado!", { title: "Ops..." });
          return false;
        }

        QueryService.query("POST", "dashboards/perfis", null, { Usuario: self.usuarioInstagram, Senha: self.senhaInstagram })
          .then(function (d)
          {
            d = d.data;

            if (d.Sucesso === false)
            {
              // Modal de challenge
              if (!!d.Challenge)
              {
                self.challenge.tipo = d.Challenge.Tipo;
                self.challenge.mensagem = d.Challenge.Mensagem;

                if (d.Challenge.Tipo == "PhoneNumber" || d.Challenge.Tipo == "Email")
                {
                  self.etapaContratacao = 3;
                }

                if (d.Challenge.Tipo == "Error" || d.Challenge.Tipo == "SubmitPhoneRequired" || d.Challenge.Tipo == "TwoFactorRequired")
                {
                  SweetAlert.error(d.Challenge.Mensagem, { title: "Ops..." });
                  return false;
                }
              }
              else
              {
                SweetAlert.error(d.Mensagem, { title: "Ops..." });
                return false;
              }
            }
            else
            {
              self.IdPerfil = d;
              self.etapaContratacao = 2;
            }
          });
      }
    };

    self.obterContratacoes = function ()
    {
      QueryService
        .query("GET", "dashboards/contratacoes", { idCliente: self.conta.Id })
        .then(function (d)
        {
          self.contratacoes = d.data;
          self.contratacaoSelecionada = self.contratacoes.filter(o => o.IdPerfil == null)[0];
          self.contratacaoDisponivel = self.contratacoes.some(o => o.IdPerfil == null);
        });
    };

    self.atualizarCadastro = function (form)
    {
      if (form.$valid)
      {
        CadastroService.atualizarCadastro(self.conta).then(function (d)
        {
          var autenticacao = { Conta: self.conta, Perfil: self.perfil };

          LocalStorage.update("autenticacao", autenticacao);

          SweetAlert.success("Dados atualizados!", { title: "Sucesso" });
        });
      }
    };

    self.modalCancelarAssinatura = function ()
    {
      angular.element('#modal-cancelar-assinatura').modal({ backdrop: 'static', keyboard: false, show: true });
    };

    self.modalAdicionarPerfil = function ()
    {
      self.etapaContratacao = 1;
      self.usuarioInstagram = "";
      self.senhaInstagram = "";

      angular.element('#modal-adicionar-perfil').modal({ backdrop: 'static', keyboard: false, show: true });
    };

    self.modalAlterarSenha = function ()
    {
      angular.element('#modal-senha').modal({ backdrop: 'static', keyboard: false, show: true });
    };

    self.modalContratar = function (form)
    {
      self.etapaAssinatura = 1;

      if (!form.$valid)
      {
        SweetAlert.error("Revise os seus dados cadastrais, pois existem campos que não foram preenchidos.", { title: "Ops..." });
        return false;
      }

      angular.element('#modal-contratar').modal({ backdrop: 'static', keyboard: false, show: true });
    };

    self.contratar = function (form)
    {
      if (form.$valid)
      {
        self.pagamento.IdPlano = self.planoSelecionado.Id;
        self.pagamento.IdCliente = self.conta.Id;

        QueryService
          .query("POST", "dashboards/contratacoes", null, self.pagamento)
          .then(function (d)
          {
            angular.element('#modal-contratar').modal('hide');

            SweetAlert.success("Novo plano contratado!", { title: "Sucesso" });

            $state.transitionTo($state.current, {}, { reload: true, inherit: false, notify: true });
          });
      }
    };

    self.alterarSenha = function (form)
    {
      if (form.$valid)
      {
        CadastroService.atualizarSenha(self.conta).then(function (d)
        {
          SweetAlert.success("Senha alterada!", { title: "Sucesso" });

          angular.element('#modal-senha').modal('hide');

          delete self.conta.ConfirmaSenha;
          delete self.conta.Senha;
          delete self.conta.NovaSenha;
        });
      }
    };

    // Init
    (function ()
    {
      // Carrega os anos de cartões
      for (var i = 0; i < 11; i++)
      {
        self.anos.push({ resumido: +(moment().format('YY')) + i, completo: +(moment().format('YYYY')) + i });
      }

      // Carrega os ramos de atividades
      CadastroService.ramosAtividades().then(function (d)
      {
        self.ramosAtividades = d;
      });

      // Carrega os planos sem o trial
      CadastroService.planos(false).then(function (d)
      {
        self.planos = d;
        self.planoSelecionado = self.planos[0];
      });

      // Carrega as contratações
      self.obterContratacoes();
    })();
  }
})();
