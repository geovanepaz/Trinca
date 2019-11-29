;
(function ()
{
  "use strict";

  angular
    .module("boilerplate")
    .controller("GerenciarPerfisController", GerenciarPerfisController);

  GerenciarPerfisController.$inject = ["$scope", "$state", "$window", "LocalStorage", "QueryService", "SweetAlert", "CadastroService"];

  function GerenciarPerfisController($scope, $state, $window, LocalStorage, QueryService, SweetAlert, CadastroService)
  {
    // 'controller as'
    var self = this;

    // Padrão
    self.autenticacao = LocalStorage.get("autenticacao") || {};
    self.conta = self.autenticacao.Conta;
    self.perfil = self.autenticacao.Perfil;
    self.perfilSelecionado = LocalStorage.get("perfilselecionado") || {};

    // Alteração de senha
    self.usuarioAlteracaoSenha = "";
    self.Instagram = { Usuario: "", Senha: "" };

    // Planos que poderão ser contratados
    self.planos = {};
    self.planoSelecionado = {};
    self.etapaAssinatura = 1;
    self.idPerfilRenovar = "";

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
      IdPerfil: "",
      NumeroCartao: "",
      NomeCartao: "",
      CpfCartao: "",
      MesExpiracao: "",
      AnoExpiracao: "",
      CodigoSeguranca: "",
      Parcelamento: ""
    };

    self.etapaAlteracaoSenha = 1;

    // Challenge
    self.challenge = {
      tipo: "",
      mensagem: "",
      codigo: ""
    };


    self.modalAlterarSenha = function (usuario)
    {
      self.Instagram.Usuario = usuario;
      angular.element('#modal-senha').modal({ backdrop: 'static', keyboard: false, show: true });
    };


    self.modalContratar = function (idPerfil)
    {
      self.idPerfilRenovar = idPerfil;
      angular.element('#modal-contratar').modal({ backdrop: 'static', keyboard: false, show: true });
    };


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
          .query("POST", rota, null, { Codigo: self.challenge.codigo, Usuario: self.Instagram.Usuario, Senha: self.Instagram.Senha })
          .then(function (d)
          {
            d = d.data;

            if (d.Sucesso)
            {
              QueryService.query("PATCH", "perfis/senhas", null, self.Instagram)
                .then(function (d)
                {
                  angular.element('#modal-senha').modal('hide');
                  self.Instagram.Usuario = "";
                  self.Instagram.Senha = "";

                  SweetAlert.success("Senha alterada!", { title: "Sucesso" });
                });
            }
            else
            {
              SweetAlert.error(d.Mensagem, { title: "Ops..." });
              return false;
            }
          });
      }
    };

    self.alterarSenha = function (form)
    {
      if (form.$valid)
      {
        QueryService
          .query("PATCH", "perfis/senhas", null, self.Instagram)
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
                  self.etapaAlteracaoSenha = 2;
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
              angular.element('#modal-senha').modal('hide');
              self.Instagram.Usuario = "";
              self.Instagram.Senha = "";

              SweetAlert.success("Senha alterada!", { title: "Sucesso" });
            }
          });
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


    self.remover = function (id)
    {
      SweetAlert
        .confirm("Confirma a remoção deste perfil?", { title: "Tem certeza?", cancelButtonText: 'Cancelar!' })
        .then(function (acao)
        {
          if (acao)
          {
            QueryService
              .query("DELETE", "perfis", { id }, null)
              .then(function ()
              {
                if (id == self.perfilSelecionado.IdPerfil)
                {
                  LocalStorage.remove('perfilselecionado');
                }

                renovarLogin();
              });
          }
        });
    };


    // Todo: Seguir este exemplo
    self.alterarComportamento = function (idPerfil, status)
    {
      QueryService
        .query("PATCH", "perfis/comportamentos", null, { Id: idPerfil, Status: status })
        .then(function ()
        {
          var perfil = _.find(self.autenticacao.Perfil, { 'IdPerfil': idPerfil });

          perfil.Status = status === 0 ? "Trabalhando" : "Parado";

          LocalStorage.update("autenticacao", self.autenticacao);

          $state.transitionTo($state.current, {}, { reload: true, inherit: false, notify: true });
        });
    };


    self.contratar = function (form)
    {
      if (form.$valid)
      {
        self.pagamento.IdPlano = self.planoSelecionado.Id;
        self.pagamento.IdCliente = self.conta.Id;
        self.pagamento.IdPerfil = self.idPerfilRenovar;

        QueryService
          .query("POST", "dashboards/contratacoes/renovacoes", null, self.pagamento)
          .then(function (d)
          {
            angular.element('#modal-contratar').modal('hide');

            SweetAlert.success("Novo plano contratado!", { title: "Sucesso" }).then(function ()
            {
              if (self.idPerfilRenovar == self.perfilSelecionado.IdPerfil)
              {
                LocalStorage.remove('perfilselecionado');
              }

              renovarLogin();
            });
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

      // Carrega os planos sem o trial
      CadastroService.planos(false).then(function (d)
      {
        self.planos = d;
        self.planoSelecionado = self.planos[0];
      });
    })();


  }
})();
